using Advent_of_Code.Utilities;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public partial class ChallengeSolution03 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var lines = ReadMemory();
        var muls = MulRegex()
            .Matches(lines)
            .Select(match => match.Value)
            .ToList();

        var sum = 0;
        foreach (var mul in muls)
        {
            var leftParenthesis = mul.IndexOf('(');
            var rightParenthesis = mul.IndexOf(')');

            var values = mul[(leftParenthesis + 1)..rightParenthesis]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            sum += values[0] * values[1];
        }

        Console.WriteLine($"Multiplication sum: {sum}");
    }

    protected override void SolveSecondPart()
    {

    }

    public string ReadMemory()
    {
        return string.Join("", Reader.ReadLines(this));
    }

    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();
}
