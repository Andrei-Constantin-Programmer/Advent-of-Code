﻿// Task: https://adventofcode.com/2020/day/9

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution09 : ChallengeSolution<ChallengeSolution09>
{
    private List<long> numbers;
    private string[] lines;

    public ChallengeSolution09(IConsole console, ISolutionReader<ChallengeSolution09> reader) : base(console, reader)
    {
        numbers = new List<long>();
        lines = _reader.ReadLines();
        for (int i = 0; i < 25; i++)
        {
            numbers.Add(Convert.ToInt64(lines[i]));
        }
    }

    public override void SolveFirstPart()
    {
        bool found = false;
        for (int i = 25; i < lines.Length && !found; i++)
        {
            long x = Convert.ToInt64(lines[i]);
            if (!IsSumOfTwo(x, i))
            {
                found = true;
                _console.WriteLine(x);
            }
            else
                numbers.Add(x);
        }
    }

    public override void SolveSecondPart()
    {
        bool found = false;
        int pos = 0;
        long number = 0;
        for (int i = 25; i < lines.Length && !found; i++)
        {
            long x = Convert.ToInt64(lines[i]);
            if (!IsSumOfTwo(x, i))
            {
                found = true;
                pos = i;
                number = x;
            }
            else
                numbers.Add(x);
        }

        _console.WriteLine(GetContiguousSet(number, pos));
    }


    private bool IsSumOfTwo(long number, int index)
    {
        bool found = false;

        for (int i = index - 25; i < index - 1 && !found; i++)
            for (int j = i + 1; j < index && !found; j++)
            {
                //_console.WriteLine(numbers[i]+" + "+numbers[j]+" = "+(numbers[i]+numbers[j]));
                if (numbers[i] + numbers[j] == number)
                    found = true;
            }

        return found;
    }

    private long GetContiguousSet(long number, int index)
    {
        long min = -1, max = -1;
        bool found = false;

        for (int i = 0; i < numbers.Count && !found; i++)
        {
            min = max = numbers[i];
            int j = i + 1;
            long sum = numbers[i];
            while (sum < number && j < numbers.Count)
            {
                sum += numbers[j];
                if (numbers[j] > max)
                    max = numbers[j];
                if (numbers[j] < min)
                    min = numbers[j];
                j++;
            }
            if (sum == number)
            {
                found = true;
            }
        }

        return min + max;
    }
}
