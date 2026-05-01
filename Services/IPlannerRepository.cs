using Eu5ModPlanner.Models;

namespace Eu5ModPlanner.Services;

public interface IPlannerRepository
{
    ModPlannerData GetData();
    Country AddCountry(Country country);
    ContentEntry AddContentEntry(ContentEntry entry);
    ContentEntry UpdateContentEntry(ContentEntry entry);
    bool DeleteContentEntry(Guid id);
    bool AssignContentToCountry(Guid countryId, Guid contentEntryId);
}
