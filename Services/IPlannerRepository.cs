using Eu5ModPlanner.Models;

namespace Eu5ModPlanner.Services;

public interface IPlannerRepository
{
    ModPlannerData GetData();
    Country AddCountry(Country country);
    Country UpdateCountry(Country country);
    bool SetCountryArchived(Guid id, bool isArchived);
    Buff AddBuff(Buff buff);
    Buff UpdateBuff(Buff buff);
    bool DeleteBuff(Guid id);
    ContentEntry AddContentEntry(ContentEntry entry);
    ContentEntry UpdateContentEntry(ContentEntry entry);
    bool SetContentArchived(Guid id, bool isArchived);
    bool DeleteContentEntry(Guid id);
    bool AssignContentToCountry(Guid countryId, Guid contentEntryId);
    bool AssignContentToCultureGroup(Guid cultureGroupId, Guid contentEntryId);
}
