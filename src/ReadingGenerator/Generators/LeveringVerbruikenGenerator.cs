using System;
using System.Collections.Generic;
using CosmosHackathon.Domain;

namespace CosmosHackathon.ReadingGenerator.Generators
{
    public class LeveringVerbruikenGenerator : IReadingGenerator
    {
        public const string LeveringStandenChannel = "elektra/stand/levering";
        public const string LeveringStandenUnit = "kWh";
        private readonly decimal _initialValue;
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly int _maxDifference;
        private readonly IDictionary<string, string> _properties;
        private readonly Random _random;

        public LeveringVerbruikenGenerator(decimal initialValue, int minValue, int maxValue, int maxDifference, IDictionary<string, string> properties = null, int? randomSeed = null)
        {
            _initialValue = initialValue;
            _minValue = minValue;
            _maxValue = maxValue;
            _maxDifference = maxDifference;
            _properties = properties ?? new Dictionary<string, string>();
            _random = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random();
        }

        public IEnumerable<DeviceReading> GenerateReadings(string deviceId, DateTime start, DateTime end, TimeSpan readingOffset)
        {
            var readingTimestamp = start;
            var value = _initialValue;

            while (readingTimestamp < end)
            {
                yield return new DeviceReading(deviceId, readingTimestamp, value, LeveringStandenChannel, LeveringStandenUnit);

                value = GenerateNextReading(value);
                readingTimestamp = readingTimestamp.Add(readingOffset);
            }
        }

        private decimal GenerateNextReading(decimal currentValue)
        {
            var nextMin = Convert.ToInt32(Decimal.Round(currentValue - _maxDifference, 0));
            var nextMax = Convert.ToInt32(Decimal.Round(currentValue + _maxDifference, 0));

            if (nextMin < 0)
            {
                nextMin = 0;
            }

            if (nextMax > _maxValue)
            {
                nextMax = _maxValue;
            }

            var nextRoundedValue = (decimal)_random.Next(nextMin, nextMax);
            var decimals = ((decimal)_random.Next(0, 100)) / 100;
            return nextRoundedValue + decimals;
        }
    }
}
