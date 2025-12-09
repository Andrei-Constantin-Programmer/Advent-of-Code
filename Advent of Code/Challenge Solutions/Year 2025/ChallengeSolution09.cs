// Task: https://adventofcode.com/2025/day/9

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution09(IConsole console, ISolutionReader<ChallengeSolution09> reader)
    : ChallengeSolution<ChallengeSolution09>(console, reader)
{
    public override void SolveFirstPart()
    {
        var redTiles = ReadRedTileCoordinates();

        var largestArea = ComputeLargestArea(redTiles, (_, _) => true);

        Console.WriteLine($"Largest area: {largestArea}");
    }

    public override void SolveSecondPart()
    {
        var redTiles = ReadRedTileCoordinates();
        var (horizontalMargins, verticalMargins) = GetMargins(redTiles);
        
        var largestArea = ComputeLargestArea(redTiles, (tile1, tile2) 
            => AreaIsMadeUpOfGreenTiles(redTiles, horizontalMargins, verticalMargins, tile1, tile2));
        
        Console.WriteLine($"Largest area: {largestArea}");
    }

    private static bool AreaIsMadeUpOfGreenTiles(
        List<Tile> redTiles,
        List<HorizontalMargin> horizontalMargins,
        List<VerticalMargin> verticalMargins,
        Tile tile1,
        Tile tile2)
    {
        if (!IsWithinMargins(redTiles, tile1, tile2))
        {
            return false;
        }

        var minRow = Math.Min(tile1.Row, tile2.Row);
        var maxRow = Math.Max(tile1.Row, tile2.Row);
        var minCol = Math.Min(tile1.Col, tile2.Col);
        var maxCol = Math.Max(tile1.Col, tile2.Col);
        
        return HasNoHorizontalMarginIntersections(horizontalMargins, minRow, maxRow, minCol, maxCol) 
               && HasNoVerticalMarginIntersections(verticalMargins, minCol, maxCol, minRow, maxRow);
    }

    private static bool HasNoVerticalMarginIntersections(List<VerticalMargin> verticalMargins, long minCol, long maxCol, long minRow, long maxRow)
    {
        foreach (var verticalMargin in verticalMargins)
        {
            if (minCol >= verticalMargin.Col || verticalMargin.Col >= maxCol)
            {
                continue;
            }

            var startRow = Math.Max(verticalMargin.RowStart, minRow);
            var endRow = Math.Min(verticalMargin.RowEnd, maxRow);
            var intersects = startRow < endRow;
            
            if (intersects)
            {
                return false;
            }
        }

        return true;
    }

    private static bool HasNoHorizontalMarginIntersections(List<HorizontalMargin> horizontalMargins, long minRow, long maxRow, long minCol, long maxCol)
    {
        foreach (var horizontalMargin in horizontalMargins)
        {
            if (minRow >= horizontalMargin.Row || horizontalMargin.Row >= maxRow)
            {
                continue;
            }

            var startCol = Math.Max(horizontalMargin.ColStart, minCol);
            var endCol = Math.Min(horizontalMargin.ColEnd, maxCol);
            var intersects = startCol < endCol;
            
            if (intersects)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsWithinMargins(List<Tile> redTiles, Tile tile1, Tile tile2)
    {
        var (centreRow, centreCol) = ((tile1.Row + tile2.Row) / 2d, (tile1.Col + tile2.Col) / 2d);

        var marginsHit = 0;
        for (var i = 0; i < redTiles.Count; i++)
        {
            var marginTile1 = redTiles[i];
            var marginTile2 = redTiles[(i + 1) % redTiles.Count];

            if ((marginTile1.Row > centreRow) == (marginTile2.Row > centreRow))
            {
                continue;
            }

            var slope = (marginTile2.Col - marginTile1.Col) / (double)(marginTile2.Row - marginTile1.Row);
            var diffFromLeft = centreRow - marginTile1.Row;
            var colHit = marginTile1.Col + diffFromLeft * slope;

            if (colHit > centreCol)
            {
                marginsHit++;
            }
        }

        return marginsHit % 2 == 1;
    }

    private static (List<HorizontalMargin>, List<VerticalMargin>) GetMargins(List<Tile> redTiles)
    {
        List<HorizontalMargin> horizontalMargins = [];
        List<VerticalMargin> verticalMargins = [];
        
        for (var i = 0; i < redTiles.Count; i++)
        {
            var tile1 = redTiles[i];
            var tile2 = redTiles[(i + 1) % redTiles.Count];

            if (tile1.Row == tile2.Row)
            {
                var colStart = Math.Min(tile1.Col, tile2.Col);
                var colEnd = Math.Max(tile1.Col, tile2.Col);
                horizontalMargins.Add(new(tile1.Row, colStart, colEnd));
            }
            else if (tile1.Col == tile2.Col)
            {
                var rowStart = Math.Min(tile1.Row, tile2.Row);
                var rowEnd = Math.Max(tile1.Row, tile2.Row);
                verticalMargins.Add(new(tile1.Col, rowStart, rowEnd));
            }
        }

        return (horizontalMargins, verticalMargins);
    }
    
    private static long ComputeLargestArea(List<Tile> redTiles, Func<Tile, Tile, bool> requirement)
    {
        List<long> areas = [];
        for (var i = 0; i < redTiles.Count - 1; i++)
        {
            for (var j = i + 1; j < redTiles.Count; j++)
            {
                var tile1 = redTiles[i];
                var tile2 = redTiles[j];
                if (requirement(tile1, tile2))
                {
                    areas.Add(ComputeArea(tile1, tile2));
                }
            }
        }

        var largestArea = areas.Max();
        return largestArea;
    }

    private static long ComputeArea(Tile tile1, Tile tile2)
    {
        var side1 = Math.Abs(tile1.Row - tile2.Row) + 1;
        var side2 = Math.Abs(tile1.Col - tile2.Col) + 1;

        return side1 * side2;
    }

    private List<Tile> ReadRedTileCoordinates() =>
        Reader.ReadLines()
            .Select(line => line
                .Split(',')
                .Select(long.Parse)
                .ToArray())
            .Select(coords => new Tile(coords[1], coords[0]))
            .ToList();

    private record HorizontalMargin(long Row, long ColStart, long ColEnd);
    
    private record VerticalMargin(long Col, long RowStart, long RowEnd);
    
    private record Tile(long Row, long Col);
}
