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
        
        // Console.WriteLine(GetSumOfFittingArrangements(conditionRecords));
    }
    
    private static long GetSumOfFittingArrangements(List<(string, int[])> conditionRecords)
    {
        var arrangementSum = 0;

        foreach (var (conditions, groupSizes) in conditionRecords)
        {
            var fittingArrangements = GenerateFittingArrangements(conditions, groupSizes);
            arrangementSum += fittingArrangements.Count;
        }

        return arrangementSum;
    }

    private static List<string> GenerateFittingArrangements(string conditions, int[] groupSizes)
    {
        List<string> result = new();
        GenerateArrangements(conditions, groupSizes, -1, 0, 0, result);
        return result;

        static void GenerateArrangements(string conditions, int[] groupSizes, int groupCount, int groupSize, int index, List<string> result)
        {
            if (index == conditions.Length)
            {
                if (groupCount == groupSizes.Length - 1
                    && (groupSize == 0 || groupSize == groupSizes[groupCount]))
                {
                    result.Add(conditions);
                }

                return;
            }

            if (conditions[index] == UNKNOWN)
            {
                var newConditions = conditions.ToCharArray();
                newConditions[index] = OPERATIONAL;
                if (groupSize == 0 || (groupCount < groupSizes.Length && groupSize == groupSizes[groupCount]))
                {
                    GenerateArrangements(new(newConditions), groupSizes, groupCount, 0, index + 1, result);
                }

                newConditions[index] = DAMAGED;
                GenerateArrangements(new(newConditions), groupSizes, groupSize == 0 ? groupCount + 1 : groupCount, groupSize + 1, index + 1, result);
            }
            else
            {
                if (conditions[index] == DAMAGED)
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

                GenerateArrangements(conditions, groupSizes, groupCount, groupSize, index + 1, result);
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
