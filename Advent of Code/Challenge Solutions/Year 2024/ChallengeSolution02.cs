using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution02 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var reports = ReadReports();
        var safeReports = reports.Count(r => IsSafeReport(r));

        Console.WriteLine($"Safe reports: {safeReports}");
    }

    protected override void SolveSecondPart()
    {
        var reports = ReadReports();
        var safeReports = 0;

        foreach (var report in reports)
        {
            if (IsSafeReport(report))
            {
                safeReports++;
            }
            else
            {
                for (var level = 0; level < report.Count; level++)
                {
                    if (IsSafeReport(report, level))
                    {
                        safeReports++;
                        break;
                    }
                }
            }
        }

        Console.WriteLine($"Safe reports: {safeReports}");
    }

    private static bool IsSafeReport(List<int> report, int? skipIndex = null)
    {
        var isIncreasing = DetermineTrend(report, skipIndex);

        for (var level = 0; level < report.Count - 1; level++)
        {
            if (skipIndex == level)
            {
                continue;
            }

            if (skipIndex == level + 1)
            {
                if (level + 2 < report.Count
                    && IsUnsafeLevelDifference(isIncreasing, report[level], report[level + 2]))
                {
                    return false;
                }

                continue;
            }

            if (IsUnsafeLevelDifference(isIncreasing, report[level], report[level + 1]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsUnsafeLevelDifference(bool isIncreasing, int level1, int level2)
    {
        if (isIncreasing && level1 > level2)
        {
            return true;
        }

        if (!isIncreasing && level1 < level2)
        {
            return true;
        }

        var difference = Math.Abs(level1 - level2);
        return difference is 0 or > 3;
    }

    private static bool DetermineTrend(List<int> report, int? skipIndex) =>
        skipIndex switch
        {
            0 => report[1] < report[2],
            1 => report[0] < report[2],
            _ => report[0] < report[1],
        };

    private IEnumerable<List<int>> ReadReports()
    {
        foreach (var line in Reader.ReadLines(this))
        {
            yield return line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}
