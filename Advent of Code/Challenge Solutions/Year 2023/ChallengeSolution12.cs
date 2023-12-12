using Advent_of_Code.Utilities;
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
        
        //Console.WriteLine(GetSumOfFittingArrangements(conditionRecords));
    }
    
    private static long GetSumOfFittingArrangements(List<(string, int[])> conditionRecords)
    {
        long arrangementSum = 0;

        foreach (var (conditions, groupSizes) in conditionRecords)
        {
            arrangementSum += GetArrangementCount(conditions, groupSizes);
        }

        return arrangementSum;
    }

    private static long GetArrangementCount(string arrangement, int[] groupSizes)
    {
        return GetArrangementCountInner(arrangement, groupSizes);

        static long GetArrangementCountInner(string arrangement, int[] groupSizes)
        {
            return arrangement.FirstOrDefault() switch
            {
                OPERATIONAL => GetArrangementCountInner(arrangement[1..], groupSizes),
                DAMAGED => ProcessDamagedSpring(arrangement, groupSizes),
                UNKNOWN => GetArrangementCountInner($"{OPERATIONAL}{arrangement[1..]}", groupSizes)
                           + GetArrangementCountInner($"{DAMAGED}{arrangement[1..]}", groupSizes),

                _ => groupSizes.Length == 0 ? 1 : 0
            };
        }

        static long ProcessDamagedSpring(string arrangement, int[] groupSizes)
        {
            if (groupSizes.Length == 0)
            {
                return 0;
            }

            var groupSize = groupSizes[0];
            groupSizes = groupSizes[1..];

            var maybeDamagedSprings = arrangement.TakeWhile(c => c != OPERATIONAL).Count();

            if (maybeDamagedSprings < groupSize)
            {
                return 0;
            }
            if (arrangement.Length == groupSize)
            {
                return GetArrangementCountInner(string.Empty, groupSizes);
            }
            if (arrangement[groupSize] == DAMAGED)
            {
                return 0;
            }

            return GetArrangementCountInner(arrangement[(groupSize + 1)..], groupSizes);
        }
    }

    private List<(string, int[])> ReadConditionRecords(int multiplier = 1) => Reader.ReadLines(this)
        .Select(line =>
        {
            var elements = line.Split(' ');
            var springConditions = Regex.Replace(
                string.Join(UNKNOWN,
                    Enumerable.Repeat(elements[0], multiplier)),
                $"({Regex.Escape(OPERATIONAL.ToString())})\\\\i+",
                "$1");

            var contiguousDamageGroupSizes = string.Join(',',
                Enumerable.Repeat(elements[1], multiplier))
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            return (springConditions, contiguousDamageGroupSizes);
        })
        .ToList();
}
