// Task: https://adventofcode.com/2021/day/5

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution05(IConsole console) : ChallengeSolution(console)
{
    private const int n = 1000;

    public override void SolveFirstPart()
    {
        Solution(false);
    }

    public override void SolveSecondPart()
    {
        Solution(true);
    }

    private void Solution(bool diagonal)
    {
        var diagram = new int[n, n];

        foreach (var line in Reader.ReadLines(this))
        {
            string[] coordinates = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
            string[] coordinateStart = coordinates[0].Split(",");
            string[] coordinateEnd = coordinates[1].Split(",");
            int x1 = Convert.ToInt32(coordinateStart[0]);
            int y1 = Convert.ToInt32(coordinateStart[1]);
            int x2 = Convert.ToInt32(coordinateEnd[0]);
            int y2 = Convert.ToInt32(coordinateEnd[1]);

            AddToDiagram(diagram, x1, y1, x2, y2, diagonal);
        }

        _console.WriteLine(GetOverlappingPoints(diagram));
    }

    private void AddToDiagram(int[,] diagram, int x1, int y1, int x2, int y2, bool diagonal)
    {
        if (x1 == x2)
        {
            if (y1 > y2)
            {
                int aux = y1;
                y1 = y2;
                y2 = aux;
            }

            for (int i = y1; i <= y2; i++)
                diagram[i, x1]++;
        }
        else if (y1 == y2)
        {
            if (x1 > x2)
            {
                int aux = x1;
                x1 = x2;
                x2 = aux;
            }

            for (int j = x1; j <= x2; j++)
                diagram[y1, j]++;
        }
        else if (diagonal)
        {
            if (x1 > x2)
            {
                int aux = x1;
                x1 = x2;
                x2 = aux;

                aux = y1;
                y1 = y2;
                y2 = aux;
            }

            for (int i = x1, j = y1; i <= x2; i++)
            {
                diagram[j, i]++;
                if (y1 < y2)
                    j++;
                else
                    j--;
            }
        }
    }

    private void PrintDiagram(int[,] diagram)
    {
        for (int i = 0; i < Math.Sqrt(diagram.Length); i++)
        {
            for (int j = 0; j < Math.Sqrt(diagram.Length); j++)
                if (diagram[i, j] != 0)
                    _console.Write(diagram[i, j] + " ");
                else
                    _console.Write(". ");
            _console.WriteLine();
        }
        _console.WriteLine();
    }

    private int GetOverlappingPoints(int[,] diagram)
    {
        int no = 0;

        for (int i = 0; i < Math.Sqrt(diagram.Length); i++)
            for (int j = 0; j < Math.Sqrt(diagram.Length); j++)
                if (diagram[i, j] >= 2)
                    no++;

        return no;
    }
}
