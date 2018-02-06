using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CosmosHackathon.Domain;

namespace CosmosHackathon.ReadingGenerator.Persisters
{
    public class ConsoleReadingPersister : IReadingPersister
    {
        public Task Persist(DeviceReading reading)
        {
            PrintReading(reading);
            return Task.CompletedTask;
        }

        public Task Persist(IEnumerable<DeviceReading> readings)
        {
            foreach (var reading in readings)
            {
                PrintReading(reading);
            }

            return Task.CompletedTask;
        }

        private void PrintReading(DeviceReading reading)
        {
            Console.WriteLine($"{reading}");
        }
    }
}
