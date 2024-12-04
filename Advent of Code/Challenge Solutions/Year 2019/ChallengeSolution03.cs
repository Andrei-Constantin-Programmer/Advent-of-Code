// Task: https://adventofcode.com/2019/day/3

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019;

internal class ChallengeSolution03 : ChallengeSolution
{
    private static readonly Point _origin = new(0, 0);

    protected override void SolveFirstPart()
    {
        var lowestManhattanDistance =
            ReadIntersections().Keys
            .Select(point => ManhattanDistance(_origin, point))
            .Min();

        Console.WriteLine(lowestManhattanDistance);
    }

    protected override void SolveSecondPart()
    {
        var fewestCombinedSteps =
            ReadIntersections().Values
            .Min();

        Console.WriteLine(fewestCombinedSteps);
    }

    private Dictionary<Point, int> ReadIntersections()
    {
        var inputLines = Reader.ReadLines(this);
        var firstWireInstructions = ParseWireSteps(inputLines[0]);
        var secondWireInstructions = ParseWireSteps(inputLines[1]);

        var firstWirePath = GetWirePath(firstWireInstructions);
        var secondWirePath = GetWirePath(secondWireInstructions);

        var intersections =
            firstWirePath.Keys
            .Intersect(secondWirePath.Keys)
            .Where(point => point != _origin)
            .ToDictionary(point => point,
                          point => firstWirePath[point] + secondWirePath[point]);

        return intersections;
    }

    private static Dictionary<Point, int> GetWirePath(List<Instruction> instructions)
    {
        Dictionary<Point, int> path = new() { { _origin, 0 } };

        Point currentPoint = _origin;
        var totalSteps = 0;
        foreach (var instruction in instructions)
        {
            for (var i = 1; i <= instruction.Distance; i++)
            {
                Point newPoint = CreateNewPoint(currentPoint, instruction);

                totalSteps++;
                path.TryAdd(newPoint, totalSteps);
                currentPoint = newPoint;
            }
        }

        return path;
    }

    private static Point CreateNewPoint(Point currentPoint, Instruction instruction) => instruction.Direction switch
    {
        Direction.Up => new(currentPoint.X, currentPoint.Y + 1),
        Direction.Down => new(currentPoint.X, currentPoint.Y - 1),
        Direction.Left => new(currentPoint.X - 1, currentPoint.Y),
        Direction.Right => new(currentPoint.X + 1, currentPoint.Y),

        _ => throw new ArgumentException($"Unknown direction {instruction.Direction}"),
    };

    private static List<Instruction> ParseWireSteps(string stepsString) => stepsString
        .Split(',')
        .Select(step => new Instruction(ParseDirection(step[0]), int.Parse(step[1..])))
        .ToList();

    private static long ManhattanDistance(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    private static Direction ParseDirection(char direction) => direction switch
    {
        'U' => Direction.Up,
        'D' => Direction.Down,
        'L' => Direction.Left,
        'R' => Direction.Right,

        _ => throw new ArgumentException($"Unknown direction {direction}")
    };

    private record Point(int X, int Y);

    private record Instruction(Direction Direction, int Distance);

    private enum Direction
    {
        Up, Down, Left, Right
    }
}
