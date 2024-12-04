// Task: https://adventofcode.com/2023/day/10

using Advent_of_Code.Utilities;
using System.Data;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution10(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        var lines = Reader.ReadLines(this);
        var grid = GetInputGrid(lines);
        var startTile = GetStartTile(grid, out var _);

        _console.WriteLine(ComputeStepsToFarthestElement(startTile));
    }

    public override void SolveSecondPart()
    {
        var lines = Reader.ReadLines(this);
        var tileGrid = GetInputGrid(lines);
        var startTile = GetStartTile(tileGrid, out var startPosition);
        var knotGrid = ConvertTileGridToKnotGrid(lines, tileGrid, startPosition);
        RemoveJunkPipes(tileGrid, startTile, knotGrid);

        var enclosedTiles = FindEnclosedTileCount(knotGrid);

        _console.WriteLine(enclosedTiles);
    }

    private static int FindEnclosedTileCount(char[,] knotGrid)
    {
        var enclosedTiles = 0;

        for (var row = 0; row < knotGrid.GetLength(0); row++)
        {
            var knotsCrossed = 0;

            for (var col = 0; col < knotGrid.GetLength(1); col++)
            {
                var character = knotGrid[row, col];
                if (IsWall(character))
                {
                    knotsCrossed++;
                }
                if (character == (char)TileShape.Empty
                 && knotsCrossed % 2 == 1)
                {
                    enclosedTiles++;
                }
            }
        }

        return enclosedTiles;

        static bool IsWall(char character) => (TileShape)character is
            TileShape.HorizontalPipe
            or TileShape.VerticalPipe
            or TileShape.BottomLeftCorner
            or TileShape.BottomRightCorner
            or TileShape.TopLeftCorner
            or TileShape.TopRightCorner;
    }

    private static void RemoveJunkPipes(Tile[,] tileGrid, Tile startTile, char[,] knotGrid)
    {
        var tilesInLoop = GetTilesInLoop(startTile);

        for (var row = 0; row < knotGrid.GetLength(0); row++)
        {
            for (var col = 0; col < knotGrid.GetLength(1); col++)
            {
                if (!tilesInLoop.Contains(tileGrid[row, col]))
                {
                    knotGrid[row, col] = (char)TileShape.Empty;
                }
            }
        }

        static HashSet<Tile> GetTilesInLoop(Tile startTile)
        {
            HashSet<Tile> tiles = new() { startTile };

            Tile previousTile = startTile;
            Tile currentTile = startTile.Neighbour1!;

            while (currentTile != startTile)
            {
                tiles.Add(currentTile);
                UpdateTiles(ref currentTile, ref previousTile);
            }

            return tiles;
        }
    }

    private static char[,] ConvertTileGridToKnotGrid(string[] lines, Tile[,] grid, (int row, int column) startPosition)
    {
        TileShape startShape = GetStartTileShape(grid, startPosition.row, startPosition.column);

        List<string> knotLines = new(lines);
        for (var row = 0; row < knotLines.Count; row++)
        {
            knotLines[row] = knotLines[row].Replace((char)TileShape.Start, (char)startShape);
            knotLines[row] = RemoveNarrowCorridors(knotLines[row]);
            knotLines[row] = CollapseClosingPipeSegments(knotLines[row]);
        }

        var knotGrid = new char[knotLines.Count, knotLines[0].Length];
        for (var row = 0; row < knotGrid.GetLength(0); row++)
        {
            for (var col = 0; col < knotGrid.GetLength(1); col++)
            {
                knotGrid[row, col] = knotLines[row][col];
            }
        }

        return knotGrid;

        static string RemoveNarrowCorridors(string line) =>
            Regex.Replace(line, "F-*7|L-*J", match => new string(' ', match.Length));

        static string CollapseClosingPipeSegments(string line) =>
            Regex.Replace(line, "F-*J|L-*7", match => $"|{new string(' ', match.Length - 1)}");
    }

    private static TileShape GetStartTileShape(Tile[,] grid, int startRow, int startColumn)
    {
        var startTile = grid[startRow, startColumn];

        Tile? leftNeighbour = GetNeighbour(grid, startRow, startColumn, 0, -1);
        Tile? rightNeighbour = GetNeighbour(grid, startRow, startColumn, 0, +1);
        Tile? topNeighbour = GetNeighbour(grid, startRow, startColumn, -1, 0);
        Tile? bottomNeighbour = GetNeighbour(grid, startRow, startColumn, +1, 0);

        var hasLeftNeighbour = leftNeighbour == startTile.Neighbour1 || leftNeighbour == startTile.Neighbour2;
        var hasRightNeighbour = rightNeighbour == startTile.Neighbour1 || rightNeighbour == startTile.Neighbour2;
        var hasTopNeighbour = topNeighbour == startTile.Neighbour1 || topNeighbour == startTile.Neighbour2;
        var hasBottomNeighbour = bottomNeighbour == startTile.Neighbour1 || bottomNeighbour == startTile.Neighbour2;

        return (hasLeftNeighbour, hasRightNeighbour, hasTopNeighbour, hasBottomNeighbour) switch
        {
            (true, true, false, false) => TileShape.HorizontalPipe,
            (false, false, true, true) => TileShape.VerticalPipe,
            (true, false, true, false) => TileShape.BottomRightCorner,
            (true, false, false, true) => TileShape.TopRightCorner,
            (false, true, true, false) => TileShape.BottomLeftCorner,
            (false, true, false, true) => TileShape.TopLeftCorner,

            _ => throw new Exception("Unknown shape for the starting position")
        };
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
    }

    private static void UpdateTiles(ref Tile currentTile, ref Tile previousTile)
    {
        var tempTile = currentTile;
        currentTile = currentTile.Neighbour1 == previousTile
            ? currentTile.Neighbour2!
            : currentTile.Neighbour1!;
        previousTile = tempTile;
    }

    private static Tile GetStartTile(Tile[,] grid, out (int row, int column) position)
    {
        InitialiseNeighbours(grid, out var start);

        position = start.position;
        return start.tile ?? throw new Exception("Input grid features no start position S");
    }

    private static void InitialiseNeighbours(Tile[,] grid, out (Tile tile, (int row, int column) position) start)
    {
        start = (grid[0, 0], (0, 0));
        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                var currentTile = grid[row, col];

                if (currentTile.Shape is TileShape.Start)
                {
                    start = (currentTile, (row, col));
                }

                var neighbours = GetNeighbours(grid, row, col);

                foreach (var neighbour in neighbours)
                {
                    currentTile.AddNeighbour(neighbour);
                    neighbour.AddNeighbour(currentTile);
                }
            }
        }
    }

    private static List<Tile> GetNeighbours(Tile[,] grid, int row, int column)
    {
        (Tile?, Predicate<TileShape>) topNeighbour = (GetNeighbour(grid, row, column, -1, 0), IsBottomLinked);
        (Tile?, Predicate<TileShape>) bottomNeighbour = (GetNeighbour(grid, row, column, +1, 0), IsTopLinked);
        (Tile?, Predicate<TileShape>) leftNeighbour = (GetNeighbour(grid, row, column, 0, -1), IsRightLinked);
        (Tile?, Predicate<TileShape>) rightNeighbour = (GetNeighbour(grid, row, column, 0, +1), IsLeftLinked);

        return grid[row, column].Shape switch
        {
            TileShape.VerticalPipe => GetLinkedNeighbours(new() { topNeighbour, bottomNeighbour }),
            TileShape.HorizontalPipe => GetLinkedNeighbours(new() { leftNeighbour, rightNeighbour }),
            TileShape.BottomLeftCorner => GetLinkedNeighbours(new() { topNeighbour, rightNeighbour }),
            TileShape.BottomRightCorner => GetLinkedNeighbours(new() { topNeighbour, leftNeighbour }),
            TileShape.TopLeftCorner => GetLinkedNeighbours(new() { bottomNeighbour, rightNeighbour }),
            TileShape.TopRightCorner => GetLinkedNeighbours(new() { bottomNeighbour, leftNeighbour }),

            _ => new()
        };
    }

    private static Tile? GetNeighbour(Tile[,] grid, int row, int column, int rowOffset, int columnOffset)
    {
        var newRow = row + rowOffset;
        var newColumn = column + columnOffset;

        if (newRow >= 0
         && newRow < grid.GetLength(0)
         && newColumn >= 0
         && newColumn < grid.GetLength(1))
        {
            return grid[newRow, newColumn];
        }

        return null;
    }

    private static Tile[,] GetInputGrid(string[] lines)
    {
        var rows = lines.Length;
        var columns = lines[0].Length;
        var grid = new Tile[rows, columns];

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                grid[row, col] = new Tile(lines[row][col]);
            }
        }

        return grid;
    }

    private static bool IsRightLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.HorizontalPipe
        or TileShape.BottomLeftCorner
        or TileShape.TopLeftCorner;

    private static bool IsLeftLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.HorizontalPipe
        or TileShape.BottomRightCorner
        or TileShape.TopRightCorner;

    private static bool IsBottomLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.VerticalPipe
        or TileShape.TopRightCorner
        or TileShape.TopLeftCorner;

    private static bool IsTopLinked(TileShape shape) =>
        shape is TileShape.Start
        or TileShape.VerticalPipe
        or TileShape.BottomRightCorner
        or TileShape.BottomLeftCorner;

    private static List<Tile> GetLinkedNeighbours(List<(Tile? tile, Predicate<TileShape> condition)> neighbours) => neighbours
                .Where(neighbour => neighbour.tile is not null
                    && neighbour.condition(neighbour.tile.Shape))
                .Select(neighbour => neighbour.tile!)
                .ToList();

    private class Tile
    {
        public TileShape Shape { get; }
        public Tile? Neighbour1 { get; private set; }
        public Tile? Neighbour2 { get; private set; }

        public Tile(char tileShape)
        {
            Shape = (TileShape)tileShape;
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
        Empty = '.',
        VerticalPipe = '|',
        HorizontalPipe = '-',
        BottomLeftCorner = 'L',
        BottomRightCorner = 'J',
        TopLeftCorner = 'F',
        TopRightCorner = '7',
        Start = 'S'
    }
}