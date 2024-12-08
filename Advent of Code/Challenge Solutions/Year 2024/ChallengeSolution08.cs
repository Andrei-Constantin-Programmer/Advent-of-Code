// Task: https://adventofcode.com/2024/day/8

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution08(IConsole console, ISolutionReader<ChallengeSolution08> reader)
    : ChallengeSolution<ChallengeSolution08>(console, reader)
{
    private const char EMPTY = '.';

    public override void SolveFirstPart()
    {
        var antennas = ReadAntennaPositions(out var maxIndex);
        var antinodes = ComputeAntinodes(antennas, maxIndex);

        _console.WriteLine($"Antinodes: {antinodes.Count}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static HashSet<Point> ComputeAntinodes(List<Antenna> antennas, int maxIndex)
    {
        var antinodes = new List<Point>();

        var antennasByFrequency = antennas
            .GroupBy(a => a.Frequency)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var frequency in antennasByFrequency.Keys)
        {
            var antennaPositions = antennasByFrequency[frequency];

            for (var i = 0; i < antennaPositions.Count - 1; i++)
            {
                var antenna1 = antennaPositions[i];

                for (var j = i + 1; j < antennaPositions.Count; j++)
                {
                    var antenna2 = antennaPositions[j];

                    var rowDistance = antenna1.Point.Row - antenna2.Point.Row;
                    var colDistance = antenna1.Point.Col - antenna2.Point.Col;

                    antinodes.Add(new(antenna1.Point.Row + rowDistance, antenna1.Point.Col + colDistance));
                    antinodes.Add(new(antenna2.Point.Row - rowDistance, antenna2.Point.Col - colDistance));
                }
            }
        }

        _ = antinodes
            .RemoveAll(antinode =>
                antinode.Row < 0
                || antinode.Col < 0
                || antinode.Row > maxIndex
                || antinode.Col > maxIndex);

        return [.. antinodes];
    }

    private List<Antenna> ReadAntennaPositions(out int maxIndex)
    {
        var antennaPositions = new List<Antenna>();
        var lines = ReadMap();

        maxIndex = lines.Length - 1;

        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                if (lines[row][col] is not EMPTY)
                {
                    antennaPositions.Add(new Antenna(new(row, col), lines[row][col]));
                }
            }

        }

        return antennaPositions;
    }

    private string[] ReadMap() => _reader.ReadLines();

    private record struct Antenna(Point Point, char Frequency);

    private record struct Point(int Row, int Col);
}
