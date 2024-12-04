﻿using Advent_of_Code.Challenge_Solutions;
using static Advent_of_Code.Utilities.PathUtils;

namespace Advent_of_Code.Utilities;

public class SolutionReader<TSolution> : ISolutionReader<TSolution>
    where TSolution : ChallengeSolution
{
    public string[] ReadLines()
    {
        var (year, day) = GetYearAndDayFromSolution();

        return File.ReadAllLines(GetFilePath(FileType.Input, year, day));
    }

    public TextReader GetInputFile()
    {
        var (year, day) = GetYearAndDayFromSolution();

        return File.OpenText(GetFilePath(FileType.Input, year, day));
    }

    private static (int year, int day) GetYearAndDayFromSolution()
    {
        var solutionType = typeof(TSolution);

        var year = int.Parse(solutionType.Namespace![^4..]);
        var day = int.Parse(new string(
            solutionType.Name
            .Reverse()
            .TakeWhile(char.IsDigit)
            .Reverse()
            .ToArray()));

        return (year, day);
    }
}
