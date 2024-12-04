// Task: https://adventofcode.com/2024/day/1

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution01 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        (var leftList, var rightList) = ReadLocationIds();

        int totalDistance = leftList
            .Order()
            .Zip(rightList.Order(), (x, y) => Math.Abs(x - y))
            .Sum();

        Console.WriteLine($"Total distance: {totalDistance}");
    }

    protected override void SolveSecondPart()
    {
        (var leftList, var rightList) = ReadLocationIds();

        var similarityScore = leftList
            .Sum(x => x * rightList.Count(y => y == x));

        Console.WriteLine($"Similarity score: {similarityScore}");
    }

    private (List<int>, List<int>) ReadLocationIds()
    {
        List<int> leftList = [];
        List<int> rightList = [];

        try
        {
            foreach (var line in Reader.ReadLines(this))
            {
                var values = line
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                leftList.Add(values[0]);
                rightList.Add(values[1]);
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error reading location IDs", e);
        }

        return (leftList, rightList);
    }
}
