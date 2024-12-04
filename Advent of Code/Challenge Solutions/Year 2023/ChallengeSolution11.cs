// Task: https://adventofcode.com/2023/day/11

using Advent_of_Code.Utilities;
using System.Text;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution11(IConsole console) : ChallengeSolution(console)
{
    private const char GALAXY = '#';

    public override void SolveFirstPart()
    {
        var galaxy = Reader.ReadLines(this).ToList();
        var manhattanDistanceSum = GetManhattanDistanceSum(galaxy, 2);

        _console.WriteLine(manhattanDistanceSum);
    }

    public override void SolveSecondPart()
    {
        var galaxy = Reader.ReadLines(this).ToList();
        var manhattanDistanceSum = GetManhattanDistanceSum(galaxy, 1_000_000);

        _console.WriteLine(manhattanDistanceSum);
    }

    private static long GetManhattanDistanceSum(List<string> galaxy, int expansionRate)
    {
        List<Point> galaxyPositions = GetGalaxies(galaxy);
        HashSet<int> emptyRows = GetEmptyRows(galaxy);
        HashSet<int> emptyCols = GetEmptyRows(TransposeGalaxy(galaxy));

        long manhattanDistanceSum = 0;

        for (var i = 0; i < galaxyPositions.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxyPositions.Count; j++)
            {
                var distance = ManhattanDistance(galaxyPositions[i], galaxyPositions[j]);
                manhattanDistanceSum += distance;

                if (distance > 0)
                {
                    for (var row = Math.Min(galaxyPositions[i].Row, galaxyPositions[j].Row);
                        row < Math.Max(galaxyPositions[i].Row, galaxyPositions[j].Row);
                        row++)
                    {
                        manhattanDistanceSum += (expansionRate - 1) * (emptyRows.Contains(row) ? 1 : 0);
                    }

                    for (var col =
                        Math.Min(galaxyPositions[i].Col, galaxyPositions[j].Col);
                        col < Math.Max(galaxyPositions[i].Col, galaxyPositions[j].Col);
                        col++)
                    {
                        manhattanDistanceSum += (expansionRate - 1) * (emptyCols.Contains(col) ? 1 : 0);
                    }
                }
            }
        }

        return manhattanDistanceSum;
    }

    private static int ManhattanDistance(Point a, Point b) => Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);

    private static HashSet<int> GetEmptyRows(List<string> galaxy) =>
        Enumerable.Range(0, galaxy.Count)
        .Where(row => !galaxy[row].Contains(GALAXY))
        .ToHashSet();

    private static List<Point> GetGalaxies(List<string> galaxy)
    {
        List<Point> galaxyPositions = new();
        for (var row = 0; row < galaxy.Count; row++)
        {
            for (var col = 0; col < galaxy[0].Length; col++)
            {
                if (galaxy[row][col] == GALAXY)
                {
                    galaxyPositions.Add(new(row, col));
                }
            }
        }

        return galaxyPositions;
    }

    private static List<string> TransposeGalaxy(List<string> galaxy)
    {
        List<string> transposedGalaxy = new();

        for (var col = 0; col < galaxy[0].Length; col++)
        {
            StringBuilder rowBuilder = new();
            for (var row = 0; row < galaxy.Count; row++)
            {
                rowBuilder.Append(galaxy[row][col]);
            }

            transposedGalaxy.Add(rowBuilder.ToString());
        }

        return transposedGalaxy;
    }

    private record Point(int Row, int Col);
}
