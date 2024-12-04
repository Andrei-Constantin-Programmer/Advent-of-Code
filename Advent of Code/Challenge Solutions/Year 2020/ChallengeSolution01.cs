// Task: https://adventofcode.com/2020/day/1

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution01 : ChallengeSolution
{
    private static int[] a;

    public ChallengeSolution01(IConsole console) : base(console)
    {
        string[] lines = Reader.ReadLines(this);
        a = new int[lines.Length];
        for (int i = 0; i < lines.Length; i++)
            a[i] = Convert.ToInt32(lines[i]);
    }

    public override void SolveFirstPart()
    {
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i; j < a.Length; j++)
            {
                if (a[i] + a[j] == 2020)
                    _console.WriteLine(a[i] * a[j]);
            }
        }
    }

    public override void SolveSecondPart()
    {
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i; j < a.Length; j++)
            {
                for (int l = j; l < a.Length; l++)
                {
                    if (a[i] + a[j] + a[l] == 2020)
                        _console.WriteLine(a[i] * a[j] * a[l]);
                }
            }
        }
    }
}
