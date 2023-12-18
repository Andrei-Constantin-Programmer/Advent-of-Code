using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution18 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var lines = Reader.ReadLines(this);
        var edges = GetEdges(lines, out var perimeter);
        var corners = edges.Keys
            .SelectMany(edge => new[] { edge.start, edge.end })
            .Distinct()
            .ToList();

        Console.WriteLine(GetPolygonArea(corners) + (perimeter / 2) + 1);
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

    private static Dictionary<(Point start, Point end), string> GetEdges(string[] lines, out int perimeter)
    {
        Dictionary<(Point start, Point end), string> edges = new();
        Point currentPoint = new(0, 0);

        perimeter = 0;
        foreach (var line in lines)
        {
            var elements = line.Split(' ');
            var direction = (Direction)elements[0][0];
            var edgeLength = int.Parse(elements[1]);
            var color = elements[2][1..^1];

            Point endPoint = direction switch
            {
                Direction.Right => new(currentPoint.Row, currentPoint.Column + edgeLength),
                Direction.Left => new(currentPoint.Row, currentPoint.Column - edgeLength),
                Direction.Up => new(currentPoint.Row - edgeLength, currentPoint.Column),
                Direction.Down => new(currentPoint.Row + edgeLength, currentPoint.Column),

                _ => throw new Exception($"Unknown direction {direction}")
            };
            
            edges.Add((currentPoint, endPoint), color);
            perimeter += edgeLength;

            currentPoint = endPoint;
        }

        return edges;
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private record Point(int Row, int Column);

    private enum Direction
    {
        Right = 'R',
        Left = 'L',
        Up = 'U',
        Down = 'D'
    }
}
