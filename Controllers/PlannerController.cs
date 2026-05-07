using Eu5ModPlanner.Models;
using Eu5ModPlanner.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Eu5ModPlanner.Controllers;

public sealed class PlannerController : Controller
{
    private const string CountriesLibrary = "Countries";
    private const string CultureGroupsLibrary = "CultureGroups";
    private const string CulturesLibrary = "Cultures";

    private static readonly HashSet<ContentType> AllowedCultureGroupContentTypes =
    [
        ContentType.Advance,
        ContentType.Reform,
        ContentType.Privilege,
        ContentType.Law,
        ContentType.Value,
        ContentType.Building
    ];

    private readonly IPlannerRepository _repository;
    private readonly IPlannerAuthService _authService;
    private readonly IWebHostEnvironment _environment;
    private static readonly JsonSerializerOptions EventRequirementJsonOptions = new(JsonSerializerDefaults.Web);

    public PlannerController(IPlannerRepository repository, IPlannerAuthService authService, IWebHostEnvironment environment)
    {
        _repository = repository;
        _authService = authService;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Index(Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        var data = _repository.GetData();
        var countries = data.Countries.OrderBy(country => country.IsArchived).ThenBy(country => country.Tag).ThenBy(country => country.Name).ToList();
        var activeCountries = countries.Where(country => !country.IsArchived).ToList();
        var archivedCountries = countries.Where(country => country.IsArchived).ToList();
        var allCultureGroups = data.ContentEntries
            .Where(entry => entry.Type == ContentType.CultureGroup)
            .OrderBy(entry => entry.IsArchived)
            .ThenBy(entry => entry.Name)
            .ToList();
        var allCultures = data.ContentEntries
            .Where(entry => entry.Type == ContentType.Culture)
            .OrderBy(entry => entry.IsArchived)
            .ThenBy(entry => entry.Name)
            .ToList();
        var effectLabelSuggestions = data.ContentEntries
            .SelectMany(entry => entry.Effects
                .Concat(entry.EventOptions.SelectMany(option => option.Effects))
                .Concat(entry.SituationActions.SelectMany(action => action.Effects))
                .Concat(data.Buffs.SelectMany(buff => buff.Effects)))
            .Select(effect => effect.Label)
            .Where(label => !string.IsNullOrWhiteSpace(label))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(label => label)
            .ToList();

        var activeLibrary = ResolveActiveLibrary(library, countryId, cultureId, cultureGroupId);
        var selectedCultureGroup = cultureGroupId.HasValue
            ? allCultureGroups.FirstOrDefault(entry => entry.Id == cultureGroupId.Value)
            : null;
        var selectedCulture = cultureId.HasValue
            ? allCultures.FirstOrDefault(entry => entry.Id == cultureId.Value)
            : null;
        var selectedCountry = activeLibrary == CountriesLibrary
            ? countries.FirstOrDefault(country => country.Id == countryId)
                ?? activeCountries.FirstOrDefault()
                ?? archivedCountries.FirstOrDefault()
            : null;
        if (activeLibrary != CulturesLibrary)
        {
            selectedCulture = null;
        }

        if (activeLibrary != CultureGroupsLibrary)
        {
            selectedCultureGroup = null;
        }

        var selectedContent = default(ContentEntry);
        var selectedCountryContent = new List<ContentEntry>();
        var hasWriteAccess = HasWriteAccess();
        var hasAnyAccounts = _authService.HasAnyAccounts();
        var canManageWriteAccess = _environment.IsDevelopment() == false && hasAnyAccounts;
        var canManageUsers = _authService.CanManageUsers(User, _environment.IsDevelopment());
        var canRegisterUsers = _environment.IsDevelopment() == false && hasAnyAccounts;
        var currentRole = _authService.GetCurrentRole(User, _environment.IsDevelopment());
        var currentDisplayName = _authService.GetCurrentDisplayName(User, _environment.IsDevelopment());
        var availableCultureGroups = allCultureGroups.Where(entry => !entry.IsArchived).ToList();
        var archivedCultureGroups = allCultureGroups.Where(entry => entry.IsArchived).ToList();
        var availableCultures = allCultures.Where(entry => !entry.IsArchived).ToList();
        var archivedCultures = allCultures.Where(entry => entry.IsArchived).ToList();

        if (activeLibrary == CultureGroupsLibrary && selectedCultureGroup is not null)
        {
            selectedCountryContent = data.ContentEntries
                .Where(entry => selectedCultureGroup.CultureGroupContentEntryIds.Contains(entry.Id))
                .OrderBy(entry => entry.Type)
                .ThenBy(entry => entry.Name)
                .ToList();
        }
        else if (activeLibrary == CountriesLibrary && selectedCountry is not null)
        {
            selectedCountryContent = data.ContentEntries
                .Where(entry => selectedCountry.ContentEntryIds.Contains(entry.Id))
                .Where(entry => entry.Type != ContentType.CultureGroup && entry.Type != ContentType.Culture)
                .OrderBy(entry => entry.Type)
                .ThenBy(entry => entry.Name)
                .ToList();
        }

        selectedContent = activeLibrary switch
        {
            CulturesLibrary => selectedCulture,
            CultureGroupsLibrary => selectedCountryContent.FirstOrDefault(entry => entry.Id == contentId) ?? selectedCultureGroup,
            _ => selectedCountryContent.FirstOrDefault(entry => entry.Id == contentId) ?? selectedCountryContent.FirstOrDefault()
        };

        var viewModel = new PlannerIndexViewModel
        {
            ActiveLibrary = activeLibrary,
            Countries = countries,
            ActiveCountries = activeCountries,
            ArchivedCountries = archivedCountries,
            AvailableCultures = availableCultures,
            ArchivedCultures = archivedCultures,
            AvailableCultureGroups = availableCultureGroups,
            ArchivedCultureGroups = archivedCultureGroups,
            EffectLabelSuggestions = effectLabelSuggestions,
            AvailableBuffs = data.Buffs.OrderBy(buff => buff.Name).ToList(),
            SelectedCountry = selectedCountry,
            SelectedCulture = selectedCulture,
            SelectedCultureGroup = selectedCultureGroup,
            SelectedCountryContent = selectedCountryContent,
            SelectedContent = selectedContent,
            HasWriteAccess = hasWriteAccess,
            HasAnyAccounts = hasAnyAccounts,
            CanManageWriteAccess = canManageWriteAccess,
            CanManageUsers = canManageUsers,
            CanRegisterUsers = canRegisterUsers,
            IsAuthenticatedUser = User.Identity?.IsAuthenticated == true,
            IsCultureGroupScope = selectedCultureGroup is not null,
            CurrentUserDisplayName = currentDisplayName,
            CurrentUserRoleName = currentRole?.ToString() ?? string.Empty,
            SelectedContentPayloadJson = selectedContent is null ? null : JsonSerializer.Serialize(ToEditPayload(selectedContent)),
            AvailableBuffsPayloadJson = JsonSerializer.Serialize(data.Buffs.OrderBy(buff => buff.Name).Select(ToEditBuff).ToList()),
            Users = data.Users.OrderBy(user => user.IsActive ? 0 : 1).ThenBy(user => user.Username).ToList(),
            CountryForm = new CountryInputModel(),
            AdvanceForm = BuildDefaultInputModel(selectedCountry?.Id ?? Guid.Empty, selectedCultureGroup?.Id),
            BuffForm = new BuffInputModel(),
            LoginForm = new UserLoginInputModel(),
            RegistrationForm = new UserRegistrationInputModel(),
            UserForm = new UserAccountInputModel()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddCountry([Bind(Prefix = "CountryForm")] CountryInputModel input)
    {
        if (TryRejectWriteAccess(null))
        {
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Country could not be added.";
            return RedirectToAction(nameof(Index));
        }

        var country = new Country
        {
            Name = input.Name.Trim(),
            Tag = input.Tag.Trim().ToUpperInvariant(),
            IsArchived = false
        };

        _repository.AddCountry(country);
        TempData["Message"] = $"Added country {country.Tag}.";
        return RedirectToAction(nameof(Index), new { countryId = country.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCountry([Bind(Prefix = "CountryForm")] CountryInputModel input)
    {
        if (TryRejectWriteAccess(input.Id))
        {
            return RedirectToAction(nameof(Index), new { countryId = input.Id });
        }

        if (!ModelState.IsValid || input.Id is null || input.Id == Guid.Empty)
        {
            TempData["Message"] = "Country could not be updated.";
            return RedirectToAction(nameof(Index), new { countryId = input.Id });
        }

        var existingCountry = _repository.GetData().Countries.FirstOrDefault(country => country.Id == input.Id.Value);
        if (existingCountry is null)
        {
            TempData["Message"] = "Country could not be updated.";
            return RedirectToAction(nameof(Index));
        }

        existingCountry.Name = input.Name.Trim();
        existingCountry.Tag = input.Tag.Trim().ToUpperInvariant();
        _repository.UpdateCountry(existingCountry);

        TempData["Message"] = $"Updated country {existingCountry.Tag}.";
        return RedirectToAction(nameof(Index), new { countryId = existingCountry.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ArchiveCountry(Guid countryId)
    {
        if (TryRejectWriteAccess(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId });
        }

        _repository.SetCountryArchived(countryId, true);
        TempData["Message"] = "Country archived.";
        return RedirectToAction(nameof(Index), new { countryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RestoreCountry(Guid countryId)
    {
        if (TryRejectWriteAccess(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId });
        }

        _repository.SetCountryArchived(countryId, false);
        TempData["Message"] = "Country restored.";
        return RedirectToAction(nameof(Index), new { countryId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ArchiveLibraryEntry(Guid contentId, string? library)
    {
        if (TryRejectWriteAccess(null))
        {
            return RedirectToAction(nameof(Index), new { contentId, library });
        }

        var entry = _repository.GetData().ContentEntries.FirstOrDefault(item => item.Id == contentId);
        if (entry is null || (entry.Type != ContentType.Culture && entry.Type != ContentType.CultureGroup))
        {
            TempData["Message"] = "Entry could not be archived.";
            return RedirectToAction(nameof(Index), new { library });
        }

        _repository.SetContentArchived(contentId, true);
        TempData["Message"] = $"{entry.Type switch { ContentType.Culture => "Culture", _ => "Culture group" }} archived.";
        return RedirectToAction(nameof(Index), new
        {
            library = entry.Type == ContentType.Culture ? CulturesLibrary : CultureGroupsLibrary,
            cultureId = entry.Type == ContentType.Culture ? (Guid?)contentId : null,
            cultureGroupId = entry.Type == ContentType.CultureGroup ? (Guid?)contentId : null
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RestoreLibraryEntry(Guid contentId, string? library)
    {
        if (TryRejectWriteAccess(null))
        {
            return RedirectToAction(nameof(Index), new { contentId, library });
        }

        var entry = _repository.GetData().ContentEntries.FirstOrDefault(item => item.Id == contentId);
        if (entry is null || (entry.Type != ContentType.Culture && entry.Type != ContentType.CultureGroup))
        {
            TempData["Message"] = "Entry could not be restored.";
            return RedirectToAction(nameof(Index), new { library });
        }

        _repository.SetContentArchived(contentId, false);
        TempData["Message"] = $"{entry.Type switch { ContentType.Culture => "Culture", _ => "Culture group" }} restored.";
        return RedirectToAction(nameof(Index), new
        {
            library = entry.Type == ContentType.Culture ? CulturesLibrary : CultureGroupsLibrary,
            cultureId = entry.Type == ContentType.Culture ? (Guid?)contentId : null,
            cultureGroupId = entry.Type == ContentType.CultureGroup ? (Guid?)contentId : null
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddContent([Bind(Prefix = "AdvanceForm")] AdvanceInputModel input)
    {
        if (TryRejectWriteAccess(input.CountryId))
        {
            return RedirectToAction(nameof(Index), new { countryId = input.CountryId });
        }

        return SaveContent(input, isEdit: false);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditContent([Bind(Prefix = "AdvanceForm")] AdvanceInputModel input)
    {
        if (TryRejectWriteAccess(input.CountryId))
        {
            return RedirectToAction(nameof(Index), new { countryId = input.CountryId });
        }

        return SaveContent(input, isEdit: true);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddBuff([Bind(Prefix = "BuffForm")] BuffInputModel input, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (TryRejectWriteAccess(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        return SaveBuff(input, isEdit: false, countryId, cultureId, cultureGroupId, contentId, library);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditBuff([Bind(Prefix = "BuffForm")] BuffInputModel input, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (TryRejectWriteAccess(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        return SaveBuff(input, isEdit: true, countryId, cultureId, cultureGroupId, contentId, library);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteBuff(Guid buffId, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (TryRejectWriteAccess(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        _repository.DeleteBuff(buffId);
        TempData["Message"] = "Buff deleted.";
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveBuffApi(BuffInputModel input, bool isEdit)
    {
        if (TryRejectWriteAccess(null))
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { ok = false, message = "Read-only mode. Sign in to edit the database." });
        }

        return SaveBuff(input, isEdit, null, null, null, null, null, jsonResponse: true);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteBuffApi(Guid buffId)
    {
        if (TryRejectWriteAccess(null))
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { ok = false, message = "Read-only mode. Sign in to edit the database." });
        }

        var deleted = _repository.DeleteBuff(buffId);
        if (!deleted)
        {
            return NotFound(new { ok = false, message = "Buff could not be deleted." });
        }

        return Json(new { ok = true, message = "Buff deleted.", buffId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind(Prefix = "LoginForm")] UserLoginInputModel input, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (_environment.IsDevelopment())
        {
            TempData["Message"] = "Local development mode already has write access.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        if (!_authService.HasAnyAccounts())
        {
            TempData["Message"] = "No active user accounts are configured yet.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Invalid login details.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        var user = _authService.Authenticate(input.Username, input.Password);
        if (user is null)
        {
            TempData["Message"] = "Invalid login details.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            _authService.CreatePrincipal(user));

        TempData["Message"] = $"Signed in as {user.DisplayName}.";
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register([Bind(Prefix = "RegistrationForm")] UserRegistrationInputModel input, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (_environment.IsDevelopment())
        {
            TempData["Message"] = "Local development mode already has full access.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        if (!_authService.HasAnyAccounts())
        {
            TempData["Message"] = "Account registration is not available until the first admin account exists.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        var validation = _authService.ValidateRegistration(input);
        if (!ModelState.IsValid || !validation.IsValid)
        {
            TempData["Message"] = validation.IsValid ? "Account could not be created." : validation.Message;
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        var user = _authService.RegisterUser(input);
        TempData["Message"] = $"Account created for {user.DisplayName}. An admin can promote it to editor access if needed.";
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (_environment.IsDevelopment())
        {
            TempData["Message"] = "Local development mode keeps write access enabled.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Message"] = "Signed out.";
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveUser([Bind(Prefix = "UserForm")] UserAccountInputModel input, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (TryRejectUserManagement(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        var isEdit = input.Id.HasValue && input.Id.Value != Guid.Empty;
        var validation = _authService.ValidateUserInput(input, isEdit);
        if (!ModelState.IsValid || !validation.IsValid)
        {
            TempData["Message"] = validation.IsValid
                ? (isEdit ? "User could not be updated." : "User could not be created.")
                : validation.Message;
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        var user = isEdit
            ? _authService.UpdateUser(input)
            : _authService.CreateUser(input);

        TempData["Message"] = isEdit
            ? $"Updated user {user.Username}."
            : $"Created user {user.Username}.";
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetUserActive(Guid userId, bool isActive, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library)
    {
        if (TryRejectUserManagement(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        _authService.SetUserActive(userId, isActive, out var message);
        TempData["Message"] = message;
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    private IActionResult SaveContent(AdvanceInputModel input, bool isEdit)
    {
        var data = _repository.GetData();
        var normalizedCultureGroupScopeId = input.Type is ContentType.Culture or ContentType.CultureGroup
            ? null
            : input.CultureGroupId;
        var availableBuffs = data.Buffs.ToDictionary(buff => buff.Id, buff => buff);
        var effects = BuildEffects(input.Effects, ValueEffectSide.Default, availableBuffs, allowBuffEffects: false);
        var leftEffects = BuildEffects(input.LeftEffects, ValueEffectSide.Left, availableBuffs, allowBuffEffects: false);
        var rightEffects = BuildEffects(input.RightEffects, ValueEffectSide.Right, availableBuffs, allowBuffEffects: false);
        var situationStartEffects = BuildEffects(input.SituationStartEffects, ValueEffectSide.SituationOnStart, availableBuffs);
        var situationMonthlyEffects = BuildEffects(input.SituationMonthlyEffects, ValueEffectSide.SituationOnMonthly, availableBuffs);
        var situationEndingEffects = BuildEffects(input.SituationEndingEffects, ValueEffectSide.SituationOnEnding, availableBuffs);
        var situationEndedEffects = BuildEffects(input.SituationEndedEffects, ValueEffectSide.SituationOnEnded, availableBuffs);
        var selectedCountry = data.Countries.FirstOrDefault(country => country.Id == input.CountryId);
        var selectedCultureGroup = normalizedCultureGroupScopeId.HasValue
            ? data.ContentEntries.FirstOrDefault(entry => entry.Id == normalizedCultureGroupScopeId.Value && entry.Type == ContentType.CultureGroup && !entry.IsArchived)
            : null;
        var existingEntry = isEdit && input.ContentId.HasValue
            ? data.ContentEntries.FirstOrDefault(entry => entry.Id == input.ContentId.Value)
            : null;
        var selectedCountryCustomEstates = selectedCountry is null
            ? []
            : data.ContentEntries
                .Where(entry => entry.Type == ContentType.CustomEstate && selectedCountry.ContentEntryIds.Contains(entry.Id))
                .Select(entry => entry.Name)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        var selectedCountryEventEntries = selectedCountry is null
            ? []
            : data.ContentEntries
                .Where(entry => entry.Type == ContentType.Event && selectedCountry.ContentEntryIds.Contains(entry.Id))
                .ToList();
        var availableCultureGroups = data.ContentEntries
            .Where(entry => entry.Type == ContentType.CultureGroup && !entry.IsArchived)
            .OrderBy(entry => entry.Name)
            .ToList();
        var archivedCultureGroupNames = data.ContentEntries
            .Where(entry => entry.Type == ContentType.CultureGroup && entry.IsArchived)
            .Select(entry => entry.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var hasEstateRename = !string.IsNullOrWhiteSpace(input.NobilityEstateName)
            || !string.IsNullOrWhiteSpace(input.BurghersEstateName)
            || !string.IsNullOrWhiteSpace(input.ClergyEstateName)
            || !string.IsNullOrWhiteSpace(input.PeasantsEstateName);
        var hasNamedContent = input.Type is ContentType.Advance or ContentType.Reform or ContentType.CustomEstate or ContentType.Privilege or ContentType.Law or ContentType.Building or ContentType.Event or ContentType.Situation or ContentType.Culture or ContentType.CultureGroup
            && !string.IsNullOrWhiteSpace(input.Name);
        var hasValueLabels = input.Type != ContentType.Value
            || (!string.IsNullOrWhiteSpace(input.ValueLeftLabel) && !string.IsNullOrWhiteSpace(input.ValueRightLabel));
        var hasCustomEstateName = input.Type == ContentType.CustomEstate && !string.IsNullOrWhiteSpace(input.Name);
        var constructionCosts = BuildResourceAmounts(input.ConstructionCosts);
        var productionMethods = BuildProductionMethods(input.ProductionMethods);
        var eventRequirements = BuildEventRequirements(input.EventRequirementTreeJson, input.EventRequirements);
        var eventOptions = BuildEventOptions(input.EventOptions, availableBuffs);
        var situationActions = BuildSituationActions(input.SituationActions, availableBuffs);
        var resolvedCultureGroupNames = BuildCultureGroupNames(input.CultureGroupMembershipNames, input.CultureGroupIds, availableCultureGroups);
        var resolvedCultureGroupIds = availableCultureGroups
            .Where(group => resolvedCultureGroupNames.Contains(group.Name, StringComparer.OrdinalIgnoreCase))
            .Select(group => group.Id)
            .Distinct()
            .ToList();
        var prerequisiteEventIds = (input.EventPrerequisiteIds ?? [])
            .Distinct()
            .ToList();
        var invalidPrivilegeCustomEstate = input.Type == ContentType.Privilege
            && input.PrivilegeEstateTarget == PrivilegeEstateTarget.Custom
            && (string.IsNullOrWhiteSpace(input.PrivilegeCustomEstateName)
                || selectedCountryCustomEstates.Contains(input.PrivilegeCustomEstateName.Trim(), StringComparer.OrdinalIgnoreCase) == false);
        var resolvedLawCategory = input.LawCategory == "Custom"
            ? input.LawCustomCategory?.Trim() ?? string.Empty
            : input.LawCategory?.Trim() ?? string.Empty;
        var resolvedLawSubcategory = input.LawCategory == "Custom" || input.LawSubcategory == "Custom"
            ? input.LawCustomSubcategory?.Trim() ?? string.Empty
            : input.LawSubcategory?.Trim() ?? string.Empty;
        var invalidLawCategory = input.Type == ContentType.Law && string.IsNullOrWhiteSpace(resolvedLawCategory);
        var invalidLawSubcategory = input.Type == ContentType.Law && string.IsNullOrWhiteSpace(resolvedLawSubcategory);
        var invalidLawCustomEstate = input.Type == ContentType.Law
            && input.LawEstatePreferenceTarget == PrivilegeEstateTarget.Custom
            && (string.IsNullOrWhiteSpace(input.LawCustomEstateName)
                || selectedCountryCustomEstates.Contains(input.LawCustomEstateName.Trim(), StringComparer.OrdinalIgnoreCase) == false);
        var invalidBuffEffects = HasInvalidBuffEffects(input.Effects, availableBuffs, allowBuffEffects: false)
            || HasInvalidBuffEffects(input.LeftEffects, availableBuffs, allowBuffEffects: false)
            || HasInvalidBuffEffects(input.RightEffects, availableBuffs, allowBuffEffects: false)
            || HasInvalidBuffEffects(input.SituationStartEffects, availableBuffs)
            || HasInvalidBuffEffects(input.SituationMonthlyEffects, availableBuffs)
            || HasInvalidBuffEffects(input.SituationEndingEffects, availableBuffs)
            || HasInvalidBuffEffects(input.SituationEndedEffects, availableBuffs)
            || (input.EventOptions ?? []).Any(option => HasInvalidBuffEffects(option.Effects, availableBuffs))
            || (input.SituationActions ?? []).Any(action => HasInvalidBuffEffects(action.Effects, availableBuffs));
        var invalidCultureGroupScope = normalizedCultureGroupScopeId.HasValue
            && selectedCultureGroup is null;
        var invalidCultureMembership = input.Type == ContentType.Culture
            && resolvedCultureGroupNames.Count == 0;
        var invalidArchivedCultureMembership = input.Type == ContentType.Culture
            && resolvedCultureGroupNames.Any(name => archivedCultureGroupNames.Contains(name));
        var invalidTopLevelCultureEntityCountryScope = selectedCountry is not null
            && input.Type is ContentType.Culture or ContentType.CultureGroup;
        var editingSelectedCultureGroup = isEdit
            && selectedCultureGroup is not null
            && existingEntry?.Id == selectedCultureGroup.Id;
        var invalidCultureGroupContentType = normalizedCultureGroupScopeId.HasValue
            && !editingSelectedCultureGroup
            && AllowedCultureGroupContentTypes.Contains(input.Type) == false;

        var invalidEffects = input.Type is ContentType.Advance or ContentType.Reform or ContentType.Privilege or ContentType.Law or ContentType.Building && effects.Count == 0;
        var invalidValue = input.Type == ContentType.Value && (!hasValueLabels || leftEffects.Count == 0 || rightEffects.Count == 0);
        var invalidEstateRename = input.Type == ContentType.DefaultEstateRename && !hasEstateRename;
        var invalidNamedContent = !hasNamedContent && input.Type is ContentType.Advance or ContentType.Reform or ContentType.CustomEstate or ContentType.Privilege or ContentType.Law or ContentType.Building or ContentType.Event or ContentType.Situation or ContentType.Culture or ContentType.CultureGroup;
        var invalidCustomEstate = input.Type == ContentType.CustomEstate && !hasCustomEstateName;
        var invalidBuilding = input.Type == ContentType.Building
            && (constructionCosts.Count == 0
                || productionMethods.Count == 0
                || input.BuildingTimeMonths <= 0
                || input.BuildingDucatCost < 0);
        var invalidEvent = input.Type == ContentType.Event
            && (string.IsNullOrWhiteSpace(input.EventDescription)
                || eventOptions.Count == 0
                || (input.EventTriggerMode == EventTriggerMode.MonthlyChance && input.EventMonthlyChance <= 0)
                || (input.EventYearStart.HasValue && input.EventYearEnd.HasValue && input.EventYearStart.Value > input.EventYearEnd.Value)
                || prerequisiteEventIds.Any(id => selectedCountryEventEntries.All(entry => entry.Id != id))
                || (isEdit && input.ContentId.HasValue && prerequisiteEventIds.Contains(input.ContentId.Value)));
        var invalidSituation = input.Type == ContentType.Situation
            && (string.IsNullOrWhiteSpace(input.SituationCanStart)
                || string.IsNullOrWhiteSpace(input.SituationVisible)
                || string.IsNullOrWhiteSpace(input.SituationCanEnd)
                || (input.SituationMonthlySpawnChance.HasValue
                    && (input.SituationMonthlySpawnChance.Value < 0m || input.SituationMonthlySpawnChance.Value > 100m)));
        var invalidSituationAction = input.Type == ContentType.Situation
            && (input.SituationActions ?? [])
                .Any(action => string.IsNullOrWhiteSpace(action.Name)
                    && (!string.IsNullOrWhiteSpace(action.Requirements)
                        || !string.IsNullOrWhiteSpace(action.Cost)
                        || !string.IsNullOrWhiteSpace(action.Cooldown)
                        || (action.Effects ?? []).Any(effect => !string.IsNullOrWhiteSpace(effect.Label))));
        var invalidEditTarget = isEdit && (input.ContentId is null || existingEntry is null);
        var requiresCountryScope = input.CultureGroupId.HasValue == false
            && input.Type is not ContentType.Culture
            && input.Type is not ContentType.CultureGroup;
        var invalidCountryScope = requiresCountryScope && selectedCountry is null;

        if (!ModelState.IsValid || invalidCountryScope || invalidCultureGroupScope || invalidCultureMembership || invalidArchivedCultureMembership || invalidTopLevelCultureEntityCountryScope || invalidCultureGroupContentType || invalidEffects || invalidValue || invalidEstateRename || invalidNamedContent || invalidCustomEstate || invalidPrivilegeCustomEstate || invalidLawCategory || invalidLawSubcategory || invalidLawCustomEstate || invalidBuilding || invalidEvent || invalidSituation || invalidSituationAction || invalidBuffEffects || invalidEditTarget)
        {
            TempData["Message"] = invalidCultureMembership || invalidArchivedCultureMembership
                ? "Culture must belong to at least one existing culture group."
                : isEdit ? "Content could not be updated." : "Content could not be added.";
            return RedirectToAction(nameof(Index), new
            {
                countryId = selectedCountry?.Id,
                cultureId = input.Type == ContentType.Culture ? input.ContentId : null,
                cultureGroupId = normalizedCultureGroupScopeId ?? (input.Type == ContentType.CultureGroup ? input.ContentId : null),
                contentId = input.Type is ContentType.Culture or ContentType.CultureGroup ? null : input.ContentId,
                library = GetLibraryForContent(input.Type, normalizedCultureGroupScopeId)
            });
        }

        var valueLeftLabel = input.ValueLeftLabel?.Trim() ?? string.Empty;
        var valueRightLabel = input.ValueRightLabel?.Trim() ?? string.Empty;
        var combinedValueEffects = leftEffects.Concat(rightEffects).ToList();
        var combinedSituationEffects = situationStartEffects
            .Concat(situationMonthlyEffects)
            .Concat(situationEndingEffects)
            .Concat(situationEndedEffects)
            .ToList();

        var prerequisiteLinks = prerequisiteEventIds
            .Join(selectedCountryEventEntries, id => id, entry => entry.Id, (id, entry) => new EventPrerequisiteLink
            {
                RequiredEventId = id,
                RequiredEventName = entry.Name
            })
            .ToList();

        var entry = new ContentEntry
        {
            Id = existingEntry?.Id ?? Guid.NewGuid(),
            Type = input.Type,
            IsArchived = existingEntry?.IsArchived ?? false,
            Name = input.Type switch
            {
                ContentType.DefaultEstateRename => "Default Estate Rename",
                ContentType.Value => $"{valueLeftLabel} vs {valueRightLabel}",
                ContentType.Culture => input.Name!.Trim(),
                ContentType.CultureGroup => input.Name!.Trim(),
                ContentType.Event => input.Name!.Trim(),
                ContentType.Situation => input.Name!.Trim(),
                _ => input.Name!.Trim()
            },
            IsMajorReform = input.Type == ContentType.Reform && input.IsMajorReform,
            NobilityEstateName = input.NobilityEstateName?.Trim() ?? string.Empty,
            BurghersEstateName = input.BurghersEstateName?.Trim() ?? string.Empty,
            ClergyEstateName = input.ClergyEstateName?.Trim() ?? string.Empty,
            PeasantsEstateName = input.PeasantsEstateName?.Trim() ?? string.Empty,
            FoodConsumptionPerThousand = input.FoodConsumptionPerThousand,
            AssimilationConversionSpeed = input.AssimilationConversionSpeed,
            EstateClass = input.EstateClass,
            CanPromote = input.CanPromote,
            PromotionSpeed = input.CanPromote ? input.PromotionSpeed : 0,
            MigrationSpeed = input.MigrationSpeed,
            PrivilegeEstateTarget = input.PrivilegeEstateTarget,
            PrivilegeCustomEstateName = input.PrivilegeEstateTarget == PrivilegeEstateTarget.Custom
                ? input.PrivilegeCustomEstateName?.Trim() ?? string.Empty
                : string.Empty,
            SatisfactionBonusPercent = input.SatisfactionBonusPercent,
            EstatePowerPercent = input.EstatePowerPercent,
            LawCategoryName = resolvedLawCategory,
            LawSubcategoryName = resolvedLawSubcategory,
            LawEstatePreferenceTarget = input.LawEstatePreferenceTarget,
            LawCustomEstateName = input.LawEstatePreferenceTarget == PrivilegeEstateTarget.Custom
                ? input.LawCustomEstateName?.Trim() ?? string.Empty
                : string.Empty,
            ValueLeftLabel = valueLeftLabel,
            ValueRightLabel = valueRightLabel,
            BuildingConstructionScope = input.BuildingConstructionScope,
            BuildingDucatCost = input.BuildingDucatCost,
            BuildingTimeMonths = input.BuildingTimeMonths,
            EventDescription = input.EventDescription?.Trim() ?? string.Empty,
            EventYearStart = input.EventYearStart,
            EventYearEnd = input.EventYearEnd,
            EventTriggerMode = input.EventTriggerMode,
            EventScenarioName = string.Empty,
            EventMonthlyChance = input.EventTriggerMode == EventTriggerMode.MonthlyChance
                ? input.EventMonthlyChance
                : 0,
            SituationDescription = input.SituationDescription?.Trim() ?? string.Empty,
            SituationCanStart = input.SituationCanStart?.Trim() ?? string.Empty,
            SituationVisible = input.SituationVisible?.Trim() ?? string.Empty,
            SituationCanEnd = input.SituationCanEnd?.Trim() ?? string.Empty,
            SituationMonthlySpawnChance = input.SituationMonthlySpawnChance,
            CultureGroupIds = input.Type == ContentType.Culture
                ? resolvedCultureGroupIds
                : [],
            CultureGroupNames = input.Type == ContentType.Culture
                ? resolvedCultureGroupNames
                : [],
            CultureGroupContentEntryIds = existingEntry?.CultureGroupContentEntryIds ?? [],
            SituationActions = situationActions,
            ConstructionCosts = constructionCosts,
            ProductionMethods = productionMethods,
            EventRequirements = eventRequirements,
            EventOptions = eventOptions,
            EventPrerequisites = prerequisiteLinks,
            Effects = input.Type switch
            {
                ContentType.DefaultEstateRename or ContentType.CustomEstate or ContentType.Event or ContentType.Culture or ContentType.CultureGroup => [],
                ContentType.Value => combinedValueEffects,
                ContentType.Situation => combinedSituationEffects,
                _ => effects
            }
        };

        if (isEdit)
        {
            _repository.UpdateContentEntry(entry);
        }
        else
        {
            _repository.AddContentEntry(entry);
            if (normalizedCultureGroupScopeId.HasValue)
            {
                _repository.AssignContentToCultureGroup(normalizedCultureGroupScopeId.Value, entry.Id);
            }
            else if (entry.Type is not ContentType.Culture and not ContentType.CultureGroup)
            {
                _repository.AssignContentToCountry(input.CountryId, entry.Id);
            }
        }

        var contentName = entry.Type switch
        {
            ContentType.Reform => "reform",
            ContentType.DefaultEstateRename => "estate rename",
            ContentType.CustomEstate => "custom estate",
            ContentType.Privilege => "privilege",
            ContentType.Law => "law",
            ContentType.Value => "value",
            ContentType.Building => "building",
            ContentType.Event => "event",
            ContentType.Situation => "situation",
            ContentType.Culture => "culture",
            ContentType.CultureGroup => "culture group",
            _ => "advance"
        };
        TempData["Message"] = isEdit
            ? $"Updated {contentName} {entry.Name}."
            : $"Added {contentName} {entry.Name}.";
        return RedirectToAction(nameof(Index), new
        {
            countryId = entry.Type is ContentType.Culture or ContentType.CultureGroup
                ? selectedCountry?.Id
                : input.CountryId,
            cultureId = entry.Type == ContentType.Culture ? (Guid?)entry.Id : null,
            cultureGroupId = normalizedCultureGroupScopeId ?? (entry.Type == ContentType.CultureGroup ? (Guid?)entry.Id : null),
            contentId = entry.Type is ContentType.Culture or ContentType.CultureGroup ? (Guid?)null : entry.Id,
            library = GetLibraryForContent(entry.Type, normalizedCultureGroupScopeId)
        });
    }

    private IActionResult SaveBuff(BuffInputModel input, bool isEdit, Guid? countryId, Guid? cultureId, Guid? cultureGroupId, Guid? contentId, string? library, bool jsonResponse = false)
    {
        var data = _repository.GetData();
        var availableBuffs = data.Buffs.ToDictionary(buff => buff.Id, buff => buff);
        var buffEffects = BuildEffects(
            input.Effects,
            ValueEffectSide.Default,
            availableBuffs,
            allowBuffEffects: false);
        var existingBuff = isEdit && input.Id.HasValue
            ? data.Buffs.FirstOrDefault(buff => buff.Id == input.Id.Value)
            : null;
        var invalidBuff = !ModelState.IsValid
            || string.IsNullOrWhiteSpace(input.Name)
            || buffEffects.Count == 0
            || HasInvalidBuffEffects(input.Effects, availableBuffs, allowBuffEffects: false)
            || (isEdit && (input.Id is null || existingBuff is null));

        if (invalidBuff)
        {
            if (jsonResponse)
            {
                return BadRequest(new { ok = false, message = isEdit ? "Buff could not be updated." : "Buff could not be added." });
            }

            TempData["Message"] = isEdit ? "Buff could not be updated." : "Buff could not be added.";
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        var buff = new Buff
        {
            Id = existingBuff?.Id ?? Guid.NewGuid(),
            Name = input.Name.Trim(),
            Effects = buffEffects
        };

        if (isEdit)
        {
            _repository.UpdateBuff(buff);
            if (jsonResponse)
            {
                return Json(new { ok = true, message = $"Updated buff {buff.Name}.", buff = ToEditBuff(buff) });
            }

            TempData["Message"] = $"Updated buff {buff.Name}.";
        }
        else
        {
            _repository.AddBuff(buff);
            if (jsonResponse)
            {
                return Json(new { ok = true, message = $"Added buff {buff.Name}.", buff = ToEditBuff(buff) });
            }

            TempData["Message"] = $"Added buff {buff.Name}.";
        }

        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteContent(Guid? countryId, Guid contentId, Guid? cultureId, Guid? cultureGroupId, string? library)
    {
        if (TryRejectWriteAccess(countryId))
        {
            return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, contentId, library });
        }

        _repository.DeleteContentEntry(contentId);
        TempData["Message"] = "Content deleted.";
        return RedirectToAction(nameof(Index), new { countryId, cultureId, cultureGroupId, library });
    }

    private static string ResolveActiveLibrary(string? library, Guid? countryId, Guid? cultureId, Guid? cultureGroupId)
    {
        if (string.Equals(library, CultureGroupsLibrary, StringComparison.OrdinalIgnoreCase))
        {
            return CultureGroupsLibrary;
        }

        if (string.Equals(library, CulturesLibrary, StringComparison.OrdinalIgnoreCase))
        {
            return CulturesLibrary;
        }

        if (cultureGroupId.HasValue)
        {
            return CultureGroupsLibrary;
        }

        if (cultureId.HasValue)
        {
            return CulturesLibrary;
        }

        return CountriesLibrary;
    }

    private static string GetLibraryForContent(ContentType type, Guid? cultureGroupId) =>
        cultureGroupId.HasValue || type == ContentType.CultureGroup
            ? CultureGroupsLibrary
            : type == ContentType.Culture
                ? CulturesLibrary
                : CountriesLibrary;

    private bool TryRejectWriteAccess(Guid? countryId)
    {
        if (HasWriteAccess())
        {
            return false;
        }

        TempData["Message"] = _authService.HasAnyAccounts()
            ? "Read-only mode. Sign in to edit the database."
            : "Read-only mode. No active user accounts are configured yet.";
        return true;
    }

    private bool TryRejectUserManagement(Guid? countryId)
    {
        if (_authService.CanManageUsers(User, _environment.IsDevelopment()))
        {
            return false;
        }

        TempData["Message"] = "Only admins can manage user accounts.";
        return true;
    }

    private bool HasWriteAccess() =>
        _authService.CanWrite(User, _environment.IsDevelopment());

    private static bool HasInvalidBuffEffects(
        IEnumerable<AdvanceEffectInputModel>? source,
        IReadOnlyDictionary<Guid, Buff> availableBuffs,
        bool allowBuffEffects = true) =>
        (source ?? [])
            .Any(effect => effect.ValueType == EffectValueType.Buff
                && (!allowBuffEffects
                    || !effect.BuffId.HasValue
                    || !availableBuffs.ContainsKey(effect.BuffId.Value)
                    || (effect.BuffDurationUnit != BuffDurationUnit.UntilEndOfGame && effect.BuffDurationValue <= 0)));

    private static List<ContentEffect> BuildEffects(
        IEnumerable<AdvanceEffectInputModel>? source,
        ValueEffectSide side,
        IReadOnlyDictionary<Guid, Buff> availableBuffs,
        bool allowBuffEffects = true) =>
        (source ?? [])
            .Where(effect =>
                effect.ValueType == EffectValueType.Buff
                    ? effect.BuffId.HasValue
                        && allowBuffEffects
                        && (effect.BuffDurationUnit == BuffDurationUnit.UntilEndOfGame || effect.BuffDurationValue > 0)
                    : !string.IsNullOrWhiteSpace(effect.Label))
            .Select(effect => new ContentEffect
            {
                Label = effect.Label?.Trim() ?? string.Empty,
                ValueType = effect.ValueType,
                NumericAmount = effect.NumericAmount,
                NumericUnit = effect.NumericUnit,
                BoolValue = effect.BoolValue,
                BuffId = effect.BuffId,
                BuffName = effect.BuffId.HasValue && availableBuffs.TryGetValue(effect.BuffId.Value, out var buff)
                    ? buff.Name
                    : effect.BuffName?.Trim() ?? string.Empty,
                BuffDurationValue = effect.BuffDurationValue,
                BuffDurationUnit = effect.BuffDurationUnit,
                Side = side
            })
            .ToList();

    private static AdvanceInputModel BuildDefaultInputModel(Guid countryId, Guid? cultureGroupId) =>
        new()
        {
            CountryId = countryId,
            CultureGroupId = cultureGroupId,
            Type = ContentType.Advance,
            IsMajorReform = false,
            Effects = [new AdvanceEffectInputModel()],
            LeftEffects = [new AdvanceEffectInputModel()],
            RightEffects = [new AdvanceEffectInputModel()],
            SituationStartEffects = [new AdvanceEffectInputModel()],
            SituationMonthlyEffects = [new AdvanceEffectInputModel()],
            SituationEndingEffects = [new AdvanceEffectInputModel()],
            SituationEndedEffects = [new AdvanceEffectInputModel()],
            SituationActions = [new SituationActionInputModel()],
            ConstructionCosts = [new ResourceAmountInputModel()],
            ProductionMethods = [new ProductionMethodInputModel()],
            EventRequirementTreeJson = JsonSerializer.Serialize(CreateDefaultRequirementTree(), EventRequirementJsonOptions),
            EventRequirements = [new EventRequirementInputModel()],
            EventOptions = [new EventOptionInputModel()]
        };

    private static object ToEditPayload(ContentEntry entry) => new
    {
        id = entry.Id,
        type = entry.Type.ToString(),
        name = entry.Type == ContentType.DefaultEstateRename || entry.Type == ContentType.Value ? string.Empty : entry.Name,
        cultureGroupIds = entry.CultureGroupIds,
        cultureGroupMembershipNames = entry.CultureGroupNames.Count == 0
            ? new List<string> { string.Empty }
            : entry.CultureGroupNames.ToList(),
        isMajorReform = entry.IsMajorReform,
        nobilityEstateName = entry.NobilityEstateName,
        burghersEstateName = entry.BurghersEstateName,
        clergyEstateName = entry.ClergyEstateName,
        peasantsEstateName = entry.PeasantsEstateName,
        foodConsumptionPerThousand = entry.FoodConsumptionPerThousand,
        assimilationConversionSpeed = entry.AssimilationConversionSpeed,
        estateClass = entry.EstateClass.ToString(),
        canPromote = entry.CanPromote,
        promotionSpeed = entry.PromotionSpeed,
        migrationSpeed = entry.MigrationSpeed,
        privilegeEstateTarget = entry.PrivilegeEstateTarget.ToString(),
        privilegeCustomEstateName = entry.PrivilegeCustomEstateName,
        satisfactionBonusPercent = entry.SatisfactionBonusPercent,
        estatePowerPercent = entry.EstatePowerPercent,
        lawCategory = entry.LawCategoryName,
        lawSubcategory = entry.LawSubcategoryName,
        lawEstatePreferenceTarget = entry.LawEstatePreferenceTarget.ToString(),
        lawCustomEstateName = entry.LawCustomEstateName,
        valueLeftLabel = entry.ValueLeftLabel,
        valueRightLabel = entry.ValueRightLabel,
        buildingConstructionScope = entry.BuildingConstructionScope.ToString(),
        buildingDucatCost = entry.BuildingDucatCost,
        buildingTimeMonths = entry.BuildingTimeMonths,
        eventDescription = entry.EventDescription,
        eventYearStart = entry.EventYearStart,
        eventYearEnd = entry.EventYearEnd,
        eventTriggerMode = entry.EventTriggerMode.ToString(),
        eventScenarioName = entry.EventScenarioName,
        eventMonthlyChance = entry.EventMonthlyChance,
        situationDescription = entry.SituationDescription,
        situationCanStart = entry.SituationCanStart,
        situationVisible = entry.SituationVisible,
        situationCanEnd = entry.SituationCanEnd,
        situationMonthlySpawnChance = entry.SituationMonthlySpawnChance,
        eventRequirementTree = ToEditRequirementTree(BuildEditRequirementTree(entry.EventRequirements)),
        eventPrerequisiteIds = entry.EventPrerequisites.Select(link => link.RequiredEventId).ToList(),
        eventOptions = entry.EventOptions.Select(ToEditEventOption).ToList(),
        constructionCosts = entry.ConstructionCosts.Select(ToEditResource).ToList(),
        productionMethods = entry.ProductionMethods.Select(ToEditProductionMethod).ToList(),
        effects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.Default).Select(ToEditEffect).ToList(),
        leftEffects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.Left).Select(ToEditEffect).ToList(),
        rightEffects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.Right).Select(ToEditEffect).ToList(),
        situationStartEffects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.SituationOnStart).Select(ToEditEffect).ToList(),
        situationMonthlyEffects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.SituationOnMonthly).Select(ToEditEffect).ToList(),
        situationEndingEffects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.SituationOnEnding).Select(ToEditEffect).ToList(),
        situationEndedEffects = entry.Effects.Where(effect => effect.Side == ValueEffectSide.SituationOnEnded).Select(ToEditEffect).ToList(),
        situationActions = entry.SituationActions.Select(ToEditSituationAction).ToList()
    };

    private static object ToEditEffect(ContentEffect effect) => new
    {
        label = effect.Label,
        displayText = effect.DisplayText,
        valueType = effect.ValueType.ToString(),
        numericAmount = effect.NumericAmount,
        numericUnit = effect.NumericUnit.ToString(),
        boolValue = effect.BoolValue,
        buffId = effect.BuffId,
        buffName = effect.BuffName,
        buffDurationValue = effect.BuffDurationValue,
        buffDurationUnit = effect.BuffDurationUnit.ToString()
    };

    private static object ToEditBuff(Buff buff) => new
    {
        id = buff.Id,
        name = buff.Name,
        effects = buff.Effects.Select(ToEditEffect).ToList()
    };

    private static object ToEditResource(ContentResourceAmount resource) => new
    {
        resourceName = resource.ResourceName,
        amount = resource.Amount
    };

    private static object ToEditProductionMethod(ContentProductionMethod method) => new
    {
        name = method.Name,
        inputs = method.Inputs.Select(ToEditResource).ToList(),
        outputs = method.Outputs.Select(ToEditResource).ToList()
    };

    private static object ToEditEventOption(EventOption option) => new
    {
        text = option.Text,
        effects = option.Effects.Select(ToEditEffect).ToList()
    };

    private static object ToEditSituationAction(SituationAction action) => new
    {
        name = action.Name,
        requirements = action.Requirements,
        cost = action.Cost,
        cooldown = action.Cooldown,
        effects = action.Effects.Select(ToEditEffect).ToList()
    };

    private static object ToEditRequirementTree(object requirementTree) =>
        requirementTree is EventRequirement requirement
            ? new
            {
                id = requirement.Id,
                nodeType = requirement.NodeType.ToString(),
                groupOperator = requirement.GroupOperator.ToString(),
                expression = requirement.Expression,
                children = requirement.Children.Select(child => ToEditRequirementTree(child)).ToList()
            }
            : requirementTree;

    private static List<ContentResourceAmount> BuildResourceAmounts(IEnumerable<ResourceAmountInputModel>? source) =>
        (source ?? [])
            .Where(item => !string.IsNullOrWhiteSpace(item.ResourceName) && item.Amount > 0)
            .Select(item => new ContentResourceAmount
            {
                ResourceName = item.ResourceName!.Trim(),
                Amount = item.Amount
            })
            .ToList();

    private static List<ContentProductionMethod> BuildProductionMethods(IEnumerable<ProductionMethodInputModel>? source) =>
        (source ?? [])
            .Where(method => !string.IsNullOrWhiteSpace(method.Name))
            .Select(method => new ContentProductionMethod
            {
                Name = method.Name!.Trim(),
                Inputs = BuildResourceAmounts(method.Inputs),
                Outputs = BuildResourceAmounts(method.Outputs)
            })
            .Where(method => method.Inputs.Count > 0 || method.Outputs.Count > 0)
            .ToList();

    private static List<EventRequirement> BuildEventRequirements(string? treeJson, IEnumerable<EventRequirementInputModel>? legacySource)
    {
        if (!string.IsNullOrWhiteSpace(treeJson))
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<EventRequirement>(treeJson, EventRequirementJsonOptions);
                var normalized = NormalizeRequirementNode(parsed);
                return normalized.HasMeaningfulContent ? [normalized] : [];
            }
            catch (JsonException)
            {
            }
        }

        var legacyRequirements = (legacySource ?? [])
            .Where(requirement => !string.IsNullOrWhiteSpace(requirement.Expression))
            .Select(requirement => new EventRequirement
            {
                Expression = requirement.Expression!.Trim()
            })
            .ToList();

        if (legacyRequirements.Count == 0)
        {
            return [];
        }

        return
        [
            new EventRequirement
            {
                NodeType = EventRequirementNodeType.Group,
                GroupOperator = EventRequirementGroupOperator.And,
                Children = legacyRequirements
            }
        ];
    }

    private static List<EventOption> BuildEventOptions(IEnumerable<EventOptionInputModel>? source, IReadOnlyDictionary<Guid, Buff> availableBuffs) =>
        (source ?? [])
            .Where(option => !string.IsNullOrWhiteSpace(option.Text))
            .Select(option => new EventOption
            {
                Text = option.Text!.Trim(),
                Effects = BuildEffects(option.Effects, ValueEffectSide.Default, availableBuffs)
            })
            .ToList();

    private static List<string> BuildCultureGroupNames(IEnumerable<string>? typedNames, IEnumerable<Guid>? selectedIds, IReadOnlyList<ContentEntry> availableCultureGroups)
    {
        var names = new List<string>();

        if (typedNames is not null)
        {
            names.AddRange(
                typedNames
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .Select(name => name.Trim())
                    .Where(name => !string.IsNullOrWhiteSpace(name)));
        }

        if (selectedIds is not null)
        {
            names.AddRange(
                availableCultureGroups
                    .Where(group => selectedIds.Contains(group.Id))
                    .Select(group => group.Name));
        }

        return names
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name)
            .ToList();
    }

    private static List<SituationAction> BuildSituationActions(IEnumerable<SituationActionInputModel>? source, IReadOnlyDictionary<Guid, Buff> availableBuffs) =>
        (source ?? [])
            .Where(action => !string.IsNullOrWhiteSpace(action.Name))
            .Select(action => new SituationAction
            {
                Name = action.Name!.Trim(),
                Requirements = action.Requirements?.Trim() ?? string.Empty,
                Cost = action.Cost?.Trim() ?? string.Empty,
                Cooldown = action.Cooldown?.Trim() ?? string.Empty,
                Effects = BuildEffects(action.Effects, ValueEffectSide.Default, availableBuffs)
            })
            .ToList();

    private static EventRequirement CreateDefaultRequirementTree() =>
        new()
        {
            NodeType = EventRequirementNodeType.Group,
            GroupOperator = EventRequirementGroupOperator.And,
            Children =
            [
                new EventRequirement
                {
                    NodeType = EventRequirementNodeType.Condition,
                    Expression = string.Empty
                }
            ]
        };

    private static object BuildEditRequirementTree(IReadOnlyList<EventRequirement> requirements)
    {
        if (requirements.Count == 1)
        {
            return NormalizeRequirementNode(requirements[0]);
        }

        if (requirements.Count > 1)
        {
            return NormalizeRequirementNode(new EventRequirement
            {
                NodeType = EventRequirementNodeType.Group,
                GroupOperator = EventRequirementGroupOperator.And,
                Children = requirements.ToList()
            });
        }

        return CreateDefaultRequirementTree();
    }

    private static EventRequirement NormalizeRequirementNode(EventRequirement? node)
    {
        if (node is null)
        {
            return CreateDefaultRequirementTree();
        }

        if (node.NodeType == EventRequirementNodeType.Group)
        {
            return new EventRequirement
            {
                Id = node.Id == Guid.Empty ? Guid.NewGuid() : node.Id,
                NodeType = EventRequirementNodeType.Group,
                GroupOperator = node.GroupOperator,
                Children = (node.Children ?? [])
                    .Select(NormalizeRequirementNode)
                    .ToList()
            };
        }

        return new EventRequirement
        {
            Id = node.Id == Guid.Empty ? Guid.NewGuid() : node.Id,
            NodeType = EventRequirementNodeType.Condition,
            Expression = node.Expression?.Trim() ?? string.Empty
        };
    }
}
