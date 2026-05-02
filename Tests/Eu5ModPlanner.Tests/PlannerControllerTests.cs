using Eu5ModPlanner.Controllers;
using Eu5ModPlanner.Models;
using Eu5ModPlanner.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Xunit;

namespace Eu5ModPlanner.Tests;

public sealed class PlannerControllerTests
{
    [Fact]
    public void Index_InDevelopment_EnablesWriteAccessWithoutLoginControls()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.Index(null, null);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PlannerIndexViewModel>(view.Model);
        Assert.True(model.HasWriteAccess);
        Assert.False(model.CanManageWriteAccess);
    }

    [Fact]
    public void Index_SplitsActiveAndArchivedCountries()
    {
        var repository = new InMemoryPlannerRepository();
        repository.Data.Countries.Add(new Country { Name = "Wallonia", Tag = "WAL" });
        repository.Data.Countries.Add(new Country { Name = "Archivedonia", Tag = "ARC", IsArchived = true });
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.Index(null, null);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PlannerIndexViewModel>(view.Model);
        Assert.Single(model.ActiveCountries);
        Assert.Single(model.ArchivedCountries);
    }

    [Fact]
    public void AddCountry_InDevelopment_AddsCountryAndRedirects()
    {
        var repository = new InMemoryPlannerRepository();
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.AddCountry(new CountryInputModel
        {
            Name = "Wallonia",
            Tag = "wal"
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        var country = Assert.Single(repository.Data.Countries);
        Assert.Equal("Wallonia", country.Name);
        Assert.Equal("WAL", country.Tag);
        Assert.False(country.IsArchived);
        Assert.Equal("Added country WAL.", controller.TempData["Message"]);
    }

    [Fact]
    public void AddCountry_InProductionWithoutWriteAccess_RejectsWrite()
    {
        var repository = new InMemoryPlannerRepository();
        var controller = CreateController(repository, isDevelopment: false);

        var result = controller.AddCountry(new CountryInputModel
        {
            Name = "Wallonia",
            Tag = "WAL"
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        Assert.Empty(repository.Data.Countries);
        Assert.Equal("Read-only mode. Sign in to edit the database.", controller.TempData["Message"]);
    }

    [Fact]
    public void UpdateCountry_UpdatesExistingCountryDetails()
    {
        var repository = CreateRepositoryWithCountry();
        var existing = repository.Data.Countries[0];
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.UpdateCountry(new CountryInputModel
        {
            Id = existing.Id,
            Name = "Commonwealth of Wallonia",
            Tag = "cwl"
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        Assert.Equal(existing.Id, redirect.RouteValues?["countryId"]);
        Assert.Equal("Commonwealth of Wallonia", existing.Name);
        Assert.Equal("CWL", existing.Tag);
        Assert.Equal("Updated country CWL.", controller.TempData["Message"]);
    }

    [Fact]
    public void ArchiveAndRestoreCountry_TogglesArchivedFlag()
    {
        var repository = CreateRepositoryWithCountry();
        var existing = repository.Data.Countries[0];
        var controller = CreateController(repository, isDevelopment: true);

        var archiveResult = controller.ArchiveCountry(existing.Id);
        var archiveRedirect = Assert.IsType<RedirectToActionResult>(archiveResult);
        Assert.Equal(nameof(PlannerController.Index), archiveRedirect.ActionName);
        Assert.True(existing.IsArchived);
        Assert.Equal("Country archived.", controller.TempData["Message"]);

        var restoreResult = controller.RestoreCountry(existing.Id);
        var restoreRedirect = Assert.IsType<RedirectToActionResult>(restoreResult);
        Assert.Equal(nameof(PlannerController.Index), restoreRedirect.ActionName);
        Assert.False(existing.IsArchived);
        Assert.Equal("Country restored.", controller.TempData["Message"]);
    }

    [Fact]
    public void AddContent_Advance_SavesEffectAndAssignsCountry()
    {
        var repository = CreateRepositoryWithCountry();
        var country = repository.Data.Countries[0];
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.AddContent(new AdvanceInputModel
        {
            CountryId = country.Id,
            Type = ContentType.Advance,
            Name = "Centralized Arsenal",
            Effects =
            [
                NumericEffect("Research Speed", 5, ModifierUnit.Percent)
            ]
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        var entry = Assert.Single(repository.Data.ContentEntries);
        Assert.Equal(ContentType.Advance, entry.Type);
        Assert.Equal("Centralized Arsenal", entry.Name);
        Assert.Single(entry.Effects);
        Assert.Equal("Research Speed", entry.Effects[0].Label);
        Assert.Contains(entry.Id, country.ContentEntryIds);
    }

    [Fact]
    public void AddContent_Reform_SavesMajorReformFlag()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.Reform,
            Name = "Royal Tax Code",
            IsMajorReform = true,
            Effects = [NumericEffect("Tax Income", 10, ModifierUnit.Percent)]
        });

        var entry = Assert.Single(repository.Data.ContentEntries);
        Assert.Equal(ContentType.Reform, entry.Type);
        Assert.True(entry.IsMajorReform);
    }

    [Fact]
    public void AddContent_DefaultEstateRename_SavesEstateNames()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.DefaultEstateRename,
            NobilityEstateName = "Swordlords",
            BurghersEstateName = "Guildmasters"
        });

        var entry = Assert.Single(repository.Data.ContentEntries);
        Assert.Equal(ContentType.DefaultEstateRename, entry.Type);
        Assert.Equal("Default Estate Rename", entry.Name);
        Assert.Equal("Swordlords", entry.NobilityEstateName);
        Assert.Equal("Guildmasters", entry.BurghersEstateName);
        Assert.Empty(entry.Effects);
    }

    [Fact]
    public void AddContent_CustomEstate_SavesEstateStatistics()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.CustomEstate,
            Name = "Frontier Clans",
            FoodConsumptionPerThousand = 12,
            AssimilationConversionSpeed = 25,
            EstateClass = EstateClass.Lower,
            CanPromote = true,
            PromotionSpeed = 15,
            MigrationSpeed = 8
        });

        var entry = Assert.Single(repository.Data.ContentEntries);
        Assert.Equal(ContentType.CustomEstate, entry.Type);
        Assert.Equal("Frontier Clans", entry.Name);
        Assert.Equal(12, entry.FoodConsumptionPerThousand);
        Assert.Equal(25, entry.AssimilationConversionSpeed);
        Assert.Equal(EstateClass.Lower, entry.EstateClass);
        Assert.True(entry.CanPromote);
        Assert.Equal(15, entry.PromotionSpeed);
        Assert.Equal(8, entry.MigrationSpeed);
    }

    [Fact]
    public void AddContent_Privilege_WithCustomEstate_SavesPrivilegeFields()
    {
        var repository = CreateRepositoryWithCountryAndCustomEstate(out var country, out _);
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = country.Id,
            Type = ContentType.Privilege,
            Name = "Merchant Charters",
            PrivilegeEstateTarget = PrivilegeEstateTarget.Custom,
            PrivilegeCustomEstateName = "Frontier Clans",
            SatisfactionBonusPercent = 10,
            EstatePowerPercent = -5,
            Effects = [NumericEffect("Trade Efficiency", 8, ModifierUnit.Percent)]
        });

        var entry = repository.Data.ContentEntries.Single(item => item.Type == ContentType.Privilege);
        Assert.Equal("Frontier Clans", entry.PrivilegeCustomEstateName);
        Assert.Equal(10, entry.SatisfactionBonusPercent);
        Assert.Equal(-5, entry.EstatePowerPercent);
    }

    [Fact]
    public void AddContent_Law_WithCustomCategoryAndEstate_SavesLawFields()
    {
        var repository = CreateRepositoryWithCountryAndCustomEstate(out var country, out _);
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = country.Id,
            Type = ContentType.Law,
            Name = "Guild Harbor Ordinance",
            LawCategory = "Custom",
            LawCustomCategory = "Commercial Traditions",
            LawSubcategory = "Custom",
            LawCustomSubcategory = "Harbor Ordinances",
            LawEstatePreferenceTarget = PrivilegeEstateTarget.Custom,
            LawCustomEstateName = "Frontier Clans",
            Effects = [NumericEffect("Naval Morale", 3, ModifierUnit.Flat)]
        });

        var entry = repository.Data.ContentEntries.Single(item => item.Type == ContentType.Law);
        Assert.Equal("Commercial Traditions", entry.LawCategoryName);
        Assert.Equal("Harbor Ordinances", entry.LawSubcategoryName);
        Assert.Equal("Frontier Clans", entry.LawCustomEstateName);
    }

    [Fact]
    public void AddContent_Value_SavesLeftAndRightEffects()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.Value,
            ValueLeftLabel = "Spiritual",
            ValueRightLabel = "Humanist",
            LeftEffects = [NumericEffect("Clergy Loyalty", 15, ModifierUnit.Percent)],
            RightEffects = [NumericEffect("Innovation Gain", 10, ModifierUnit.Percent)]
        });

        var entry = repository.Data.ContentEntries.Single(item => item.Type == ContentType.Value);
        Assert.Equal("Spiritual vs Humanist", entry.Name);
        Assert.Single(entry.LeftEffects);
        Assert.Single(entry.RightEffects);
    }

    [Fact]
    public void AddContent_Building_SavesCostsProductionMethodsAndEffects()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.Building,
            Name = "State Granary",
            BuildingConstructionScope = BuildingConstructionScope.TownOnly,
            BuildingDucatCost = 150,
            BuildingTimeMonths = 12,
            ConstructionCosts =
            [
                new ResourceAmountInputModel { ResourceName = "Wood", Amount = 20 },
                new ResourceAmountInputModel { ResourceName = "Stone", Amount = 10 }
            ],
            ProductionMethods =
            [
                new ProductionMethodInputModel
                {
                    Name = "Grain Storage",
                    Inputs = [new ResourceAmountInputModel { ResourceName = "Grain", Amount = 2 }],
                    Outputs = [new ResourceAmountInputModel { ResourceName = "Supplies", Amount = 1 }]
                }
            ],
            Effects = [NumericEffect("Local Food Output", 10, ModifierUnit.Percent)]
        });

        var entry = repository.Data.ContentEntries.Single(item => item.Type == ContentType.Building);
        Assert.Equal(BuildingConstructionScope.TownOnly, entry.BuildingConstructionScope);
        Assert.Equal(150, entry.BuildingDucatCost);
        Assert.Equal(12, entry.BuildingTimeMonths);
        Assert.Equal(2, entry.ConstructionCosts.Count);
        Assert.Single(entry.ProductionMethods);
        Assert.Single(entry.Effects);
    }

    [Fact]
    public void AddContent_Event_SavesRequirementsOptionsAndPrerequisites()
    {
        var repository = CreateRepositoryWithCountryAndEvent(out var country, out var prerequisiteEvent);
        var controller = CreateController(repository, isDevelopment: true);

        controller.AddContent(new AdvanceInputModel
        {
            CountryId = country.Id,
            Type = ContentType.Event,
            Name = "The City Falls",
            EventDescription = "The city walls finally give way.",
            EventTriggerMode = EventTriggerMode.MonthlyChance,
            EventMonthlyChance = 5,
            EventYearStart = 1500,
            EventYearEnd = 1520,
            EventRequirements =
            [
                new EventRequirementInputModel
                {
                    Expression = "religion = religion:catholic"
                }
            ],
            EventPrerequisiteIds = [prerequisiteEvent.Id],
            EventOptions =
            [
                new EventOptionInputModel
                {
                    Text = "Seize the treasury",
                    Effects = [NumericEffect("Treasury", 100, ModifierUnit.Flat)]
                }
            ]
        });

        var entry = repository.Data.ContentEntries.Single(item => item.Type == ContentType.Event && item.Name == "The City Falls");
        Assert.Equal("The city walls finally give way.", entry.EventDescription);
        Assert.Single(entry.EventRequirements);
        Assert.Single(entry.EventOptions);
        Assert.Single(entry.EventPrerequisites);
        Assert.Equal(prerequisiteEvent.Id, entry.EventPrerequisites[0].RequiredEventId);
    }

    [Fact]
    public void EditContent_UpdatesExistingEntry()
    {
        var repository = CreateRepositoryWithCountryAndAdvance(out var country, out var existingEntry);
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.EditContent(new AdvanceInputModel
        {
            ContentId = existingEntry.Id,
            CountryId = country.Id,
            Type = ContentType.Advance,
            Name = "Refined Arsenal",
            Effects = [NumericEffect("Research Speed", 7, ModifierUnit.Percent)]
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        var updatedEntry = repository.Data.ContentEntries.Single(entry => entry.Id == existingEntry.Id);
        Assert.Equal("Refined Arsenal", updatedEntry.Name);
        Assert.Equal(7, updatedEntry.Effects[0].NumericAmount);
        Assert.Equal("Updated advance Refined Arsenal.", controller.TempData["Message"]);
    }

    [Fact]
    public void DeleteContent_RemovesEntryAndCountryAssignment()
    {
        var repository = CreateRepositoryWithCountryAndAdvance(out var country, out var existingEntry);
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.DeleteContent(country.Id, existingEntry.Id);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        Assert.Empty(repository.Data.ContentEntries);
        Assert.DoesNotContain(existingEntry.Id, country.ContentEntryIds);
        Assert.Equal("Content deleted.", controller.TempData["Message"]);
    }

    [Fact]
    public void AddContent_InvalidBuilding_IsRejected()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.Building,
            Name = "Broken Granary",
            BuildingDucatCost = 50,
            BuildingTimeMonths = 0,
            Effects = [NumericEffect("Local Food Output", 10, ModifierUnit.Percent)]
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        Assert.Empty(repository.Data.ContentEntries);
        Assert.Equal("Content could not be added.", controller.TempData["Message"]);
    }

    [Fact]
    public void AddContent_InvalidEvent_IsRejected()
    {
        var repository = CreateRepositoryWithCountry();
        var controller = CreateController(repository, isDevelopment: true);

        var result = controller.AddContent(new AdvanceInputModel
        {
            CountryId = repository.Data.Countries[0].Id,
            Type = ContentType.Event,
            Name = "Broken Event",
            EventDescription = "Oops",
            EventTriggerMode = EventTriggerMode.MonthlyChance,
            EventMonthlyChance = 0,
            EventOptions =
            [
                new EventOptionInputModel
                {
                    Text = "Do it",
                    Effects = [NumericEffect("Treasury", 10, ModifierUnit.Flat)]
                }
            ]
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(nameof(PlannerController.Index), redirect.ActionName);
        Assert.Empty(repository.Data.ContentEntries);
        Assert.Equal("Content could not be added.", controller.TempData["Message"]);
    }

    private static PlannerController CreateController(InMemoryPlannerRepository repository, bool isDevelopment)
    {
        var controller = new PlannerController(
            repository,
            Options.Create(new AdminAuthOptions
            {
                Username = "admin",
                Password = "password"
            }),
            new TestWebHostEnvironment(isDevelopment));

        var httpContext = new DefaultHttpContext();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        controller.TempData = new TempDataDictionary(httpContext, new NullTempDataProvider());
        return controller;
    }

    private static InMemoryPlannerRepository CreateRepositoryWithCountry()
    {
        var repository = new InMemoryPlannerRepository();
        repository.Data.Countries.Add(new Country
        {
            Name = "Wallonia",
            Tag = "WAL"
        });

        return repository;
    }

    private static InMemoryPlannerRepository CreateRepositoryWithCountryAndCustomEstate(out Country country, out ContentEntry customEstate)
    {
        var repository = CreateRepositoryWithCountry();
        country = repository.Data.Countries[0];
        customEstate = new ContentEntry
        {
            Type = ContentType.CustomEstate,
            Name = "Frontier Clans"
        };
        repository.AddContentEntry(customEstate);
        repository.AssignContentToCountry(country.Id, customEstate.Id);
        return repository;
    }

    private static InMemoryPlannerRepository CreateRepositoryWithCountryAndEvent(out Country country, out ContentEntry eventEntry)
    {
        var repository = CreateRepositoryWithCountry();
        country = repository.Data.Countries[0];
        eventEntry = new ContentEntry
        {
            Type = ContentType.Event,
            Name = "The Siege Begins",
            EventDescription = "Opening event",
            EventOptions =
            [
                new EventOption
                {
                    Text = "Continue"
                }
            ]
        };
        repository.AddContentEntry(eventEntry);
        repository.AssignContentToCountry(country.Id, eventEntry.Id);
        return repository;
    }

    private static InMemoryPlannerRepository CreateRepositoryWithCountryAndAdvance(out Country country, out ContentEntry advance)
    {
        var repository = CreateRepositoryWithCountry();
        country = repository.Data.Countries[0];
        advance = new ContentEntry
        {
            Type = ContentType.Advance,
            Name = "Centralized Arsenal",
            Effects =
            [
                new ContentEffect
                {
                    Label = "Research Speed",
                    NumericAmount = 5,
                    NumericUnit = ModifierUnit.Percent
                }
            ]
        };
        repository.AddContentEntry(advance);
        repository.AssignContentToCountry(country.Id, advance.Id);
        return repository;
    }

    private static AdvanceEffectInputModel NumericEffect(string label, decimal amount, ModifierUnit unit) =>
        new()
        {
            Label = label,
            ValueType = EffectValueType.Numeric,
            NumericAmount = amount,
            NumericUnit = unit
        };

    private sealed class InMemoryPlannerRepository : IPlannerRepository
    {
        public ModPlannerData Data { get; } = new();

        public ModPlannerData GetData() => Data;

        public Country AddCountry(Country country)
        {
            Data.Countries.Add(country);
            return country;
        }

        public Country UpdateCountry(Country country) => country;

        public bool SetCountryArchived(Guid id, bool isArchived)
        {
            var country = Data.Countries.FirstOrDefault(item => item.Id == id);
            if (country is null)
            {
                return false;
            }

            country.IsArchived = isArchived;
            return true;
        }

        public ContentEntry AddContentEntry(ContentEntry entry)
        {
            Data.ContentEntries.Add(entry);
            return entry;
        }

        public ContentEntry UpdateContentEntry(ContentEntry entry)
        {
            var existing = Data.ContentEntries.First(item => item.Id == entry.Id);
            var index = Data.ContentEntries.IndexOf(existing);
            Data.ContentEntries[index] = entry;
            return entry;
        }

        public bool DeleteContentEntry(Guid id)
        {
            foreach (var country in Data.Countries)
            {
                country.ContentEntryIds.Remove(id);
            }

            return Data.ContentEntries.RemoveAll(entry => entry.Id == id) > 0;
        }

        public bool AssignContentToCountry(Guid countryId, Guid contentEntryId)
        {
            var country = Data.Countries.FirstOrDefault(item => item.Id == countryId);
            if (country is null)
            {
                return false;
            }

            if (!country.ContentEntryIds.Contains(contentEntryId))
            {
                country.ContentEntryIds.Add(contentEntryId);
            }

            return true;
        }
    }

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public TestWebHostEnvironment(bool isDevelopment)
        {
            EnvironmentName = isDevelopment ? "Development" : "Production";
            ApplicationName = "Eu5ModPlanner.Tests";
            WebRootPath = string.Empty;
            WebRootFileProvider = new NullFileProvider();
            ContentRootPath = string.Empty;
            ContentRootFileProvider = new NullFileProvider();
        }

        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string WebRootPath { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }

    private sealed class NullTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
        }
    }
}
