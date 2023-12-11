using Advent_of_Code.Utilities;
using System.Data;
using System.Text.RegularExpressions;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution10 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var lines = Reader.ReadLines(this);
        var grid = GetInputGrid(lines);
        var startTile = GetStartTile(grid, out var _);
        
        Console.WriteLine(ComputeStepsToFarthestElement(startTile));
    }

    protected override void SolveSecondPart()
    {
        var lines = Reader.ReadLines(this);
        var tileGrid = GetInputGrid(lines);
        var startTile = GetStartTile(tileGrid, out var startPosition);
        var knotGrid = GetKnotFormForGrid(lines, tileGrid, startTile, startPosition);
        RemoveJunkPipes(tileGrid, startTile, knotGrid);

        var enclosedTiles = FindEnclosedTileCount(knotGrid);

        Console.WriteLine(enclosedTiles);
    }

    private static void RemoveJunkPipes(Tile[,] tileGrid, Tile startTile, char[,] knotGrid)
    {
        var tilesInLoop = GetTilesInLoop(startTile);
        
        for (var i = 0; i < knotGrid.GetLength(0); i++)
        {
            for (var j = 0; j < knotGrid.GetLength(1); j++)
            {
                if (!tilesInLoop.Contains(tileGrid[i, j]))
                {
                    knotGrid[i, j] = (char)TileShape.Empty;
                }
            }
        }
    }

    private static HashSet<Tile> GetTilesInLoop(Tile startTile)
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

    private static int FindEnclosedTileCount(char[,] knotGrid)
    {
        var enclosedTiles = 0;

        for (var i = 0; i < knotGrid.GetLength(0); i++)
        {
            var knotsCrossed = 0;

            for (var j = 0; j < knotGrid.GetLength(1); j++)
            {
                var character = knotGrid[i, j];
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

    private static char[,] GetKnotFormForGrid(string[] lines, Tile[,] grid, Tile startTile, (int row, int column) startPosition)
    {
        TileShape startShape = GetStartTileShape(grid, startTile, startPosition);

        List<string> knotLines = new(lines);
        for (var i = 0; i < knotLines.Count; i++)
        {
            knotLines[i] = knotLines[i].Replace((char)TileShape.Start, (char)startShape);
            knotLines[i] = RemoveNarrowCorridors(knotLines[i]);
            knotLines[i] = CollapseClosingHorizontalPipeSegments(knotLines[i]);
        }

        var knotGrid = new char[knotLines.Count, knotLines[0].Length];
        for (var i = 0; i < knotGrid.GetLength(0); i++)
        {
            for (var j = 0; j < knotGrid.GetLength(1); j++)
            {
                knotGrid[i, j] = knotLines[i][j];
            }
        }

        return knotGrid;

        static string RemoveNarrowCorridors(string line) =>
            Regex.Replace(line, "F-*7|L-*J", match => new string(' ', match.Length));

        static string CollapseClosingHorizontalPipeSegments(string line) =>
            Regex.Replace(line, "F-*J|L-*7", match => $"|{new string(' ', match.Length - 1)}");
    }

    private static TileShape GetStartTileShape(Tile[,] grid, Tile startTile, (int row, int column) startPosition)
    {
        Tile? leftNeighbour = startPosition.column - 1 >= 0 ? grid[startPosition.row, startPosition.column - 1] : null;
        Tile? rightNeighbour = startPosition.column + 1 < grid.GetLength(1) ? grid[startPosition.row, startPosition.column + 1] : null;
        Tile? topNeighbour = startPosition.row - 1 >= 0 ? grid[startPosition.row - 1, startPosition.column] : null;
        Tile? bottomNeighbour = startPosition.row + 1 < grid.GetLength(0) ? grid[startPosition.row + 1, startPosition.column] : null;

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

        static void InitialiseNeighbours(Tile[,] grid, out (Tile tile, (int row, int column) position) start)
        {
            start = (grid[0, 0], (0, 0));
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    var currentTile = grid[i, j];

                    if (currentTile.Shape is TileShape.Start)
                    {
                        start = (currentTile, (i, j));
                    }

                    var neighbours = GetNeighbours(grid, i, j);

                    foreach (var neighbour in neighbours)
                    {
                        currentTile.AddNeighbour(neighbour);
                        neighbour.AddNeighbour(currentTile);
                    }
                }
            }
        }

        static List<Tile> GetNeighbours(Tile[,] grid, int row, int column)
        {
            (Tile?, Predicate<TileShape>) topNeighbour = (
                row - 1 >= 0 ? grid[row - 1, column] : null,
                IsBottomLinked);
            (Tile?, Predicate<TileShape>) bottomNeighbour = (
                row + 1 < grid.GetLength(0) ? grid[row + 1, column] : null,
                IsTopLinked);
            (Tile?, Predicate<TileShape>) leftNeighbour = (
                column - 1 >= 0 ? grid[row, column - 1] : null,
                IsRightLinked);
            (Tile?, Predicate<TileShape>) rightNeighbour = (
                column + 1 < grid.GetLength(1) ? grid[row, column + 1] : null,
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
            };
        }

        static List<Tile> GetLinkedNeighbours(List<(Tile? tile, Predicate<TileShape> condition)> neighbours) => neighbours
            .Where(neighbour => neighbour.tile is not null
                && neighbour.condition(neighbour.tile.Shape))
            .Select(neighbour => neighbour.tile!)
            .ToList();

        static bool IsTopLinked(TileShape shape) =>
            shape is TileShape.Start
            or TileShape.VerticalPipe
            or TileShape.BottomRightCorner
            or TileShape.BottomLeftCorner;

        static bool IsBottomLinked(TileShape shape) =>
            shape is TileShape.Start
            or TileShape.VerticalPipe
            or TileShape.TopRightCorner
            or TileShape.TopLeftCorner;

        static bool IsLeftLinked(TileShape shape) =>
            shape is TileShape.Start
            or TileShape.HorizontalPipe
            or TileShape.BottomRightCorner
            or TileShape.TopRightCorner;

        static bool IsRightLinked(TileShape shape) =>
            shape is TileShape.Start
            or TileShape.HorizontalPipe
            or TileShape.BottomLeftCorner
            or TileShape.TopLeftCorner;

    }

    private static Tile[,] GetInputGrid(string[] lines)
    {
        var rows = lines.Length;
        var columns = lines[0].Length;
        var grid = new Tile[rows, columns];

        var counter = 0;
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                grid[i, j] = new Tile(counter++, lines[i][j]);
            }
        }

        return grid;
    }

    private class Tile
    {
        public int Id { get; }
        public TileShape Shape { get; }
        public Tile? Neighbour1 { get; private set; }
        public Tile? Neighbour2 { get; private set; }

        public Tile(int id, char tileShape)
        {
            Id = id;
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