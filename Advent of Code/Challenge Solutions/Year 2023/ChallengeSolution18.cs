// Task: https://adventofcode.com/2023/day/18

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution18(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        var lines = Reader.ReadLines(this);
        var corners = GetCornersWithLeftInstructions(lines, out var perimeter);
        
        Console.WriteLine(GetLavaStorageAmount(corners, perimeter));
    }

    public override void SolveSecondPart()
    {
        var lines = Reader.ReadLines(this);
        var corners = GetCornersWithRightInstructions(lines, out var perimeter);

        Console.WriteLine(GetLavaStorageAmount(corners, perimeter));
    }

    private static long GetLavaStorageAmount(List<Point> corners, long perimeter)
        => (long)GetPolygonArea(corners) + (perimeter / 2) + 1;

    private static double GetPolygonArea(List<Point> corners)
    {
        double area = 0;

        for (var i = 0; i < corners.Count; i++)
        {
            var j = (i + 1) % corners.Count;
            area += (corners[i].Column * corners[j].Row) - (corners[j].Column * corners[i].Row);
        }

        area /= 2;
        return Math.Abs(area);
    }

    private static List<Point> GetCornersWithRightInstructions(string[] lines, out long perimeter)
    {
        List<Point> corners = new();
        Point currentPoint = new(0, 0);

        perimeter = 0;
        foreach (var line in lines)
        {
            var instruction = line.Split(' ')[2];
            var direction = instruction[^2] switch
            {
                '0' => Direction.Right,
                '1' => Direction.Down,
                '2' => Direction.Left,
                '3' => Direction.Up,

                _ => throw new Exception($"Malformed instruction {instruction}")
            };

            var edgeLength = Convert.ToInt64(instruction[2..^2], fromBase: 16);
            Point endPoint = GetEndPoint(currentPoint, direction, edgeLength);

            corners.Add(currentPoint);
            corners.Add(endPoint);
            perimeter += edgeLength;

            currentPoint = endPoint;
        }

        return corners;
    }

    private static List<Point> GetCornersWithLeftInstructions(string[] lines, out int perimeter)
    {
        List<Point> corners = new();
        Point currentPoint = new(0, 0);

        perimeter = 0;
        foreach (var line in lines)
        {
            var elements = line.Split(' ');
            var direction = (Direction)elements[0][0];
            var edgeLength = int.Parse(elements[1]);
            
            Point endPoint = GetEndPoint(currentPoint, direction, edgeLength);

            corners.Add(currentPoint);
            corners.Add(endPoint);

            perimeter += edgeLength;
            currentPoint = endPoint;
        }

        return corners;
    }

    private static Point GetEndPoint(Point currentPoint, Direction direction, long edgeLength) => direction switch
    {
        Direction.Right => new(currentPoint.Row, currentPoint.Column + edgeLength),
        Direction.Left => new(currentPoint.Row, currentPoint.Column - edgeLength),
        Direction.Up => new(currentPoint.Row - edgeLength, currentPoint.Column),
        Direction.Down => new(currentPoint.Row + edgeLength, currentPoint.Column),

        _ => throw new Exception($"Unknown direction {direction}")
    };

    private record Point(long Row, long Column);

    private enum Direction
    {
        Right = 'R',
        Left = 'L',
        Up = 'U',
        Down = 'D'
    }
}
