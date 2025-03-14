﻿// Task: https://adventofcode.com/2022/day/1

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution01(IConsole console, ISolutionReader<ChallengeSolution01> reader)
    : ChallengeSolution<ChallengeSolution01>(console, reader)
{
    public override void SolveFirstPart()
    {
        _console.WriteLine(GetElfCaloriesList().First());
    }

    public override void SolveSecondPart()
    {
        _console.WriteLine(GetElfCaloriesList().GetRange(0, 3).Sum());
    }

    private List<int> GetElfCaloriesList()
    {
        var elfCalories = new List<int>();
        int currentCaloriesSum = 0;
        foreach (var line in _reader.ReadLines().Select(line => line.Trim()))
        {
            if (line.Length == 0)
            {
                elfCalories.Add(currentCaloriesSum);
                currentCaloriesSum = 0;
            }
            else
            {
                currentCaloriesSum += Convert.ToInt32(line);
            }
        }

        elfCalories = elfCalories.OrderByDescending(x => x).ToList();
        return elfCalories;
    }
}
