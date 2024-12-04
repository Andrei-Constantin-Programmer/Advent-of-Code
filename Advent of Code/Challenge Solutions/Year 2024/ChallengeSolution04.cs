// Task: https://adventofcode.com/2024/day/4

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution04(IConsole console, ISolutionReader<ChallengeSolution04> reader)
    : ChallengeSolution<ChallengeSolution04>(console, reader)
{
    public override void SolveFirstPart()
    {
        var wordSearch = ReadWordSearch();
        var word = "XMAS";

        var count = 0;

        for (var i = 0; i < wordSearch.Length; i++)
        {
            for (var j = 0; j < wordSearch[i].Length; j++)
            {
                var rowCanBeWord = j <= wordSearch[i].Length - word.Length;
                var rowCanBeWordReverse = j >= word.Length - 1;
                var columnCanBeWord = i < wordSearch.Length - word.Length + 1;
                var columnCanBeWordReverse = i >= word.Length - 1;

                if (rowCanBeWord)
                {
                    var wordRow = wordSearch[i][j..(j + word.Length)];

                    if (wordRow == word)
                    {
                        count++;
                    }

                    if (columnCanBeWord)
                    {
                        if (wordSearch.GetDiagonalBottomRight(i, j, word.Length) == word)
                        {
                            count++;
                        }
                    }

                    if (columnCanBeWordReverse)
                    {
                        if (wordSearch.GetDiagonalTopRight(i, j, word.Length) == word)
                        {
                            count++;
                        }
                    }
                }

                if (rowCanBeWordReverse)
                {
                    var wordRowReverse = string.Concat(wordSearch[i][(j - word.Length + 1)..(j + 1)].Reverse());

                    if (wordRowReverse == word)
                    {
                        count++;
                    }

                    if (columnCanBeWord)
                    {
                        if (wordSearch.GetDiagonalBottomLeft(i, j, word.Length) == word)
                        {
                            count++;
                        }
                    }

                    if (columnCanBeWordReverse)
                    {
                        if (wordSearch.GetDiagonalTopLeft(i, j, word.Length) == word)
                        {
                            count++;
                        }
                    }
                }

                if (columnCanBeWord)
                {
                    if (wordSearch.GetColumn(i, j, word.Length) == word)
                    {
                        count++;
                    }
                }

                if (columnCanBeWordReverse)
                {
                    if (wordSearch.GetColumnReverse(i - word.Length + 1, j, word.Length) == word)
                    {
                        count++;
                    }
                }
            }
        }

        _console.WriteLine($"XMAS Count: {count}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private string[] ReadWordSearch() => _reader.ReadLines();
}

internal static class ChallengeSolution04Extensions
{
    public static string GetColumn(this string[] source, int start, int column, int count)
        => string.Concat(source
            .Skip(start)
            .Take(count)
            .Select(row => row[column]));

    public static string GetColumnReverse(this string[] source, int start, int column, int count)
        => string.Concat(source
            .Skip(start)
            .Take(count)
            .Select(row => row[column])
            .Reverse());

    public static string GetDiagonalBottomRight(this string[] source, int i, int j, int count) => new
        (Enumerable.Range(0, count)
        .Select(k => source[i + k][j + k])
        .ToArray());

    public static string GetDiagonalTopRight(this string[] source, int i, int j, int count) => new
        (Enumerable.Range(0, count)
        .Select(k => source[i - k][j + k])
        .ToArray());

    public static string GetDiagonalBottomLeft(this string[] source, int i, int j, int count) => new
        (Enumerable.Range(0, count)
        .Select(k => source[i + k][j - k])
        .ToArray());

    public static string GetDiagonalTopLeft(this string[] source, int i, int j, int count) => new
        (Enumerable.Range(0, count)
        .Select(k => source[i - k][j - k])
        .ToArray());
}