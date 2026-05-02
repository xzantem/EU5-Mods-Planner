using System.ComponentModel.DataAnnotations;

namespace Eu5ModPlanner.Models;

public sealed class PlannerIndexViewModel
{
    public required IReadOnlyList<Country> Countries { get; init; }
    public required IReadOnlyList<Country> ActiveCountries { get; init; }
    public required IReadOnlyList<Country> ArchivedCountries { get; init; }
    public required IReadOnlyList<ContentEntry> SelectedCountryContent { get; init; }
    public required IReadOnlyList<string> EffectLabelSuggestions { get; init; }
    public required CountryInputModel CountryForm { get; init; }
    public required AdvanceInputModel AdvanceForm { get; init; }
    public required AdminLoginInputModel LoginForm { get; init; }
    public bool HasWriteAccess { get; init; }
    public bool IsLoginConfigured { get; init; }
    public bool CanManageWriteAccess { get; init; }
    public Country? SelectedCountry { get; init; }
    public ContentEntry? SelectedContent { get; init; }
    public string? SelectedContentPayloadJson { get; init; }
}

public sealed class CountryInputModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(12)]
    public string Tag { get; set; } = string.Empty;

    public bool IsArchived { get; set; }
}

public sealed class AdvanceInputModel
{
    public Guid? ContentId { get; set; }

    [Required]
    public Guid CountryId { get; set; }

    [Required]
    public ContentType Type { get; set; } = ContentType.Advance;

    [StringLength(100)]
    public string? Name { get; set; }

    public bool IsMajorReform { get; set; }

    [StringLength(80)]
    public string? NobilityEstateName { get; set; }

    [StringLength(80)]
    public string? BurghersEstateName { get; set; }

    [StringLength(80)]
    public string? ClergyEstateName { get; set; }

    [StringLength(80)]
    public string? PeasantsEstateName { get; set; }

    [Range(0, 999999)]
    public int FoodConsumptionPerThousand { get; set; }

    [Range(0, 100)]
    public decimal AssimilationConversionSpeed { get; set; }

    [Required]
    public EstateClass EstateClass { get; set; } = EstateClass.Upper;

    public bool CanPromote { get; set; }

    [Range(0, 999999)]
    public decimal PromotionSpeed { get; set; }

    [Range(-999999, 999999)]
    public decimal MigrationSpeed { get; set; }

    [Required]
    public PrivilegeEstateTarget PrivilegeEstateTarget { get; set; } = PrivilegeEstateTarget.Nobles;

    [StringLength(100)]
    public string? PrivilegeCustomEstateName { get; set; }

    [Range(-100, 100)]
    public int SatisfactionBonusPercent { get; set; }

    [Range(-100, 100)]
    public int EstatePowerPercent { get; set; }

    [StringLength(100)]
    public string? LawCategory { get; set; }

    [StringLength(100)]
    public string? LawCustomCategory { get; set; }

    [StringLength(150)]
    public string? LawSubcategory { get; set; }

    [StringLength(150)]
    public string? LawCustomSubcategory { get; set; }

    [Required]
    public PrivilegeEstateTarget LawEstatePreferenceTarget { get; set; } = PrivilegeEstateTarget.Nobles;

    [StringLength(100)]
    public string? LawCustomEstateName { get; set; }

    public List<AdvanceEffectInputModel> Effects { get; set; } = [new()];
    public List<AdvanceEffectInputModel> LeftEffects { get; set; } = [new()];
    public List<AdvanceEffectInputModel> RightEffects { get; set; } = [new()];

    [StringLength(100)]
    public string? ValueLeftLabel { get; set; }

    [StringLength(100)]
    public string? ValueRightLabel { get; set; }

    [Required]
    public BuildingConstructionScope BuildingConstructionScope { get; set; } = BuildingConstructionScope.AllLocations;

    [Range(0, 999999)]
    public decimal BuildingDucatCost { get; set; }

    [Range(0, 999999)]
    public int BuildingTimeMonths { get; set; }

    public List<ResourceAmountInputModel> ConstructionCosts { get; set; } = [new()];
    public List<ProductionMethodInputModel> ProductionMethods { get; set; } = [new()];

    [StringLength(4000)]
    public string? EventDescription { get; set; }

    [Range(0, 9999)]
    public int? EventYearStart { get; set; }

    [Range(0, 9999)]
    public int? EventYearEnd { get; set; }

    [Required]
    public EventTriggerMode EventTriggerMode { get; set; } = EventTriggerMode.MonthlyChance;

    [StringLength(150)]
    public string? EventScenarioName { get; set; }

    [Range(0, 100)]
    public decimal EventMonthlyChance { get; set; }

    public string? EventRequirementTreeJson { get; set; }

    public List<EventRequirementInputModel> EventRequirements { get; set; } = [new()];
    public List<EventOptionInputModel> EventOptions { get; set; } = [new()];
    public List<Guid> EventPrerequisiteIds { get; set; } = [];
}

public sealed class AdvanceEffectInputModel
{
    [StringLength(120)]
    public string? Label { get; set; }

    [Required]
    public EffectValueType ValueType { get; set; } = EffectValueType.Numeric;

    [Range(-999999, 999999)]
    public decimal NumericAmount { get; set; }

    [Required]
    public ModifierUnit NumericUnit { get; set; } = ModifierUnit.Flat;

    public bool BoolValue { get; set; }
}

public sealed class ResourceAmountInputModel
{
    [StringLength(120)]
    public string? ResourceName { get; set; }

    [Range(0, 999999)]
    public decimal Amount { get; set; }
}

public sealed class ProductionMethodInputModel
{
    [StringLength(120)]
    public string? Name { get; set; }

    public List<ResourceAmountInputModel> Inputs { get; set; } = [new()];
    public List<ResourceAmountInputModel> Outputs { get; set; } = [new()];
}

public sealed class EventRequirementInputModel
{
    [StringLength(200)]
    public string? Expression { get; set; }
}

public sealed class EventOptionInputModel
{
    [StringLength(250)]
    public string? Text { get; set; }

    public List<AdvanceEffectInputModel> Effects { get; set; } = [new()];
}

public sealed class AdminLoginInputModel
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
