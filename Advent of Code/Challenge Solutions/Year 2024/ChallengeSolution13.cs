// Task: https://adventofcode.com/2024/day/13

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution13(IConsole console, ISolutionReader<ChallengeSolution13> reader)
    : ChallengeSolution<ChallengeSolution13>(console, reader)
{
    const int ACost = 3;
    const int BCost = 1;
    const int MaxPresses = 100;

    public override void SolveFirstPart()
    {
        var machines = ReadMachines();
        var totalTokens = ComputeTotalTokens(machines);

        _console.WriteLine($"Tokens: {totalTokens}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int ComputeTotalTokens(List<Machine> machines)
    {
        var totalTokens = 0;
        foreach (var machine in machines)
        {
            var (aPresses, bPresses) = FindMinimalPressesRequired(machine);

            if (aPresses > MaxPresses
                || bPresses > MaxPresses)
            {
                continue;
            }

            totalTokens += (aPresses * ACost) + (bPresses * BCost);
        }

        return totalTokens;
    }

    private static (int aPresses, int bPresses) FindMinimalPressesRequired(Machine machine)
    {
        var aPresses = 0;
        var bPresses = 0;
        var minCost = int.MaxValue;

        for (var a = 0; a * machine.A.X <= machine.Prize.X; a++)
        {
            var remainingX = machine.Prize.X - (a * machine.A.X);
            if (remainingX % machine.B.X == 0)
            {
                var bX = remainingX / machine.B.X;

                var totalY = (a * machine.A.Y) + (bX * machine.B.Y);
                if (totalY == machine.Prize.Y)
                {
                    var currentCost = (a * ACost) + (bX * BCost);

                    if (currentCost < minCost)
                    {
                        aPresses = a;
                        bPresses = bX;
                        minCost = currentCost;
                    }
                }
            }
        }

        return (aPresses, bPresses);
    }

    private static (int gcd, int aCoefficient, int bCoefficient) GreatestCommonDivisorWithCoefficients(int a, int b)
    {
        if (b == 0)
        {
            return (a, 1, 0);
        }

        var (gcd, aCoeff, bCoeff) = GreatestCommonDivisorWithCoefficients(b, a % b);
        var x = bCoeff;
        var y = aCoeff - (a / b * bCoeff);

        return (gcd, x, y);
    }


    private List<Machine> ReadMachines()
    {
        List<Machine> machines = [];
        var lines = _reader.ReadLines();

        for (var i = 0; i < lines.Length; i++)
        {
            var pointA = ExtractPoint(lines[i++]);
            var pointB = ExtractPoint(lines[i++]);
            var prize = ExtractPoint(lines[i++]);

            Machine machine = new(pointA, pointB, prize);
            machines.Add(machine);
        }

        return machines;
    }

    private static Point ExtractPoint(string input)
    {
        var xPlus = input.IndexOf('+');

        if (xPlus >= 0)
        {
            var yPlus = input.IndexOf('+', xPlus + 1);

            return new Point(
                int.Parse(input[(xPlus + 1)..input.IndexOf(',')]),
                int.Parse(input[(yPlus + 1)..])
            );
        }

        var xEqual = input.IndexOf('=');
        var yEqual = input.IndexOf('=', xEqual + 1);

        return new Point(
                int.Parse(input[(xEqual + 1)..input.IndexOf(',')]),
                int.Parse(input[(yEqual + 1)..])
            );
    }

    private record Machine(Point A, Point B, Point Prize);

    private record struct Point(int X, int Y);
}