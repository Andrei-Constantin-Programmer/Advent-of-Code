// Task: https://adventofcode.com/2024/day/11

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution11(IConsole console, ISolutionReader<ChallengeSolution11> reader)
    : ChallengeSolution<ChallengeSolution11>(console, reader)
{
    public override void SolveFirstPart()
    {
        SolveStoneProblem(25);
    }

    public override void SolveSecondPart()
    {
        SolveStoneProblem(75);
    }

    private void SolveStoneProblem(int blinks)
    {
        var stoneCountsPerValue = ReadStones()
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        var currentStoneCounts = stoneCountsPerValue;
        for (var blink = 0; blink < blinks; blink++)
        {
            currentStoneCounts = BlinkStones(currentStoneCounts);
        }

        var finalStoneCount = currentStoneCounts.Values.Sum();

        Console.WriteLine($"Final stone count: {finalStoneCount}");
    }

    private static Dictionary<long, long> BlinkStones(Dictionary<long, long> stoneCounts)
    {
        Dictionary<long, long> newStoneCounts = [];

        foreach (var (stone, count) in stoneCounts)
        {
            var newStones = FindNewStones(stone);
            foreach (var newStone in newStones)
            {
                if (newStoneCounts.ContainsKey(newStone))
                {
                    newStoneCounts[newStone] += count;
                }
                else
                {
                    newStoneCounts[newStone] = count;
                }
            }
        }

        return newStoneCounts;
    }

    private static List<long> FindNewStones(long stone)
    {
        List<long> newResults = [];
        var stoneString = stone.ToString();
        var stoneLength = stoneString.Length;

        if (stone == 0)
        {
            newResults.Add(1);
        }
        else if (stoneLength % 2 == 0)
        {
            var (firstHalf, secondHalf) = SplitStone(stoneString, stoneLength);

            newResults.Add(firstHalf);
            newResults.Add(secondHalf);
        }
        else
        {
            newResults.Add(stone * 2024);
        }

        return newResults;
    }

    private static (long, long) SplitStone(string stoneString, int stoneLength)
    {
        var middle = stoneLength / 2;
        var firstHalf = long.Parse(stoneString[..middle]);
        var secondHalf = long.Parse(stoneString[middle..]);

        return (firstHalf, secondHalf);
    }

    private List<long> ReadStones() => Reader.ReadLines()[0]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToList();
}
