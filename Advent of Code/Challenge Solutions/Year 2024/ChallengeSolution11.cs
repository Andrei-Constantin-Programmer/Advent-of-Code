// Task: https://adventofcode.com/2024/day/11

using Advent_of_Code.Utilities;

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
        var stones = new Queue<long>(ReadStones());
        Dictionary<long, List<long>> stoneStore = [];

        for (var blink = 0; blink < blinks; blink++)
        {
            var stoneCount = stones.Count;
            ComputeNextStoneSet(stones, stoneStore, stoneCount);
        }

        _console.WriteLine(stones.Count);
    }

    private static void ComputeNextStoneSet(Queue<long> stones, Dictionary<long, List<long>> stoneStore, int stoneCount)
    {
        for (var i = 0; i < stoneCount; i++)
        {
            var stone = stones.Dequeue();

            if (stoneStore.TryGetValue(stone, out var results))
            {
                foreach (var result in results)
                {
                    stones.Enqueue(result);
                }

                continue;
            }

            var stoneString = stone.ToString();
            var stoneLength = stoneString.Length;
            List<long> newStones = FindNewStones(stone, stoneString, stoneLength);

            foreach (var newStone in newStones)
            {
                stones.Enqueue(newStone);
            }

            stoneStore[stone] = newStones;
        }
    }

    private static List<long> FindNewStones(long stone, string stoneString, int stoneLength)
    {
        List<long> newResults = [];

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

    private List<long> ReadStones() => _reader.ReadLines()[0]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToList();
}
