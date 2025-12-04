// Task: https://adventofcode.com/2025/day/4

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution04(IConsole console, ISolutionReader<ChallengeSolution04> reader)
    : ChallengeSolution<ChallengeSolution04>(console, reader)
{
    private const char Empty = '.';
    private const char PaperRoll = '@';
    private const int MaxPaperRolls = 4;

    public override void SolveFirstPart()
    {
        var grid = ReadGrid();

        var reachablePaperRolls = 0;

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid.Length; j++)
            {
                if (grid[i][j] == Empty)
                {
                    continue;
                }

                var neighbours = GetNeighbours(grid, i, j).ToList();

                if (neighbours.Count(neighbour => neighbour == PaperRoll) < MaxPaperRolls)
                {
                    reachablePaperRolls++;
                }
            }
        }

        _console.WriteLine($"Reachable paper rolls: {reachablePaperRolls}");
    }

    public override void SolveSecondPart()
    {
        var grid = ReadGrid();

        var removedPaperRolls = 0;

        var noMoreRollsAccessible = false;
        while (!noMoreRollsAccessible)
        {
            noMoreRollsAccessible = true;
            List<(int, int)> rollsToRemove = [];

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid.Length; j++)
                {
                    if (grid[i][j] == Empty)
                    {
                        continue;
                    }

                    var neighbours = GetNeighbours(grid, i, j).ToList();

                    if (neighbours.Count(neighbour => neighbour == PaperRoll) < MaxPaperRolls)
                    {
                        noMoreRollsAccessible = false;
                        rollsToRemove.Add((i, j));
                    }
                }
            }

            foreach (var (row, col) in rollsToRemove)
            {
                grid[row][col] = Empty;
            }

            removedPaperRolls += rollsToRemove.Count;
        }

        _console.WriteLine($"Removed paper rolls: {removedPaperRolls}");
    }

    private static IEnumerable<char> GetNeighbours(char[][] grid, int i, int j)
    {
        List<(int, int)> potentialNeighbouringPositions =
            [
                (i - 1, j),
                (i + 1, j),
                (i, j - 1),
                (i, j + 1),
                (i - 1, j - 1),
                (i - 1, j + 1),
                (i + 1, j - 1),
                (i + 1, j + 1),
            ];

        foreach (var (row, col) in potentialNeighbouringPositions)
        {
            if (row < 0 || row >= grid.Length
                || col < 0 || col >= grid[row].Length)
            {
                continue;
            }

            yield return grid[row][col];
        }
    }

    private char[][] ReadGrid() => _reader
        .ReadLines()
        .Select(line => line.ToCharArray())
        .ToArray();
}
