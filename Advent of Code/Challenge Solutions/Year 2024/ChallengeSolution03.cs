// Task: https://adventofcode.com/2024/day/3

using Advent_of_Code.Utilities;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public partial class ChallengeSolution03(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        var lines = ReadMemory();
        var muls = MulRegex()
            .Matches(lines)
            .Select(match => match.Value)
            .ToList();

        var sum = 0;
        foreach (var mul in muls)
        {
            var (x, y) = ConvertMul(mul);
            sum += x * y;
        }

        _console.WriteLine($"Multiplication sum: {sum}");
    }

    public override void SolveSecondPart()
    {
        var lines = ReadMemory();
        var instructions = InstructionRegex()
            .Matches(lines)
            .Select(match => match.Value)
            .ToList();

        var doMultiplication = true;
        var sum = 0;

        foreach (var instruction in instructions)
        {
            if (instruction == "do()")
            {
                doMultiplication = true;
                continue;
            }

            if (instruction == "don't()")
            {
                doMultiplication = false;
                continue;
            }

            if (doMultiplication)
            {
                var (x, y) = ConvertMul(instruction);
                sum += x * y;
            }
        }

        _console.WriteLine($"Multiplication sum: {sum}");
    }

    private static (int, int) ConvertMul(string mul)
    {
        var leftParenthesis = mul.IndexOf('(');
        var rightParenthesis = mul.IndexOf(')');
        var values = mul[(leftParenthesis + 1)..rightParenthesis]
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        return (values[0], values[1]);
    }

    public string ReadMemory()
    {
        return string.Join("", Reader.ReadLines(this));
    }

    [GeneratedRegex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)")]
    private static partial Regex InstructionRegex();

    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();
}
