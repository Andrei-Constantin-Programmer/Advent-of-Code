﻿// Task: https://adventofcode.com/2021/day/13

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution13(IConsole console, ISolutionReader<ChallengeSolution13> reader)
    : ChallengeSolution<ChallengeSolution13>(console, reader)
{
    private int n, m;
    private char[,] matrix;

    public override void SolveFirstPart()
    {
        var points = new List<Point>();

        var lines = _reader.ReadLines();
        var foldPosition = lines.ToList().IndexOf(string.Empty);
        var pointLines = lines[..foldPosition];
        var foldLines = lines[(foldPosition + 1)..];

        foreach (var line in pointLines)
        {
            if (line.Trim().Equals(""))
                break;
            string[] values = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
            points.Add(new Point(int.Parse(values[1]), int.Parse(values[0])));
        }

        CreateMatrix(points);

        SolveFold(foldLines[0]);

        int dots = 0;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (matrix[i, j] == '#')
                    dots++;

        _console.WriteLine(dots);
    }

    public override void SolveSecondPart()
    {
        var points = new List<Point>();
        var lines = _reader.ReadLines();
        var foldPosition = lines.ToList().IndexOf(string.Empty);
        var pointLines = lines[..foldPosition];
        var foldLines = lines[(foldPosition + 1)..];

        foreach (var line in pointLines)
        {
            if (line.Trim().Equals(""))
                break;

            string[] values = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
            points.Add(new Point(int.Parse(values[1]), int.Parse(values[0])));
        }

        CreateMatrix(points);

        foreach (var line in foldLines)
            SolveFold(line);

        for (int i = 0; i < n; i++)
        {
            string l = "";
            for (int j = 0; j < m; j++)
            {
                l += matrix[i, j] == '.' ? " " : "#" + "";
            }

            _console.WriteLine(l);
        }
    }

    private void CreateMatrix(List<Point> points)
    {
        var sorted = new List<Point>(points);
        sorted.Sort((p1, p2) => p1.x.CompareTo(p2.x));
        n = sorted[sorted.Count - 1].x + 1;
        sorted.Sort((p1, p2) => p1.y.CompareTo(p2.y));
        m = sorted[sorted.Count - 1].y + 1;

        matrix = new char[n, m];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                matrix[i, j] = '.';

        foreach (var p in points)
        {
            matrix[p.x, p.y] = '#';
        }
    }

    private void SolveFold(string line)
    {
        string foldAlong = line.Substring(line.IndexOf('y') != -1 ? line.IndexOf('y') : line.IndexOf('x'));
        string[] split = foldAlong.Split('=', StringSplitOptions.RemoveEmptyEntries);
        char along = split[0][0];
        int value = int.Parse(split[1]);

        Fold(along, value);
        //printMatrix();
    }

    private void Fold(char along, int value)
    {
        if (along == 'x')
        {
            var newMatrix = new char[n, value];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < value; j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                    if (newMatrix[i, j] == '.' && (2 * value - j < m))
                        newMatrix[i, j] = matrix[i, 2 * value - j];
                }

            m = value;
            matrix = newMatrix;
        }
        else
        {
            var newMatrix = new char[value, m];

            for (int i = 0; i < value; i++)
                for (int j = 0; j < m; j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                    if (newMatrix[i, j] == '.' && (2 * value - i < n))
                        newMatrix[i, j] = matrix[2 * value - i, j];
                }

            n = value;
            matrix = newMatrix;
        }
    }


    private void PrintMatrix()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
                _console.Write(matrix[i, j]);
            _console.WriteLine();
        }
        _console.WriteLine();
    }
}

class Point
{
    public int x { get; }
    public int y { get; }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + x + " " + y + ")";
    }
}
