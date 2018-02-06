using CosmosHackathon.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosHackathon.ReadingGenerator.Generators
{
    public interface IReadingGenerator
    {
        IEnumerable<DeviceReading> GenerateReadings(string deviceId, DateTime start, DateTime end, TimeSpan readingOffset);
    }
}
