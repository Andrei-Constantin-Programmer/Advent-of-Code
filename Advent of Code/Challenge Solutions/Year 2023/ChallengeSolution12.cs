using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution12 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var conditionRecords = ReadConditionRecords();

        var arrangementSum = 0;

        foreach (var (conditions, groupSizes) in conditionRecords)
        {
            var arrangements = GenerateArrangements(conditions);
            foreach (var arrangement in arrangements)
            {
                if (FitsGroupSizes(arrangement, groupSizes))
                {
                    arrangementSum++;
                }
            }
        }

        Console.WriteLine(arrangementSum);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static bool FitsGroupSizes(SpringCondition[] arrangement, int[] groupSizes)
    {
        var groupCount = -1;
        var currentSize = 0;

        var fitsGroupSizes = true;
        for (var i = 0; i < arrangement.Length && fitsGroupSizes; i++)
        {
            if (arrangement[i] == SpringCondition.Damaged)
            {
                if (currentSize++ == 0)
                {
                    groupCount++;
                }
            }
            else if (currentSize > 0)
            {
                if (groupCount >= groupSizes.Length || currentSize != groupSizes[groupCount])
                {
                    fitsGroupSizes = false;
                }
                currentSize = 0;
            }
        }
        if (currentSize > 0 && (groupCount >= groupSizes.Length || currentSize != groupSizes[groupCount]))
        {
            fitsGroupSizes = false;
        }
        if (groupCount < groupSizes.Length - 1)
        {
            fitsGroupSizes = false;
        }

        return fitsGroupSizes;
    }

    private static List<SpringCondition[]> GenerateArrangements(SpringCondition[] conditions)
    {
        List<SpringCondition[]> result = new();
        GenerateArrangements(conditions, 0, result);
        return result;

        static void GenerateArrangements(SpringCondition[] conditions, int index, List<SpringCondition[]> result)
        {
            if (index == conditions.Length)
            {
                result.Add((SpringCondition[])conditions.Clone());
                return;
            }

            if (conditions[index] == SpringCondition.Unknown)
            {
                conditions[index] = SpringCondition.Operational;
                GenerateArrangements(conditions, index + 1, result);
                conditions[index] = SpringCondition.Damaged;
                GenerateArrangements(conditions, index + 1, result);
                conditions[index] = SpringCondition.Unknown;
            }
            else
            {
                GenerateArrangements(conditions, index + 1, result);
            }
        }
    }

    private List<(SpringCondition[], int[])> ReadConditionRecords() => Reader.ReadLines(this)
        .Select(line =>
        {
            var elements = line.Split(' ');
            var springConditions = elements[0]
                .Select(condition => (SpringCondition)condition)
                .ToArray();

            var contiguousDamageGroupSizes = elements[1]
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
