using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CosmosHackathon.Domain;

namespace CosmosHackathon.ReadingGenerator.Persisters
{
    public class CosmosReadingPersister : IReadingPersister
    {
        public Task Persist(DeviceReading reading)
        {
            throw new NotImplementedException();
        }

        public Task Persist(IEnumerable<DeviceReading> readings)
        {
            throw new NotImplementedException();
        }
    }
}
