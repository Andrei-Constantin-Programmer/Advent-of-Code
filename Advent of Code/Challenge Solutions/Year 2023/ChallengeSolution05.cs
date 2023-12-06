namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution05 : ChallengeSolution
{
    private readonly List<int> _seeds;
    private readonly List<Mapping> _seedToSoilMappings;
    private readonly List<Mapping> _soilToFertilizerMappings;
    private readonly List<Mapping> _fertilizerToWaterMappings;
    private readonly List<Mapping> _waterToLightMappings;
    private readonly List<Mapping> _lightToTemperatureMappings;
    private readonly List<Mapping> _temperatureToHumidityMappings;
    private readonly List<Mapping> _humidityToLocationMappings;

    public ChallengeSolution05()
    {
        var lines = ReadInputLines();

        _seeds = ParseSeeds(lines[0]);
        _seedToSoilMappings = ParseMappings(lines, "seed-to-soil map:", "soil-to-fertilizer map:");
        _soilToFertilizerMappings = ParseMappings(lines, "soil-to-fertilizer map:", "fertilizer-to-water map:");
        _fertilizerToWaterMappings = ParseMappings(lines, "fertilizer-to-water map:", "water-to-light map:");
        _waterToLightMappings = ParseMappings(lines, "water-to-light map:", "light-to-temperature map:");
        _lightToTemperatureMappings = ParseMappings(lines, "light-to-temperature map:", "temperature-to-humidity map:");
        _temperatureToHumidityMappings = ParseMappings(lines, "temperature-to-humidity map:", "humidity-to-location map:");
        _humidityToLocationMappings = ParseMappings(lines, "humidity-to-location map:", lines.Last());
    }

    protected override void SolveFirstPart()
    {
        throw new NotImplementedException();
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static List<Mapping> ParseMappings(List<string> inputLines, string startMarker, string endMarker)
    {
        List<Mapping> mappings = new();

        for (var i = inputLines.IndexOf(startMarker) + 1; inputLines[i] != endMarker; i++)
        {
            var mappingElements = inputLines[i]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            mappings.Add(new(
                Range: new Range(
                    start: Math.Min(mappingElements[0], mappingElements[1]),
                    end: Math.Max(mappingElements[0], mappingElements[1])),
                RangeLength: mappingElements[2]));
        }

        return mappings;
    }

    private static List<int> ParseSeeds(string firstLine) => firstLine
        .Split(": ", StringSplitOptions.RemoveEmptyEntries)[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
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

    private record Mapping(Range Range, long RangeLength);
}
