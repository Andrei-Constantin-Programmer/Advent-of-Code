using Advent_of_Code.Utilities;
using System.Text;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution13 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var patterns = ReadPatterns();
        Console.WriteLine(GetNoteSummary(patterns));
    }

    protected override void SolveSecondPart()
    {
        var patterns = ReadPatterns();
        Console.WriteLine(GetNoteSummary(patterns, includeSmudge: true));
    }

    private static int GetNoteSummary(List<string[]> patterns, bool includeSmudge = false)
    {
        var noteSummary = 0;
        foreach (var pattern in patterns)
        {
            var aboveHorizontalMirror = GetRowsAboveMirror(pattern, includeSmudge);
            var beforeVerticalMirror = aboveHorizontalMirror == 0 ? GetRowsAboveMirror(Transpose(pattern), includeSmudge) : 0;

            noteSummary += beforeVerticalMirror + (100 * aboveHorizontalMirror);
        }

        return noteSummary;
    }

    private static int GetRowsAboveMirror(string[] pattern, bool includeSmudge)
    {
        var mirrorRow = -1;
        for (var row = 0; row < pattern.Length - 1 && mirrorRow == -1; row++)
        {
            if (IsMirrorRow(pattern, row, includeSmudge))
            {
                mirrorRow = row;
            }
        }

        return mirrorRow >= 0 ? mirrorRow + 1 : 0;
    }

    private static bool IsMirrorRow(string[] pattern, int mirrorRow, bool includeSmudge)
    {
        var isMirror = AreRowsEqual(pattern[mirrorRow], pattern[mirrorRow + 1], includeSmudge, out var cleanedSmudge);
        var wasSmudgeCleaned = cleanedSmudge;

        if (!isMirror)
        {
            return false;
        }

        for (int rightRow = mirrorRow + 2, leftRow = mirrorRow - 1;
            rightRow < pattern.Length && leftRow >= 0;
            rightRow++, leftRow--)
        {
            if (!AreRowsEqual(pattern[rightRow], pattern[leftRow], includeSmudge && !cleanedSmudge, out cleanedSmudge))
            {
                return false;
            }

            if (cleanedSmudge)
            {
                wasSmudgeCleaned = true;
            }
        }

        return includeSmudge ? wasSmudgeCleaned : true;
    }

    private static bool AreRowsEqual(string row1, string row2, bool includeSmudge, out bool cleanedSmudge)
    {
        cleanedSmudge = false;
        if (!includeSmudge)
        {
            return row1 == row2;
        }

        for (var col = 0; col < row1.Length; col++)
        {
            if (row1[col] != row2[col])
            {
                if (cleanedSmudge)
                {
                    return false;
                }

                cleanedSmudge = true;
            }
        }

        return true;
    }

    private List<string[]> ReadPatterns()
    {
        var lines = Reader.ReadLines(this);
        List<string[]> patterns = new();
        var emptySpaceIndexes = lines
            .Select((line, index) => new { line, index })
            .Where(pair => string.IsNullOrEmpty(pair.line))
            .Select(pair => pair.index)
            .ToList();

        for (int index = 0, previousIndex = -1; index <= emptySpaceIndexes.Count; index++, previousIndex++)
        {
            var previous = previousIndex >= 0 ? emptySpaceIndexes[previousIndex] + 1 : 0;
            var current = index < emptySpaceIndexes.Count ? emptySpaceIndexes[index] : lines.Length;

            patterns.Add(lines[previous..current]);
        }

        return patterns;
    }

    private static string[] Transpose(string[] pattern)
    {
        var transposedPattern = new string[pattern[0].Length];

        for (var col = 0; col < pattern[0].Length; col++)
        {
            StringBuilder rowBuilder = new();
            for (var row = 0; row < pattern.Length; row++)
            {
                rowBuilder.Append(pattern[row][col]);
            }

            transposedPattern[col] = rowBuilder.ToString();
        }

        return transposedPattern;
    }
}
