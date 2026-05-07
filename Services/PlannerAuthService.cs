using System.Security.Claims;
using Eu5ModPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Eu5ModPlanner.Services;

public sealed class PlannerAuthService : IPlannerAuthService
{
    public const string DisplayNameClaimType = "planner_display_name";

    private readonly IPlannerRepository _repository;
    private readonly IPasswordHasher<PlannerUser> _passwordHasher;
    private readonly BootstrapAdminOptions _bootstrapAdminOptions;

    public PlannerAuthService(
        IPlannerRepository repository,
        IPasswordHasher<PlannerUser> passwordHasher,
        IOptions<BootstrapAdminOptions> bootstrapAdminOptions)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _bootstrapAdminOptions = bootstrapAdminOptions.Value;
    }

    public bool HasAnyAccounts() =>
        _repository.GetUsers().Any(user => user.IsActive);

    public PlannerUser? Authenticate(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var user = _repository.GetUserByUsername(username.Trim());
        if (user is null || !user.IsActive)
        {
            return null;
        }

        var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return verification is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded
            ? user
            : null;
    }

    public ClaimsPrincipal CreatePrincipal(PlannerUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(DisplayNameClaimType, user.DisplayName)
        };

        var identity = new ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    public bool CanWrite(ClaimsPrincipal principal, bool isDevelopment) =>
        isDevelopment || GetCurrentRole(principal, isDevelopment) is PlannerUserRole.Admin or PlannerUserRole.Editor;

    public bool CanManageUsers(ClaimsPrincipal principal, bool isDevelopment) =>
        isDevelopment || GetCurrentRole(principal, isDevelopment) == PlannerUserRole.Admin;

    public PlannerUserRole? GetCurrentRole(ClaimsPrincipal principal, bool isDevelopment)
    {
        if (isDevelopment)
        {
            return PlannerUserRole.Admin;
        }

        var roleValue = principal.FindFirstValue(ClaimTypes.Role);
        return Enum.TryParse<PlannerUserRole>(roleValue, ignoreCase: true, out var role)
            ? role
            : null;
    }

    public string GetCurrentDisplayName(ClaimsPrincipal principal, bool isDevelopment)
    {
        if (isDevelopment)
        {
            return "Local Development";
        }

        return principal.FindFirstValue(DisplayNameClaimType)
            ?? principal.Identity?.Name
            ?? string.Empty;
    }

    public void EnsureBootstrapAdmin()
    {
        if (string.IsNullOrWhiteSpace(_bootstrapAdminOptions.Username)
            || string.IsNullOrWhiteSpace(_bootstrapAdminOptions.Password))
        {
            return;
        }

        var existing = _repository.GetUserByUsername(_bootstrapAdminOptions.Username.Trim());
        if (existing is not null)
        {
            return;
        }

        var displayName = string.IsNullOrWhiteSpace(_bootstrapAdminOptions.DisplayName)
            ? _bootstrapAdminOptions.Username.Trim()
            : _bootstrapAdminOptions.DisplayName.Trim();

        var user = new PlannerUser
        {
            Username = _bootstrapAdminOptions.Username.Trim(),
            DisplayName = displayName,
            Role = PlannerUserRole.Admin,
            IsActive = true,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, _bootstrapAdminOptions.Password);
        _repository.AddUser(user);
    }

    public (bool IsValid, string Message) ValidateUserInput(UserAccountInputModel input, bool isEdit)
    {
        if (string.IsNullOrWhiteSpace(input.Username) || string.IsNullOrWhiteSpace(input.DisplayName))
        {
            return (false, isEdit ? "User could not be updated." : "User could not be created.");
        }

        var username = input.Username.Trim();
        var existingByUsername = _repository.GetUserByUsername(username);

        if (!isEdit)
        {
            if (existingByUsername is not null)
            {
                return (false, "Username is already taken.");
            }

            if (string.IsNullOrWhiteSpace(input.Password))
            {
                return (false, "Password is required for new users.");
            }
        }
        else
        {
            var existingUserId = input.Id.GetValueOrDefault();
            var existingUser = input.Id.HasValue && existingUserId != Guid.Empty
                ? _repository.GetUserById(existingUserId)
                : null;
            if (existingUser is null)
            {
                return (false, "User could not be updated.");
            }

            if (existingByUsername is not null && existingByUsername.Id != existingUserId)
            {
                return (false, "Username is already taken.");
            }

            if (existingUser.Role == PlannerUserRole.Admin && input.Role != PlannerUserRole.Admin)
            {
                var activeAdminCount = _repository.GetUsers().Count(user => user.IsActive && user.Role == PlannerUserRole.Admin);
                if (activeAdminCount <= 1)
                {
                    return (false, "At least one active admin account must remain.");
                }
            }
        }

        if (!string.IsNullOrEmpty(input.Password) || !string.IsNullOrEmpty(input.ConfirmPassword))
        {
            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 8)
            {
                return (false, "Passwords must be at least 8 characters long.");
            }

            if (!string.Equals(input.Password, input.ConfirmPassword, StringComparison.Ordinal))
            {
                return (false, "Passwords do not match.");
            }
        }

        return (true, string.Empty);
    }

    public PlannerUser CreateUser(UserAccountInputModel input)
    {
        var user = new PlannerUser
        {
            Username = input.Username.Trim(),
            DisplayName = input.DisplayName.Trim(),
            Role = input.Role,
            IsActive = true,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, input.Password);
        return _repository.AddUser(user);
    }

    public PlannerUser UpdateUser(UserAccountInputModel input)
    {
        var existing = _repository.GetUserById(input.Id!.Value)
            ?? throw new InvalidOperationException("User not found.");

        existing.Username = input.Username.Trim();
        existing.DisplayName = input.DisplayName.Trim();
        existing.Role = input.Role;
        existing.UpdatedUtc = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            existing.PasswordHash = _passwordHasher.HashPassword(existing, input.Password);
        }

        return _repository.UpdateUser(existing);
    }

    public bool SetUserActive(Guid userId, bool isActive, out string message)
    {
        var user = _repository.GetUserById(userId);
        if (user is null)
        {
            message = "User could not be updated.";
            return false;
        }

        if (!isActive && user.Role == PlannerUserRole.Admin)
        {
            var activeAdminCount = _repository.GetUsers().Count(existing => existing.IsActive && existing.Role == PlannerUserRole.Admin);
            if (activeAdminCount <= 1)
            {
                message = "At least one active admin account must remain.";
                return false;
            }
        }

        var updated = _repository.SetUserActive(userId, isActive);
        message = updated
            ? (isActive ? "User activated." : "User deactivated.")
            : "User could not be updated.";
        return updated;
    }
}
