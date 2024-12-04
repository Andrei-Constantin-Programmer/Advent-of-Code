// Task: https://adventofcode.com/2023/day/16

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution16(IConsole console) : ChallengeSolution(console)
{
    private const char EMPTY = '.';
    private const char CONCAVE_MIRROR = '/';
    private const char CONVEX_MIRROR = '\\';
    private const char HORIZONTAL_SPLITTER = '|';
    private const char VERTICAL_SPLITTER = '-';

    public override void SolveFirstPart()
    {
        var mirrorGrid = Reader.ReadLines(this).ToList();
        var energiseLevel = ComputeEnergiseLevel(mirrorGrid, new(0, 0, Direction.East));

        Console.WriteLine(energiseLevel);
    }

    public override void SolveSecondPart()
    {
        var mirrorGrid = Reader.ReadLines(this).ToList();
        var maxEnergiseLevel = FindMaximumEnergiseLevel(mirrorGrid);

        Console.WriteLine(maxEnergiseLevel);
    }

    private static int FindMaximumEnergiseLevel(List<string> mirrorGrid)
    {
        var maxEnergiseLevel = 0;

        for (var row = 0; row < mirrorGrid.Count; row++)
        {
            var leftwardLevel = ComputeEnergiseLevel(mirrorGrid, new(row, 0, Direction.East));
            var rightwardLevel = ComputeEnergiseLevel(mirrorGrid, new(row, mirrorGrid[0].Length - 1, Direction.West));

            maxEnergiseLevel = Math.Max(maxEnergiseLevel, Math.Max(leftwardLevel, rightwardLevel));
        }

        for (var col = 0; col < mirrorGrid[0].Length; col++)
        {
            var downwardLevel = ComputeEnergiseLevel(mirrorGrid, new(0, col, Direction.South));
            var upwardLevel = ComputeEnergiseLevel(mirrorGrid, new(mirrorGrid.Count - 1, col, Direction.North));

            maxEnergiseLevel = Math.Max(maxEnergiseLevel, Math.Max(downwardLevel, upwardLevel));
        }

        return maxEnergiseLevel;
    }

    private static int ComputeEnergiseLevel(List<string> mirrorGrid, Beam startingBeam)
    {
        var energiseGrid = CreateEnergiseGrid(mirrorGrid, startingBeam);

        var energiseLevel = 0;
        for (var row = 0; row < energiseGrid.GetLength(0); row++)
        {
            for (var col = 0; col < energiseGrid.GetLength(1); col++)
            {
                energiseLevel += energiseGrid[row, col] ? 1 : 0;
            }
        }

        return energiseLevel;
    }

    private static bool[,] CreateEnergiseGrid(List<string> mirrorGrid, Beam startingBeam)
    {
        var energiseGrid = new bool[mirrorGrid.Count, mirrorGrid[0].Length];
        energiseGrid[startingBeam.Row, startingBeam.Column] = true;

        Reflect(mirrorGrid, energiseGrid, startingBeam);
        return energiseGrid;
    }

    private static void Reflect(List<string> mirrorGrid, bool[,] energiseGrid, Beam beam)
    {
        ReflectRecursive(mirrorGrid, energiseGrid, beam, new());

        static void ReflectRecursive(List<string> mirrorGrid, bool[,] energiseGrid, Beam beam, HashSet<Beam> beams)
        {
            if (!beams.Add(beam))
            {
                return;
            }

            var isHorizontalRay = beam.Direction is Direction.East or Direction.West;
            var rayStart = isHorizontalRay ? beam.Column : beam.Row;
            var (rayEnd, rayIncrement) = beam.Direction switch
            {
                Direction.East => (mirrorGrid[0].Length, 1),
                Direction.South => (mirrorGrid.Count, 1),
                Direction.West or Direction.North => (0, -1),

                _ => throw new Exception($"Unknown direction {beam.Direction}")
            };

            for (var ray = rayStart; rayIncrement > 0 ? (ray < rayEnd) : (ray >= rayEnd); ray += rayIncrement)
            {
                var row = isHorizontalRay ? beam.Row : ray;
                var col = isHorizontalRay ? ray : beam.Column;

                energiseGrid[row, col] = true;
                var currentCharacter = mirrorGrid[row][col];
                if (currentCharacter == EMPTY)
                {
                    continue;
                }

                if (currentCharacter == CONCAVE_MIRROR)
                {
                    ReflectRecursive(mirrorGrid, energiseGrid, GetNextConcaveBeam(beam.Direction, row, col), beams);
                    break;
                }

                if (currentCharacter == CONVEX_MIRROR)
                {
                    ReflectRecursive(mirrorGrid, energiseGrid, GetNextConvexBeam(beam.Direction, row, col), beams);
                    break;
                }

                var splitter = isHorizontalRay ? HORIZONTAL_SPLITTER : VERTICAL_SPLITTER;
                if (currentCharacter == splitter)
                {
                    ReflectRecursive(mirrorGrid, energiseGrid, GetNextConcaveBeam(beam.Direction, row, col), beams);
                    ReflectRecursive(mirrorGrid, energiseGrid, GetNextConvexBeam(beam.Direction, row, col), beams);
                    break;
                }
            }
        }
    }

    private static Beam GetNextConcaveBeam(Direction currentDirection, int row, int col) => currentDirection switch
    {
        Direction.East => new(row - 1, col, Direction.North),
        Direction.West => new(row + 1, col, Direction.South),
        Direction.North => new(row, col + 1, Direction.East),
        Direction.South => new(row, col - 1, Direction.West),

        _ => throw new Exception($"Unknown direction {currentDirection}")
    };

    private static Beam GetNextConvexBeam(Direction currentDirection, int row, int col) => currentDirection switch
    {
        Direction.East => new(row + 1, col, Direction.South),
        Direction.West => new(row - 1, col, Direction.North),
        Direction.North => new(row, col - 1, Direction.West),
        Direction.South => new(row, col + 1, Direction.East),

        _ => throw new Exception($"Unknown direction {currentDirection}")
    };

    private record Beam(int Row, int Column, Direction Direction);

    private enum Direction
    {
        North, East, South, West
    }
}
