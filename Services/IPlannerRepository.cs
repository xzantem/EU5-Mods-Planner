using Eu5ModPlanner.Models;

namespace Eu5ModPlanner.Services;

public interface IPlannerRepository
{
    ModPlannerData GetData();
    Country AddCountry(Country country);
    Country UpdateCountry(Country country);
    bool SetCountryArchived(Guid id, bool isArchived);
    ContentEntry AddContentEntry(ContentEntry entry);
    ContentEntry UpdateContentEntry(ContentEntry entry);
    bool DeleteContentEntry(Guid id);
    bool AssignContentToCountry(Guid countryId, Guid contentEntryId);
}
