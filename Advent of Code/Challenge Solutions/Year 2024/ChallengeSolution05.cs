// Task: https://adventofcode.com/2024/day/5

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution05 : ChallengeSolution<ChallengeSolution05>
{
    private readonly Dictionary<int, List<int>> _rules;
    private readonly List<int[]> _updates;

    public ChallengeSolution05(IConsole console, ISolutionReader<ChallengeSolution05> reader) : base(console, reader)
    {
        var lines = reader.ReadLines();
        var emptyLineIndex = Array.FindIndex(lines, string.IsNullOrEmpty);
        var (rulesSection, updatesSection) = (lines.Take(emptyLineIndex).ToArray(), lines.Skip(emptyLineIndex + 1).ToArray());

        _rules = GetRules(rulesSection);
        _updates = GetUpdates(updatesSection);
    }

    public override void SolveFirstPart()
    {

    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static Dictionary<int, List<int>> GetRules(string[] pageOrderingRules)
    {
        Dictionary<int, List<int>> pageOrderings = [];

        foreach (var pageOrderingRule in pageOrderingRules)
        {
            var pageNumbers = pageOrderingRule.Split('|').Select(int.Parse).ToArray();
            if (!pageOrderings.TryGetValue(pageNumbers[0], out List<int>? pagesAfter))
            {
                pageOrderings.Add(pageNumbers[0], [pageNumbers[1]]);
            }
            else
            {
                pagesAfter.Add(pageNumbers[1]);
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
