using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution12 : ChallengeSolution
{
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
    
    private static long GetSumOfFittingArrangements(List<(SpringCondition[], int[])> conditionRecords)
    {
        var arrangementSum = 0;

        foreach (var (conditions, groupSizes) in conditionRecords)
        {
            var fittingArrangements = GenerateFittingArrangements(conditions, groupSizes);
            arrangementSum += fittingArrangements.Count;
        }

        return arrangementSum;
    }

    private static List<SpringCondition[]> GenerateFittingArrangements(SpringCondition[] conditions, int[] groupSizes)
    {
        List<SpringCondition[]> result = new();
        GenerateArrangements(conditions, groupSizes, -1, 0, 0, result);
        return result;

        static void GenerateArrangements(SpringCondition[] conditions, int[] groupSizes, int groupCount, int groupSize, int index, List<SpringCondition[]> result)
        {
            if (index == conditions.Length)
            {
                if (groupCount == groupSizes.Length - 1
                    && (groupSize == 0 || groupSize == groupSizes[groupCount]))
                {
                    result.Add((SpringCondition[])conditions.Clone());
                }

                return;
            }

            if (conditions[index] == SpringCondition.Unknown)
            {
                conditions[index] = SpringCondition.Operational;
                if (groupSize == 0 || (groupCount < groupSizes.Length && groupSize == groupSizes[groupCount]))
                {
                    GenerateArrangements(conditions, groupSizes, groupCount, 0, index + 1, result);
                }

                conditions[index] = SpringCondition.Damaged;
                GenerateArrangements(conditions, groupSizes, groupSize == 0 ? groupCount + 1 : groupCount, groupSize + 1, index + 1, result);

                conditions[index] = SpringCondition.Unknown;
            }
            else
            {
                if (conditions[index] == SpringCondition.Damaged)
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

    private List<(SpringCondition[], int[])> ReadConditionRecords(int multiplier = 1) => Reader.ReadLines(this)
        .Select(line =>
        {
            var elements = line.Split(' ');
            var springConditions = string.Join('?',
                Enumerable.Repeat(elements[0], multiplier))
                .Select(condition => (SpringCondition)condition)
                .ToArray();

            var contiguousDamageGroupSizes = string.Join(',',
                Enumerable.Repeat(elements[1], multiplier))
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            return (springConditions, contiguousDamageGroupSizes);
        })
        .ToList();

    private enum SpringCondition
    {
        Operational = '.',
        Damaged = '#',
        Unknown = '?'
    }
}
