using Advent_of_Code.Utilities;
using System.Text;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution11 : ChallengeSolution
{
    private const char GALAXY = '#';

    protected override void SolveFirstPart()
    {
        var originalGalaxy = Reader.ReadLines(this).ToList();
        var expandedGalaxy = ExpandGalaxy(originalGalaxy);
        HashSet<Point> galaxyPositions = GetGalaxies(expandedGalaxy);

        var manhattanDistanceSum = 0;
        for (var i = 0; i < galaxyPositions.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxyPositions.Count; j++)
            {
                manhattanDistanceSum += ManhattanDistance(galaxyPositions.ElementAt(i), galaxyPositions.ElementAt(j));
            }
        }

        Console.WriteLine(manhattanDistanceSum);
    }

    protected override void SolveSecondPart()
    {
        var galaxy = Reader.ReadLines(this).ToList();
        HashSet<Point> galaxyPositions = GetGalaxies(galaxy);
        HashSet<int> emptyRows = GetEmptyRows(galaxy);
        HashSet<int> emptyCols = GetEmptyRows(TransposeGalaxy(galaxy));

        long manhattanDistanceSum = 0;
        for (var i = 0; i < galaxyPositions.Count - 1; i++)
        {
            for (var j = i + 1; j < galaxyPositions.Count; j++)
            {
                manhattanDistanceSum += ManhattanDistancePath(galaxy, galaxyPositions.ElementAt(i), galaxyPositions.ElementAt(j),
                    out var rowsPassedThrough,
                    out var columnsPassedThrough);
                
                manhattanDistanceSum += 999_999 * rowsPassedThrough.Count(row => emptyRows.Contains(row));
                manhattanDistanceSum += 999_999 * columnsPassedThrough.Count(col => emptyCols.Contains(col));
            }
        }

        Console.WriteLine(manhattanDistanceSum);
    }

    private static int ManhattanDistancePath(List<string> galaxy, Point a, Point b, out List<int> rows, out List<int> columns)
    {
        var distance = ManhattanDistance(a, b);

        rows = Enumerable
            .Range(0, Math.Abs(a.Row - b.Row))
            .Select(row => Math.Min(a.Row, b.Row) + row)
            .ToList();

        columns = Enumerable
            .Range(0, Math.Abs(a.Col - b.Col))
            .Select(col => Math.Min(a.Col, b.Col) + col)
            .ToList();

        return distance;
    }

    private static HashSet<int> GetEmptyRows(List<string> galaxy) =>
        Enumerable.Range(0, galaxy.Count)
        .Where(row => !galaxy[row].Contains(GALAXY))
        .ToHashSet();

    private static HashSet<Point> GetGalaxies(List<string> galaxy)
    {
        HashSet<Point> galaxyPositions = new();
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

    private static List<string> ExpandGalaxy(List<string> originalGalaxy)
    {
        var rowExpanded = ExpandRows(originalGalaxy);
        var transposed = TransposeGalaxy(rowExpanded);
        var fullyExpanded = TransposeGalaxy(ExpandRows(transposed));

        return fullyExpanded;

        static List<string> ExpandRows(List<string> galaxy)
        {
            List<string> expandedGalaxy = new(galaxy);
            for (int row = 0, currentRow = 0; row < galaxy.Count; row++, currentRow++)
            {
                var line = galaxy[row];
                if (!line.Contains(GALAXY))
                {
                    expandedGalaxy.Insert(currentRow++, line);
                }
            }

            return expandedGalaxy;
        }
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

    private static int ManhattanDistance(Point a, Point b) => Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);

    private record Point(int Row, int Col);
}
