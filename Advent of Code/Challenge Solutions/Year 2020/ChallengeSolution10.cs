// Task: https://adventofcode.com/2020/day/10

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution10 : ChallengeSolution<ChallengeSolution10>
{
    private static List<long> numbers = new List<long>();

    public ChallengeSolution10(IConsole console, ISolutionReader<ChallengeSolution10> reader) : base(console, reader)
    {
        var lines = Reader.ReadLines();
        numbers.Add(0);

        foreach (string line in lines)
        {
            numbers.Add(Convert.ToInt64(line));
        }

        numbers.Sort();
        numbers.Add(numbers[numbers.Count - 1] + 3);
    }

    public override void SolveFirstPart()
    {
        int oneJolt = 0, twoJolts = 0, threeJolts = 0;
        for (int i = 1; i < numbers.Count; i++)
        {
            long dif = numbers[i] - numbers[i - 1];
            if (dif == 1)
                oneJolt++;
            else if (dif == 2)
                twoJolts++;
            else if (dif == 3)
                threeJolts++;
        }

        Console.WriteLine(oneJolt * threeJolts);
    }

    public override void SolveSecondPart()
    {
        var limit = numbers[numbers.Count - 1] + 1;
        long[] paths = new long[limit];
        paths[0] = 1;

        for (var i = 0; i < limit; i++)
        {
            for (var x = 1; x <= 3; x++)
            {
                var sum = i - x;
                if (numbers.Contains(sum))
                    paths[i] += paths[sum];
            }
        }

        Console.WriteLine(paths[paths.Length - 1]);
    }
}
