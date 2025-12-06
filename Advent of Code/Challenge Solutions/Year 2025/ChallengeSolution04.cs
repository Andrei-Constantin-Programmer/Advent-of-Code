// Task: https://adventofcode.com/2025/day/4

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

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

        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid.Length; col++)
            {
                if (IsAccessiblePaperRollPosition(grid, row, col))
                {
                    reachablePaperRolls++;
                }
            }
        }

        Console.WriteLine($"Reachable paper rolls: {reachablePaperRolls}");
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

            for (var row = 0; row < grid.Length; row++)
            {
                for (var col = 0; col < grid.Length; col++)
                {
                    if (IsAccessiblePaperRollPosition(grid, row, col))
                    {
                        noMoreRollsAccessible = false;
                        rollsToRemove.Add((row, col));
                    }
                }
            }

            foreach (var (row, col) in rollsToRemove)
            {
                grid[row][col] = Empty;
            }

            removedPaperRolls += rollsToRemove.Count;
        }

        Console.WriteLine($"Removed paper rolls: {removedPaperRolls}");
    }

    private static bool IsAccessiblePaperRollPosition(char[][] grid, int row, int col)
    {
        if (grid[row][col] == Empty)
        {
            return false;
        }

        return GetNeighbours(grid, row, col)
            .Count(neighbour => neighbour == PaperRoll) < MaxPaperRolls;
    }

    private static IEnumerable<char> GetNeighbours(char[][] grid, int row, int col)
    {
        List<(int, int)> potentialNeighbouringPositions =
            [
                (row - 1, col),
                (row + 1, col),
                (row, col - 1),
                (row, col + 1),
                (row - 1, col - 1),
                (row - 1, col + 1),
                (row + 1, col - 1),
                (row + 1, col + 1),
            ];

        foreach (var (i, j) in potentialNeighbouringPositions)
        {
            if (i < 0 || i >= grid.Length
                || j < 0 || j >= grid[i].Length)
            {
                continue;
            }

            yield return grid[i][j];
        }
    }

    private char[][] ReadGrid() => Reader
        .ReadLines()
        .Select(line => line.ToCharArray())
        .ToArray();
}
