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
            arrangementSum += GetFittingArrangementCount(conditions, groupSizes);
        }

        return arrangementSum;
    }

    private static long GetFittingArrangementCount(string arrangement, int[] groupSizes)
    {
        List<bool> arrangementFitness = new();
        GetFittingArrangementCount(arrangement, groupSizes, -1, 0, 0, arrangementFitness);
        return arrangementFitness.Count(isFit => isFit);

        static void GetFittingArrangementCount(string arrangement, int[] groupSizes, int groupCount, int groupSize, int index, List<bool> result)
        {
            if (index == arrangement.Length)
            {
                if (groupCount == groupSizes.Length - 1
                    && (groupSize == 0 || groupSize == groupSizes[groupCount]))
                {
                    result.Add(true);
                }

                return;
            }

            if (arrangement[index] == UNKNOWN)
            {
                var newArrangement = arrangement.ToCharArray();
                newArrangement[index] = OPERATIONAL;
                if (groupSize == 0 || (groupCount < groupSizes.Length && groupSize == groupSizes[groupCount]))
                {
                    GetFittingArrangementCount(new(newArrangement), groupSizes, groupCount, 0, index + 1, result);
                }

                newArrangement[index] = DAMAGED;
                GetFittingArrangementCount(new(newArrangement), groupSizes, groupSize == 0 ? groupCount + 1 : groupCount, groupSize + 1, index + 1, result);
            }
            else
            {
                if (arrangement[index] == DAMAGED)
                {
                    groupCount = groupSize++ == 0 ? groupCount + 1 : groupCount;
                }
                else if (groupSize > 0)
                {
                    if (groupCount >= groupSizes.Length || groupSize != groupSizes[groupCount])
                    {
                        return;
                    }

                    groupSize = 0;
                }

                GetFittingArrangementCount(arrangement, groupSizes, groupCount, groupSize, index + 1, result);
            }
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
