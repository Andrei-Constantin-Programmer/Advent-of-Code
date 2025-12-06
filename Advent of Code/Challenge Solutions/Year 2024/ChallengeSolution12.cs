// Task: https://adventofcode.com/2024/day/12

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution12(IConsole console, ISolutionReader<ChallengeSolution12> reader)
    : ChallengeSolution<ChallengeSolution12>(console, reader)
{
    private static Point[] Directions =>
    [
        new(-1, 0),
        new (0, 1),
        new (1, 0),
        new (0, -1),
    ];

    public override void SolveFirstPart()
    {
        SolvePlotProblem(CalculatePerimeter);
    }

    public override void SolveSecondPart()
    {
        SolvePlotProblem(CalculateSideCount);
    }

    private void SolvePlotProblem(Func<string[], List<Point>, int> sideCalculationFunction)
    {
        var plotMap = ReadGardenPlotMap();

        List<int> plotPrices = [];
        HashSet<Point> visited = [];

        var totalPrice = FindTotalPrice(plotMap, visited, sideCalculationFunction);

        Console.WriteLine($"Total Cost: {totalPrice}");
    }

    private static int FindTotalPrice(string[] plotMap, HashSet<Point> visited, Func<string[], List<Point>, int> sideCalculationFunction)
    {
        int totalPrice = 0;

        for (var row = 0; row < plotMap.Length; row++)
        {
            for (var col = 0; col < plotMap[row].Length; col++)
            {
                Point point = new(row, col);
                if (visited.Contains(point))
                {
                    continue;
                }

                var price = FindPriceByPerimeterForPlotStartingAt(plotMap, point, sideCalculationFunction, out var newlyVisited);
                totalPrice += price;

                visited.UnionWith(newlyVisited);
            }
        }

        return totalPrice;
    }

    private static int FindPriceByPerimeterForPlotStartingAt(
        string[] plotMap,
        Point point,
        Func<string[], List<Point>, int> sideCalculationFunction,
        out HashSet<Point> visited)
    {
        var plot = GetPlot(plotMap, point, out visited);

        var area = plot.Count;
        var sideNumber = sideCalculationFunction(plotMap, plot);
        System.Console.WriteLine(plotMap[point.Row][point.Col] + " - " + sideNumber);

        return area * sideNumber;
    }

    private static int CalculateSideCount(string[] plotMap, List<Point> plot)
    {
        if (plot.Count is < 3)
        {
            return 4;
        }

        (Point, Point)[] cornerChecks =
        [
            (Directions[0], Directions[1]),
            (Directions[0], Directions[3]),
            (Directions[2], Directions[1]),
            (Directions[2], Directions[3]),
        ];

        var corners = 0;
        var plant = plotMap[plot[0].Row][plot[0].Col];

        foreach (var point in plot)
        {
            foreach (var check in cornerChecks)
            {
                Point neighbour1 = new(point.Row + check.Item1.Row, point.Col + check.Item1.Col);
                Point neighbour2 = new(point.Row + check.Item2.Row, point.Col + check.Item2.Col);
                Point neighbour3 = new(point.Row + check.Item1.Row + check.Item2.Row, point.Col + check.Item1.Col + check.Item2.Col);

                var neighbour1Value = GetValueOrDefault(plotMap, neighbour1);
                var neighbour2Value = GetValueOrDefault(plotMap, neighbour2);
                var neighbour3Value = GetValueOrDefault(plotMap, neighbour3);

                var isConcaveCorner = neighbour1Value != plant
                                    && neighbour2Value != plant;

                var isConvexCorner = neighbour1Value == plant
                                   && neighbour2Value == plant
                                   && neighbour3Value != plant;

                if (isConcaveCorner || isConvexCorner)
                {
                    corners++;
                }
            }
        }

        return corners;

        static char GetValueOrDefault(string[] plotMap, Point point)
            => IsOutOfBounds(plotMap, point) ? ' ' : plotMap[point.Row][point.Col];
    }

    private static int CalculatePerimeter(string[] plotMap, List<Point> plot)
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

    private static List<Point> GetPlot(string[] plotMap, Point start, out HashSet<Point> visited)
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

    private string[] ReadGardenPlotMap()
    {
        return Reader.ReadLines();
    }

    private record Point(int Row, int Col);
}
