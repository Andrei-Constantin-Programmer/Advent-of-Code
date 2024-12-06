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

        var visitedRanges = GetVisitedPositionRanges(map, currentPosition, walls);
        HashSet<Point> points = GetVisitedPositions(visitedRanges);

        _console.WriteLine($"Guard patrolled: {points.Count}");
    }

    public override void SolveSecondPart()
    {
        var map = ReadMap();
        ((Point point, Direction direction) startPosition, List<Point> walls) = GetStartPositionAndWalls(map);

        var visitedRanges = GetVisitedPositionRanges(map, startPosition, walls);
        var originallyVisited = GetVisitedPositions(visitedRanges);

        var count = 0;

        foreach (var point in originallyVisited)
        {
            if (startPosition.point != point)
            {
                walls.Add(point);
                if (IsLooping(map, startPosition, walls))
                {
                    count++;
                }

                walls.Remove(point);
            }
        }

        _console.WriteLine($"Possible obstructions: {count}");
    }

    private static bool IsLooping(string[] map, (Point point, Direction direction) currentPosition, List<Point> walls)
    {
        HashSet<MapRange> visited = [];

        while (true)
        {
            var closestWall = GetClosestWall(currentPosition.point, currentPosition.direction, walls);
            if (closestWall is null)
            {
                return false;
            }

            (Point point, Direction direction) nextPosition = currentPosition.direction switch
            {
                Direction.Up => (new Point(closestWall.Value.Row + 1, closestWall.Value.Col), Direction.Right),
                Direction.Right => (new Point(closestWall.Value.Row, closestWall.Value.Col - 1), Direction.Down),
                Direction.Down => (new Point(closestWall.Value.Row - 1, closestWall.Value.Col), Direction.Left),
                Direction.Left => (new Point(closestWall.Value.Row, closestWall.Value.Col + 1), Direction.Up),

                _ => throw new NotImplementedException(),
            };

            if (!visited.Add(new(currentPosition.point, nextPosition.point)))
            {
                return true;
            }

            currentPosition = nextPosition;
        }
    }

    private static HashSet<Point> GetVisitedPositions(List<MapRange> visitedRanges)
    {
        HashSet<Point> points = [];
        foreach (var x in visitedRanges)
        {
            for (var row = x.Start.Row; row <= x.End.Row; row++)
            {
                for (var col = x.Start.Col; col <= x.End.Col; col++)
                {
                    points.Add(new(row, col));
                }
            }
        }

        return points;
    }

    private static List<MapRange> GetVisitedPositionRanges(string[] map, (Point point, Direction direction) currentPosition, List<Point> walls)
    {
        List<MapRange> visited = [];

        while (true)
        {
            var closestWall = GetClosestWall(currentPosition.point, currentPosition.direction, walls);
            var firstPoint = currentPosition.point;
            Point secondPoint;

            if (closestWall is null)
            {
                secondPoint = currentPosition.direction switch
                {
                    Direction.Up => new Point(0, currentPosition.point.Col),
                    Direction.Down => new Point(map.Length - 1, currentPosition.point.Col),
                    Direction.Left => new Point(currentPosition.point.Row, 0),
                    Direction.Right => new Point(currentPosition.point.Row, map[currentPosition.point.Row].Length - 1),

                    _ => throw new NotImplementedException(),
                };

                firstPoint = currentPosition.point;

                if (firstPoint.Row > secondPoint.Row
                    || firstPoint.Col > secondPoint.Col)
                {
                    (secondPoint, firstPoint) = (firstPoint, secondPoint);
                }

                visited.Add(new(firstPoint, secondPoint));
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

            secondPoint = nextPosition.point;
            if (firstPoint.Row > secondPoint.Row
                || firstPoint.Col > secondPoint.Col)
            {
                (secondPoint, firstPoint) = (firstPoint, secondPoint);
            }

            visited.Add(new(firstPoint, secondPoint));
            currentPosition = nextPosition;
        }

        return visited;
    }

    private static Point? GetClosestWall(Point point, Direction direction, List<Point> walls)
    {
        Point nullPoint = direction switch
        {
            Direction.Up or Direction.Left => new Point(-1, -1),
            Direction.Down or Direction.Right => new Point(int.MaxValue, int.MaxValue),

            _ => throw new NotImplementedException(),
        };

        walls.Add(nullPoint);

        Point? closestWall = direction switch
        {
            Direction.Up => walls
                .Where(w => (w.Col == point.Col && w.Row < point.Row) || w == nullPoint)
                .MaxBy(w => w.Row),
            Direction.Right => walls
                .Where(w => (w.Row == point.Row && w.Col > point.Col) || w == nullPoint)
                .MinBy(w => w.Col),
            Direction.Down => walls
                .Where(w => (w.Col == point.Col && w.Row > point.Row) || w == nullPoint)
                .MinBy(w => w.Row),
            Direction.Left => walls
                .Where(w => (w.Row == point.Row && w.Col < point.Col) || w == nullPoint)
                .MaxBy(w => w.Col),

            _ => null
        };

        walls.Remove(nullPoint);

        return closestWall == nullPoint ? null : closestWall;
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