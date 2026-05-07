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
    PlannerUser CreateUser(UserAccountInputModel input);
    PlannerUser UpdateUser(UserAccountInputModel input);
    bool SetUserActive(Guid userId, bool isActive, out string message);
}
