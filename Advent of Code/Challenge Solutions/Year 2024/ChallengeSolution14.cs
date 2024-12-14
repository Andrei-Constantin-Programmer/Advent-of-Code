// Task: https://adventofcode.com/2024/day/14

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution14(IConsole console, ISolutionReader<ChallengeSolution14> reader)
    : ChallengeSolution<ChallengeSolution14>(console, reader)
{
    public override void SolveFirstPart()
    {
        var robots = ReadRobots();
        var rows = 103;
        var cols = 101;
        var seconds = 100;

        List<Robot> finalPoints = ComputeRobotPositions(robots, rows, cols, seconds);
        var safetyFactor = ComputeSafetyFactor(rows, cols, finalPoints);

        _console.WriteLine($"Safety factor: {safetyFactor}");
    }

    public override void SolveSecondPart()
    {
        var robots = ReadRobots();
        var rows = 103;
        var cols = 101;

        var seconds = rows * cols;
        var minimalSafetyFactor = long.MaxValue;
        var minimalSafetyFactorSecond = -1;

        for (var second = 0; second < seconds; second++)
        {
            robots = ComputeRobotPositions(robots, rows, cols, 1);
            var safetyFactor = ComputeSafetyFactor(rows, cols, robots);

            if (safetyFactor < minimalSafetyFactor)
            {
                minimalSafetyFactor = safetyFactor;
                minimalSafetyFactorSecond = second + 1;
            }
        }

        _console.WriteLine($"Seconds to find tree: {minimalSafetyFactorSecond}");
    }

    private static List<Robot> ComputeRobotPositions(List<Robot> robots, int rows, int cols, int seconds)
    {
        var newRobots = new List<Robot>(robots.Count);

        foreach (var robot in robots)
        {
            var currentPoint = robot.Point;
            var pointsAtTime = new Dictionary<Point, int> { { currentPoint, 0 } };

            for (var second = 0; second < seconds; second++)
            {
                Point newPoint = Move(currentPoint, robot.Velocity, rows, cols);

                if (pointsAtTime.TryGetValue(newPoint, out var time))
                {
                    var timeDifference = (second + 1) - time;
                    seconds %= timeDifference;
                }
                else
                {
                    pointsAtTime[newPoint] = second + 1;
                }

                currentPoint = newPoint;
            }

            newRobots.Add(new(currentPoint, robot.Velocity));
        }

        return newRobots;
    }

    private static long ComputeSafetyFactor(int rows, int cols, List<Robot> finalPoints)
    {
        var robotsInQuadrant = FindRobotsInQuadrants(rows, cols, finalPoints);

        var safetyFactor = robotsInQuadrant.Aggregate(1L, (acc, x) => acc * x);
        return safetyFactor;
    }

    private static long[] FindRobotsInQuadrants(int rows, int cols, List<Robot> robots)
    {
        var robotsInQuadrant = new long[4];

        foreach (var robot in robots)
        {
            if (IsInThemMiddle(robot.Point, rows, cols))
            {
                continue;
            }

            if (robot.Point.Row < rows / 2)
            {
                if (robot.Point.Col < cols / 2)
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
                if (robot.Point.Col < cols / 2)
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

    private static bool IsInThemMiddle(Point point, int rows, int cols) => point.Row == rows / 2 || point.Col == cols / 2;

    private static Point Move(Point point, Point velocity, int rows, int cols)
    {
        var row = point.Row + velocity.Row;
        var col = point.Col + velocity.Col;

        if (row < 0)
        {
            row = rows + row;
        }

        row %= rows;

        if (col < 0)
        {
            col = cols + col;
        }

        col %= cols;

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

    private record Robot(Point Point, Point Velocity);

    private record Point(int Row, int Col);
}
