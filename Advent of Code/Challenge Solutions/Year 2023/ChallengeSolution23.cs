using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution23 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var hikingTrails = ReadHikingTrails();
        Point start = new(0, 1);
        Point end = new(hikingTrails.Length - 1, hikingTrails[0].Length - 2);

        HashSet<Point> visited = new();

        var longestTrail = FindLongestTrail(hikingTrails, start, end, visited, 0);

        Console.WriteLine(longestTrail);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int FindLongestTrail(string[] hikingTrails, Point start, Point end, HashSet<Point> visited, int currentLength)
    {
        visited.Add(start);
        currentLength++;

        if (start == end)
        {
            return currentLength;
        }

        var maxLength = currentLength;

        List<Point> nextMoves = GetNextTrailMoves(hikingTrails, start);
        foreach (var nextMove in nextMoves)
        {
            if (IsValidMove(nextMove))
            {
                maxLength = Math.Max(maxLength, FindLongestTrail(hikingTrails, nextMove, end, visited, currentLength));
            }
        }

        visited.Remove(start);

        return maxLength;

        bool IsValidMove(Point point)
        {
            var isOutOfBounds = point.Row < 0 || point.Row >= hikingTrails.Length
                         || point.Column < 0 || point.Column >= hikingTrails[0].Length;

            return !isOutOfBounds
                && !visited.Contains(point)
                && (Trail)hikingTrails[point.Row][point.Column] is not Trail.Forest;
        }
    }

    private static List<Point> GetNextTrailMoves(string[] hikingTrails, Point start)
    {
        var trail = (Trail)hikingTrails[start.Row][start.Column];
        return trail switch
        {
            Trail.SlopeNorth => new() { GoNorth(start) },
            Trail.SlopeSouth => new() { GoSouth(start) },
            Trail.SlopeWest => new() { GoWest(start) },
            Trail.SlopeEast => new() { GoEast(start) },
            Trail.Path => new()
            {
                GoNorth(start),
                GoSouth(start),
                GoWest(start),
                GoEast(start),
            },

            _ => new()
        };
    }


    private static Point GoNorth(Point fromPoint) => new(fromPoint.Row - 1, fromPoint.Column);
    private static Point GoSouth(Point fromPoint) => new(fromPoint.Row + 1, fromPoint.Column);
    private static Point GoWest(Point fromPoint) => new(fromPoint.Row, fromPoint.Column - 1);
    private static Point GoEast(Point fromPoint) => new(fromPoint.Row, fromPoint.Column + 1);

    private string[] ReadHikingTrails()
    {
        return Reader.ReadLines(this);
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
