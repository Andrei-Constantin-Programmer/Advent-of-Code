using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution02 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var reports = ReadReports();
        var safeReports = 0;

        foreach (var report in reports)
        {
            var isSafe = true;
            var isIncreasing = report[0] < report[1];

            for (var level = 0; level < report.Count - 1; level++)
            {
                if (isIncreasing && report[level] > report[level + 1])
                {
                    isSafe = false;
                    break;
                }

                if (!isIncreasing && report[level] < report[level + 1])
                {
                    isSafe = false;
                    break;
                }

                var difference = Math.Abs(report[level] - report[level + 1]);
                if (difference is 0 or > 3)
                {
                    isSafe = false;
                    break;
                }
            }

            if (isSafe)
            {
                safeReports++;
            }
        }

        Console.WriteLine($"Safe reports: {safeReports}");
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private List<List<int>> ReadReports()
    {
        List<List<int>> reports = [];

        foreach (var line in Reader.ReadLines(this))
        {
            var levels = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            reports.Add(levels);
        }

        return reports;
    }
}
