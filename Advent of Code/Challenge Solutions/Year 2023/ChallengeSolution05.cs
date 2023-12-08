using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution05 : ChallengeSolution
{
    private readonly List<string> _lines;

    private readonly List<List<Mapping>> _mappingLists;

    public ChallengeSolution05()
    {
        _lines = ReadInputLines();

        _mappingLists = new()
        {
            ParseMappings(_lines, "seed-to-soil map:", "soil-to-fertilizer map:"),
            ParseMappings(_lines, "soil-to-fertilizer map:", "fertilizer-to-water map:"),
            ParseMappings(_lines, "fertilizer-to-water map:", "water-to-light map:"),
            ParseMappings(_lines, "water-to-light map:", "light-to-temperature map:"),
            ParseMappings(_lines, "light-to-temperature map:", "temperature-to-humidity map:"),
            ParseMappings(_lines, "temperature-to-humidity map:", "humidity-to-location map:"),
            ParseMappings(_lines, "humidity-to-location map:", _lines.Last())
        };
    }

    protected override void SolveFirstPart()
    {
        var minimumLocation = ParseSeeds(_lines[0])
            .Select(MapSeedToLocation)
            .Min();

        Console.WriteLine(minimumLocation);
    }

    protected override void SolveSecondPart()
    {
        List<MappingRange> seedRanges = ParseSeedRanges(_lines[0]);
        
        foreach (var mappingList in _mappingLists)
        {
            foreach (var range in seedRanges)
            {
                range.IsMapped = false;
            }

            foreach (Mapping mapping in mappingList)
            {
                List<MappingRange> mappedRanges = new();
                foreach (MappingRange range in seedRanges)
                {
                    if (range.IsMapped)
                    {
                        mappedRanges.Add(range);
                        continue;
                    }

                    mappedRanges.AddRange(mapping.MapRange(range));
                }

                seedRanges = new(mappedRanges);
            }
        }

        var minimumLocation = seedRanges
            .OrderBy(x => x.Start)
            .First()
            .Start;

        Console.WriteLine(minimumLocation);
    }

    private long MapSeedToLocation(long seed) => _mappingLists.Aggregate(seed, (value, mappingList) => mappingList.Map(value));

    private static List<Mapping> ParseMappings(List<string> inputLines, string startMarker, string endMarker)
    {
        List<Mapping> mappings = new();

        for (var i = inputLines.IndexOf(startMarker) + 1; inputLines[i] != endMarker; i++)
        {
            var mappingElements = inputLines[i]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            mappings.Add(new(
                destinationRangeStart: mappingElements[0],
                sourceRangeStart: mappingElements[1],
                rangeLength: mappingElements[2]));
        }

        return mappings
            .OrderBy(mapping => mapping.SourceRangeStart)
            .ToList();
    }

    private static List<MappingRange> ParseSeedRanges(string firstLine)
    {
        List<MappingRange> seedRanges = new();
        var seedValues = ParseSeeds(firstLine);

        for (var i = 0; i < seedValues.Count; i += 2)
        {
            seedRanges.Add(new(
                start: seedValues[i],
                end: seedValues[i] + seedValues[i + 1] - 1));
        }

        return seedRanges;
    }

    private static List<long> ParseSeeds(string firstLine) => firstLine
        .Split(": ", StringSplitOptions.RemoveEmptyEntries)[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToList();

    private static List<string> ReadInputLines()
    {
        var lines = File
            .ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2023, 5))
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Trim())
            .ToList();
        lines.Add(string.Empty);

        return lines;
    }

}
public class MappingRange
{
    public long Start { get; }
    public long End { get; }
    public bool IsMapped { get; set; }

    public MappingRange(long start, long end, bool isMapped = false)
    {
        Start = start;
        End = end;
        IsMapped = isMapped;
    }
}

public static class MappingExtensions
{
    public static long Map(this List<Mapping> mappings, long value)
    {
        Mapping? mapping = mappings.FirstOrDefault(m => m.SourceRangeStart <= value && value <= m.SourceRangeEnd);

        return mapping is null
            ? value
            : mapping.DestinationRangeStart + (value - mapping.SourceRangeStart);
    }
}

public record Mapping
{
    public long DestinationRangeStart { get; }
    public long SourceRangeStart { get; }
    public long DestinationRangeEnd { get; }
    public long SourceRangeEnd { get; }

    public Mapping(long destinationRangeStart, long sourceRangeStart, long rangeLength)
    {
        DestinationRangeStart = destinationRangeStart;
        DestinationRangeEnd = destinationRangeStart + rangeLength - 1;

        SourceRangeStart = sourceRangeStart;
        SourceRangeEnd = sourceRangeStart + rangeLength - 1;
    }

    public List<MappingRange> MapRange(MappingRange range)
    {
        bool areRangeAndSourceDisjoint = range.End < SourceRangeStart
                                      || range.Start > SourceRangeEnd;
        if (areRangeAndSourceDisjoint)
        {
            return new() { range };
        }

        bool isRangeOverlappingStart = range.Start < SourceRangeStart
                                    && range.End >= SourceRangeStart;
        if (isRangeOverlappingStart)
        {
            return MapRangeOverlappingTheStart(range);
        }

        bool isRangeSubrangeOfSource = range.Start >= SourceRangeStart
                                         && range.End <= SourceRangeEnd;
        if (isRangeSubrangeOfSource)
        {
            return MapRangeOverlappingTheSource(range);
        }

        bool isRangeOverlappingEnd = range.Start >= SourceRangeStart
                                   && range.End > SourceRangeEnd;
        if (isRangeOverlappingEnd)
        {
            return MapRangeOverlappingTheEnd(range);
        }

        return new();
    }

    private List<MappingRange> MapRangeOverlappingTheStart(MappingRange range)
    {
        List<MappingRange> mappedRanges = new();

        MappingRange underflowingRange = new(range.Start, SourceRangeStart - 1);
        mappedRanges.Add(underflowingRange);

        var rangeLength = range.End - SourceRangeStart;
        MappingRange overlappingRange = new(
            start: DestinationRangeStart,
            end: Math.Min(DestinationRangeEnd, DestinationRangeStart + rangeLength),
            isMapped: true);
        mappedRanges.Add(overlappingRange);

        if (range.End > SourceRangeEnd)
        {
            MappingRange overflowingRange = new(SourceRangeEnd + 1, range.End);
            mappedRanges.Add(overflowingRange);
        }

        return mappedRanges;
    }

    private List<MappingRange> MapRangeOverlappingTheSource(MappingRange range)
    {
        List<MappingRange> mappedRanges = new();

        var startRangePosition = range.Start - SourceRangeStart;
        var endRangePosition = SourceRangeEnd - range.End;

        MappingRange overlappingRange = new(DestinationRangeStart + startRangePosition, DestinationRangeEnd - endRangePosition, true);
        mappedRanges.Add(overlappingRange);

        return mappedRanges;
    }

    private List<MappingRange> MapRangeOverlappingTheEnd(MappingRange range)
    {
        List<MappingRange> mappedRanges = new();

        var startRangePosition = SourceRangeEnd - range.Start;

        MappingRange overflowingRange = new(SourceRangeEnd + 1, range.End);
        mappedRanges.Add(overflowingRange);

        MappingRange overlappingRange = new(DestinationRangeEnd - startRangePosition, DestinationRangeEnd, true);
        mappedRanges.Add(overlappingRange);

        return mappedRanges;
    }
}