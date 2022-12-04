using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019
{
    internal class ChallengeSolution1 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(ReadModuleMasses()
                .Select(mass => CalculateFuelForMass(mass))
                .Sum());
        }

        public void SolveSecondPart()
        {
            Console.WriteLine(ReadModuleMasses()
                .Select(mass =>
                {
                    var remainingMass = mass;
                    long totalFuel = 0;

                    long fuel;
                    while((fuel = CalculateFuelForMass(remainingMass)) > 0)
                    {
                        totalFuel += fuel;
                        remainingMass = fuel;
                    }

                    return totalFuel;
                })
                .Sum());
        }

        private long CalculateFuelForMass(long moduleMass)
        {
            return moduleMass / 3 - 2;
        }

        private List<long> ReadModuleMasses()
        {
            return File.ReadAllLines(GetFileString(FileType.Input, 2019, 1))
                .Select(massString => Convert.ToInt64(massString))
                .ToList();
        }
    }
}
