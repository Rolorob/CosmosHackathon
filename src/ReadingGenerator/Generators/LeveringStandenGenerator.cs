using System;
using System.Collections.Generic;
using CosmosHackathon.Domain;

namespace CosmosHackathon.ReadingGenerator.Generators
{
    public class LeveringStandenGenerator : IReadingGenerator
    {
        public const string LeveringStandenChannel = "elektra/stand/levering";
        public const string LeveringStandenUnit = "kWh";
        private readonly decimal _initialValue;
        private readonly int _minIncrement;
        private readonly int _maxIncrement;
        private readonly IDictionary<string, string> _properties;
        protected readonly Random _random;

        public LeveringStandenGenerator(decimal initialValue, int minIncrement, int maxIncrement, IDictionary<string, string> properties = null, int? randomSeed = null)
        {
            _initialValue = initialValue;
            _minIncrement = minIncrement;
            _maxIncrement = maxIncrement;
            _properties = properties ?? new Dictionary<string, string>();
            _random = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random();
        }

        public virtual IEnumerable<DeviceReading> GenerateReadings(string deviceId, DateTime start, DateTime end, TimeSpan readingOffset)
        {
            var readingTimestamp = start;
            var value = _initialValue;

            while (readingTimestamp < end)
            {
                yield return new DeviceReading(deviceId, readingTimestamp, value, LeveringStandenChannel, LeveringStandenUnit);

                value += GenerateNextIncrement();
                readingTimestamp = readingTimestamp.Add(readingOffset);
            }
        }

        private decimal GenerateNextIncrement()
        {
            var nextIncrement =(decimal) _random.Next(_minIncrement, _maxIncrement - 1);
            var decimals = ((decimal) _random.Next(0, 100)) / 100;
            return nextIncrement + decimals;
        }
    }
}
