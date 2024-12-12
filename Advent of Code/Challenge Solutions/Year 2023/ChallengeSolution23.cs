// Task: https://adventofcode.com/2023/day/23

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution23(IConsole console, ISolutionReader<ChallengeSolution23> reader)
    : ChallengeSolution<ChallengeSolution23>(console, reader)
{
    public override void SolveFirstPart()
    {
        SolveLongestTrailProblem(considerSlopes: true);
    }

    public override void SolveSecondPart()
    {
        SolveLongestTrailProblem(considerSlopes: false);
    }

    private void SolveLongestTrailProblem(bool considerSlopes)
    {
        var hikingTrails = ReadHikingTrails(considerSlopes);
        var trailGraph = ConvertTrailMapToGraph(hikingTrails);

        Point start = new(0, 1);
        Point end = new(hikingTrails.Length - 1, hikingTrails[0].Length - 2);

        _console.WriteLine(FindLongestTrail(trailGraph, start, end));
    }

    private static int FindLongestTrail(Dictionary<Point, HashSet<Point>> graph, Point start, Point end)
    {
        Stack<(Point current, HashSet<Point> visited, int length)> stack = [];
        stack.Push((start, [start], 0));
        var longestTrail = 0;

        while (stack.Count > 0)
        {
            var (current, currentVisited, pathLength) = stack.Pop();

            if (current == end)
            {
                longestTrail = Math.Max(longestTrail, pathLength);
                continue;
            }

            foreach (var neighbor in graph[current])
            {
                if (!currentVisited.Contains(neighbor))
                {
                    HashSet<Point> newVisited = new(currentVisited) { neighbor };
                    stack.Push((neighbor, newVisited, pathLength + 1));
                }
            }
        }

        return longestTrail;
    }

    private static Dictionary<Point, HashSet<Point>> ConvertTrailMapToGraph(string[] hikingTrails)
    {
        Dictionary<Point, HashSet<Point>> graph = [];

        for (var row = 0; row < hikingTrails.Length; row++)
        {
            for (var column = 0; column < hikingTrails[0].Length; column++)
            {
                var point = new Point(row, column);
                var trail = (Trail)hikingTrails[point.Row][point.Column];

                if (trail is Trail.Forest)
                {
                    continue;
                }

                graph[point] = NextGraphElements(hikingTrails, point);
            }
        }

        return graph;
    }

    private static HashSet<Point> NextGraphElements(string[] hikingTrails, Point point)
    {
        HashSet<Point> nextGraphElements = [];

        var nextMoves = GetNextTrailMoves(hikingTrails, point);
        foreach (var nextMove in nextMoves)
        {
            if (IsWithinBounds(nextMove)
                && (Trail)hikingTrails[nextMove.Row][nextMove.Column] is not Trail.Forest)
            {
                nextGraphElements.Add(nextMove);
            }
        }

        return nextGraphElements;

        bool IsWithinBounds(Point point) =>
            point.Row >= 0
            && point.Row < hikingTrails.Length
            && point.Column > -0
            && point.Column < hikingTrails[0].Length;
    }

    private static List<Point> GetNextTrailMoves(string[] hikingTrails, Point point)
    {
        var trail = (Trail)hikingTrails[point.Row][point.Column];
        return trail switch
        {
            Trail.SlopeNorth => [GoNorth(point)],
            Trail.SlopeSouth => [GoSouth(point)],
            Trail.SlopeWest => [GoWest(point)],
            Trail.SlopeEast => [GoEast(point)],
            Trail.Path =>
            [
                GoNorth(point),
                GoSouth(point),
                GoWest(point),
                GoEast(point),
            ],

            _ => []
        };
    }

    private static Point GoNorth(Point fromPoint) => new(fromPoint.Row - 1, fromPoint.Column);
    private static Point GoSouth(Point fromPoint) => new(fromPoint.Row + 1, fromPoint.Column);
    private static Point GoWest(Point fromPoint) => new(fromPoint.Row, fromPoint.Column - 1);
    private static Point GoEast(Point fromPoint) => new(fromPoint.Row, fromPoint.Column + 1);

    private string[] ReadHikingTrails(bool considerSlopes)
    {
        var hikingTrails = _reader.ReadLines();

        if (!considerSlopes)
        {
            for (var i = 0; i < hikingTrails.Length; i++)
            {
                hikingTrails[i] = hikingTrails[i]
                    .Replace((char)Trail.SlopeNorth, (char)Trail.Path)
                    .Replace((char)Trail.SlopeSouth, (char)Trail.Path)
                    .Replace((char)Trail.SlopeWest, (char)Trail.Path)
                    .Replace((char)Trail.SlopeEast, (char)Trail.Path);
            }
        }

        return hikingTrails;
    }

    private record Point(int Row, int Column);

    private enum Trail
    {
        Path = '.',
        Forest = '#',
        SlopeNorth = '^',
        SlopeWest = '<',
        SlopeSouth = 'v',
        SlopeEast = '>'
    }
}