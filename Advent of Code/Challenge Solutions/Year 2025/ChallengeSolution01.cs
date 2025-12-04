// Task: https://adventofcode.com/2025/day/1

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution01(IConsole console, ISolutionReader<ChallengeSolution01> reader)
    : ChallengeSolution<ChallengeSolution01>(console, reader)
{
    private const int UpperDialLimit = 100;
    private const int StartingDialPosition = 50;
    private const int Zero = 0;

    public override void SolveFirstPart()
    {
        var rotations = ReadRotations();
        var password = 0;
        var currentDialPosition = StartingDialPosition;

        foreach (var rotation in rotations)
        {
            currentDialPosition = TurnDial(currentDialPosition, rotation);

            if (currentDialPosition == Zero)
            {
                password++;
            }
        }

        _console.WriteLine($"Password: {password}");
    }

    public override void SolveSecondPart()
    {
        var rotations = ReadRotations();
        var password = 0;
        var currentDialPosition = StartingDialPosition;

        foreach (var rotation in rotations)
        {
            var fullRotations = rotation.Distance / UpperDialLimit;
            var remainder = rotation.Distance - (fullRotations * UpperDialLimit);

            var hasDialMovedPastZero = currentDialPosition != Zero
                && rotation.Direction switch
                {
                    Direction.Left => currentDialPosition <= remainder,
                    Direction.Right => currentDialPosition >= UpperDialLimit - remainder,
                    _ => throw new NotImplementedException()
                };

            password += fullRotations + (hasDialMovedPastZero ? 1 : 0);

            currentDialPosition = TurnDial(currentDialPosition, rotation);
        }

        _console.WriteLine($"Password: {password}");
    }

    private static int TurnDial(int dialPosition, Rotation rotation)
    {
        var newPosition = rotation.Direction switch
        {
            Direction.Left => dialPosition - rotation.Distance,
            Direction.Right => dialPosition + rotation.Distance,
            _ => throw new NotImplementedException()
        };

        newPosition = (newPosition % UpperDialLimit + UpperDialLimit) % UpperDialLimit;

        return newPosition;
    }

    private List<Rotation> ReadRotations()
    {
        List<Rotation> rotations = [];

        var lines = _reader.ReadLines();
        foreach (var line in lines)
        {
            var direction = line[0] switch
            {
                'L' => Direction.Left,
                'R' => Direction.Right,
                _ => throw new ArgumentException()
            };

            var distance = int.Parse(line[1..]);

            rotations.Add(new(direction, distance));
        }

        return rotations;
    }

    private enum Direction { Left, Right }

    private record struct Rotation(Direction Direction, int Distance);
}
