﻿// Task: https://adventofcode.com/2020/day/6

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution06 : ChallengeSolution<ChallengeSolution06>
{
    private static bool[] questions = new bool[200];
    private static int[] groupQuestions = new int[200];
    private static string[] groups;

    public ChallengeSolution06(IConsole console, ISolutionReader<ChallengeSolution06> reader) : base(console, reader)
    {
        string lines = string.Join(Environment.NewLine, _reader.ReadLines());
        groups = lines.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
    }

    public override void SolveFirstPart()
    {
        _console.WriteLine(GetSum(groups));
    }

    public override void SolveSecondPart()
    {
        _console.WriteLine(GetSumAll(groups));
    }

    private static void CleanQuestions()
    {
        for (int i = 0; i < questions.Length; i++)
            questions[i] = false;
    }

    private static void CleanGroupQuestions()
    {
        for (int i = 0; i < groupQuestions.Length; i++)
            groupQuestions[i] = 0;
    }

    private static int GroupAnswers(string group)
    {
        int nr = 0;

        for (int i = 0; i < group.Length; i++)
        {
            char c = group[i];
            if (c >= 'a' && c <= 'z')
            {
                questions[c] = true;
            }
        }

        for (char c = 'a'; c <= 'z'; c++)
            if (questions[c])
                nr++;

        CleanQuestions();

        return nr;
    }

    private static int GroupAnswersAll(string group)
    {
        int nr = 0, members = 1;

        for (int i = 0; i < group.Length; i++)
        {
            char c = group[i];
            if (c >= 'a' && c <= 'z')
            {
                groupQuestions[c]++;
            }
            else if (c == '\n')
                members++;
        }

        for (char c = 'a'; c <= 'z'; c++)
            if (groupQuestions[c] == members && members != 0)
                nr++;

        CleanGroupQuestions();

        return nr;
    }

    private static int GetSumAll(string[] groups)
    {
        int s = 0;

        foreach (string group in groups)
            s += GroupAnswersAll(group);

        return s;
    }

    private static int GetSum(string[] groups)
    {
        int s = 0;

        foreach (string group in groups)
            s += GroupAnswers(group);

        return s;
    }
}
