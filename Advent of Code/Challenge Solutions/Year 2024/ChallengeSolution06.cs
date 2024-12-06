// Task: https://adventofcode.com/2024/day/6

using Advent_of_Code.Utilities;
using static Advent_of_Code.Challenge_Solutions.Year_2024.ChallengeSolution06;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution06(IConsole console, ISolutionReader<ChallengeSolution06> reader)
    : ChallengeSolution<ChallengeSolution06>(console, reader)
{
    public const char WALL = '#';
    public const char EMPTY = '.';

    public override void SolveFirstPart()
    {
        var map = ReadMap();
        ((Point, Direction) currentPosition, List<Point> walls) = GetStartPositionAndWalls(map);

        var visited = GetVisitedPositions(map, currentPosition, walls);

        HashSet<Point> points = [];
        foreach (var x in visited)
        {
            for (var row = Math.Min(x.Start.Row, x.End.Row); row <= Math.Max(x.Start.Row, x.End.Row); row++)
            {
                for (var col = Math.Min(x.Start.Col, x.End.Col); col <= Math.Max(x.Start.Col, x.End.Col); col++)
                {
                    points.Add(new(row, col));
                }
            }
        }

        _console.WriteLine($"Guard patrolled: {points.Count}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static List<MapRange> GetVisitedPositions(string[] map, (Point point, Direction direction) currentPosition, List<Point> walls)
    {
        List<MapRange> visited = [];

        while (true)
        {
            var closestWall = GetClosestWall(currentPosition.point, currentPosition.direction, walls);
            if (closestWall is null)
            {
                var (row, col) = currentPosition.direction switch
                {
                    Direction.Up => (0, currentPosition.point.Col),
                    Direction.Down => (map.Length - 1, currentPosition.point.Col),
                    Direction.Left => (currentPosition.point.Row, 0),
                    Direction.Right => (currentPosition.point.Row, map[currentPosition.point.Row].Length - 1),

                    _ => throw new NotImplementedException(),
                };

                visited.Add(new(currentPosition.point, new Point(row, col)));
                break;
            }

            (Point point, Direction direction) nextPosition = currentPosition.direction switch
            {
                Direction.Up => (new Point(closestWall.Value.Row + 1, closestWall.Value.Col), Direction.Right),
                Direction.Right => (new Point(closestWall.Value.Row, closestWall.Value.Col - 1), Direction.Down),
                Direction.Down => (new Point(closestWall.Value.Row - 1, closestWall.Value.Col), Direction.Left),
                Direction.Left => (new Point(closestWall.Value.Row, closestWall.Value.Col + 1), Direction.Up),

                _ => throw new NotImplementedException(),
            };

            visited.Add(new(currentPosition.point, nextPosition.point));
            currentPosition = nextPosition;
        }

        return visited;
    }

    private static Point? GetClosestWall(Point point, Direction direction, List<Point> walls)
    {
        Point? closestWall = null;

        try
        {
            closestWall = direction switch
            {
                Direction.Up => walls
                    .Where(w => w.Col == point.Col && w.Row < point.Row)
                    .MaxBy(w => w.Row),
                Direction.Right => walls
                    .Where(w => w.Row == point.Row && w.Col > point.Col)
                    .MinBy(w => w.Col),
                Direction.Down => walls
                    .Where(w => w.Col == point.Col && w.Row > point.Row)
                    .MinBy(w => w.Row),
                Direction.Left => walls
                    .Where(w => w.Row == point.Row && w.Col < point.Col)
                    .MaxBy(w => w.Col),

                _ => null
            };
        }
        catch (Exception) { }

        return closestWall;
    }

    private static ((Point, Direction), List<Point>) GetStartPositionAndWalls(string[] map)
    {
        (Point, Direction)? start = null;

        List<Point> walls = [];

        for (var row = 0; row < map.Length; row++)
        {
            for (var col = 0; col < map[row].Length; col++)
            {
                if (start is null && map[row][col].TryGetDirection(out var direction))
                {
                    start = (new Point(row, col), direction!.Value);
                }

                if (map[row][col] is WALL)
                {
                    walls.Add(new(row, col));
                }
            }
        }

        if (start is null)
        {
            throw new ArgumentException("No starting position found");
        }

        return (start.Value, walls);
    }

    private record struct MapRange(Point Start, Point End);

    private record struct Point(int Row, int Col);

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private string[] ReadMap()
    {
        return _reader.ReadLines();
    }
}

public static class ChallengeSolution06Extensions
{
    public static bool TryGetDirection(this char c, out Direction? direction)
    {
        direction = c switch
        {
            '^' => Direction.Up,
            '>' => Direction.Right,
            'v' => Direction.Down,
            '<' => Direction.Left,
            _ => null
        };

        return direction is not null;
    }

    public static bool IsEmpty(this char c) => c == EMPTY || TryGetDirection(c, out var _);
}