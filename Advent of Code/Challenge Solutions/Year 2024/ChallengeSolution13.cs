// Task: https://adventofcode.com/2024/day/13

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution13(IConsole console, ISolutionReader<ChallengeSolution13> reader)
    : ChallengeSolution<ChallengeSolution13>(console, reader)
{
    private const int ACost = 3;
    private const int BCost = 1;
    private const int MaxPresses = 100;

    public override void SolveFirstPart()
    {
        var machines = ReadMachines();
        var totalTokens = ComputeTotalTokens(machines, limitPresses: true);

        Console.WriteLine($"Tokens: {totalTokens}");
    }

    public override void SolveSecondPart()
    {
        var prizeAddition = 10000000000000L;

        var machines = ReadMachines(prizeAddition);
        var totalTokens = ComputeTotalTokens(machines, limitPresses: false);

        Console.WriteLine($"Tokens: {totalTokens}");
    }

    private static long ComputeTotalTokens(List<Machine> machines, bool limitPresses)
    {
        long totalTokens = 0;
        foreach (var machine in machines)
        {
            var (aPresses, bPresses) = FindMinimalPressesRequired(machine);

            if (limitPresses && (aPresses > MaxPresses || bPresses > MaxPresses))
            {
                continue;
            }

            if ((aPresses * machine.A.X) + (bPresses * machine.B.X) != machine.Prize.X
                || (aPresses * machine.A.Y) + (bPresses * machine.B.Y) != machine.Prize.Y)
            {
                continue;
            }


            totalTokens += (aPresses * ACost) + (bPresses * BCost);
        }

        return totalTokens;
    }

    private static (long aPresses, long bPresses) FindMinimalPressesRequired(Machine machine)
    {
        long[] equationX = [machine.A.X, machine.B.X, machine.Prize.X];
        long[] equationY = [machine.A.Y, machine.B.Y, machine.Prize.Y];

        var (aPresses, bPresses) = PerformCramersRule(equationX, equationY);

        return (aPresses, bPresses);
    }

    private static (long aPresses, long bPresses) PerformCramersRule(long[] equationX, long[] equationY)
    {
        var determinant = (equationX[0] * equationY[1]) - (equationY[0] * equationX[1]);

        var determinantA = (equationX[2] * equationY[1]) - (equationY[2] * equationX[1]);
        var determinantB = (equationX[0] * equationY[2]) - (equationY[0] * equationX[2]);

        var aPresses = determinantA / determinant;
        var bPresses = determinantB / determinant;

        return (aPresses, bPresses);
    }

    private List<Machine> ReadMachines(long prizeAddition = 0)
    {
        List<Machine> machines = [];
        var lines = Reader.ReadLines();

        for (var i = 0; i < lines.Length; i++)
        {
            var pointA = ExtractButton(lines[i++]);
            var pointB = ExtractButton(lines[i++]);
            var prize = ExtractPrize(lines[i++], prizeAddition);

            Machine machine = new(pointA, pointB, prize);
            machines.Add(machine);
        }

        return machines;
    }

    private static Point ExtractPrize(string input, long prizeAddition = 0)
    {
        return ExtractPoint(input, '=', prizeAddition);
    }

    private static Point ExtractButton(string input)
    {
        return ExtractPoint(input, '+');
    }

    private static Point ExtractPoint(string input, char separator, long prizeAddition = 0)
    {
        var first = input.IndexOf(separator);
        var second = input.IndexOf(separator, first + 1);

        return new Point(
            prizeAddition + int.Parse(input[(first + 1)..input.IndexOf(',')]),
            prizeAddition + int.Parse(input[(second + 1)..])
        );
    }

    private record Machine(Point A, Point B, Point Prize);

    private record struct Point(long X, long Y);
}