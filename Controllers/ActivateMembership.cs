using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GYMNETIC.Core.Models;
using GYMNETIC.Core.Services;
using GYMNETIC.Core.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace GYMNETIC.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivateMembershipController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailService;

    public ActivateMembershipController(ApplicationDbContext context, IEmailSender emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("activate")]
    public async Task<IActionResult> ActivateMembership([FromBody] ActivateMembershipDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var preCustomer = await _context.PreCustomer
            .FirstOrDefaultAsync(p => p.Email == dto.Email && p.AccessCode == dto.AccessCode && p.IsApproved);

        if (preCustomer == null)
            return BadRequest("Código de acesso inválido ou conta não aprovada.");

        var existingCustomer = await _context.Customer.FirstOrDefaultAsync(c => c.Email == dto.Email);
        if (existingCustomer != null)
            return BadRequest("Já existe uma conta com este email.");

        var customer = new Customer
        {
            Email = preCustomer.Email,
            FirstName = preCustomer.FirstName,
            LastName = preCustomer.LastName,
            PhoneNumber = preCustomer.PhoneNumber,
            Address = preCustomer.Address,
            PostCode = preCustomer.PostCode,
            DateOfBirth = preCustomer.DateOfBirth,
            Gender = preCustomer.Gender,
            PhotoUrl = preCustomer.PhotoUrl,
            LegalName = preCustomer.LegalName,
            NIF = preCustomer.NIF,
            MonthlyPlanId = preCustomer.MonthlyPlanId,
            MembershipStartDate = DateTime.UtcNow,
            MembershipEndDate = DateTime.UtcNow.AddMonths(1),
            IsActive = true
        };

        _context.Customer.Add(customer);
        _context.PreCustomer.Remove(preCustomer);

        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(customer.Email, "Bem-vindo ao GYMNETIC",
            $"Olá {customer.FirstName}, sua conta foi ativada com sucesso! Seu acesso é válido até {customer.MembershipEndDate:dd/MM/yyyy}.");

        return Ok(new { message = "Associação ativada com sucesso!", customerId = customer.Id });
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetAvailablePlans()
    {
        var plans = await _context.MonthlyPlans
            .Where(p => p.IsActive) // Corrigido: usa o campo IsActive
            .Select(p => new { p.Id, p.PlanName, p.Description, p.Price, p.Features })
            .ToListAsync();

        return Ok(plans);
    }

    [HttpPut("customer/{id}/change-plan")]
    public async Task<IActionResult> ChangeMembershipPlan(int id, [FromBody] ChangePlanDto dto)
    {
        var customer = await _context.Customer.FindAsync(id);
        if (customer == null)
            return NotFound("Cliente não encontrado.");

        var plan = await _context.MonthlyPlans.FindAsync(dto.NewPlanId);
        if (plan == null)
            return BadRequest("Plano inválido.");

        customer.MonthlyPlanId = dto.NewPlanId;
        // Lógica adicional para ajustar datas ou preços, se necessário

        await _context.SaveChangesAsync();

        return Ok("Plano alterado com sucesso.");
    }

    [HttpPut("customer/{id}/renew")]
    public async Task<IActionResult> RenewMembership(int id, [FromBody] RenewMembershipDto dto)
    {
        var customer = await _context.Customer.FindAsync(id);
        if (customer == null)
            return NotFound("Cliente não encontrado.");

        customer.MembershipEndDate = customer.MembershipEndDate.AddMonths(dto.Months);
        customer.IsActive = true;

        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(customer.Email, "Renovação de Associação",
            $"Sua associação foi renovada até {customer.MembershipEndDate:dd/MM/yyyy}.");

        return Ok("Associação renovada com sucesso.");
    }

    [HttpPut("customer/{id}/cancel")]
    public async Task<IActionResult> CancelMembership(int id)
    {
        var customer = await _context.Customer.FindAsync(id);
        if (customer == null)
            return NotFound("Cliente não encontrado.");

        customer.IsActive = false;
        customer.CancellationDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(customer.Email, "Cancelamento de Associação",
            "Lamentamos que esteja a cancelar sua associação. Sua conta será desativada no final do período atual.");

        return Ok("Associação cancelada com sucesso.");
    }

    public class ActivateMembershipDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string AccessCode { get; set; } = string.Empty;
    }
    public class ChangePlanDto
    {
        [Required]
        public int NewPlanId { get; set; }
    }
    
    public class RenewMembershipDto
    {
        [Required]
        [Range(1, 12)]
        public int Months { get; set; } = 1;
    }
}