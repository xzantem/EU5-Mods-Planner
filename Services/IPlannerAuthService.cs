using System.Security.Claims;
using Eu5ModPlanner.Models;

namespace Eu5ModPlanner.Services;

public interface IPlannerAuthService
{
    bool HasAnyAccounts();
    PlannerUser? Authenticate(string username, string password);
    ClaimsPrincipal CreatePrincipal(PlannerUser user);
    bool CanWrite(ClaimsPrincipal principal, bool isDevelopment);
    bool CanManageUsers(ClaimsPrincipal principal, bool isDevelopment);
    PlannerUserRole? GetCurrentRole(ClaimsPrincipal principal, bool isDevelopment);
    string GetCurrentDisplayName(ClaimsPrincipal principal, bool isDevelopment);
    void EnsureBootstrapAdmin();
    (bool IsValid, string Message) ValidateUserInput(UserAccountInputModel input, bool isEdit);
    (bool IsValid, string Message) ValidateRegistration(UserRegistrationInputModel input);
    PlannerUser CreateUser(UserAccountInputModel input);
    PlannerUser RegisterUser(UserRegistrationInputModel input);
    PlannerUser UpdateUser(UserAccountInputModel input);
    bool SetUserActive(Guid userId, bool isActive, out string message);
}
