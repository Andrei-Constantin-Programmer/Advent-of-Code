﻿// Task: https://adventofcode.com/2024/day/10

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution10(IConsole console, ISolutionReader<ChallengeSolution10> reader)
    : ChallengeSolution<ChallengeSolution10>(console, reader)
{
    public override void SolveFirstPart()
    {
        var topographicMap = ReadTopographicMap();
        var trailheads = FindTrailheads(topographicMap);

        var scoreSum = trailheads
            .Select(trailhead => ComputeScore(topographicMap, trailhead))
            .Sum();

        _console.WriteLine($"Sum of scores: {scoreSum}");
    }

    public override void SolveSecondPart()
    {
        var topographicMap = ReadTopographicMap();
        var trailheads = FindTrailheads(topographicMap);

        var ratingSum = trailheads
            .Select(trailhead => ComputeRating(topographicMap, trailhead))
            .Sum();

        _console.WriteLine($"Sum of ratings: {ratingSum}");
    }

    private static int ComputeRating(byte[][] topographicMap, Point trailhead)
    {
        var ratingMap = Enumerable.Range(0, topographicMap.Length)
            .Select(_ => Enumerable
                .Repeat(-1, topographicMap[0].Length)
                .ToArray())
            .ToArray();

        return ComputeTrails(topographicMap, ratingMap, trailhead);

        static int ComputeTrails(byte[][] topographicMap, int[][] ratingMap, Point trailhead)
        {
            if (ratingMap[trailhead.Row][trailhead.Col] != -1)
            {
                return ratingMap[trailhead.Row][trailhead.Col];
            }

            if (topographicMap[trailhead.Row][trailhead.Col] == 9)
            {
                ratingMap[trailhead.Row][trailhead.Col] = 1;
                return 1;
            }

            var directions = GetDirections(trailhead);
            var rating = 0;
            foreach (var direction in directions)
            {
                if (!IsValidStep(topographicMap, trailhead, direction))
                {
                    continue;
                }

                rating += ComputeTrails(topographicMap, ratingMap, direction);
            }

            ratingMap[trailhead.Row][trailhead.Col] = rating;
            return rating;
        }
    }

    private static int ComputeScore(byte[][] topographicMap, Point trailhead)
    {
        return FindNines(topographicMap, trailhead).Count;

        static HashSet<Point> FindNines(byte[][] topographicMap, Point start)
        {
            if (topographicMap[start.Row][start.Col] == 9)
            {
                return [start];
            }

            HashSet<Point> visitedNines = [];

            var directions = GetDirections(start);
            foreach (var direction in directions)
            {
                if (!IsValidStep(topographicMap, start, direction))
                {
                    continue;
                }

                var visited = FindNines(topographicMap, direction);

                foreach (var nine in visited)
                {
                    _ = visitedNines.Add(nine);
                }
            }

            return visitedNines;
        }
    }

    private static Point[] GetDirections(Point trailhead)
    {
        Point up = new(trailhead.Row - 1, trailhead.Col);
        Point down = new(trailhead.Row + 1, trailhead.Col);
        Point left = new(trailhead.Row, trailhead.Col - 1);
        Point right = new(trailhead.Row, trailhead.Col + 1);

        Point[] directions = [up, down, left, right];
        return directions;
    }

    private static bool IsValidStep(byte[][] topographicMap, Point from, Point to)
    {
        if (to.Row < 0 || to.Row >= topographicMap.Length ||
            to.Col < 0 || to.Col >= topographicMap[0].Length)
        {
            return false;
        }

        return topographicMap[to.Row][to.Col] == topographicMap[from.Row][from.Col] + 1;
    }

    private static List<Point> FindTrailheads(byte[][] topographicMap)
    {
        List<Point> trailheads = [];

        for (var row = 0; row < topographicMap.Length; row++)
        {
            for (var col = 0; col < topographicMap.Length; col++)
            {
                if (topographicMap[row][col] == 0)
                {
                    trailheads.Add(new(row, col));
                }
            }
        }

        return trailheads;
    }

    private byte[][] ReadTopographicMap() => _reader
        .ReadLines()
        .Select(line => line
            .Select(c => (byte)(c - '0'))
            .ToArray())
        .ToArray();

    private record struct Point(int Row, int Col);
}
