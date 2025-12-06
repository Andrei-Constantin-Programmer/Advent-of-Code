// Task: https://adventofcode.com/2024/day/8

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution08(IConsole console, ISolutionReader<ChallengeSolution08> reader)
    : ChallengeSolution<ChallengeSolution08>(console, reader)
{
    private const char EMPTY = '.';

    public override void SolveFirstPart()
    {
        var antennas = ReadAntennaPositions(out var maxIndex);
        var antinodes = ComputeAntinodes(antennas, maxIndex);

        Console.WriteLine($"Antinodes: {antinodes.Count}");
    }

    public override void SolveSecondPart()
    {
        var antennas = ReadAntennaPositions(out var maxIndex);
        List<Point> antinodes = ComputeAllColinearAntinodes(antennas, maxIndex);

        HashSet<Point> allAntinodes = [.. antinodes];
        allAntinodes
            .UnionWith(antennas.Select(antenna => antenna.Point));

        Console.WriteLine($"Antinodes: {allAntinodes.Count}");
    }

    private static List<Point> ComputeAllColinearAntinodes(List<Antenna> antennas, int maxIndex)
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

                    var (rowDistance, colDistance) = ComputeAntennaDistance(antenna1.Point, antenna2.Point);

                    for (int row = antenna1.Point.Row + rowDistance, col = antenna1.Point.Col + colDistance;
                        row >= 0 && row <= maxIndex && col >= 0 && col <= maxIndex;
                        row += rowDistance, col += colDistance)
                    {
                        antinodes.Add(new(row, col));
                    }

                    for (int row = antenna2.Point.Row - rowDistance, col = antenna2.Point.Col - colDistance;
                        row >= 0 && row <= maxIndex && col >= 0 && col <= maxIndex;
                        row -= rowDistance, col -= colDistance)
                    {
                        antinodes.Add(new(row, col));
                    }
                }
            }
        }

        return antinodes;
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

                    var (rowDistance, colDistance) = ComputeAntennaDistance(antenna1.Point, antenna2.Point);

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

    private static (int RowDifference, int ColDifference) ComputeAntennaDistance(Point antenna1, Point antenna2)
        => (antenna1.Row - antenna2.Row, antenna1.Col - antenna2.Col);

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

    private string[] ReadMap() => Reader.ReadLines();

    private record struct Antenna(Point Point, char Frequency);

    private record struct Point(int Row, int Col);
}
