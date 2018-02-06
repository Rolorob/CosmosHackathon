using System;
using System.Collections.Generic;
using System.Linq;
using CosmosHackathon.Domain;

namespace CosmosHackathon.ReadingGenerator.Generators
{
    public class LeveringStandenWithGapsGenerator : LeveringStandenGenerator
    {
        private readonly int _minGapSize;
        private readonly int _maxGapSize;

        /// <summary>
        /// This generator generates data via it's base, but at random will create gaps in the data.
        /// </summary>
        public LeveringStandenWithGapsGenerator(decimal initialValue, int minIncrement, int maxIncrement, int minGapSize, int maxGapSize, IDictionary<string, string> properties = null, int? randomSeed = null)
            : base(initialValue, minIncrement, maxIncrement, properties, randomSeed)
        {
            _minGapSize = minGapSize;
            _maxGapSize = maxGapSize;
        }

        public override IEnumerable<DeviceReading> GenerateReadings(string deviceId, DateTime start, DateTime end, TimeSpan readingOffset)
        {
            var readings = base.GenerateReadings(deviceId, start, end, readingOffset).ToList();

            var readingIndex = 0;

            var subsetOfReadings = new List<DeviceReading>();
            var amountLeft = readings.Count;

            do
            {
                var take = GetNextAmountToTake(amountLeft);
                amountLeft = amountLeft - take;

                var skip = GetNextAmountToSkip(amountLeft);
                amountLeft = amountLeft - skip;

                subsetOfReadings.AddRange(readings.Skip(readingIndex).Take(take));
                readingIndex = readingIndex + take + skip;
            } while (amountLeft > 0);

            return subsetOfReadings;
        }

        private int GetNextAmountToTake(int amountLeft)
        {
            var amountToTake = _random.Next(30, 120);
            if (amountToTake > amountLeft)
            {
                return amountLeft;
            }
            return amountToTake;
        }

        private int GetNextAmountToSkip(int amountLeft)
        {
            var nextGapSize = _random.Next(_minGapSize, _maxGapSize);
            if (nextGapSize > amountLeft)
            {
                return amountLeft;
            }
            return nextGapSize;
        }
    }
}
