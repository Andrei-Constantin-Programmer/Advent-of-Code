using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution17 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var heatLossMap = ReadHeatLossMap();
        var leastSpillage = FindLeastSpillage(heatLossMap, new(0, 3));

        Console.WriteLine(leastSpillage);
    }

    protected override void SolveSecondPart()
    {
        var heatLossMap = ReadHeatLossMap();
        var leastSpillage = FindLeastSpillage(heatLossMap, new(4, 10));

        Console.WriteLine(leastSpillage);
    }

    private static int FindLeastSpillage(byte[,] heatLossMap, Crucible crucible)
    {
        Point finalPoint = new(heatLossMap.GetLength(0) - 1, heatLossMap.GetLength(1) - 1);

        PriorityQueue<Block, int> blocksBySpillage = new();
        HashSet<(Point current, Point next, int consecutiveSteps)> checkedConfigurations = new();

        blocksBySpillage.Enqueue(new(new(0, 0), Direction.South, 1), 0);
        blocksBySpillage.Enqueue(new(new(0, 0), Direction.East, 1), 0);

        while (blocksBySpillage.TryDequeue(out var block, out var spillage))
        {
            var hasReachedMinimumTurningPoint = block.ConsecutiveSteps + 1 >= crucible.MinimumTurningBlocks;

            if (block.Point == finalPoint && hasReachedMinimumTurningPoint)
            {
                return spillage;
            }

            var neighbours = GetNeighbouringBlocks(heatLossMap, block);
            if (!hasReachedMinimumTurningPoint)
            {
                neighbours.RemoveAll(neighbour => neighbour.direction != block.Direction);
            }

            foreach (var (nextPoint, nextDirection) in neighbours)
            {
                var consecutiveSteps = nextDirection == block.Direction
                    ? block.ConsecutiveSteps + 1
                    : 0;

                if (consecutiveSteps >= crucible.MaximumTurningBlocks)
                {
                    continue;
                }

                var notChecked = checkedConfigurations.Add((block.Point, nextPoint, consecutiveSteps));
                if (notChecked)
                {
                    blocksBySpillage.Enqueue(
                        new Block(nextPoint, nextDirection, consecutiveSteps),
                        spillage + heatLossMap[nextPoint.Row, nextPoint.Column]);
                }
            }
        }

        return -1;
    }

    private static List<(Point point, Direction direction)> GetNeighbouringBlocks(byte[,] heatLossMap, Block block)
    {
        List<(Point point, Direction direction)> neighbours = new();

        if (block.Direction != Direction.South && block.Point.Row > 0)
        {
            neighbours.Add((new(block.Point.Row - 1, block.Point.Column), Direction.North));
        }

        if (block.Direction != Direction.North && block.Point.Row < heatLossMap.GetLength(0) - 1)
        {
            neighbours.Add((new(block.Point.Row + 1, block.Point.Column), Direction.South));
        }

        if (block.Direction != Direction.East && block.Point.Column > 0)
        {
            neighbours.Add((new(block.Point.Row, block.Point.Column - 1), Direction.West));
        }

        if (block.Direction != Direction.West && block.Point.Column < heatLossMap.GetLength(1) - 1)
        {
            neighbours.Add((new(block.Point.Row, block.Point.Column + 1), Direction.East));
        }

        return neighbours;
    }

    private byte[,] ReadHeatLossMap()
    {
        var lines = Reader.ReadLines(this);
        var heatLossMap = new byte[lines.Length, lines[0].Length];

        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[0].Length; col++)
            {
                heatLossMap[row, col] = (byte)(lines[row][col] - '0');
            }
        }

        return heatLossMap;
    }

    private class Block
    {
        public Point Point { get; init; }
        public Direction Direction { get; }
        public int ConsecutiveSteps { get; }
        
        public Block(Point point, Direction direction, int consecutiveSteps)
        {
            Point = point;
            Direction = direction;
            ConsecutiveSteps = consecutiveSteps;
        }
    }

    private enum Direction
    {
        North, West, East, South, None
    }

    private record Point(int Row, int Column);

    private record Crucible(int MinimumTurningBlocks, int MaximumTurningBlocks);
}
