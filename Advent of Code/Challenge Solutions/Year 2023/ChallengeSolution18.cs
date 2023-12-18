using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution18 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var lines = Reader.ReadLines(this);
        var edges = GetEdgesWithLeftInstructions(lines, out var perimeter);
        
        Console.WriteLine(GetLavaStorageAmount(edges, perimeter));
    }

    protected override void SolveSecondPart()
    {
        var lines = Reader.ReadLines(this);
        var edges = GetEdgesWithRightInstructions(lines, out var perimeter);

        Console.WriteLine(GetLavaStorageAmount(edges, perimeter));
    }

    private static long GetLavaStorageAmount(HashSet<(Point start, Point end)> edges, long perimeter)
    {
        var corners = edges
            .SelectMany(edge => new[] { edge.start, edge.end })
            .Distinct()
        .ToList();

        return (long)GetPolygonArea(corners) + (perimeter / 2) + 1;
    }

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

    private static HashSet<(Point start, Point end)> GetEdgesWithRightInstructions(string[] lines, out long perimeter)
    {
        HashSet<(Point start, Point end)> edges = new();
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

            Point endPoint = direction switch
            {
                Direction.Right => new(currentPoint.Row, currentPoint.Column + edgeLength),
                Direction.Left => new(currentPoint.Row, currentPoint.Column - edgeLength),
                Direction.Up => new(currentPoint.Row - edgeLength, currentPoint.Column),
                Direction.Down => new(currentPoint.Row + edgeLength, currentPoint.Column),

                _ => throw new Exception($"Unknown direction {direction}")
            };

            edges.Add((currentPoint, endPoint));
            perimeter += edgeLength;

            currentPoint = endPoint;
        }

        return edges;
    }

    private static HashSet<(Point start, Point end)> GetEdgesWithLeftInstructions(string[] lines, out int perimeter)
    {
        HashSet<(Point start, Point end)> edges = new();
        Point currentPoint = new(0, 0);

        perimeter = 0;
        foreach (var line in lines)
        {
            var elements = line.Split(' ');
            var direction = (Direction)elements[0][0];
            var edgeLength = int.Parse(elements[1]);
            
            Point endPoint = direction switch
            {
                Direction.Right => new(currentPoint.Row, currentPoint.Column + edgeLength),
                Direction.Left => new(currentPoint.Row, currentPoint.Column - edgeLength),
                Direction.Up => new(currentPoint.Row - edgeLength, currentPoint.Column),
                Direction.Down => new(currentPoint.Row + edgeLength, currentPoint.Column),

                _ => throw new Exception($"Unknown direction {direction}")
            };

            edges.Add((currentPoint, endPoint));
            perimeter += edgeLength;

            currentPoint = endPoint;
        }

        return edges;
    }

    private record Point(long Row, long Column);

    private enum Direction
    {
        Right = 'R',
        Left = 'L',
        Up = 'U',
        Down = 'D'
    }
}
