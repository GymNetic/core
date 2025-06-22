using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GYMNETIC.Core.Data;
using GYMNETIC.Core.Models;
using GYMNETIC.Core.Services;

namespace GYMNETIC.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailService;

    public MembershipsController(ApplicationDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailService = emailSender;
    }

    public class PreCustomerDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AccessCode { get; set; }
        [Required]
        public int MonthlyPlanId { get; set; }
        [Required]
        public string Address { get; set; }
    }

    [HttpPost("precustomer")]
    public async Task<IActionResult> CreatePreCustomer([FromBody] PreCustomerDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var preCustomer = new PreCustomer
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            AccessCode = dto.AccessCode,
            MonthlyPlanId = dto.MonthlyPlanId,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow,
            IsApproved = false
        };

        _context.PreCustomers.Add(preCustomer);
        await _context.SaveChangesAsync();

        return Ok(preCustomer);
    }
    [HttpPost("precustomer/{id}/approve")]
    public async Task<IActionResult> ApprovePreCustomer(int id)
    {
        var preCustomer = await _context.PreCustomers.FindAsync(id);
        if (preCustomer == null)
            return NotFound();

        if (preCustomer.IsApproved)
            return BadRequest("Já aprovado.");

        preCustomer.AccessCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        preCustomer.IsApproved = true;

        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(preCustomer.Email, "Código de acesso", $"Seu código: {preCustomer.AccessCode}");
        
        return Ok("Pré-cliente aprovado e código enviado.");
    }
}