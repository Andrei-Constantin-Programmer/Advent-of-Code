// Task: https://adventofcode.com/2022/day/24

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

internal class ChallengeSolution24 : ChallengeSolution
{
    private static readonly Point _noBlizzardPoint = new(-1, -1);

    private const char NORTH = '^';
    private const char WEST = '<';
    private const char EAST = '>';
    private const char SOUTH = 'v';

    protected override void SolveFirstPart()
    {
        throw new NotImplementedException();
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static bool ContainsBlizzard(char[,] valley, Point point, int minute)
    {
        for (var row = 0; row < valley.GetLength(0); row++)
        {
            if (IsBlizzard(valley, new(row, point.Column), minute, out var blizzardPosition)
                && blizzardPosition == point)
            {
                return true;
            }
        }

        for (var col = 0; col < valley.GetLength(1); col++)
        {
            if (IsBlizzard(valley, new(point.Row, col), minute, out var blizzardPosition)
                && blizzardPosition == point)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsBlizzard(char[,] valley, Point point, int minute, out Point currentPosition)
    {
        currentPosition = valley[point.Row, point.Column] switch
        {
            NORTH => new(SubtractOrLoopAround(point.Row, minute, valley.GetLength(0)), point.Column),
            SOUTH => new(AddOrLoopAround(point.Row, minute, valley.GetLength(0)), point.Column),
            WEST => new(point.Row, SubtractOrLoopAround(point.Column, minute, valley.GetLength(1))),
            EAST => new(point.Row, AddOrLoopAround(point.Column, minute, valley.GetLength(1))),

            _ => _noBlizzardPoint
        };
        
        return currentPosition != _noBlizzardPoint;
    }

    private static int SubtractOrLoopAround(int a, int b, int loopingPosition) => (((a - b) % loopingPosition) + loopingPosition) % loopingPosition;

    private static int AddOrLoopAround(int a, int b, int maxValue) => (a + b) % maxValue;

    private char[,] ReadValley()
    {
        var lines = Reader.ReadLines(this);
        var valley = new char[lines.Length - 2, lines[0].Length - 2];

        for (var row = 1; row < lines.Length - 1; row++)
        {
            for (var col = 1; col < lines[row].Length - 1; col++)
            {
                valley[row - 1, col - 1] = lines[row][col];
            }
        }

        return valley;
    }

    private record Point(int Row, int Column);
}
