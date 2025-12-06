// Task: https://adventofcode.com/2024/day/4

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution04(IConsole console, ISolutionReader<ChallengeSolution04> reader)
    : ChallengeSolution<ChallengeSolution04>(console, reader)
{
    public override void SolveFirstPart()
    {
        var wordSearch = ReadWordSearch();
        var word = "XMAS";

        var count = 0;

        for (var row = 0; row < wordSearch.Length; row++)
        {
            for (var col = 0; col < wordSearch[row].Length; col++)
            {
                var canFitHorizontally = col <= wordSearch[row].Length - word.Length;
                var canFitHorizontallyReverse = col >= word.Length - 1;
                var canFitVertically = row < wordSearch.Length - word.Length + 1;
                var canFitVerticallyReverse = row >= word.Length - 1;

                var canFitDiagonalBottomRight = canFitHorizontally && canFitVertically;
                var canFitDiagonalTopRight = canFitHorizontally && canFitVerticallyReverse;
                var canFitDiagonalBottomLeft = canFitHorizontallyReverse && canFitVertically;
                var canFitDiagonalTopLeft = canFitHorizontallyReverse && canFitVerticallyReverse;

                if (canFitHorizontally && wordSearch.GetWordHorizontally(row, col, word.Length) == word)
                {
                    count++;
                }
                if (canFitHorizontallyReverse && wordSearch.GetWordHorizontallyReverse(row, col, word.Length) == word)
                {
                    count++;
                }

                if (canFitVertically && wordSearch.GetWordVertically(row, col, word.Length) == word)
                {
                    count++;
                }
                if (canFitVerticallyReverse && wordSearch.GetWordVerticallyReverse(row, col, word.Length) == word)
                {
                    count++;
                }

                if (canFitDiagonalBottomRight && wordSearch.GetWordDiagonalBottomRight(row, col, word.Length) == word)
                {
                    count++;
                }
                if (canFitDiagonalBottomLeft && wordSearch.GetWordDiagonalBottomLeft(row, col, word.Length) == word)
                {
                    count++;
                }
                if (canFitDiagonalTopRight && wordSearch.GetWordDiagonalTopRight(row, col, word.Length) == word)
                {
                    count++;
                }
                if (canFitDiagonalTopLeft && wordSearch.GetWordDiagonalTopLeft(row, col, word.Length) == word)
                {
                    count++;
                }
            }
        }

        Console.WriteLine($"XMAS Count: {count}");
    }

    public override void SolveSecondPart()
    {
        var wordSearch = ReadWordSearch();

        var count = 0;

        for (var row = 1; row < wordSearch.Length - 1; row++)
        {
            for (var col = 1; col < wordSearch[row].Length - 1; col++)
            {
                if (wordSearch[row][col] != 'A')
                {
                    continue;
                }

                var topLeft = wordSearch[row - 1][col - 1];
                var topRight = wordSearch[row - 1][col + 1];
                var bottomLeft = wordSearch[row + 1][col - 1];
                var bottomRight = wordSearch[row + 1][col + 1];

                if (topLeft == topRight && topLeft == 'M'
                    && bottomLeft == bottomRight && bottomLeft == 'S')
                {
                    count++;
                }
                else if (topRight == bottomRight && topRight == 'M'
                    && topLeft == bottomLeft && topLeft == 'S')
                {
                    count++;
                }
                else if (bottomRight == bottomLeft && bottomRight == 'M'
                    && topRight == topLeft && topRight == 'S')
                {
                    count++;
                }
                else if (bottomLeft == topLeft && bottomLeft == 'M'
                    && bottomRight == topRight && bottomRight == 'S')
                {
                    count++;
                }
            }
        }

        Console.WriteLine($"X-MAS Count: {count}");
    }

    private string[] ReadWordSearch() => Reader.ReadLines();
}

internal static class ChallengeSolution04Extensions
{
    public static string GetWordHorizontally(this string[] source, int row, int col, int length)
        => source[row][col..(col + length)];

    public static string GetWordHorizontallyReverse(this string[] source, int row, int col, int length)
        => string.Concat(source[row][(col - length + 1)..(col + 1)].Reverse());

    public static string GetWordVertically(this string[] source, int row, int col, int length)
        => string.Concat(source
            .Skip(row)
            .Take(length)
            .Select(row => row[col]));

    public static string GetWordVerticallyReverse(this string[] source, int row, int col, int length)
        => new(Enumerable.Range(0, length)
            .Select(k => source[row - k][col])
            .ToArray());

    public static string GetWordDiagonalBottomRight(this string[] source, int row, int col, int length)
        => new(Enumerable.Range(0, length)
            .Select(k => source[row + k][col + k])
            .ToArray());

    public static string GetWordDiagonalTopRight(this string[] source, int row, int col, int length)
        => new(Enumerable.Range(0, length)
            .Select(k => source[row - k][col + k])
            .ToArray());

    public static string GetWordDiagonalBottomLeft(this string[] source, int row, int col, int length)
        => new(Enumerable.Range(0, length)
            .Select(k => source[row + k][col - k])
            .ToArray());

    public static string GetWordDiagonalTopLeft(this string[] source, int row, int col, int length)
        => new(Enumerable.Range(0, length)
            .Select(k => source[row - k][col - k])
            .ToArray());
}