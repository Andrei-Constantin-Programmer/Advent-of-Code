// Task: https://adventofcode.com/2025/day/5

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution05(IConsole console, ISolutionReader<ChallengeSolution05> reader)
    : ChallengeSolution<ChallengeSolution05>(console, reader)
{
    public override void SolveFirstPart()
    {
        var (ranges, ingredientIds) = ReadFreshRangesAndIngredientIds();
        ranges = MergeOverlappingRanges(ranges);

        var freshCount = 0;
        foreach (var ingredient in ingredientIds)
        {
            if (ranges.Any(r => r.Contains(ingredient)))
            {
                freshCount++;
            }
        }    
        
        _console.WriteLine($"Fresh ingredients: {freshCount}");
    }

    public override void SolveSecondPart()
    {
        var (ranges, _) = ReadFreshRangesAndIngredientIds();
        ranges = MergeOverlappingRanges(ranges);

        var allFreshCount = ranges.Sum(range => range.Count);
        
        _console.WriteLine($"All fresh ingredients: {allFreshCount}");
    }

    private static List<Range> MergeOverlappingRanges(List<Range> ranges)
    {
        var newRanges = ranges
            .OrderBy(r => r.Start)
            .ToList();

        for (var current = 0; current < newRanges.Count - 1; current++)
        {
            var next = current + 1;
            while (next < newRanges.Count 
                   && Range.TryMerge(newRanges[current], newRanges[next], out var newRange))
            {
                newRanges[current] = newRange;
                newRanges.RemoveAt(next);
            }
        }

        return newRanges;
    }

    private (List<Range>, List<long>) ReadFreshRangesAndIngredientIds()
    {
        var lines = _reader.ReadLines();
        
        List<Range> ranges = [];
        int rangeIndex;
        for (rangeIndex = 0; rangeIndex < lines.Length; rangeIndex++)
        {
            var line = lines[rangeIndex];
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var elements = line.Split('-', StringSplitOptions.TrimEntries);
            ranges.Add(new(long.Parse(elements[0]), long.Parse(elements[1])));
        }

        var ingredientIds = lines[(rangeIndex + 1)..]
            .Select(long.Parse)
            .ToList();
        
        return (ranges, ingredientIds);
    }

    private readonly record struct Range(long Start, long End)
    {
        public long Count { get; } = End - Start + 1;
        
        public bool Contains(long value) => Start <= value && value <= End;

        public static bool TryMerge(Range range1, Range range2, out Range mergedRange)
        {
            if (range1.Start > range2.End || range1.End < range2.Start)
            {
                mergedRange = range1;
                return false;
            }

            var start = Math.Min(range1.Start, range2.Start);
            var end = Math.Max(range1.End, range2.End);

            mergedRange = new(start, end);
            return true;
        }
    }
}
