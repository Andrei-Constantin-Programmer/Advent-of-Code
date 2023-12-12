using Advent_of_Code.Utilities;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution12 : ChallengeSolution
{
    private const char OPERATIONAL = '.';
    private const char DAMAGED = '#';
    private const char UNKNOWN = '?';
        
    protected override void SolveFirstPart()
    {
        var conditionRecords = ReadConditionRecords();
        
        Console.WriteLine(GetSumOfFittingArrangements(conditionRecords));
    }

    protected override void SolveSecondPart()
    {
        var conditionRecords = ReadConditionRecords(multiplier: 5);
        
        Console.WriteLine(GetSumOfFittingArrangements(conditionRecords));
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
                UNKNOWN => GetArrangementCount($"{OPERATIONAL}{arrangement[1..]}", groupSizes, memo)
                            + GetArrangementCount($"{DAMAGED}{arrangement[1..]}", groupSizes, memo),

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

            var maybeDamagedSprings = arrangement
                .TakeWhile(c => c != OPERATIONAL)
                .Count();

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
