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
        Point currentPoint = GetStartPosition(map);

        var visited = GetVisitedNodes(map, currentPoint);

        var count = GetUniquePointCount(visited);

        _console.WriteLine($"Guard patrolled: {count}");
    }

    private static List<MapRange> GetVisitedNodes(string[] map, Point currentPoint)
    {
        List<MapRange> visited = [];
        var hasExitMap = false;

        while (!hasExitMap)
        {
            if (currentPoint.Direction == Direction.Up)
            {
                for (var row = currentPoint.Row - 1; row >= 0; row--)
                {
                    if (map[row][currentPoint.Col].IsEmpty())
                    {
                        continue;
                    }

                    if (row == 0 && map[row][currentPoint.Col] != WALL)
                    {
                        visited.Add(new(currentPoint, new Point(row, currentPoint.Col, Direction.Right)));
                        hasExitMap = true;
                    }
                    else
                    {
                        Point newPoint = new(row + 1, currentPoint.Col, Direction.Right);
                        visited.Add(new(currentPoint, newPoint));
                        currentPoint = newPoint;
                    }

                    break;
                }
            }
            else if (currentPoint.Direction == Direction.Right)
            {
                for (var col = currentPoint.Col + 1; col < map[currentPoint.Row].Length; col++)
                {
                    if (map[currentPoint.Row][col].IsEmpty())
                    {
                        continue;
                    }

                    if (col == map[currentPoint.Row].Length - 1 && map[currentPoint.Row][col] != WALL)
                    {
                        visited.Add(new(currentPoint, new Point(currentPoint.Row, col, Direction.Down)));
                        hasExitMap = true;
                    }
                    else
                    {
                        Point newPoint = new(currentPoint.Row, col - 1, Direction.Down);
                        visited.Add(new(currentPoint, newPoint));
                        currentPoint = newPoint;
                    }

                    break;
                }
            }
            else if (currentPoint.Direction == Direction.Down)
            {
                for (var row = currentPoint.Row + 1; row < map.Length; row++)
                {
                    if (map[row][currentPoint.Col].IsEmpty())
                    {
                        if (row == map.Length - 1)
                        {
                            visited.Add(new(currentPoint, new Point(row, currentPoint.Col, Direction.Left)));
                            hasExitMap = true;
                            break;
                        }
                    }
                    else
                    {
                        var newPoint = new Point(row - 1, currentPoint.Col, Direction.Left);
                        visited.Add(new(currentPoint, newPoint));
                        currentPoint = newPoint;
                        break;
                    }
                }
            }
            else if (currentPoint.Direction == Direction.Left)
            {
                for (var col = currentPoint.Col - 1; col >= 0; col--)
                {
                    if (map[currentPoint.Row][col].IsEmpty())
                    {
                        continue;
                    }

                    if (col == 0 && map[currentPoint.Row][col] != WALL)
                    {
                        visited.Add(new(currentPoint, new Point(currentPoint.Row, col, Direction.Up)));
                        hasExitMap = true;
                    }
                    else
                    {
                        var newPoint = new Point(currentPoint.Row, col + 1, Direction.Up);
                        visited.Add(new(currentPoint, newPoint));
                        currentPoint = newPoint;
                    }

                    break;
                }
            }
        }

        return visited;
    }

    private static int GetUniquePointCount(List<MapRange> visited)
    {
        var horizontalIntervals = new Dictionary<int, List<(int Start, int End)>>();
        var verticalIntervals = new Dictionary<int, List<(int Start, int End)>>();

        foreach (var range in visited)
        {
            if (range.Start.Direction is Direction.Left or Direction.Right)
            {
                var row = range.Start.Row;
                var startCol = Math.Min(range.Start.Col, range.End.Col);
                var endCol = Math.Max(range.Start.Col, range.End.Col);

                if (!horizontalIntervals.ContainsKey(row))
                {
                    horizontalIntervals[row] = [];
                }

                horizontalIntervals[row].Add((startCol, endCol));
            }
            else
            {
                var col = range.Start.Col;
                var startRow = Math.Min(range.Start.Row, range.End.Row);
                var endRow = Math.Max(range.Start.Row, range.End.Row);

                if (!verticalIntervals.ContainsKey(col))
                {
                    verticalIntervals[col] = [];
                }

                verticalIntervals[col].Add((startRow, endRow));
            }
        }

        var horizontalPoints = CountUniquePointsInIntervals(horizontalIntervals);
        var verticalPoints = CountUniquePointsInIntervals(verticalIntervals);

        var intersectionPoints = CountIntersections(horizontalIntervals, verticalIntervals);

        return horizontalPoints + verticalPoints - intersectionPoints;
    }

    private static int CountUniquePointsInIntervals(Dictionary<int, List<(int Start, int End)>> intervalsByFixedAxis)
    {
        var uniqueCount = 0;

        foreach (var intervals in intervalsByFixedAxis.Values)
        {
            intervals.Sort((a, b) => a.Start != b.Start ? a.Start.CompareTo(b.Start) : a.End.CompareTo(b.End));

            var currentStart = intervals[0].Start;
            var currentEnd = intervals[0].End;

            foreach (var (Start, End) in intervals)
            {
                if (Start > currentEnd)
                {
                    uniqueCount += currentEnd - currentStart + 1;
                    currentStart = Start;
                    currentEnd = End;
                }
                else
                {
                    currentEnd = Math.Max(currentEnd, End);
                }
            }

            uniqueCount += currentEnd - currentStart + 1;
        }

        return uniqueCount;
    }

    private static int CountIntersections(
        Dictionary<int, List<(int Start, int End)>> horizontal,
        Dictionary<int, List<(int Start, int End)>> vertical)
    {
        var intersections = 0;

        foreach (var (row, horizontalIntervals) in horizontal)
        {
            foreach (var (col, verticalIntervals) in vertical)
            {
                foreach (var (HorizontalStart, HorizontalEnd) in horizontalIntervals)
                {
                    foreach (var (VerticalStart, VerticalEnd) in verticalIntervals)
                    {
                        if (HorizontalStart <= col && col <= HorizontalEnd
                            && VerticalStart <= row && row <= VerticalEnd)
                        {
                            intersections++;
                        }
                    }
                }
            }
        }

        return intersections;
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static Point GetStartPosition(string[] map)
    {
        for (var row = 0; row < map.Length; row++)
        {
            for (var col = 0; col < map[row].Length; col++)
            {
                if (map[row][col].TryGetDirection(out var direction))
                {
                    return new(row, col, direction!.Value);
                }
            }
        }

        throw new ArgumentException("No starting position found");
    }

    private record struct MapRange(Point Start, Point End);

    private record struct Point(int Row, int Col, Direction Direction);

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