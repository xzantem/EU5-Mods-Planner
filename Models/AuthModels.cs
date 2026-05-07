using System.ComponentModel.DataAnnotations;

namespace Eu5ModPlanner.Models;

public enum PlannerUserRole
{
    Viewer,
    Editor,
    Admin
}

public sealed class PlannerUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(60)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public PlannerUserRole Role { get; set; } = PlannerUserRole.Viewer;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}

public sealed class BootstrapAdminOptions
{
    public const string SectionName = "BootstrapAdmin";

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string DisplayName { get; set; } = "Administrator";
}

public sealed class UserLoginInputModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public sealed class UserAccountInputModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(60)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    public PlannerUserRole Role { get; set; } = PlannerUserRole.Viewer;

    [StringLength(200)]
    public string Password { get; set; } = string.Empty;

    [StringLength(200)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
