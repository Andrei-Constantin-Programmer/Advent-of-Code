// Task: https://adventofcode.com/2024/day/12

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution12(IConsole console, ISolutionReader<ChallengeSolution12> reader)
    : ChallengeSolution<ChallengeSolution12>(console, reader)
{
    private static Point[] Directions =>
    [
        new(-1, 0),
        new (1, 0),
        new (0, -1),
        new (0, 1),
    ];

    public override void SolveFirstPart()
    {
        var plotMap = ReadGardenPlotMap();
        List<long> plotPrices = [];
        HashSet<Point> visited = [];

        long totalPrice = 0;

        for (var row = 0; row < plotMap.Length; row++)
        {
            for (var col = 0; col < plotMap[row].Length; col++)
            {
                Point point = new(row, col);
                if (visited.Contains(point))
                {
                    continue;
                }

                var price = FindPriceForPlotStartingAt(plotMap, point, out var newlyVisited);
                totalPrice += price;

                visited.UnionWith(newlyVisited);
            }
        }

        _console.WriteLine($"Total Cost: {totalPrice}");
    }

    private long FindPriceForPlotStartingAt(string[] plotMap, Point point, out HashSet<Point> visited)
    {
        var plot = GetPlot(plotMap, point, out visited);

        var area = plot.Count;
        var perimeter = CalculatePerimeter(plotMap, plot);

        return area * perimeter;
    }

    private int CalculatePerimeter(string[] plotMap, List<Point> plot)
    {
        var perimeter = 0;

        var plant = plotMap[plot[0].Row][plot[0].Col];
        foreach (var point in plot)
        {
            foreach (var direction in Directions)
            {
                Point neighbour = new(point.Row + direction.Row, point.Col + direction.Col);

                if (IsOutOfBounds(plotMap, neighbour)
                    || plotMap[neighbour.Row][neighbour.Col] != plant)
                {
                    perimeter++;
                }
            }
        }

        return perimeter;
    }

    private List<Point> GetPlot(string[] plotMap, Point start, out HashSet<Point> visited)
    {
        List<Point> result = [];
        visited = [];

        var plant = plotMap[start.Row][start.Col];

        Queue<Point> queue = [];
        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            foreach (var direction in Directions)
            {
                Point nextPoint = new(current.Row + direction.Row, current.Col + direction.Col);

                if (IsOutOfBounds(plotMap, nextPoint))
                {
                    continue;
                }

                if (visited.Contains(nextPoint)
                    || plotMap[nextPoint.Row][nextPoint.Col] != plant)
                {
                    continue;
                }

                queue.Enqueue(nextPoint);
                visited.Add(nextPoint);
            }
        }

        return result;
    }

    private static bool IsOutOfBounds(string[] plotMap, Point point)
        => point.Row < 0
        || point.Row >= plotMap.Length
        || point.Col < 0
        || point.Col >= plotMap[point.Row].Length;

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private string[] ReadGardenPlotMap()
    {
        return _reader.ReadLines();
    }

    private record Point(int Row, int Col);
}
