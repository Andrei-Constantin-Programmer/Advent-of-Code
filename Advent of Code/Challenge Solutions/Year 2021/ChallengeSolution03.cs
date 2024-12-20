﻿// Task: https://adventofcode.com/2021/day/3

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution03(IConsole console, ISolutionReader<ChallengeSolution03> reader)
    : ChallengeSolution<ChallengeSolution03>(console, reader)
{
    public override void SolveFirstPart()
    {
        var bits = Enumerable.Range(0, 12).Select(x => new BitAmmount { }).ToList();

        foreach (var line in _reader.ReadLines())
        {
            for (int i = 0; i < line.Length; i++)
                if (line[i] == '0')
                    bits[i].zeroes++;
                else
                    bits[i].ones++;
        }


        string gamma = "", epsilon = "";
        foreach (var bit in bits)
        {
            gamma += bit.GetHighestChar();
            epsilon += bit.GetLowestChar();
        }

        _console.WriteLine(Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2));
    }

    public override void SolveSecondPart()
    {
        string[] lines = _reader.ReadLines();
        var O2 = new List<string>(lines);
        var CO2 = new List<string>(lines);

        var bits = Enumerable.Range(0, 12).Select(x => new BitAmmount { }).ToList();
        int index = 0;
        while (O2.Count > 1)
        {
            foreach (var o2 in O2)
            {
                if (o2[index] == '1')
                    bits[index].ones++;
                else
                    bits[index].zeroes++;
            }

            for (int i = 0; i < O2.Count; i++)
            {
                if (O2[i][index] != bits[index].GetHighestChar())
                {
                    O2.RemoveAt(i);
                    i--;
                }
            }

            index++;
        }

        bits = Enumerable.Range(0, 12).Select(x => new BitAmmount { }).ToList();
        index = 0;
        while (CO2.Count > 1)
        {
            foreach (var co2 in CO2)
            {
                if (co2[index] == '0')
                    bits[index].zeroes++;
                else
                    bits[index].ones++;
            }

            for (int i = 0; i < CO2.Count; i++)
            {
                if (CO2[i][index] != bits[index].GetLowestChar())
                {
                    CO2.RemoveAt(i);
                    i--;
                }
            }

            index++;
        }

        _console.WriteLine(Convert.ToInt32(O2[0], 2) * Convert.ToInt32(CO2[0], 2));
    }
}

class BitAmmount
{
    public int zeroes { get; set; }
    public int ones { get; set; }

    public char GetHighestChar()
    {
        return Math.Max(ones, zeroes) == ones ? '1' : '0';
    }

    public char GetLowestChar()
    {
        return Math.Min(ones, zeroes) == zeroes ? '0' : '1';
    }
}
