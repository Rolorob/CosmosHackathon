using CosmosHackathon.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosHackathon.ReadingGenerator
{
    public interface IReadingPersister
    {
        Task Persist(DeviceReading reading);

        Task Persist(IEnumerable<DeviceReading> readings);
    }
}
