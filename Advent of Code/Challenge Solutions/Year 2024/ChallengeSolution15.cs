// Task: https://adventofcode.com/2024/day/15

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution15(IConsole console, ISolutionReader<ChallengeSolution15> reader)
    : ChallengeSolution<ChallengeSolution15>(console, reader)
{
    private const char Wall = '#';
    private const char Box = 'O';
    private const char Robot = '@';
    private const char Empty = '.';

    private const int GpsRowMultiplier = 100;

    private static readonly Dictionary<Direction, Point> _directionChanges = new()
    {
        { Direction.Up,     new Point(-1, 0) },
        { Direction.Right,  new Point(0, 1) },
        { Direction.Down,   new Point(1, 0) },
        { Direction.Left,   new Point(0, -1) }
    };

    public override void SolveFirstPart()
    {
        var (warehouseMap, directions) = ReadWarehouseInformation(out var robotPosition);
        PerformMoveOperations(warehouseMap, directions, robotPosition);

        PrintMap(warehouseMap);
        var gpsCoordinatesSum = FindBoxGpsCoordinatesSum(warehouseMap);

        _console.WriteLine($"GPS Box Coordinate Sum: {gpsCoordinatesSum}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static void PerformMoveOperations(char[][] warehouseMap, List<Direction> directions, Point robotPosition)
    {
        foreach (var direction in directions)
        {
            var directionChange = _directionChanges[direction];
            var newRobotPosition = Move(robotPosition, directionChange);

            if (warehouseMap[newRobotPosition.Row][newRobotPosition.Col] is Empty)
            {
                warehouseMap[newRobotPosition.Row][newRobotPosition.Col] = Robot;
                warehouseMap[robotPosition.Row][robotPosition.Col] = Empty;
                robotPosition = newRobotPosition;
                continue;
            }

            if (warehouseMap[newRobotPosition.Row][newRobotPosition.Col] is Wall
                || warehouseMap[newRobotPosition.Row][newRobotPosition.Col] is not Box)
            {
                continue;
            }

            var boxPosition = newRobotPosition;
            Point? emptyPoint = FindNextEmptyPoint(warehouseMap, directionChange, boxPosition);

            if (emptyPoint is null)
            {
                continue;
            }

            warehouseMap[emptyPoint.Row][emptyPoint.Col] = Box;
            warehouseMap[newRobotPosition.Row][newRobotPosition.Col] = Robot;
            warehouseMap[robotPosition.Row][robotPosition.Col] = Empty;
            robotPosition = newRobotPosition;
        }
    }

    private static Point? FindNextEmptyPoint(char[][] warehouseMap, Point directionChange, Point boxPosition)
    {
        Point? emptyPoint = null;

        while (emptyPoint is null)
        {
            boxPosition = Move(boxPosition, directionChange);
            if (warehouseMap[boxPosition.Row][boxPosition.Col] is Wall)
            {
                return null;
            }

            if (warehouseMap[boxPosition.Row][boxPosition.Col] is Empty)
            {
                emptyPoint = boxPosition;
            }
        }

        return emptyPoint;
    }

    private static int FindBoxGpsCoordinatesSum(char[][] warehouseMap)
    {
        var sum = 0;

        for (var i = 1; i < warehouseMap.Length - 1; i++)
        {
            for (var j = 1; j < warehouseMap[i].Length - 1; j++)
            {
                if (warehouseMap[i][j] is Box)
                {
                    sum += GpsRowMultiplier * i + j;
                }
            }
        }

        return sum;
    }

    private void PrintMap(char[][] warehouseMap)
    {
        for (var i = 0; i < warehouseMap.Length; i++)
        {
            for (var j = 0; j < warehouseMap[i].Length; j++)
            {
                _console.Write(warehouseMap[i][j]);
            }

            _console.WriteLine();
        }
        _console.WriteLine();
    }

    private static Point Move(Point position, Point directionChange) => new(position.Row + directionChange.Row, position.Col + directionChange.Col);

    private (char[][] warehouseMap, List<Direction> directions) ReadWarehouseInformation(out Point robotPosition)
    {
        var lines = _reader.ReadLines();

        List<char[]> warehouseMap = [];
        var separatorLineIndex = -1;

        robotPosition = new(-1, -1);
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line.Trim()))
            {
                separatorLineIndex = i;
                break;
            }

            warehouseMap.Add(line.ToCharArray());

            var robotIndex = line.IndexOf(Robot);
            if (robotIndex >= 0)
            {
                robotPosition = new(i, robotIndex);
            }
        }

        List<Direction> directions = [];
        for (var i = separatorLineIndex + 1; i < lines.Length; i++)
        {
            directions.AddRange(lines[i].Select(c => (Direction)c));
        }

        return (warehouseMap.ToArray(), directions);
    }

    private enum Direction
    {
        Up = '^',
        Right = '>',
        Down = 'v',
        Left = '<'
    }

    private record Point(int Row, int Col);
}
