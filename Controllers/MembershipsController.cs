﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GYMNETIC.Core.Data;
using GYMNETIC.Core.Models;
using GYMNETIC.Core.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

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
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string AccessCode { get; set; } = string.Empty;
        [Required]
        public int MonthlyPlanId { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
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

        _context.PreCustomer.Add(preCustomer);
        await _context.SaveChangesAsync();

        return Ok(preCustomer);
    }

    [HttpPost("precustomer/{id}/approve")]
    public async Task<IActionResult> ApprovePreCustomer(int id)
    {
        var preCustomer = await _context.PreCustomer.FindAsync(id);
        if (preCustomer == null)
            return NotFound();

        if (preCustomer.IsApproved)
            return BadRequest("Já aprovado.");

        preCustomer.AccessCode = Guid.NewGuid().ToString("N")[..8].ToUpper();
        preCustomer.IsApproved = true;

        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(preCustomer.Email, "Código de acesso", $"Seu código: {preCustomer.AccessCode}");
        
        return Ok("Pré-cliente aprovado e código enviado.");
    }
}