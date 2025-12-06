// Task: https://adventofcode.com/2024/day/14

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution14(IConsole console, ISolutionReader<ChallengeSolution14> reader)
    : ChallengeSolution<ChallengeSolution14>(console, reader)
{
    private const int Rows = 103;
    private const int Cols = 101;

    public override void SolveFirstPart()
    {
        var robots = ReadRobots();
        var seconds = 100;

        ComputeRobotPositions(robots, seconds, out var _);
        var safetyFactor = ComputeSafetyFactor(robots);

        _console.WriteLine($"Safety factor: {safetyFactor}");
    }

    public override void SolveSecondPart()
    {
        var robots = ReadRobots();
        var seconds = Rows * Cols;

        ComputeRobotPositions(robots, seconds, out var secondWithMinimalSafetyFactor);

        _console.WriteLine($"Seconds to find tree: {secondWithMinimalSafetyFactor}");
    }

    private static void ComputeRobotPositions(List<Robot> robots, int seconds, out int secondWithMinimalSafetyFactor)
    {
        var minimalSafetyFactor = long.MaxValue;
        secondWithMinimalSafetyFactor = -1;

        for (var second = 0; second < seconds; second++)
        {
            foreach (var robot in robots)
            {
                robot.Point = Move(robot.Point, robot.Velocity);
            }

            if (ComputeSafetyFactor(robots) < minimalSafetyFactor)
            {
                minimalSafetyFactor = ComputeSafetyFactor(robots);
                secondWithMinimalSafetyFactor = second + 1;
            }
        }
    }

    private static long ComputeSafetyFactor(List<Robot> finalPoints)
    {
        var robotsInQuadrant = FindRobotsInQuadrants(finalPoints);

        var safetyFactor = robotsInQuadrant.Aggregate(1L, (acc, x) => acc * x);
        return safetyFactor;
    }

    private static long[] FindRobotsInQuadrants(List<Robot> robots)
    {
        var robotsInQuadrant = new long[4];

        foreach (var robot in robots)
        {
            if (IsInThemMiddle(robot.Point))
            {
                continue;
            }

            if (robot.Point.Row < Rows / 2)
            {
                if (robot.Point.Col < Cols / 2)
                {
                    robotsInQuadrant[0]++;
                }
                else
                {
                    robotsInQuadrant[1]++;
                }
            }
            else
            {
                if (robot.Point.Col < Cols / 2)
                {
                    robotsInQuadrant[2]++;
                }
                else
                {
                    robotsInQuadrant[3]++;
                }
            }
        }

        return robotsInQuadrant;
    }

    private static bool IsInThemMiddle(Point point) => point.Row == Rows / 2 || point.Col == Cols / 2;

    private static Point Move(Point point, Point velocity)
    {
        var row = point.Row + velocity.Row;
        var col = point.Col + velocity.Col;

        if (row < 0)
        {
            row = Rows + row;
        }

        row %= Rows;

        if (col < 0)
        {
            col = Cols + col;
        }

        col %= Cols;

        return new Point(row, col);
    }

    private List<Robot> ReadRobots()
    {
        List<Robot> robots = [];
        var lines = _reader.ReadLines();

        foreach (var line in lines)
        {
            var elements = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var pointValues = elements[0]
                .Split('=')[1]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var velocityValues = elements[1]
                .Split('=')[1]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            Point point = new(pointValues[1], pointValues[0]);
            Point velocity = new(velocityValues[1], velocityValues[0]);

            robots.Add(new Robot(point, velocity));
        }

        return robots;
    }

    private class Robot(Point point, Point velocity)
    {
        public Point Point { get; set; } = point;
        public Point Velocity { get; } = velocity;
    }

    private record Point(int Row, int Col);
}
