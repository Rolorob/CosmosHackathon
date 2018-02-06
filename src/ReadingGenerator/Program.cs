using CosmosHackathon.ReadingGenerator.Generators;
using CosmosHackathon.ReadingGenerator.Persisters;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CosmosHackathon.ReadingGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintInstructions();
                return;
            }

            var generatorString = args[0];
            var persisterString = args[1];

            DateTime start = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime end = DateTime.UtcNow;
            TimeSpan readingOffset = TimeSpan.FromMinutes(15);

            IReadingGenerator generator;
            IReadingPersister persister;
            try
            {
                generator = GetGenerator(generatorString);
                persister = GetPersister(persisterString);
            }
            catch (NotImplementedException)
            { return; }

            var runTask = Run(generator, persister, start, end, readingOffset);

            Task.WaitAll(runTask);

            Console.WriteLine("Done!");
        }

        private static async Task Run(IReadingGenerator generator, IReadingPersister persister, DateTime start, DateTime end, TimeSpan readingOffset)
        {
            string deviceId = Guid.NewGuid().ToString();
            var readings = generator.GenerateReadings(deviceId, start, end, readingOffset);

            await persister.Persist(readings)
                .ConfigureAwait(false);
        }

        private static IReadingGenerator GetGenerator(string generator)
        {
            switch (generator.ToLowerInvariant())
            {
                case "leveringstanden":
                    return new LeveringStandenGenerator(initialValue: 0, minIncrement: 3, maxIncrement: 20);
                case "leveringstandengaps":
                    return new LeveringStandenWithGapsGenerator(initialValue: 0, minIncrement: 3, maxIncrement: 20, minGapSize: 2, maxGapSize: 10);
                case "kwmax":
                    return new KwMaxGenerator(initialValue: 50, minValue: 0, maxValue: 120, maxDifference: 8);
                case "leveringverbruiken":
                    return new LeveringVerbruikenGenerator(initialValue: 4, minValue: 0, maxDifference: 2, maxValue: 12);
                default:
                    PrintInvalidGenerator(generator);
                    PrintInstructions();
                    throw new NotImplementedException();
            }
        }

        private static IReadingPersister GetPersister(string persister)
        {
            switch (persister.ToLowerInvariant())
            {
                case "console":
                    return new ConsoleReadingPersister();
                case "cosmosdb":
                    return new CosmosReadingPersister();
                default:
                    PrintInvalidPersister(persister);
                    PrintInstructions();
                    throw new NotImplementedException();
            }
        }

        private static void PrintInvalidGenerator(string generator)
        {
            Console.WriteLine($"Invalid generator '{generator}' provided. (Maybe you should update the switch case in Program.cs)");
            Console.WriteLine();
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Syntax: ReadingGenerator.exe <generator> <persister>");
            Console.WriteLine();
            Console.WriteLine("Example: ReadingGenerator.exe <LeveringStanden> <Console>");
        }

        private static void PrintInvalidPersister(string persister)
        {
            Console.WriteLine($"Invalid persister '{persister}' provided. (Maybe you should update the switch case in Program.cs)");
            Console.WriteLine();
        }
    }
}
