// Task: https://adventofcode.com/2020/day/3

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution03 : ChallengeSolution<ChallengeSolution03>
{
    private string[] lines;

    public ChallengeSolution03(IConsole console, ISolutionReader<ChallengeSolution03> reader) : base(console, reader)
    {
        lines = _reader.ReadLines();
    }

    public override void SolveFirstPart()
    {
        _console.WriteLine(CountTrees(lines));
    }

    public override void SolveSecondPart()
    {
        int p = CountTreesDownOne(lines);
        p *= CountTreesDownTwo(lines);
        _console.WriteLine(p);
    }

    private static int CountTrees(string[] lines)
    {
        int n = lines.Length, m = lines[0].Length;
        int trees = 0;
        int pos = 0;

        for (int line = 0; line < n; line++)
        {
            pos = pos >= m ? pos - m : pos;
            if (lines[line][pos] == '#')
            {
                trees++;
            }

            pos += 3;
        }

        return trees;
    }

    private static int CountTreesDownOne(string[] lines)
    {
        int n = lines.Length, m = lines[0].Length;
        int trees1 = 0, trees3 = 0, trees5 = 0, trees7 = 0;
        int pos1 = 0, pos3 = 0, pos5 = 0, pos7 = 0;

        for (int line = 0; line < n; line++)
        {
            pos1 = pos1 >= m ? pos1 - m : pos1;
            pos3 = pos3 >= m ? pos3 - m : pos3;
            pos5 = pos5 >= m ? pos5 - m : pos5;
            pos7 = pos7 >= m ? pos7 - m : pos7;

            if (lines[line][pos1] == '#')
            {
                trees1++;
            }
            if (lines[line][pos3] == '#')
            {
                trees3++;
            }
            if (lines[line][pos5] == '#')
            {
                trees5++;
            }
            if (lines[line][pos7] == '#')
            {
                trees7++;
            }

            pos1++;
            pos3 += 3;
            pos5 += 5;
            pos7 += 7;
        }

        return trees1 * trees3 * trees5 * trees7;
    }

    private static int CountTreesDownTwo(string[] lines)
    {
        int n = lines.Length, m = lines[0].Length;
        int trees = 0;
        int pos = 0;

        for (int line = 0; line < n; line += 2)
        {
            pos = pos >= m ? pos - m : pos;
            if (lines[line][pos] == '#')
            {
                trees++;
            }

            pos++;
        }

        return trees;
    }
}
