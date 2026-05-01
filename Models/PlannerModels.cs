using System.ComponentModel.DataAnnotations;

namespace Eu5ModPlanner.Models;

public enum ContentType
{
    Advance,
    Reform,
    DefaultEstateRename,
    CustomEstate,
    Privilege,
    Law,
    Value,
    Building,
    Event
}

public enum EstateClass
{
    Upper,
    Lower
}

public enum PrivilegeEstateTarget
{
    Nobles,
    Burghers,
    Peasants,
    Clergy,
    Tribes,
    Custom
}

public enum ModifierUnit
{
    Flat,
    Percent
}

public enum EffectValueType
{
    Numeric,
    Boolean,
    Text
}

public enum ValueEffectSide
{
    Default,
    Left,
    Right
}

public enum BuildingConstructionScope
{
    AllLocations,
    RuralOnly,
    TownOnly,
    CapitalOnly
}

public enum EventTriggerMode
{
    MonthlyChance,
    Instant
}

public enum EventRequirementNodeType
{
    Condition,
    Group
}

public enum EventRequirementGroupOperator
{
    And,
    Or
}

public sealed class ModPlannerData
{
    public List<Country> Countries { get; set; } = [];
    public List<ContentEntry> ContentEntries { get; set; } = [];
}

public sealed class Country
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(12)]
    public string Tag { get; set; } = string.Empty;

    public List<Guid> ContentEntryIds { get; set; } = [];
}

public sealed class ContentEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ContentType Type { get; set; } = ContentType.Advance;

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsMajorReform { get; set; }

    [StringLength(80)]
    public string NobilityEstateName { get; set; } = string.Empty;

    [StringLength(80)]
    public string BurghersEstateName { get; set; } = string.Empty;

    [StringLength(80)]
    public string ClergyEstateName { get; set; } = string.Empty;

    [StringLength(80)]
    public string PeasantsEstateName { get; set; } = string.Empty;

    [Range(0, 999999)]
    public int FoodConsumptionPerThousand { get; set; }

    [Range(0, 100)]
    public decimal AssimilationConversionSpeed { get; set; }

    public EstateClass EstateClass { get; set; } = EstateClass.Upper;

    public bool CanPromote { get; set; }

    public decimal PromotionSpeed { get; set; }

    public decimal MigrationSpeed { get; set; }

    public PrivilegeEstateTarget PrivilegeEstateTarget { get; set; } = PrivilegeEstateTarget.Nobles;

    [StringLength(100)]
    public string PrivilegeCustomEstateName { get; set; } = string.Empty;

    public int SatisfactionBonusPercent { get; set; }

    public int EstatePowerPercent { get; set; }

    [StringLength(100)]
    public string LawCategoryName { get; set; } = string.Empty;

    [StringLength(150)]
    public string LawSubcategoryName { get; set; } = string.Empty;

    public PrivilegeEstateTarget LawEstatePreferenceTarget { get; set; } = PrivilegeEstateTarget.Nobles;

    [StringLength(100)]
    public string LawCustomEstateName { get; set; } = string.Empty;

    [StringLength(100)]
    public string ValueLeftLabel { get; set; } = string.Empty;

    [StringLength(100)]
    public string ValueRightLabel { get; set; } = string.Empty;

    public BuildingConstructionScope BuildingConstructionScope { get; set; } = BuildingConstructionScope.AllLocations;

    [Range(0, 999999)]
    public decimal BuildingDucatCost { get; set; }

    [Range(0, 999999)]
    public int BuildingTimeMonths { get; set; }

    [StringLength(4000)]
    public string EventDescription { get; set; } = string.Empty;

    public int? EventYearStart { get; set; }

    public int? EventYearEnd { get; set; }

    public EventTriggerMode EventTriggerMode { get; set; } = EventTriggerMode.MonthlyChance;

    [StringLength(150)]
    public string EventScenarioName { get; set; } = string.Empty;

    public decimal EventMonthlyChance { get; set; }

    public List<ContentEffect> Effects { get; set; } = [];
    public List<ContentResourceAmount> ConstructionCosts { get; set; } = [];
    public List<ContentProductionMethod> ProductionMethods { get; set; } = [];
    public List<EventRequirement> EventRequirements { get; set; } = [];
    public List<EventOption> EventOptions { get; set; } = [];
    public List<EventPrerequisiteLink> EventPrerequisites { get; set; } = [];

    public string PrimaryEffectDisplay => Effects.FirstOrDefault()?.DisplayText ?? string.Empty;

    public string TypeTranslationKey => Type switch
    {
        ContentType.Reform => "type-reform",
        ContentType.DefaultEstateRename => "type-default-estate-rename",
        ContentType.CustomEstate => "type-custom-estate",
        ContentType.Privilege => "type-privilege",
        ContentType.Law => "type-law",
        ContentType.Value => "type-value",
        ContentType.Building => "type-building",
        ContentType.Event => "type-event",
        _ => "type-advance"
    };

    public IReadOnlyList<string> DisplayLines =>
        Type switch
        {
            ContentType.DefaultEstateRename => EstateRenameLines,
            ContentType.CustomEstate => CustomEstateLines,
            ContentType.Privilege => PrivilegeLines,
            ContentType.Law => LawLines,
            ContentType.Value => ValueLines,
            ContentType.Building => BuildingLines,
            ContentType.Event => EventLines,
            _ => Effects.Select(effect => effect.DisplayText).ToList()
        };

    public IReadOnlyList<string> EstateRenameLines =>
        new[]
        {
            BuildEstateLine("Nobility", NobilityEstateName),
            BuildEstateLine("Burghers", BurghersEstateName),
            BuildEstateLine("Clergy", ClergyEstateName),
            BuildEstateLine("Peasants", PeasantsEstateName)
        }
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .ToList();

    public IReadOnlyList<string> CustomEstateLines =>
        new[]
        {
            $"Food Consumption: {FoodConsumptionPerThousand} / 1000 pops",
            $"Assimilation and Conversion Speed: {AssimilationConversionSpeed:0.##}%",
            $"Class: {(EstateClass == EstateClass.Upper ? "Upper Class" : "Lower Class")}",
            $"Can Promote: {(CanPromote ? "true" : "false")}",
            CanPromote ? $"Promotion Speed: {PromotionSpeed:0.##}%" : string.Empty,
            $"Migration Speed: {MigrationSpeed:0.##}%"
        }
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .ToList();

    public IReadOnlyList<string> PrivilegeLines =>
        new[]
        {
            $"Estate: {PrivilegeEstateDisplayName}",
            $"Satisfaction Bonus: {SatisfactionBonusPercent:+0;-0;0}%",
            $"Estate Power: {EstatePowerPercent:+0;-0;0}%"
        }
        .Concat(Effects.Select(effect => effect.DisplayText))
        .ToList();

    public IReadOnlyList<string> LawLines =>
        new[]
        {
            $"Category: {LawCategoryName}",
            $"Subcategory: {LawSubcategoryName}",
            $"Preferred Estate: {LawEstatePreferenceDisplayName}"
        }
        .Concat(Effects.Select(effect => effect.DisplayText))
        .ToList();

    public IReadOnlyList<string> ValueLines =>
        BuildValueLines();

    public IReadOnlyList<string> BuildingLines =>
        new[]
        {
            $"Construction Requirement: {BuildingConstructionScopeDisplayName}",
            $"Build Cost: {BuildingDucatCost:0.##} ducats",
            $"Build Time: {BuildingTimeMonths} months"
        }
        .Concat(ConstructionCosts.Select(cost => $"Construction Cost: {cost.DisplayText}"))
        .Concat(ProductionMethods.SelectMany(method => method.DisplayLines))
        .Concat(Effects.Select(effect => effect.DisplayText))
        .ToList();

    public IReadOnlyList<string> EventLines =>
        BuildEventLines();

    public IReadOnlyList<ContentEffect> LeftEffects =>
        Effects.Where(effect => effect.Side == ValueEffectSide.Left).ToList();

    public IReadOnlyList<ContentEffect> RightEffects =>
        Effects.Where(effect => effect.Side == ValueEffectSide.Right).ToList();

    private IReadOnlyList<string> BuildValueLines()
    {
        var lines = new List<string>();

        if (!string.IsNullOrWhiteSpace(ValueLeftLabel))
        {
            lines.Add(ValueLeftLabel);
        }

        lines.AddRange(LeftEffects.Select(effect => effect.DisplayText));

        if (!string.IsNullOrWhiteSpace(ValueRightLabel))
        {
            lines.Add(ValueRightLabel);
        }

        lines.AddRange(RightEffects.Select(effect => effect.DisplayText));

        return lines;
    }

    private IReadOnlyList<string> BuildEventLines()
    {
        var lines = new List<string>();

        if (!string.IsNullOrWhiteSpace(EventDescription))
        {
            lines.Add($"Description: {EventDescription}");
        }

        if (EventRequirements.Count > 0)
        {
            lines.Add("Requirements");
            lines.AddRange(EventRequirements.SelectMany(requirement => requirement.ToDisplayLines()));
        }

        lines.Add(EventYearStart.HasValue || EventYearEnd.HasValue
            ? $"Year Range: {FormatEventYearRange()}"
            : "Year Range: Any time");

        lines.Add(EventTriggerMode == EventTriggerMode.Instant
            ? "Trigger: Instant"
            : $"Trigger: Monthly chance - {EventMonthlyChance:0.##}%");

        if (EventPrerequisites.Count > 0)
        {
            lines.Add("Previous Events");
            lines.AddRange(EventPrerequisites.Select(link => $"- {link.RequiredEventName}"));
        }

        if (EventOptions.Count > 0)
        {
            lines.Add("Options");
            foreach (var option in EventOptions)
            {
                lines.Add($"Option: {option.Text}");
                if (option.Effects.Count == 0)
                {
                    lines.Add("  No instant effect");
                }
                else
                {
                    lines.AddRange(option.Effects.Select(effect => $"  {effect.DisplayText}"));
                }
            }
        }

        return lines;
    }

    private string FormatEventYearRange()
    {
        if (EventYearStart.HasValue && EventYearEnd.HasValue)
        {
            return $"{EventYearStart.Value} - {EventYearEnd.Value}";
        }

        if (EventYearStart.HasValue)
        {
            return $"{EventYearStart.Value}+";
        }

        return $"Up to {EventYearEnd!.Value}";
    }

    public string PrivilegeEstateDisplayName =>
        PrivilegeEstateTarget == PrivilegeEstateTarget.Custom && !string.IsNullOrWhiteSpace(PrivilegeCustomEstateName)
            ? PrivilegeCustomEstateName
            : GetEstateDisplayName(PrivilegeEstateTarget);

    public string LawEstatePreferenceDisplayName =>
        LawEstatePreferenceTarget == PrivilegeEstateTarget.Custom && !string.IsNullOrWhiteSpace(LawCustomEstateName)
            ? LawCustomEstateName
            : GetEstateDisplayName(LawEstatePreferenceTarget);

    public string BuildingConstructionScopeDisplayName =>
        BuildingConstructionScope switch
        {
            BuildingConstructionScope.RuralOnly => "Rural Only",
            BuildingConstructionScope.TownOnly => "Town Only",
            BuildingConstructionScope.CapitalOnly => "Capital Only",
            _ => "All Locations"
        };

    private static string BuildEstateLine(string baseName, string customName) =>
        string.IsNullOrWhiteSpace(customName) ? string.Empty : $"{baseName} -> {customName}";

    private static string GetEstateDisplayName(PrivilegeEstateTarget estateTarget) =>
        estateTarget switch
        {
            PrivilegeEstateTarget.Nobles => "Nobles",
            PrivilegeEstateTarget.Burghers => "Burghers",
            PrivilegeEstateTarget.Peasants => "Peasants",
            PrivilegeEstateTarget.Clergy => "Clergy",
            PrivilegeEstateTarget.Tribes => "Tribes",
            _ => "Custom Estate"
        };
}

public sealed class ContentEffect
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(120)]
    public string Label { get; set; } = string.Empty;

    public EffectValueType ValueType { get; set; } = EffectValueType.Numeric;

    [Range(-999999, 999999)]
    public decimal NumericAmount { get; set; }

    public ModifierUnit NumericUnit { get; set; } = ModifierUnit.Flat;

    public bool BoolValue { get; set; }

    public ValueEffectSide Side { get; set; } = ValueEffectSide.Default;

    public string DisplayText =>
        ValueType switch
        {
            EffectValueType.Boolean => $"{Label}: {(BoolValue ? "true" : "false")}",
            EffectValueType.Text => Label,
            _ => $"{NumericAmount.ToString("+0.##;-0.##;0")}{(NumericUnit == ModifierUnit.Percent ? "%" : string.Empty)} {Label}"
        };
}

public sealed class ContentResourceAmount
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(120)]
    public string ResourceName { get; set; } = string.Empty;

    [Range(0.01, 999999)]
    public decimal Amount { get; set; }

    public string DisplayText => $"{Amount:0.##} {ResourceName}";
}

public sealed class ContentProductionMethod
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    public List<ContentResourceAmount> Inputs { get; set; } = [];
    public List<ContentResourceAmount> Outputs { get; set; } = [];

    public IReadOnlyList<string> DisplayLines =>
        new[] { $"Production Method: {Name}" }
            .Concat(Inputs.Select(input => $"Monthly Input: {input.DisplayText}"))
            .Concat(Outputs.Select(output => $"Monthly Output: {output.DisplayText}"))
            .ToList();
}

public sealed class EventRequirement
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(200)]
    public string Expression { get; set; } = string.Empty;

    public EventRequirementNodeType NodeType { get; set; } = EventRequirementNodeType.Condition;

    public EventRequirementGroupOperator GroupOperator { get; set; } = EventRequirementGroupOperator.And;

    public List<EventRequirement> Children { get; set; } = [];

    public bool HasMeaningfulContent =>
        NodeType == EventRequirementNodeType.Condition
            ? !string.IsNullOrWhiteSpace(Expression)
            : Children.Any(child => child.HasMeaningfulContent);

    public IReadOnlyList<string> ToDisplayLines(int depth = 0)
    {
        var indent = new string(' ', depth * 2);

        if (NodeType == EventRequirementNodeType.Condition)
        {
            return string.IsNullOrWhiteSpace(Expression)
                ? []
                : [$"{indent}- {Expression}"];
        }

        var lines = new List<string> { $"{indent}{GroupOperator.ToString().ToUpperInvariant()}" };
        foreach (var child in Children)
        {
            lines.AddRange(child.ToDisplayLines(depth + 1));
        }

        return lines;
    }
}

public sealed class EventOption
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, StringLength(250)]
    public string Text { get; set; } = string.Empty;

    public List<ContentEffect> Effects { get; set; } = [];
}

public sealed class EventPrerequisiteLink
{
    public Guid RequiredEventId { get; set; }

    [Required, StringLength(100)]
    public string RequiredEventName { get; set; } = string.Empty;
}
