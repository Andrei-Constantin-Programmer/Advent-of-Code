// Task: https://adventofcode.com/2021/day/7

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution07(IConsole console, ISolutionReader<ChallengeSolution07> reader)
    : ChallengeSolution<ChallengeSolution07>(console, reader)
{
    public override void SolveFirstPart()
    {
        Solution(false);
    }

    public override void SolveSecondPart()
    {
        Solution(true);
    }

    private void Solution(bool increasingFuel)
    {
        List<int> initialPositions = new List<int>(Array.ConvertAll(_reader.ReadLines()[0].Split(",", StringSplitOptions.RemoveEmptyEntries), int.Parse));
        Dictionary<int, int> fishByPosition = new Dictionary<int, int>();
        foreach (var x in initialPositions)
        {
            if (fishByPosition.ContainsKey(x))
                fishByPosition[x]++;
            else
                fishByPosition.Add(x, 1);
        }

        int minFuel = int.MaxValue, minPos = -1;
        for (int i = 0; i < fishByPosition.Count; i++)
        {
            int sum = 0;
            if (increasingFuel)
                foreach (var x in fishByPosition)
                    sum += x.Value * GetUsedFuel(i, x.Key);
            else
                foreach (var x in fishByPosition)
                    sum += x.Value * Math.Abs(x.Key - i);

            if (sum < minFuel)
            {
                minFuel = sum;
                minPos = i;
            }
        }

        _console.WriteLine(minFuel);
    }

    private int GetUsedFuel(int start, int end)
    {
        if (start > end)
        {
            int aux = start;
            start = end;
            end = aux;
        }

        int no = 1, sum = 0;
        while (start < end)
        {
            sum += no;
            no++;
            start++;
        }

        return sum;
    }
}
