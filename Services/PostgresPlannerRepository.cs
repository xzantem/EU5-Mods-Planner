using Eu5ModPlanner.Models;
using Npgsql;
using System.Text.Json;

namespace Eu5ModPlanner.Services;

public sealed class PostgresPlannerRepository : IPlannerRepository
{
    private const string EventRequirementJsonPrefix = "json:";
    private static readonly JsonSerializerOptions EventRequirementJsonOptions = new(JsonSerializerDefaults.Web);
    private readonly string _connectionString;

    public PostgresPlannerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PlannerDatabase")
            ?? throw new InvalidOperationException("Connection string 'PlannerDatabase' is missing.");

        EnsureSchema();
    }

    public ModPlannerData GetData()
    {
        var data = new ModPlannerData();
        using var connection = OpenConnection();

        using (var command = new NpgsqlCommand(
                   """
                   select id, name, tag, is_archived
                   from countries
                   order by is_archived, tag, name;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                data.Countries.Add(new Country
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Tag = reader.GetString(2),
                    IsArchived = !reader.IsDBNull(3) && reader.GetBoolean(3)
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, name
                   from buffs
                   order by name;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                data.Buffs.Add(new Buff
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, type, name, is_archived, modifier_key, modifier_amount, modifier_unit, is_major_reform,
                          nobility_estate_name, burghers_estate_name, clergy_estate_name, peasants_estate_name,
                          food_consumption_per_thousand, assimilation_conversion_speed,
                          estate_class, can_promote, promotion_speed, migration_speed,
                          privilege_estate_target, privilege_custom_estate_name, satisfaction_bonus_percent, estate_power_percent,
                          law_category_name, law_subcategory_name, law_estate_preference_target, law_custom_estate_name,
                          value_left_label, value_right_label,
                          building_construction_scope, building_ducat_cost, building_time_months,
                          event_description, event_year_start, event_year_end, event_trigger_mode, event_scenario_name, event_monthly_chance,
                          situation_description, situation_can_start, situation_visible, situation_can_end, situation_monthly_spawn_chance
                   from content_entries
                   order by name;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var entry = new ContentEntry
                {
                    Id = reader.GetGuid(0),
                    Type = (ContentType)reader.GetInt32(1),
                    Name = reader.GetString(2),
                    IsArchived = !reader.IsDBNull(3) && reader.GetBoolean(3),
                    IsMajorReform = !reader.IsDBNull(7) && reader.GetBoolean(7),
                    NobilityEstateName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                    BurghersEstateName = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                    ClergyEstateName = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    PeasantsEstateName = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                    FoodConsumptionPerThousand = reader.IsDBNull(12) ? 0 : decimal.ToInt32(reader.GetDecimal(12)),
                    AssimilationConversionSpeed = reader.IsDBNull(13) ? 0m : reader.GetDecimal(13),
                    EstateClass = reader.IsDBNull(14) ? EstateClass.Upper : (EstateClass)reader.GetInt32(14),
                    CanPromote = !reader.IsDBNull(15) && reader.GetBoolean(15),
                    PromotionSpeed = reader.IsDBNull(16) ? 0m : reader.GetDecimal(16),
                    MigrationSpeed = reader.IsDBNull(17) ? 0m : reader.GetDecimal(17),
                    PrivilegeEstateTarget = reader.IsDBNull(18) ? PrivilegeEstateTarget.Nobles : (PrivilegeEstateTarget)reader.GetInt32(18),
                    PrivilegeCustomEstateName = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                    SatisfactionBonusPercent = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                    EstatePowerPercent = reader.IsDBNull(21) ? 0 : reader.GetInt32(21),
                    LawCategoryName = reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                    LawSubcategoryName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                    LawEstatePreferenceTarget = reader.IsDBNull(24) ? PrivilegeEstateTarget.Nobles : (PrivilegeEstateTarget)reader.GetInt32(24),
                    LawCustomEstateName = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                    ValueLeftLabel = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                    ValueRightLabel = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                    BuildingConstructionScope = reader.IsDBNull(28) ? BuildingConstructionScope.AllLocations : (BuildingConstructionScope)reader.GetInt32(28),
                    BuildingDucatCost = reader.IsDBNull(29) ? 0m : reader.GetDecimal(29),
                    BuildingTimeMonths = reader.IsDBNull(30) ? 0 : reader.GetInt32(30),
                    EventDescription = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                    EventYearStart = reader.IsDBNull(32) ? null : reader.GetInt32(32),
                    EventYearEnd = reader.IsDBNull(33) ? null : reader.GetInt32(33),
                    EventTriggerMode = reader.IsDBNull(34) ? EventTriggerMode.MonthlyChance : (EventTriggerMode)reader.GetInt32(34),
                    EventScenarioName = reader.IsDBNull(35) ? string.Empty : reader.GetString(35),
                    EventMonthlyChance = reader.IsDBNull(36) ? 0m : reader.GetDecimal(36),
                    SituationDescription = reader.IsDBNull(37) ? string.Empty : reader.GetString(37),
                    SituationCanStart = reader.IsDBNull(38) ? string.Empty : reader.GetString(38),
                    SituationVisible = reader.IsDBNull(39) ? string.Empty : reader.GetString(39),
                    SituationCanEnd = reader.IsDBNull(40) ? string.Empty : reader.GetString(40),
                    SituationMonthlySpawnChance = reader.IsDBNull(41) ? null : reader.GetDecimal(41)
                };

                if (!reader.IsDBNull(4))
                {
                    entry.Effects.Add(new ContentEffect
                    {
                        Label = reader.GetString(4),
                        ValueType = EffectValueType.Numeric,
                        NumericAmount = reader.IsDBNull(5) ? 0m : reader.GetDecimal(5),
                        NumericUnit = reader.IsDBNull(6) ? ModifierUnit.Flat : (ModifierUnit)reader.GetInt32(6)
                    });
                }

                data.ContentEntries.Add(entry);
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, content_entry_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id, buff_name, buff_duration_value, buff_duration_unit, value_side, sort_order
                   from content_effects
                   order by content_entry_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry))
                {
                    continue;
                }

                if (entry.Effects.Count == 1 && string.IsNullOrWhiteSpace(entry.Effects[0].Label) == false)
                {
                    var legacy = entry.Effects[0];
                    if (legacy.Label == reader.GetString(2))
                    {
                        entry.Effects.Clear();
                    }
                }

                entry.Effects.Add(ReadEffect(reader, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, buff_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id_ref, buff_name, buff_duration_value, buff_duration_unit, sort_order
                   from buff_effects
                   order by buff_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var buffsById = data.Buffs.ToDictionary(buff => buff.Id);
            while (reader.Read())
            {
                var buffId = reader.GetGuid(1);
                if (!buffsById.TryGetValue(buffId, out var buff))
                {
                    continue;
                }

                buff.Effects.Add(ReadEffect(reader, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, content_entry_id, resource_name, amount, sort_order
                   from content_build_costs
                   order by content_entry_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry))
                {
                    continue;
                }

                entry.ConstructionCosts.Add(new ContentResourceAmount
                {
                    Id = reader.GetGuid(0),
                    ResourceName = reader.GetString(2),
                    Amount = reader.GetDecimal(3)
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, content_entry_id, name, sort_order
                   from content_production_methods
                   order by content_entry_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry))
                {
                    continue;
                }

                entry.ProductionMethods.Add(new ContentProductionMethod
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(2)
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, production_method_id, resource_name, amount, is_output, sort_order
                   from content_production_resources
                   order by production_method_id, is_output, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var methodsById = data.ContentEntries
                .SelectMany(entry => entry.ProductionMethods)
                .ToDictionary(method => method.Id);
            while (reader.Read())
            {
                var methodId = reader.GetGuid(1);
                if (!methodsById.TryGetValue(methodId, out var method))
                {
                    continue;
                }

                var resource = new ContentResourceAmount
                {
                    Id = reader.GetGuid(0),
                    ResourceName = reader.GetString(2),
                    Amount = reader.GetDecimal(3)
                };

                if (!reader.IsDBNull(4) && reader.GetBoolean(4))
                {
                    method.Outputs.Add(resource);
                }
                else
                {
                    method.Inputs.Add(resource);
                }
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, content_entry_id, expression, sort_order
                   from content_event_requirements
                   order by content_entry_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry))
                {
                    continue;
                }

                entry.EventRequirements.Add(ParseStoredEventRequirement(reader.GetGuid(0), reader.GetString(2)));
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, content_entry_id, option_text, sort_order
                   from content_event_options
                   order by content_entry_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry))
                {
                    continue;
                }

                entry.EventOptions.Add(new EventOption
                {
                    Id = reader.GetGuid(0),
                    Text = reader.GetString(2)
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, event_option_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id, buff_name, buff_duration_value, buff_duration_unit, sort_order
                   from content_event_option_effects
                   order by event_option_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var optionsById = data.ContentEntries
                .SelectMany(entry => entry.EventOptions)
                .ToDictionary(option => option.Id);
            while (reader.Read())
            {
                var eventOptionId = reader.GetGuid(1);
                if (!optionsById.TryGetValue(eventOptionId, out var option))
                {
                    continue;
                }

                option.Effects.Add(ReadEffect(reader, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, content_entry_id, name, requirements, cost_text, cooldown_text, sort_order
                   from content_situation_actions
                   order by content_entry_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry))
                {
                    continue;
                }

                entry.SituationActions.Add(new SituationAction
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(2),
                    Requirements = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Cost = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    Cooldown = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select id, situation_action_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id, buff_name, buff_duration_value, buff_duration_unit, sort_order
                   from content_situation_action_effects
                   order by situation_action_id, sort_order, id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var actionsById = data.ContentEntries
                .SelectMany(entry => entry.SituationActions)
                .ToDictionary(action => action.Id);
            while (reader.Read())
            {
                var situationActionId = reader.GetGuid(1);
                if (!actionsById.TryGetValue(situationActionId, out var action))
                {
                    continue;
                }

                action.Effects.Add(ReadEffect(reader, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select content_entry_id, required_content_entry_id, sort_order
                   from content_event_prerequisites
                   order by content_entry_id, sort_order, required_content_entry_id;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var contentEntryId = reader.GetGuid(0);
                var requiredContentEntryId = reader.GetGuid(1);
                if (!entriesById.TryGetValue(contentEntryId, out var entry) || !entriesById.TryGetValue(requiredContentEntryId, out var prerequisiteEntry))
                {
                    continue;
                }

                entry.EventPrerequisites.Add(new EventPrerequisiteLink
                {
                    RequiredEventId = requiredContentEntryId,
                    RequiredEventName = prerequisiteEntry.Name
                });
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select country_id, content_entry_id
                   from country_content_entries;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var countriesById = data.Countries.ToDictionary(country => country.Id);
            while (reader.Read())
            {
                var countryId = reader.GetGuid(0);
                var contentEntryId = reader.GetGuid(1);
                if (countriesById.TryGetValue(countryId, out var country))
                {
                    country.ContentEntryIds.Add(contentEntryId);
                }
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select culture_content_entry_id, culture_group_id
                   from culture_memberships;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var cultureId = reader.GetGuid(0);
                var cultureGroupId = reader.GetGuid(1);
                if (entriesById.TryGetValue(cultureId, out var culture)
                    && entriesById.TryGetValue(cultureGroupId, out var group))
                {
                    culture.CultureGroupIds.Add(cultureGroupId);
                    culture.CultureGroupNames.Add(group.Name);
                }
            }
        }

        using (var command = new NpgsqlCommand(
                   """
                   select culture_group_id, content_entry_id
                   from culture_group_content_entries;
                   """,
                   connection))
        using (var reader = command.ExecuteReader())
        {
            var entriesById = data.ContentEntries.ToDictionary(entry => entry.Id);
            while (reader.Read())
            {
                var cultureGroupId = reader.GetGuid(0);
                var contentEntryId = reader.GetGuid(1);
                if (entriesById.TryGetValue(cultureGroupId, out var group))
                {
                    group.CultureGroupContentEntryIds.Add(contentEntryId);
                }
            }
        }

        return data;
    }

    public Country AddCountry(Country country)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            insert into countries (id, name, tag, is_archived)
            values (@id, @name, @tag, @isArchived);
            """,
            connection);

        command.Parameters.AddWithValue("id", country.Id);
        command.Parameters.AddWithValue("name", country.Name);
        command.Parameters.AddWithValue("tag", country.Tag);
        command.Parameters.AddWithValue("isArchived", country.IsArchived);
        command.ExecuteNonQuery();

        return country;
    }

    public Country UpdateCountry(Country country)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            update countries
            set name = @name,
                tag = @tag,
                is_archived = @isArchived
            where id = @id;
            """,
            connection);

        command.Parameters.AddWithValue("id", country.Id);
        command.Parameters.AddWithValue("name", country.Name);
        command.Parameters.AddWithValue("tag", country.Tag);
        command.Parameters.AddWithValue("isArchived", country.IsArchived);
        command.ExecuteNonQuery();

        return country;
    }

    public bool SetCountryArchived(Guid id, bool isArchived)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            update countries
            set is_archived = @isArchived
            where id = @id;
            """,
            connection);

        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("isArchived", isArchived);
        return command.ExecuteNonQuery() > 0;
    }

    public Buff AddBuff(Buff buff)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        using (var command = new NpgsqlCommand(
                   """
                   insert into buffs (id, name)
                   values (@id, @name);
                   """,
                   connection,
                   transaction))
        {
            command.Parameters.AddWithValue("id", buff.Id);
            command.Parameters.AddWithValue("name", buff.Name);
            command.ExecuteNonQuery();
        }

        InsertBuffEffects(buff, connection, transaction);
        transaction.Commit();
        return buff;
    }

    public Buff UpdateBuff(Buff buff)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        using (var deleteEffectsCommand = new NpgsqlCommand("delete from buff_effects where buff_id = @buffId;", connection, transaction))
        {
            deleteEffectsCommand.Parameters.AddWithValue("buffId", buff.Id);
            deleteEffectsCommand.ExecuteNonQuery();
        }

        using (var command = new NpgsqlCommand(
                   """
                   update buffs
                   set name = @name
                   where id = @id;
                   """,
                   connection,
                   transaction))
        {
            command.Parameters.AddWithValue("id", buff.Id);
            command.Parameters.AddWithValue("name", buff.Name);
            command.ExecuteNonQuery();
        }

        InsertBuffEffects(buff, connection, transaction);
        transaction.Commit();
        return buff;
    }

    public bool DeleteBuff(Guid id)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand("delete from buffs where id = @id;", connection);
        command.Parameters.AddWithValue("id", id);
        return command.ExecuteNonQuery() > 0;
    }

    public ContentEntry AddContentEntry(ContentEntry entry)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        InsertContentEntry(entry, connection, transaction);
        InsertEffects(entry, connection, transaction);
        InsertConstructionCosts(entry, connection, transaction);
        InsertProductionMethods(entry, connection, transaction);
        InsertEventRequirements(entry, connection, transaction);
        InsertEventOptions(entry, connection, transaction);
        InsertEventPrerequisites(entry, connection, transaction);
        InsertSituationActions(entry, connection, transaction);
        InsertCultureMemberships(entry, connection, transaction);

        transaction.Commit();
        return entry;
    }

    public ContentEntry UpdateContentEntry(ContentEntry entry)
    {
        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        using (var deleteEffectsCommand = new NpgsqlCommand("delete from content_effects where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteEffectsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteEffectsCommand.ExecuteNonQuery();
        }

        using (var deleteBuildCostsCommand = new NpgsqlCommand("delete from content_build_costs where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteBuildCostsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteBuildCostsCommand.ExecuteNonQuery();
        }

        using (var deleteProductionMethodsCommand = new NpgsqlCommand("delete from content_production_methods where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteProductionMethodsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteProductionMethodsCommand.ExecuteNonQuery();
        }

        using (var deleteEventRequirementsCommand = new NpgsqlCommand("delete from content_event_requirements where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteEventRequirementsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteEventRequirementsCommand.ExecuteNonQuery();
        }

        using (var deleteEventOptionsCommand = new NpgsqlCommand("delete from content_event_options where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteEventOptionsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteEventOptionsCommand.ExecuteNonQuery();
        }

        using (var deleteCultureMembershipsCommand = new NpgsqlCommand("delete from culture_memberships where culture_content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteCultureMembershipsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteCultureMembershipsCommand.ExecuteNonQuery();
        }

        using (var deleteEventPrerequisitesCommand = new NpgsqlCommand("delete from content_event_prerequisites where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteEventPrerequisitesCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteEventPrerequisitesCommand.ExecuteNonQuery();
        }

        using (var deleteSituationActionsCommand = new NpgsqlCommand("delete from content_situation_actions where content_entry_id = @contentEntryId;", connection, transaction))
        {
            deleteSituationActionsCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
            deleteSituationActionsCommand.ExecuteNonQuery();
        }

        using (var updateCommand = new NpgsqlCommand(
                   """
                   update content_entries
                   set type = @type,
                       name = @name,
                       is_archived = @isArchived,
                       is_major_reform = @isMajorReform,
                       nobility_estate_name = @nobilityEstateName,
                       burghers_estate_name = @burghersEstateName,
                       clergy_estate_name = @clergyEstateName,
                       peasants_estate_name = @peasantsEstateName,
                       food_consumption_per_thousand = @foodConsumptionPerThousand,
                       assimilation_conversion_speed = @assimilationConversionSpeed,
                       estate_class = @estateClass,
                       can_promote = @canPromote,
                       promotion_speed = @promotionSpeed,
                       migration_speed = @migrationSpeed,
                       privilege_estate_target = @privilegeEstateTarget,
                       privilege_custom_estate_name = @privilegeCustomEstateName,
                       satisfaction_bonus_percent = @satisfactionBonusPercent,
                       estate_power_percent = @estatePowerPercent,
                       law_category_name = @lawCategoryName,
                       law_subcategory_name = @lawSubcategoryName,
                       law_estate_preference_target = @lawEstatePreferenceTarget,
                       law_custom_estate_name = @lawCustomEstateName,
                       value_left_label = @valueLeftLabel,
                       value_right_label = @valueRightLabel,
                       building_construction_scope = @buildingConstructionScope,
                       building_ducat_cost = @buildingDucatCost,
                       building_time_months = @buildingTimeMonths,
                       event_description = @eventDescription,
                       event_year_start = @eventYearStart,
                       event_year_end = @eventYearEnd,
                       event_trigger_mode = @eventTriggerMode,
                       event_scenario_name = @eventScenarioName,
                       event_monthly_chance = @eventMonthlyChance,
                       situation_description = @situationDescription,
                       situation_can_start = @situationCanStart,
                       situation_visible = @situationVisible,
                       situation_can_end = @situationCanEnd,
                       situation_monthly_spawn_chance = @situationMonthlySpawnChance
                    where id = @id;
                   """,
                   connection,
                   transaction))
        {
            AddContentEntryParameters(updateCommand, entry);
            updateCommand.ExecuteNonQuery();
        }

        InsertEffects(entry, connection, transaction);
        InsertConstructionCosts(entry, connection, transaction);
        InsertProductionMethods(entry, connection, transaction);
        InsertEventRequirements(entry, connection, transaction);
        InsertEventOptions(entry, connection, transaction);
        InsertEventPrerequisites(entry, connection, transaction);
        InsertSituationActions(entry, connection, transaction);
        InsertCultureMemberships(entry, connection, transaction);

        transaction.Commit();
        return entry;
    }

    public bool SetContentArchived(Guid id, bool isArchived)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            update content_entries
            set is_archived = @isArchived
            where id = @id;
            """,
            connection);

        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("isArchived", isArchived);
        return command.ExecuteNonQuery() > 0;
    }

    public bool DeleteContentEntry(Guid id)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand("delete from content_entries where id = @id;", connection);
        command.Parameters.AddWithValue("id", id);
        return command.ExecuteNonQuery() > 0;
    }

    public bool AssignContentToCountry(Guid countryId, Guid contentEntryId)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            insert into country_content_entries (country_id, content_entry_id)
            values (@countryId, @contentEntryId)
            on conflict do nothing;
            """,
            connection);

        command.Parameters.AddWithValue("countryId", countryId);
        command.Parameters.AddWithValue("contentEntryId", contentEntryId);
        return command.ExecuteNonQuery() > 0;
    }

    public bool AssignContentToCultureGroup(Guid cultureGroupId, Guid contentEntryId)
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            insert into culture_group_content_entries (culture_group_id, content_entry_id)
            values (@cultureGroupId, @contentEntryId)
            on conflict do nothing;
            """,
            connection);

        command.Parameters.AddWithValue("cultureGroupId", cultureGroupId);
        command.Parameters.AddWithValue("contentEntryId", contentEntryId);
        return command.ExecuteNonQuery() > 0;
    }

    private void EnsureSchema()
    {
        using var connection = OpenConnection();
        using var command = new NpgsqlCommand(
            """
            create table if not exists countries (
                id uuid primary key,
                name varchar(80) not null,
                tag varchar(12) not null,
                is_archived boolean not null default false
            );

            alter table countries add column if not exists name varchar(80);
            alter table countries add column if not exists tag varchar(12);
            alter table countries add column if not exists is_archived boolean not null default false;

            create table if not exists buffs (
                id uuid primary key,
                name varchar(150) not null
            );

            create table if not exists content_entries (
                id uuid primary key,
                type integer not null,
                name varchar(100) not null
            );

            alter table content_entries add column if not exists is_archived boolean not null default false;
            alter table content_entries add column if not exists modifier_key varchar(100);
            alter table content_entries add column if not exists modifier_amount numeric(12,2);
            alter table content_entries add column if not exists modifier_unit integer not null default 0;
            alter table content_entries add column if not exists is_major_reform boolean not null default false;
            alter table content_entries add column if not exists nobility_estate_name varchar(80);
            alter table content_entries add column if not exists burghers_estate_name varchar(80);
            alter table content_entries add column if not exists clergy_estate_name varchar(80);
            alter table content_entries add column if not exists peasants_estate_name varchar(80);
            alter table content_entries add column if not exists food_consumption_per_thousand numeric(12,2) not null default 0;
            alter table content_entries add column if not exists assimilation_speed numeric(5,2) not null default 0;
            alter table content_entries add column if not exists conversion_speed numeric(5,2) not null default 0;
            alter table content_entries add column if not exists assimilation_conversion_speed numeric(5,2) not null default 0;
            alter table content_entries add column if not exists estate_class integer not null default 0;
            alter table content_entries add column if not exists can_promote boolean not null default false;
            alter table content_entries add column if not exists promotion_speed numeric(12,2) not null default 0;
            alter table content_entries add column if not exists migration_speed numeric(12,2) not null default 0;
            alter table content_entries add column if not exists privilege_estate_target integer not null default 0;
            alter table content_entries add column if not exists privilege_custom_estate_name varchar(100);
            alter table content_entries add column if not exists satisfaction_bonus_percent integer not null default 0;
            alter table content_entries add column if not exists estate_power_percent integer not null default 0;
            alter table content_entries add column if not exists law_category_name varchar(100);
            alter table content_entries add column if not exists law_subcategory_name varchar(150);
            alter table content_entries add column if not exists law_estate_preference_target integer not null default 0;
            alter table content_entries add column if not exists law_custom_estate_name varchar(100);
            alter table content_entries add column if not exists value_left_label varchar(100);
            alter table content_entries add column if not exists value_right_label varchar(100);
            alter table content_entries add column if not exists building_construction_scope integer not null default 0;
            alter table content_entries add column if not exists building_ducat_cost numeric(12,2) not null default 0;
            alter table content_entries add column if not exists building_time_months integer not null default 0;
            alter table content_entries add column if not exists event_description varchar(4000);
            alter table content_entries add column if not exists event_year_start integer;
            alter table content_entries add column if not exists event_year_end integer;
            alter table content_entries add column if not exists event_trigger_mode integer not null default 0;
            alter table content_entries add column if not exists event_scenario_name varchar(150);
            alter table content_entries add column if not exists event_monthly_chance numeric(7,2) not null default 0;
            alter table content_entries add column if not exists situation_description varchar(4000);
            alter table content_entries add column if not exists situation_can_start text;
            alter table content_entries add column if not exists situation_visible text;
            alter table content_entries add column if not exists situation_can_end text;
            alter table content_entries add column if not exists situation_monthly_spawn_chance numeric(7,2);

            update content_entries
            set assimilation_conversion_speed = greatest(coalesce(assimilation_speed, 0), coalesce(conversion_speed, 0))
            where assimilation_conversion_speed = 0
              and (coalesce(assimilation_speed, 0) <> 0 or coalesce(conversion_speed, 0) <> 0);

            create table if not exists content_effects (
                id uuid primary key,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                label varchar(120) not null,
                value_type integer not null default 0,
                numeric_amount numeric(12,2),
                numeric_unit integer not null default 0,
                bool_value boolean not null default false,
                buff_id uuid references buffs(id) on delete set null,
                buff_name varchar(150),
                buff_duration_value integer not null default 0,
                buff_duration_unit integer not null default 0,
                value_side integer not null default 0,
                sort_order integer not null default 0
            );

            alter table content_effects add column if not exists value_side integer not null default 0;
            alter table content_effects add column if not exists buff_id uuid references buffs(id) on delete set null;
            alter table content_effects add column if not exists buff_name varchar(150);
            alter table content_effects add column if not exists buff_duration_value integer not null default 0;
            alter table content_effects add column if not exists buff_duration_unit integer not null default 0;

            create table if not exists buff_effects (
                id uuid primary key,
                buff_id uuid not null references buffs(id) on delete cascade,
                label varchar(120) not null,
                value_type integer not null default 0,
                numeric_amount numeric(12,2),
                numeric_unit integer not null default 0,
                bool_value boolean not null default false,
                buff_id_ref uuid references buffs(id) on delete set null,
                buff_name varchar(150),
                buff_duration_value integer not null default 0,
                buff_duration_unit integer not null default 0,
                sort_order integer not null default 0
            );

            create table if not exists country_content_entries (
                country_id uuid not null references countries(id) on delete cascade,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                primary key (country_id, content_entry_id)
            );

            create table if not exists culture_memberships (
                culture_content_entry_id uuid not null references content_entries(id) on delete cascade,
                culture_group_id uuid not null references content_entries(id) on delete cascade,
                primary key (culture_content_entry_id, culture_group_id)
            );

            create table if not exists culture_group_content_entries (
                culture_group_id uuid not null references content_entries(id) on delete cascade,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                primary key (culture_group_id, content_entry_id)
            );

            create table if not exists content_build_costs (
                id uuid primary key,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                resource_name varchar(120) not null,
                amount numeric(12,2) not null,
                sort_order integer not null default 0
            );

            create table if not exists content_production_methods (
                id uuid primary key,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                name varchar(120) not null,
                sort_order integer not null default 0
            );

            create table if not exists content_production_resources (
                id uuid primary key,
                production_method_id uuid not null references content_production_methods(id) on delete cascade,
                resource_name varchar(120) not null,
                amount numeric(12,2) not null,
                is_output boolean not null default false,
                sort_order integer not null default 0
            );

            create table if not exists content_event_requirements (
                id uuid primary key,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                expression varchar(200) not null,
                sort_order integer not null default 0
            );

            create table if not exists content_event_options (
                id uuid primary key,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                option_text varchar(250) not null,
                sort_order integer not null default 0
            );

            create table if not exists content_event_option_effects (
                id uuid primary key,
                event_option_id uuid not null references content_event_options(id) on delete cascade,
                label varchar(120) not null,
                value_type integer not null default 0,
                numeric_amount numeric(12,2),
                numeric_unit integer not null default 0,
                bool_value boolean not null default false,
                buff_id uuid references buffs(id) on delete set null,
                buff_name varchar(150),
                buff_duration_value integer not null default 0,
                buff_duration_unit integer not null default 0,
                sort_order integer not null default 0
            );

            alter table content_event_option_effects add column if not exists buff_id uuid references buffs(id) on delete set null;
            alter table content_event_option_effects add column if not exists buff_name varchar(150);
            alter table content_event_option_effects add column if not exists buff_duration_value integer not null default 0;
            alter table content_event_option_effects add column if not exists buff_duration_unit integer not null default 0;

            create table if not exists content_event_prerequisites (
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                required_content_entry_id uuid not null references content_entries(id) on delete cascade,
                sort_order integer not null default 0,
                primary key (content_entry_id, required_content_entry_id)
            );

            create table if not exists content_situation_actions (
                id uuid primary key,
                content_entry_id uuid not null references content_entries(id) on delete cascade,
                name varchar(150) not null,
                requirements varchar(4000),
                cost_text varchar(500),
                cooldown_text varchar(200),
                sort_order integer not null default 0
            );

            create table if not exists content_situation_action_effects (
                id uuid primary key,
                situation_action_id uuid not null references content_situation_actions(id) on delete cascade,
                label varchar(120) not null,
                value_type integer not null default 0,
                numeric_amount numeric(12,2),
                numeric_unit integer not null default 0,
                bool_value boolean not null default false,
                buff_id uuid references buffs(id) on delete set null,
                buff_name varchar(150),
                buff_duration_value integer not null default 0,
                buff_duration_unit integer not null default 0,
                sort_order integer not null default 0
            );

            alter table content_situation_action_effects add column if not exists buff_id uuid references buffs(id) on delete set null;
            alter table content_situation_action_effects add column if not exists buff_name varchar(150);
            alter table content_situation_action_effects add column if not exists buff_duration_value integer not null default 0;
            alter table content_situation_action_effects add column if not exists buff_duration_unit integer not null default 0;
            """,
            connection);

        command.ExecuteNonQuery();
    }

    private NpgsqlConnection OpenConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private static void InsertContentEntry(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        using var command = new NpgsqlCommand(
            """
            insert into content_entries (
                id, type, name, is_archived, is_major_reform,
                nobility_estate_name, burghers_estate_name, clergy_estate_name, peasants_estate_name,
                food_consumption_per_thousand, assimilation_conversion_speed,
                estate_class, can_promote, promotion_speed, migration_speed,
                privilege_estate_target, privilege_custom_estate_name, satisfaction_bonus_percent, estate_power_percent,
                law_category_name, law_subcategory_name, law_estate_preference_target, law_custom_estate_name,
                value_left_label, value_right_label,
                building_construction_scope, building_ducat_cost, building_time_months,
                event_description, event_year_start, event_year_end, event_trigger_mode, event_scenario_name, event_monthly_chance,
                situation_description, situation_can_start, situation_visible, situation_can_end, situation_monthly_spawn_chance)
            values (
                @id, @type, @name, @isArchived, @isMajorReform,
                @nobilityEstateName, @burghersEstateName, @clergyEstateName, @peasantsEstateName,
                @foodConsumptionPerThousand, @assimilationConversionSpeed,
                @estateClass, @canPromote, @promotionSpeed, @migrationSpeed,
                @privilegeEstateTarget, @privilegeCustomEstateName, @satisfactionBonusPercent, @estatePowerPercent,
                @lawCategoryName, @lawSubcategoryName, @lawEstatePreferenceTarget, @lawCustomEstateName,
                @valueLeftLabel, @valueRightLabel,
                @buildingConstructionScope, @buildingDucatCost, @buildingTimeMonths,
                @eventDescription, @eventYearStart, @eventYearEnd, @eventTriggerMode, @eventScenarioName, @eventMonthlyChance,
                @situationDescription, @situationCanStart, @situationVisible, @situationCanEnd, @situationMonthlySpawnChance);
            """,
            connection,
            transaction);

        AddContentEntryParameters(command, entry);
        command.ExecuteNonQuery();
    }

    private static void InsertEffects(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var index = 0; index < entry.Effects.Count; index++)
        {
            var effect = entry.Effects[index];
            using var command = new NpgsqlCommand(
                """
                insert into content_effects (id, content_entry_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id, buff_name, buff_duration_value, buff_duration_unit, value_side, sort_order)
                values (@id, @contentEntryId, @label, @valueType, @numericAmount, @numericUnit, @boolValue, @buffId, @buffName, @buffDurationValue, @buffDurationUnit, @valueSide, @sortOrder);
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("id", effect.Id);
            command.Parameters.AddWithValue("contentEntryId", entry.Id);
            command.Parameters.AddWithValue("label", effect.Label);
            command.Parameters.AddWithValue("valueType", (int)effect.ValueType);
            command.Parameters.AddWithValue("numericAmount", effect.NumericAmount);
            command.Parameters.AddWithValue("numericUnit", (int)effect.NumericUnit);
            command.Parameters.AddWithValue("boolValue", effect.BoolValue);
            command.Parameters.AddWithValue("buffId", effect.BuffId.HasValue ? effect.BuffId.Value : DBNull.Value);
            command.Parameters.AddWithValue("buffName", ToDbValue(effect.BuffName));
            command.Parameters.AddWithValue("buffDurationValue", effect.BuffDurationValue);
            command.Parameters.AddWithValue("buffDurationUnit", (int)effect.BuffDurationUnit);
            command.Parameters.AddWithValue("valueSide", (int)effect.Side);
            command.Parameters.AddWithValue("sortOrder", index);
            command.ExecuteNonQuery();
        }
    }

    private static void InsertBuffEffects(Buff buff, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var index = 0; index < buff.Effects.Count; index++)
        {
            var effect = buff.Effects[index];
            using var command = new NpgsqlCommand(
                """
                insert into buff_effects (id, buff_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id_ref, buff_name, buff_duration_value, buff_duration_unit, sort_order)
                values (@id, @buffId, @label, @valueType, @numericAmount, @numericUnit, @boolValue, @buffIdRef, @buffName, @buffDurationValue, @buffDurationUnit, @sortOrder);
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("id", effect.Id);
            command.Parameters.AddWithValue("buffId", buff.Id);
            command.Parameters.AddWithValue("label", effect.Label);
            command.Parameters.AddWithValue("valueType", (int)effect.ValueType);
            command.Parameters.AddWithValue("numericAmount", effect.NumericAmount);
            command.Parameters.AddWithValue("numericUnit", (int)effect.NumericUnit);
            command.Parameters.AddWithValue("boolValue", effect.BoolValue);
            command.Parameters.AddWithValue("buffIdRef", effect.BuffId.HasValue ? effect.BuffId.Value : DBNull.Value);
            command.Parameters.AddWithValue("buffName", ToDbValue(effect.BuffName));
            command.Parameters.AddWithValue("buffDurationValue", effect.BuffDurationValue);
            command.Parameters.AddWithValue("buffDurationUnit", (int)effect.BuffDurationUnit);
            command.Parameters.AddWithValue("sortOrder", index);
            command.ExecuteNonQuery();
        }
    }

    private static void InsertConstructionCosts(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var index = 0; index < entry.ConstructionCosts.Count; index++)
        {
            var cost = entry.ConstructionCosts[index];
            using var command = new NpgsqlCommand(
                """
                insert into content_build_costs (id, content_entry_id, resource_name, amount, sort_order)
                values (@id, @contentEntryId, @resourceName, @amount, @sortOrder);
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("id", cost.Id);
            command.Parameters.AddWithValue("contentEntryId", entry.Id);
            command.Parameters.AddWithValue("resourceName", cost.ResourceName);
            command.Parameters.AddWithValue("amount", cost.Amount);
            command.Parameters.AddWithValue("sortOrder", index);
            command.ExecuteNonQuery();
        }
    }

    private static void InsertProductionMethods(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var methodIndex = 0; methodIndex < entry.ProductionMethods.Count; methodIndex++)
        {
            var method = entry.ProductionMethods[methodIndex];
            using (var methodCommand = new NpgsqlCommand(
                       """
                       insert into content_production_methods (id, content_entry_id, name, sort_order)
                       values (@id, @contentEntryId, @name, @sortOrder);
                       """,
                       connection,
                       transaction))
            {
                methodCommand.Parameters.AddWithValue("id", method.Id);
                methodCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
                methodCommand.Parameters.AddWithValue("name", method.Name);
                methodCommand.Parameters.AddWithValue("sortOrder", methodIndex);
                methodCommand.ExecuteNonQuery();
            }

            InsertProductionResources(method, method.Inputs, false, connection, transaction);
            InsertProductionResources(method, method.Outputs, true, connection, transaction);
        }
    }

    private static void InsertProductionResources(ContentProductionMethod method, IReadOnlyList<ContentResourceAmount> resources, bool isOutput, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var resourceIndex = 0; resourceIndex < resources.Count; resourceIndex++)
        {
            var resource = resources[resourceIndex];
            using var command = new NpgsqlCommand(
                """
                insert into content_production_resources (id, production_method_id, resource_name, amount, is_output, sort_order)
                values (@id, @productionMethodId, @resourceName, @amount, @isOutput, @sortOrder);
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("id", resource.Id);
            command.Parameters.AddWithValue("productionMethodId", method.Id);
            command.Parameters.AddWithValue("resourceName", resource.ResourceName);
            command.Parameters.AddWithValue("amount", resource.Amount);
            command.Parameters.AddWithValue("isOutput", isOutput);
            command.Parameters.AddWithValue("sortOrder", resourceIndex);
            command.ExecuteNonQuery();
        }
    }

    private static void InsertEventRequirements(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var index = 0; index < entry.EventRequirements.Count; index++)
        {
            var requirement = entry.EventRequirements[index];
            using var command = new NpgsqlCommand(
                """
                insert into content_event_requirements (id, content_entry_id, expression, sort_order)
                values (@id, @contentEntryId, @expression, @sortOrder);
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("id", requirement.Id);
            command.Parameters.AddWithValue("contentEntryId", entry.Id);
            command.Parameters.AddWithValue("expression", SerializeStoredEventRequirement(requirement));
            command.Parameters.AddWithValue("sortOrder", index);
            command.ExecuteNonQuery();
        }
    }

    private static EventRequirement ParseStoredEventRequirement(Guid id, string storedValue)
    {
        if (storedValue.StartsWith(EventRequirementJsonPrefix, StringComparison.Ordinal))
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<EventRequirement>(
                    storedValue[EventRequirementJsonPrefix.Length..],
                    EventRequirementJsonOptions);

                return NormalizeStoredRequirement(parsed, id);
            }
            catch (JsonException)
            {
            }
        }

        return new EventRequirement
        {
            Id = id,
            NodeType = EventRequirementNodeType.Condition,
            Expression = storedValue
        };
    }

    private static EventRequirement NormalizeStoredRequirement(EventRequirement? requirement, Guid fallbackId)
    {
        if (requirement is null)
        {
            return new EventRequirement
            {
                Id = fallbackId,
                NodeType = EventRequirementNodeType.Condition,
                Expression = string.Empty
            };
        }

        if (requirement.NodeType == EventRequirementNodeType.Group)
        {
            return new EventRequirement
            {
                Id = requirement.Id == Guid.Empty ? fallbackId : requirement.Id,
                NodeType = EventRequirementNodeType.Group,
                GroupOperator = requirement.GroupOperator,
                Children = (requirement.Children ?? [])
                    .Select(child => NormalizeStoredRequirement(child, child.Id == Guid.Empty ? Guid.NewGuid() : child.Id))
                    .ToList()
            };
        }

        return new EventRequirement
        {
            Id = requirement.Id == Guid.Empty ? fallbackId : requirement.Id,
            NodeType = EventRequirementNodeType.Condition,
            Expression = requirement.Expression ?? string.Empty
        };
    }

    private static string SerializeStoredEventRequirement(EventRequirement requirement)
    {
        if (requirement.NodeType == EventRequirementNodeType.Condition && requirement.Children.Count == 0)
        {
            return requirement.Expression;
        }

        return $"{EventRequirementJsonPrefix}{JsonSerializer.Serialize(requirement, EventRequirementJsonOptions)}";
    }

    private static void InsertEventOptions(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var optionIndex = 0; optionIndex < entry.EventOptions.Count; optionIndex++)
        {
            var option = entry.EventOptions[optionIndex];
            using (var optionCommand = new NpgsqlCommand(
                       """
                       insert into content_event_options (id, content_entry_id, option_text, sort_order)
                       values (@id, @contentEntryId, @optionText, @sortOrder);
                       """,
                       connection,
                       transaction))
            {
                optionCommand.Parameters.AddWithValue("id", option.Id);
                optionCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
                optionCommand.Parameters.AddWithValue("optionText", option.Text);
                optionCommand.Parameters.AddWithValue("sortOrder", optionIndex);
                optionCommand.ExecuteNonQuery();
            }

            for (var effectIndex = 0; effectIndex < option.Effects.Count; effectIndex++)
            {
                var effect = option.Effects[effectIndex];
                using var effectCommand = new NpgsqlCommand(
                    """
                    insert into content_event_option_effects (id, event_option_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id, buff_name, buff_duration_value, buff_duration_unit, sort_order)
                    values (@id, @eventOptionId, @label, @valueType, @numericAmount, @numericUnit, @boolValue, @buffId, @buffName, @buffDurationValue, @buffDurationUnit, @sortOrder);
                    """,
                    connection,
                    transaction);

                effectCommand.Parameters.AddWithValue("id", effect.Id);
                effectCommand.Parameters.AddWithValue("eventOptionId", option.Id);
                effectCommand.Parameters.AddWithValue("label", effect.Label);
                effectCommand.Parameters.AddWithValue("valueType", (int)effect.ValueType);
                effectCommand.Parameters.AddWithValue("numericAmount", effect.NumericAmount);
                effectCommand.Parameters.AddWithValue("numericUnit", (int)effect.NumericUnit);
                effectCommand.Parameters.AddWithValue("boolValue", effect.BoolValue);
                effectCommand.Parameters.AddWithValue("buffId", effect.BuffId.HasValue ? effect.BuffId.Value : DBNull.Value);
                effectCommand.Parameters.AddWithValue("buffName", ToDbValue(effect.BuffName));
                effectCommand.Parameters.AddWithValue("buffDurationValue", effect.BuffDurationValue);
                effectCommand.Parameters.AddWithValue("buffDurationUnit", (int)effect.BuffDurationUnit);
                effectCommand.Parameters.AddWithValue("sortOrder", effectIndex);
                effectCommand.ExecuteNonQuery();
            }
        }
    }

    private static void InsertEventPrerequisites(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var index = 0; index < entry.EventPrerequisites.Count; index++)
        {
            var prerequisite = entry.EventPrerequisites[index];
            using var command = new NpgsqlCommand(
                """
                insert into content_event_prerequisites (content_entry_id, required_content_entry_id, sort_order)
                values (@contentEntryId, @requiredContentEntryId, @sortOrder)
                on conflict do nothing;
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("contentEntryId", entry.Id);
            command.Parameters.AddWithValue("requiredContentEntryId", prerequisite.RequiredEventId);
            command.Parameters.AddWithValue("sortOrder", index);
            command.ExecuteNonQuery();
        }
    }

    private static void InsertSituationActions(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var actionIndex = 0; actionIndex < entry.SituationActions.Count; actionIndex++)
        {
            var action = entry.SituationActions[actionIndex];
            using (var actionCommand = new NpgsqlCommand(
                       """
                       insert into content_situation_actions (id, content_entry_id, name, requirements, cost_text, cooldown_text, sort_order)
                       values (@id, @contentEntryId, @name, @requirements, @costText, @cooldownText, @sortOrder);
                       """,
                       connection,
                       transaction))
            {
                actionCommand.Parameters.AddWithValue("id", action.Id);
                actionCommand.Parameters.AddWithValue("contentEntryId", entry.Id);
                actionCommand.Parameters.AddWithValue("name", action.Name);
                actionCommand.Parameters.AddWithValue("requirements", ToDbValue(action.Requirements));
                actionCommand.Parameters.AddWithValue("costText", ToDbValue(action.Cost));
                actionCommand.Parameters.AddWithValue("cooldownText", ToDbValue(action.Cooldown));
                actionCommand.Parameters.AddWithValue("sortOrder", actionIndex);
                actionCommand.ExecuteNonQuery();
            }

            for (var effectIndex = 0; effectIndex < action.Effects.Count; effectIndex++)
            {
                var effect = action.Effects[effectIndex];
                using var effectCommand = new NpgsqlCommand(
                    """
                    insert into content_situation_action_effects (id, situation_action_id, label, value_type, numeric_amount, numeric_unit, bool_value, buff_id, buff_name, buff_duration_value, buff_duration_unit, sort_order)
                    values (@id, @situationActionId, @label, @valueType, @numericAmount, @numericUnit, @boolValue, @buffId, @buffName, @buffDurationValue, @buffDurationUnit, @sortOrder);
                    """,
                    connection,
                    transaction);

                effectCommand.Parameters.AddWithValue("id", effect.Id);
                effectCommand.Parameters.AddWithValue("situationActionId", action.Id);
                effectCommand.Parameters.AddWithValue("label", effect.Label);
                effectCommand.Parameters.AddWithValue("valueType", (int)effect.ValueType);
                effectCommand.Parameters.AddWithValue("numericAmount", effect.NumericAmount);
                effectCommand.Parameters.AddWithValue("numericUnit", (int)effect.NumericUnit);
                effectCommand.Parameters.AddWithValue("boolValue", effect.BoolValue);
                effectCommand.Parameters.AddWithValue("buffId", effect.BuffId.HasValue ? effect.BuffId.Value : DBNull.Value);
                effectCommand.Parameters.AddWithValue("buffName", ToDbValue(effect.BuffName));
                effectCommand.Parameters.AddWithValue("buffDurationValue", effect.BuffDurationValue);
                effectCommand.Parameters.AddWithValue("buffDurationUnit", (int)effect.BuffDurationUnit);
                effectCommand.Parameters.AddWithValue("sortOrder", effectIndex);
                effectCommand.ExecuteNonQuery();
            }
        }
    }

    private static void InsertCultureMemberships(ContentEntry entry, NpgsqlConnection connection, NpgsqlTransaction transaction)
    {
        for (var index = 0; index < entry.CultureGroupIds.Count; index++)
        {
            using var command = new NpgsqlCommand(
                """
                insert into culture_memberships (culture_content_entry_id, culture_group_id)
                values (@cultureContentEntryId, @cultureGroupId)
                on conflict do nothing;
                """,
                connection,
                transaction);

            command.Parameters.AddWithValue("cultureContentEntryId", entry.Id);
            command.Parameters.AddWithValue("cultureGroupId", entry.CultureGroupIds[index]);
            command.ExecuteNonQuery();
        }
    }

    private static ContentEffect ReadEffect(NpgsqlDataReader reader, int idOrdinal, int labelOrdinal, int valueTypeOrdinal, int numericAmountOrdinal, int numericUnitOrdinal, int boolValueOrdinal, int buffIdOrdinal, int buffNameOrdinal, int buffDurationValueOrdinal, int buffDurationUnitOrdinal, int? sideOrdinal = null) =>
        new()
        {
            Id = reader.GetGuid(idOrdinal),
            Label = reader.IsDBNull(labelOrdinal) ? string.Empty : reader.GetString(labelOrdinal),
            ValueType = (EffectValueType)reader.GetInt32(valueTypeOrdinal),
            NumericAmount = reader.IsDBNull(numericAmountOrdinal) ? 0m : reader.GetDecimal(numericAmountOrdinal),
            NumericUnit = reader.IsDBNull(numericUnitOrdinal) ? ModifierUnit.Flat : (ModifierUnit)reader.GetInt32(numericUnitOrdinal),
            BoolValue = !reader.IsDBNull(boolValueOrdinal) && reader.GetBoolean(boolValueOrdinal),
            BuffId = reader.IsDBNull(buffIdOrdinal) ? null : reader.GetGuid(buffIdOrdinal),
            BuffName = reader.IsDBNull(buffNameOrdinal) ? string.Empty : reader.GetString(buffNameOrdinal),
            BuffDurationValue = reader.IsDBNull(buffDurationValueOrdinal) ? 0 : reader.GetInt32(buffDurationValueOrdinal),
            BuffDurationUnit = reader.IsDBNull(buffDurationUnitOrdinal) ? BuffDurationUnit.Days : (BuffDurationUnit)reader.GetInt32(buffDurationUnitOrdinal),
            Side = sideOrdinal.HasValue && !reader.IsDBNull(sideOrdinal.Value) ? (ValueEffectSide)reader.GetInt32(sideOrdinal.Value) : ValueEffectSide.Default
        };

    private static void AddContentEntryParameters(NpgsqlCommand command, ContentEntry entry)
    {
        command.Parameters.AddWithValue("id", entry.Id);
        command.Parameters.AddWithValue("type", (int)entry.Type);
        command.Parameters.AddWithValue("name", entry.Name);
        command.Parameters.AddWithValue("isArchived", entry.IsArchived);
        command.Parameters.AddWithValue("isMajorReform", entry.IsMajorReform);
        command.Parameters.AddWithValue("nobilityEstateName", ToDbValue(entry.NobilityEstateName));
        command.Parameters.AddWithValue("burghersEstateName", ToDbValue(entry.BurghersEstateName));
        command.Parameters.AddWithValue("clergyEstateName", ToDbValue(entry.ClergyEstateName));
        command.Parameters.AddWithValue("peasantsEstateName", ToDbValue(entry.PeasantsEstateName));
        command.Parameters.AddWithValue("foodConsumptionPerThousand", entry.FoodConsumptionPerThousand);
        command.Parameters.AddWithValue("assimilationConversionSpeed", entry.AssimilationConversionSpeed);
        command.Parameters.AddWithValue("estateClass", (int)entry.EstateClass);
        command.Parameters.AddWithValue("canPromote", entry.CanPromote);
        command.Parameters.AddWithValue("promotionSpeed", entry.PromotionSpeed);
        command.Parameters.AddWithValue("migrationSpeed", entry.MigrationSpeed);
        command.Parameters.AddWithValue("privilegeEstateTarget", (int)entry.PrivilegeEstateTarget);
        command.Parameters.AddWithValue("privilegeCustomEstateName", ToDbValue(entry.PrivilegeCustomEstateName));
        command.Parameters.AddWithValue("satisfactionBonusPercent", entry.SatisfactionBonusPercent);
        command.Parameters.AddWithValue("estatePowerPercent", entry.EstatePowerPercent);
        command.Parameters.AddWithValue("lawCategoryName", ToDbValue(entry.LawCategoryName));
        command.Parameters.AddWithValue("lawSubcategoryName", ToDbValue(entry.LawSubcategoryName));
        command.Parameters.AddWithValue("lawEstatePreferenceTarget", (int)entry.LawEstatePreferenceTarget);
        command.Parameters.AddWithValue("lawCustomEstateName", ToDbValue(entry.LawCustomEstateName));
        command.Parameters.AddWithValue("valueLeftLabel", ToDbValue(entry.ValueLeftLabel));
        command.Parameters.AddWithValue("valueRightLabel", ToDbValue(entry.ValueRightLabel));
        command.Parameters.AddWithValue("buildingConstructionScope", (int)entry.BuildingConstructionScope);
        command.Parameters.AddWithValue("buildingDucatCost", entry.BuildingDucatCost);
        command.Parameters.AddWithValue("buildingTimeMonths", entry.BuildingTimeMonths);
        command.Parameters.AddWithValue("eventDescription", ToDbValue(entry.EventDescription));
        command.Parameters.AddWithValue("eventYearStart", entry.EventYearStart.HasValue ? entry.EventYearStart.Value : DBNull.Value);
        command.Parameters.AddWithValue("eventYearEnd", entry.EventYearEnd.HasValue ? entry.EventYearEnd.Value : DBNull.Value);
        command.Parameters.AddWithValue("eventTriggerMode", (int)entry.EventTriggerMode);
        command.Parameters.AddWithValue("eventScenarioName", ToDbValue(entry.EventScenarioName));
        command.Parameters.AddWithValue("eventMonthlyChance", entry.EventMonthlyChance);
        command.Parameters.AddWithValue("situationDescription", ToDbValue(entry.SituationDescription));
        command.Parameters.AddWithValue("situationCanStart", ToDbValue(entry.SituationCanStart));
        command.Parameters.AddWithValue("situationVisible", ToDbValue(entry.SituationVisible));
        command.Parameters.AddWithValue("situationCanEnd", ToDbValue(entry.SituationCanEnd));
        command.Parameters.AddWithValue("situationMonthlySpawnChance", entry.SituationMonthlySpawnChance.HasValue ? entry.SituationMonthlySpawnChance.Value : DBNull.Value);
    }

    private static object ToDbValue(string? value) =>
        string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;
}
