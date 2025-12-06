// Task: https://adventofcode.com/2023/day/21

using System.Diagnostics;
using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution21(IConsole console, ISolutionReader<ChallengeSolution21> reader)
    : ChallengeSolution<ChallengeSolution21>(console, reader)
{
    private const char PLOT = '.';
    private const char START = 'S';

    private static Point[] _directions =
    [
        new(-1, 0),
        new(1, 0),
        new(0, -1),
        new(0, 1),
    ];

    public override void SolveFirstPart()
    {
        var maxSteps = 64;

        var garden = ReadGarden(out var startPlot);
        var latestPlotsReached = FindLastPlotsReached(garden, startPlot, maxSteps, isInfiniteGarden: false);

        _console.WriteLine(latestPlotsReached);
    }

    public override void SolveSecondPart()
    {
        var maxSteps = 26501365;
        var expectedGardenSize = 131;

        var garden = ReadGarden(out var startPlot);
        Trace.Assert(garden.Length == garden[0].Length
                     && garden.Length == expectedGardenSize, $"Garden must be a square matrix of size {expectedGardenSize}");

        var points = GenerateInterpolationPoints(garden, startPlot);
        var latestPlotsReached = (long)LagrangePolynomial(maxSteps, points);

        _console.WriteLine(latestPlotsReached);
    }

    private static List<(int x, int y)> GenerateInterpolationPoints(string[] garden, Point startPlot)
    {
        var order = 2;

        var independentValues = Enumerable
            .Range(1, (2 * (order + 1)) - 1)
            .Where(x => x % 2 == 1)
            .Select(x => x * garden.Length / 2)
            .ToList();

        var dependentValues = independentValues
            .Select(x => FindLastPlotsReached(garden, startPlot, x, isInfiniteGarden: true))
            .ToList();

        return independentValues
            .Zip(dependentValues, (x, y) => (x, y))
            .ToList();
    }

    private static double LagrangePolynomial(long p, List<(int x, int y)> points)
    {
        long result = 0;

        foreach (var (xi, yi) in points)
        {
            long numerator = 1;
            long denominator = 1;

            foreach (var (xj, _) in points)
            {
                if (xi != xj)
                {
                    numerator *= p - xj;
                    denominator *= xi - xj;
                }
            }

            result += yi * (numerator / denominator);
        }

        return result;
    }

    private static int FindLastPlotsReached(string[] garden, Point startPlot, int maxSteps, bool isInfiniteGarden)
    {
        Queue<Point> nextPlotsToVisit = new();
        HashSet<Point> lastPlotsReached = [];

        nextPlotsToVisit.Enqueue(startPlot);

        var stepsTaken = 0;
        var lastPlotsReachedCount = 0;
        while (nextPlotsToVisit.TryDequeue(out var step) && stepsTaken < maxSteps)
        {
            foreach (var plot in GetNeighbouringPlots(garden, step, isInfiniteGarden))
            {
                lastPlotsReached.Add(plot);
            }

            if (nextPlotsToVisit.Count == 0)
            {
                nextPlotsToVisit = new Queue<Point>(lastPlotsReached);
                lastPlotsReachedCount = lastPlotsReached.Count;
                lastPlotsReached = [];
                stepsTaken++;
            }
        }

        return lastPlotsReachedCount;
    }

    private static List<Point> GetNeighbouringPlots(string[] garden, Point point, bool isInfiniteGarden)
    {
        List<Point> neighbours = [];

        foreach (var direction in _directions)
        {
            var neighbour = new Point(point.Row + direction.Row, point.Column + direction.Column);
            var normalisedNeighbour = isInfiniteGarden
                ? MovePointToFirstGarden(neighbour, garden.Length)
                : neighbour;

            if (normalisedNeighbour.Row < 0
                || normalisedNeighbour.Row >= garden.Length
                || normalisedNeighbour.Column < 0
                || normalisedNeighbour.Column >= garden[0].Length)
            {
                continue;
            }

            if (IsPlot(garden[normalisedNeighbour.Row][normalisedNeighbour.Column]))
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    private static Point MovePointToFirstGarden(Point point, int gardenSize) => new
    (
        ((point.Row % gardenSize) + gardenSize) % gardenSize,
        ((point.Column % gardenSize) + gardenSize) % gardenSize
    );

    private static bool IsPlot(char gardenPlot) => gardenPlot is PLOT or START;

    private string[] ReadGarden(out Point start)
    {
        var lines = _reader.ReadLines();
        start = new(-1, -1);

        for (var row = 0; row < lines.Length && start.Row == -1; row++)
        {
            for (var col = 0; col < lines[row].Length && start.Row == -1; col++)
            {
                if (lines[row][col] == START)
                {
                    start = new(row, col);
                }
            }
        }

        return lines;
    }

    private record Point(int Row, int Column);


}
