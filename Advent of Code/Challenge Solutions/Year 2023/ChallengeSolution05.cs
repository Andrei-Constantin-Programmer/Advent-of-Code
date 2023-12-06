namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution05 : ChallengeSolution
{
    private readonly List<string> _lines;

    private readonly List<Mapping> _seedToSoilMappings;
    private readonly List<Mapping> _soilToFertilizerMappings;
    private readonly List<Mapping> _fertilizerToWaterMappings;
    private readonly List<Mapping> _waterToLightMappings;
    private readonly List<Mapping> _lightToTemperatureMappings;
    private readonly List<Mapping> _temperatureToHumidityMappings;
    private readonly List<Mapping> _humidityToLocationMappings;

    public ChallengeSolution05()
    {
        _lines = ReadInputLines();

        _seedToSoilMappings = ParseMappings(_lines, "seed-to-soil map:", "soil-to-fertilizer map:");
        _soilToFertilizerMappings = ParseMappings(_lines, "soil-to-fertilizer map:", "fertilizer-to-water map:");
        _fertilizerToWaterMappings = ParseMappings(_lines, "fertilizer-to-water map:", "water-to-light map:");
        _waterToLightMappings = ParseMappings(_lines, "water-to-light map:", "light-to-temperature map:");
        _lightToTemperatureMappings = ParseMappings(_lines, "light-to-temperature map:", "temperature-to-humidity map:");
        _temperatureToHumidityMappings = ParseMappings(_lines, "temperature-to-humidity map:", "humidity-to-location map:");
        _humidityToLocationMappings = ParseMappings(_lines, "humidity-to-location map:", _lines.Last());
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
        throw new NotImplementedException();
    }

    private long MapSeedToLocation(long seed) =>
        _humidityToLocationMappings.Map(
            _temperatureToHumidityMappings.Map(
                _lightToTemperatureMappings.Map(
                    _waterToLightMappings.Map(
                        _fertilizerToWaterMappings.Map(
                            _soilToFertilizerMappings.Map(
                                _seedToSoilMappings.Map(seed)))))));

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
                DestinationRangeStart: mappingElements[0],
                SourceRangeStart: mappingElements[1],
                RangeLength: mappingElements[2]));
        }

        return mappings;
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

public static class MappingExtensions
{
    public static long Map(this List<Mapping> mappings, long value)
    {
        Mapping? mapping = mappings.FirstOrDefault(m => m.SourceRangeStart <= value && value <= m.SourceRangeStart + m.RangeLength);

        return mapping is null
            ? value
            : mapping.DestinationRangeStart + (value - mapping.SourceRangeStart);
    }
}

public record Mapping(long DestinationRangeStart, long SourceRangeStart, long RangeLength);