using System.ComponentModel.DataAnnotations;
using PAS.API.Validators;

namespace PAS.API.Models.Requests;

public class RegisterRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [CustomValidation(typeof(CustomValidators), nameof(CustomValidators.IsValidUsername))]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [CustomValidation(typeof(CustomValidators), nameof(CustomValidators.IsAlphanumeric))]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [CustomValidation(typeof(CustomValidators), nameof(CustomValidators.IsAlphaOnly))]
    public string Department { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [CustomValidation(typeof(CustomValidators), nameof(CustomValidators.IsStrongPassword))]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public Guid RoleId { get; set; }

    [EmailAddress]
    [CustomValidation(typeof(CustomValidators), nameof(CustomValidators.IsValidEmail))]
    public string? Email { get; set; }

    [CustomValidation(typeof(CustomValidators), nameof(CustomValidators.IsValidPhone))]
    public string? PhoneNumber { get; set; }
}