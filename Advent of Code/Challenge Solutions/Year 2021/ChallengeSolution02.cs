﻿// Task: https://adventofcode.com/2021/day/2

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution02(IConsole console, ISolutionReader<ChallengeSolution02> reader)
    : ChallengeSolution<ChallengeSolution02>(console, reader)
{
    public override void SolveFirstPart()
    {
        int depth = 0, horizontal = 0;
        foreach (var line in _reader.ReadLines())
        {
            string[] command = line.Split(" ");
            int x = Convert.ToInt32(command[1]);
            if (command[0].Equals("forward"))
                horizontal += x;
            else if (command[0].Equals("down"))
                depth += x;
            else if (command[0].Equals("up"))
                depth -= x;
        }

        _console.WriteLine(depth * horizontal);
    }

    public override void SolveSecondPart()
    {
        int depth = 0, horizontal = 0, aim = 0;

        foreach (var line in _reader.ReadLines())
        {
            string[] command = line.Split(" ");
            int x = Convert.ToInt32(command[1]);
            if (command[0].Equals("forward"))
            {
                horizontal += x;
                depth += x * aim;
            }
            else if (command[0].Equals("down"))
                aim += x;
            else if (command[0].Equals("up"))
                aim -= x;
        }


        _console.WriteLine(depth * horizontal);
    }
}
