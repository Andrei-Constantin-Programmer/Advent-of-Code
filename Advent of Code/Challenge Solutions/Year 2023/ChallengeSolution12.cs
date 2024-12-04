// Task: https://adventofcode.com/2023/day/12

using Advent_of_Code.Utilities;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution12(IConsole console) : ChallengeSolution(console)
{
    private const char OPERATIONAL = '.';
    private const char DAMAGED = '#';
    private const char UNKNOWN = '?';

    public override void SolveFirstPart()
    {
        var conditionRecords = ReadConditionRecords();

        _console.WriteLine(GetSumOfFittingArrangements(conditionRecords));
    }

    public override void SolveSecondPart()
    {
        var conditionRecords = ReadConditionRecords(multiplier: 5);

        _console.WriteLine(GetSumOfFittingArrangements(conditionRecords));
    }

    private static long GetSumOfFittingArrangements(List<(string, ImmutableStack<int>)> conditionRecords)
    {
        Dictionary<(string, ImmutableStack<int>), long> memo = new();
        long arrangementSum = 0;

        foreach (var (conditions, groupSizes) in conditionRecords)
        {
            arrangementSum += GetArrangementCount(conditions, groupSizes, memo);
        }

        return arrangementSum;
    }

    private static long GetArrangementCount(string arrangement, ImmutableStack<int> groupSizes, Dictionary<(string, ImmutableStack<int>), long> memo)
    {
        if (!memo.TryGetValue((arrangement, groupSizes), out var count))
        {
            count = arrangement.FirstOrDefault() switch
            {
                OPERATIONAL => GetArrangementCount(arrangement[1..], groupSizes, memo),
                DAMAGED => ProcessDamagedSpring(arrangement, groupSizes, memo),
                UNKNOWN => GetArrangementCount(ReplaceFirstWith(arrangement, OPERATIONAL), groupSizes, memo)
                            + GetArrangementCount(ReplaceFirstWith(arrangement, DAMAGED), groupSizes, memo),

                _ => groupSizes.Any() ? 0 : 1
            };

            memo.Add((arrangement, groupSizes), count);
        }

        return count;

        static long ProcessDamagedSpring(string arrangement, ImmutableStack<int> groupSizes, Dictionary<(string, ImmutableStack<int>), long> memo)
        {
            if (!groupSizes.Any())
            {
                return 0;
            }

            var groupSize = groupSizes.Peek();
            groupSizes = groupSizes.Pop();

            var firstOperationalSpring = arrangement.IndexOf(OPERATIONAL);
            var maybeDamagedSprings = firstOperationalSpring > -1 ? firstOperationalSpring : arrangement.Length;

            if (maybeDamagedSprings < groupSize)
            {
                return 0;
            }
            if (arrangement.Length == groupSize)
            {
                return GetArrangementCount(string.Empty, groupSizes, memo);
            }
            if (arrangement[groupSize] == DAMAGED)
            {
                return 0;
            }

            return GetArrangementCount(arrangement[(groupSize + 1)..], groupSizes, memo);
        }

        static string ReplaceFirstWith(string arrangement, char character) => new StringBuilder()
            .Append(character)
            .Append(arrangement, 1, arrangement.Length - 1)
            .ToString();
    }

    private List<(string, ImmutableStack<int>)> ReadConditionRecords(int multiplier = 1) => Reader.ReadLines(this)
        .Select(line =>
        {
            var elements = line.Split(' ');
            var springConditions = Regex.Replace(
                string.Join(UNKNOWN,
                    Enumerable.Repeat(elements[0], multiplier)),
                $"({Regex.Escape(OPERATIONAL.ToString())})\\\\i+",
                "$1");

            var contiguousDamageGroupSizes = ImmutableStack.CreateRange(string.Join(',',
                Enumerable.Repeat(elements[1], multiplier))
                .Split(',')
                .Select(int.Parse)
                .Reverse());

            return (springConditions, contiguousDamageGroupSizes);
        })
        .ToList();
}
