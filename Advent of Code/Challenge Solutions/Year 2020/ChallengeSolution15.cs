﻿// Task: https://adventofcode.com/2020/day/15

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

internal class ChallengeSolution15 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        Solution(2020);
    }

    protected override void SolveSecondPart()
    {
        Solution(30000000);
    }

    private void Solution(int n)
    {
        string input = string.Join(Environment.NewLine, Reader.ReadLines(this));
        string[] inputSeparated = input.Split(new string(","), StringSplitOptions.RemoveEmptyEntries);
        var spokenNumbers = new Dictionary<ulong, uint>();

        List<ulong> temp = new List<ulong>();

        foreach (string x in inputSeparated)
            temp.Add(Convert.ToUInt64(x));

        ulong last = temp[temp.Count - 1];
        temp.RemoveAt(temp.Count - 1);

        for (int i = 0; i < temp.Count; i++)
            spokenNumbers[temp[i]] = (uint)i + 1;

        for (uint turn = (uint)temp.Count + 1; turn < n; turn++)
        {
            if (spokenNumbers.TryGetValue(last, out uint lastTurn))
            {
                uint current = turn - lastTurn;
                spokenNumbers[last] = turn;
                last = current;
            }
            else
            {
                spokenNumbers[last] = turn;
                last = 0;
            }
        }

        Console.WriteLine(last);
    }
}
