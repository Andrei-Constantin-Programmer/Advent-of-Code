﻿// Task: https://adventofcode.com/2021/day/6

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution06(IConsole console, ISolutionReader<ChallengeSolution06> reader)
    : ChallengeSolution<ChallengeSolution06>(console, reader)
{
    public override void SolveFirstPart()
    {
        List<int> lanternfish = new List<int>(Array.ConvertAll(_reader.ReadLines()[0].Split(",", StringSplitOptions.RemoveEmptyEntries), int.Parse));

        int days = 0;

        while (days < 80)
        {
            List<int> newFish = new List<int>();
            for (int i = 0; i < lanternfish.Count; i++)
            {
                if (lanternfish[i] == 0)
                {
                    lanternfish[i] = 6;
                    newFish.Add(8);
                }
                else
                    lanternfish[i]--;
            }

            foreach (var fish in newFish)
                lanternfish.Add(fish);

            days++;
        }

        _console.WriteLine(lanternfish.Count);
    }

    public override void SolveSecondPart()
    {
        long[] fish = new long[9];
        List<int> initialFish = new List<int>(Array.ConvertAll(_reader.ReadLines()[0].Split(",", StringSplitOptions.RemoveEmptyEntries), int.Parse));
        foreach (var f in initialFish)
            fish[f]++;

        int days = 0;
        while (days < 256)
        {
            long birthingFish = fish[0];
            for (int i = 0; i < 8; i++)
                fish[i] = fish[i + 1];
            fish[8] = 0;

            fish[6] += birthingFish;
            fish[8] += birthingFish;

            days++;
        }

        long sum = 0;
        foreach (var f in fish)
            sum += f;

        _console.WriteLine(sum);
    }
}
