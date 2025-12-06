// Task: https://adventofcode.com/2025/day/3

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution03(IConsole console, ISolutionReader<ChallengeSolution03> reader)
    : ChallengeSolution<ChallengeSolution03>(console, reader)
{
    private const int BatteryJoltageDigitCount = 12;

    public override void SolveFirstPart()
    {
        var batteries = Reader.ReadLines();
        long outputJoltage = 0;

        foreach (var battery in batteries)
        {
            var (firstDigit, firstDigitPosition) = battery[0..^1]
                .Select((value, index) => (value, index))
                .MaxBy(digit => digit.value);

            var secondDigit = battery[(firstDigitPosition + 1)..].Max();

            var batteryJoltage = long.Parse($"{firstDigit}{secondDigit}");

            outputJoltage += batteryJoltage;
        }

        Console.WriteLine($"Output Joltage: {outputJoltage}");
    }

    public override void SolveSecondPart()
    {
        var batteries = Reader.ReadLines();
        long outputJoltage = 0;

        foreach (var battery in batteries)
        {
            outputJoltage += ComputeBatteryJoltage(battery, BatteryJoltageDigitCount);
        }

        Console.WriteLine($"Output Joltage: {outputJoltage}");
    }

    private static long ComputeBatteryJoltage(string battery, int digitCount)
    {
        Stack<char> joltageDigits = [];

        for (var i = 0; i < battery.Length; i++)
        {
            var digit = battery[i];

            while (CanPopDigitFromJoltage(joltageDigits, battery.Length, digitCount, i)
                && joltageDigits.Peek() < digit)
            {
                _ = joltageDigits.Pop();
            }

            if (joltageDigits.Count < digitCount)
            {
                joltageDigits.Push(digit);
            }
        }

        return long.Parse(string.Concat(joltageDigits.Reverse()));
    }

    private static bool CanPopDigitFromJoltage(Stack<char> stack, int batteryLength, int digitCount, int currentPosition)
        => stack.Count > Math.Max(0, digitCount + currentPosition - batteryLength);
}
