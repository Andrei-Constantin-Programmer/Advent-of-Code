using Advent_of_Code.Utilities;
using System.Data;
namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution10 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        Tile startTile = ReadStartTile();
        
        Console.WriteLine(ComputeStepsToFarthestElement(startTile));
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int ComputeStepsToFarthestElement(Tile startTile)
    {
        Tile previousTile1 = startTile;
        Tile previousTile2 = startTile;
        Tile tile1 = startTile.Neighbour1!;
        Tile tile2 = startTile.Neighbour2!;

        var maxDistance = 0;
        while (tile1 != tile2)
        {
            maxDistance++;

            UpdateTiles(ref tile1, ref previousTile1);
            UpdateTiles(ref tile2, ref previousTile2);
        }

        if (tile1 == tile2)
        {
            maxDistance++;
        }

        return maxDistance;

        static void UpdateTiles(ref Tile currentTile, ref Tile previousTile)
        {
            var tempTile = currentTile;
            currentTile = currentTile.Neighbour1 == previousTile
                ? currentTile.Neighbour2!
                : currentTile.Neighbour1!;
            previousTile = tempTile;
        }
    }

    private static List<Tile> GetNeighbours(Tile[,] grid, int row, int column)
    {
        (Tile?, Predicate<TileShape>) topNeighbour = (
            row - 1 >= 0 ? grid[row - 1, column] : null,
            IsBottomLinked);
        (Tile?, Predicate<TileShape>) bottomNeighbour = (
            row + 1 < grid.GetLength(1) ? grid[row + 1, column] : null,
            IsTopLinked);
        (Tile?, Predicate<TileShape>) leftNeighbour = (
            column - 1 >= 0 ? grid[row, column - 1] : null,
            IsRightLinked);
        (Tile?, Predicate<TileShape>) rightNeighbour = (
            column + 1 < grid.GetLength(0) ? grid[row, column + 1] : null,
            IsLeftLinked);

        return grid[row, column].Shape switch
        {
            TileShape.VerticalPipe => GetLinkedNeighbours(new() { topNeighbour, bottomNeighbour }),
            TileShape.HorizontalPipe => GetLinkedNeighbours(new() { leftNeighbour, rightNeighbour }),
            TileShape.BottomLeftCorner => GetLinkedNeighbours(new() { topNeighbour, rightNeighbour }),
            TileShape.BottomRightCorner => GetLinkedNeighbours(new() { topNeighbour, leftNeighbour }),
            TileShape.TopLeftCorner => GetLinkedNeighbours(new() { bottomNeighbour, rightNeighbour }),
            TileShape.TopRightCorner => GetLinkedNeighbours(new() { bottomNeighbour, leftNeighbour }),

            _ => new()
        };;
}

    private static List<Tile> GetLinkedNeighbours(List<(Tile? tile, Predicate<TileShape> condition)> neighbours) => neighbours
        .Where(neighbour => neighbour.tile is not null
            && neighbour.condition(neighbour.tile.Shape))
        .Select(neighbour => neighbour.tile!)
        .ToList();

    private static bool IsTopLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.VerticalPipe
        or TileShape.BottomRightCorner
        or TileShape.BottomLeftCorner;

    private static bool IsBottomLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.VerticalPipe
        or TileShape.TopRightCorner
        or TileShape.TopLeftCorner;

    private static bool IsLeftLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.HorizontalPipe
        or TileShape.BottomRightCorner
        or TileShape.TopRightCorner;

    private static bool IsRightLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.HorizontalPipe
        or TileShape.BottomLeftCorner
        or TileShape.TopLeftCorner;

    private Tile ReadStartTile()
    {
        Tile? start = null;

        var grid = ReadInputGrid();

        for (var i = 0; i < grid.GetLength(0); i++)
        {
            for (var j = 0; j < grid.GetLength(1); j++)
            {
                var currentTile = grid[i, j];

                if (currentTile.Shape is TileShape.Start)
                {
                    start = currentTile;
                }

                var neighbours = GetNeighbours(grid, i, j);

                foreach (var neighbour in neighbours)
                {
                    currentTile.AddNeighbour(neighbour);
                    neighbour.AddNeighbour(currentTile);
                }
            }
        }

        return start ?? throw new Exception("Input grid features no start position S");
    }

    private Tile[,] ReadInputGrid()
    {
        var lines = Reader.ReadLines(this);
        var rows = lines.Length;
        var columns = lines[0].Length;
        var grid = new Tile[rows, columns];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                grid[i, j] = new Tile(lines[i][j]);
            }
        }

        return grid;
    }

    private class Tile
    {
        public TileShape Shape { get; }
        public Tile? Neighbour1 { get; private set; }
        public Tile? Neighbour2 { get; private set; }

        public Tile(char tileShape)
        {
            Shape = tileShape switch
            {
                '.' => TileShape.Empty,
                '|' => TileShape.VerticalPipe,
                '-' => TileShape.HorizontalPipe,
                'L' => TileShape.BottomLeftCorner,
                'J' => TileShape.BottomRightCorner,
                'F' => TileShape.TopLeftCorner,
                '7' => TileShape.TopRightCorner,
                'S' => TileShape.Start,

                _ => throw new ArgumentException($"Unknown tile shape {tileShape}")
            };
        }

        public void AddNeighbour(Tile tile)
        {
            if (Neighbour1 is null)
            {
                Neighbour1 = tile;
            }
            else if (Neighbour1 != tile)
            {
                Neighbour2 = tile;
            }
        }
    }

    private enum TileShape
    {
        Empty, VerticalPipe, HorizontalPipe, BottomLeftCorner, BottomRightCorner, TopLeftCorner, TopRightCorner, Start
    }
}