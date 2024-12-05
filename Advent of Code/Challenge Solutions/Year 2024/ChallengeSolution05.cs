// Task: https://adventofcode.com/2024/day/5

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution05(IConsole console, ISolutionReader<ChallengeSolution05> reader) : ChallengeSolution<ChallengeSolution05>(console, reader)
{
    public override void SolveFirstPart()
    {
        var (rules, updates) = ReadPrinterInformation();

        var middlePageNumberSum = 0;

        foreach (var update in updates)
        {
            var isCorrect = true;

            for (var current = 0; current < update.Length - 1 && isCorrect; current++)
            {
                for (var next = current + 1; next < update.Length && isCorrect; next++)
                {
                    if (rules.TryGetValue(update[current], out var pagesBefore) && pagesBefore.Contains(update[next]))
                    {
                        isCorrect = false;
                    }
                }
            }

            if (isCorrect)
            {
                middlePageNumberSum += update[update.Length / 2];
            }
        }

        _console.WriteLine($"Correct updates's middle page number sum: {middlePageNumberSum}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private (Dictionary<int, List<int>> rules, List<int[]> updates) ReadPrinterInformation()
    {
        var lines = _reader.ReadLines();
        var emptyLineIndex = Array.FindIndex(lines, string.IsNullOrEmpty);
        var (rulesSection, updatesSection) = (lines.Take(emptyLineIndex).ToArray(), lines.Skip(emptyLineIndex + 1).ToArray());

        return (GetRules(rulesSection), GetUpdates(updatesSection));
    }

    private static Dictionary<int, List<int>> GetRules(string[] pageOrderingRules)
    {
        Dictionary<int, List<int>> pageOrderings = [];

        foreach (var pageOrderingRule in pageOrderingRules)
        {
            var pageNumbers = pageOrderingRule.Split('|').Select(int.Parse).ToArray();
            if (!pageOrderings.TryGetValue(pageNumbers[1], out List<int>? pagesBefore))
            {
                pageOrderings.Add(pageNumbers[1], [pageNumbers[0]]);
            }
            else
            {
                pagesBefore.Add(pageNumbers[0]);
            }
        }

        return pageOrderings;
    }

    private static List<int[]> GetUpdates(string[] updates) => updates
        .Select(update => update
            .Split(',')
            .Select(int.Parse)
            .ToArray())
        .ToList();
}
