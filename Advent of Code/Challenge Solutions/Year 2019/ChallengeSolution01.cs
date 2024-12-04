// Task: https://adventofcode.com/2019/day/1

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019;

public class ChallengeSolution01(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        Console.WriteLine(ReadModuleMasses()
            .Select(mass => CalculateFuelForMass(mass))
            .Sum());
    }

    public override void SolveSecondPart()
    {
        Console.WriteLine(ReadModuleMasses()
            .Select(mass =>
            {
                var remainingMass = mass;
                long totalFuel = 0;

                long fuel;
                while ((fuel = CalculateFuelForMass(remainingMass)) > 0)
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
        return Reader
            .ReadLines(this)
            .Select(massString => Convert.ToInt64(massString))
            .ToList();
    }
}
