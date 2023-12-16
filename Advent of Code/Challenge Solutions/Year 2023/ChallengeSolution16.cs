using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution16 : ChallengeSolution
{
    private const char EMPTY = '.';

    protected override void SolveFirstPart()
    {
        var mirrorGrid = Reader.ReadLines(this).ToList();
        var energiseLevel = ComputeEnergiseLevel(mirrorGrid, new(0, 0, Direction.East));

        Console.WriteLine(energiseLevel);
    }

    protected override void SolveSecondPart()
    {
        var mirrorGrid = Reader.ReadLines(this).ToList();

        long maxEnergiseLevel = 0;

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

        Console.WriteLine(maxEnergiseLevel);
    }


    private long ComputeEnergiseLevel(List<string> mirrorGrid, Beam startingBeam)
    {
        var energiseGrid = CreateEnergiseGrid(mirrorGrid, startingBeam);

        long energiseLevel = 0;
        for (var row = 0; row < energiseGrid.GetLength(0); row++)
        {
            for (var col = 0; col < energiseGrid.GetLength(1); col++)
            {
                energiseLevel += energiseGrid[row, col] ? 1 : 0;
            }
        }

        return energiseLevel;
    }

    private bool[,] CreateEnergiseGrid(List<string> mirrorGrid, Beam startingBeam)
    {
        var energiseGrid = new bool[mirrorGrid.Count, mirrorGrid[0].Length];
        energiseGrid[startingBeam.Row, startingBeam.Column] = true;

        HashSet<Beam> beams = new();
        Reflect(mirrorGrid, energiseGrid, startingBeam, beams);
        return energiseGrid;
    }

    private void Reflect(List<string> mirrorGrid, bool[,] energiseGrid, Beam beam, HashSet<Beam> beams)
    {
        if (!beams.Add(beam))
        {
            return;
        }
        
        if (beam.Direction is Direction.East)
        {
            for (var col = beam.Column; col < mirrorGrid[0].Length; col++)
            {
                energiseGrid[beam.Row, col] = true;

                var currentCharacter = mirrorGrid[beam.Row][col];
                if (currentCharacter != EMPTY)
                {
                    if (currentCharacter == '/')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row - 1, col, Direction.North), beams);
                        break;
                    }
                    if (currentCharacter == '\\')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row + 1, col, Direction.South), beams);
                        break;
                    }
                    if (currentCharacter == '|')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row - 1, col, Direction.North), beams);
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row + 1, col, Direction.South), beams);
                        break;
                    }
                }
            }
        }
        else if (beam.Direction is Direction.West)
        {
            for (var col = beam.Column; col >= 0; col--)
            {
                energiseGrid[beam.Row, col] = true;

                var currentCharacter = mirrorGrid[beam.Row][col];
                if (currentCharacter != EMPTY)
                {
                    if (currentCharacter == '/')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row + 1, col, Direction.South), beams);
                        break;
                    }
                    if (currentCharacter == '\\')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row - 1, col, Direction.North), beams);
                        break;
                    }
                    if (currentCharacter == '|')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row + 1, col, Direction.South), beams);
                        Reflect(mirrorGrid, energiseGrid, new(beam.Row - 1, col, Direction.North), beams);
                        break;
                    }
                }
            }
        }
        else if (beam.Direction is Direction.North)
        {
            for (var row = beam.Row; row >= 0; row--)
            {
                energiseGrid[row, beam.Column] = true;

                var currentCharacter = mirrorGrid[row][beam.Column];
                if (currentCharacter != EMPTY)
                {
                    if (currentCharacter == '/')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column + 1, Direction.East), beams);
                        break;
                    }
                    if (currentCharacter == '\\')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column - 1, Direction.West), beams);
                        break;
                    }
                    if (currentCharacter == '-')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column + 1, Direction.East), beams);
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column - 1, Direction.West), beams);
                        break;
                    }
                }
            }
        }
        else if (beam.Direction is Direction.South)
        {
            for (var row = beam.Row; row < mirrorGrid.Count; row++)
            {
                energiseGrid[row, beam.Column] = true;

                var currentCharacter = mirrorGrid[row][beam.Column];
                if (currentCharacter != EMPTY)
                {
                    if (currentCharacter == '/')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column - 1, Direction.West), beams);
                        break;
                    }
                    if (currentCharacter == '\\')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column + 1, Direction.East), beams);
                        break;
                    }
                    if (currentCharacter == '-')
                    {
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column - 1, Direction.West), beams);
                        Reflect(mirrorGrid, energiseGrid, new(row, beam.Column + 1, Direction.East), beams);
                        break;
                    }
                }
            }
        }
    }

    private record Beam(int Row, int Column, Direction Direction);

    private enum Direction
    {
        North, East, South, West
    }
}
