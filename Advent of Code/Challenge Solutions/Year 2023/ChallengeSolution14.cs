using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution14 : ChallengeSolution
{
    private const char ROUND_ROCK = 'O';
    private const char EMPTY = '.';

    protected override void SolveFirstPart()
    {
        var platform = ReadPlatform();
        TiltNorth(platform);

        var totalLoad = 0;
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                Console.Write(platform[row, col]);
                if (platform[row, col] == ROUND_ROCK)
                {
                    totalLoad += platform.GetLength(0) - row;
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine(totalLoad);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static void TiltNorth(char[,] platform)
    {
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == ROUND_ROCK)
                {
                    var canMove = false;
                    int shiftRow;
                    for (shiftRow = row - 1; shiftRow >= 0 && platform[shiftRow, col] == EMPTY; shiftRow--)
                    {
                        canMove = true;
                    }

                    if (canMove)
                    {
                        platform[row, col] = EMPTY;
                        platform[shiftRow + 1, col] = ROUND_ROCK;
                    }
                }
            }
        }
    }

    private static void TiltSouth(char[,] platform)
    {
        for (var row = platform.GetLength(0) - 1; row >= 0; row--)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == ROUND_ROCK)
                {
                    var canMove = false;
                    int shiftRow;
                    for (shiftRow = row + 1; shiftRow < platform.GetLength(0) && platform[shiftRow, col] == EMPTY; shiftRow++)
                    {
                        canMove = true;
                    }

                    if (canMove)
                    {
                        platform[row, col] = EMPTY;
                        platform[shiftRow - 1, col] = ROUND_ROCK;
                    }
                }
            }
        }
    }

    private static void TiltWest(char[,] platform)
    {
        for (var col = 0; col < platform.GetLength(1); col++)
        {
            for (var row = 0; row < platform.GetLength(0); row++)
            {
                if (platform[row, col] == ROUND_ROCK)
                {
                    var canMove = false;
                    int shiftCol;
                    for (shiftCol = col - 1; shiftCol >= 0 && platform[row, shiftCol] == EMPTY; shiftCol--)
                    {
                        canMove = true;
                    }

                    if (canMove)
                    {
                        platform[row, col] = EMPTY;
                        platform[row, shiftCol + 1] = ROUND_ROCK;
                    }
                }
            }
        }
    }

    private static void TiltEast(char[,] platform)
    {
        for (var col = platform.GetLength(1) - 1; col >= 0; col--)
        {
            for (var row = 0; row < platform.GetLength(0); row++)
            {
                if (platform[row, col] == ROUND_ROCK)
                {
                    var canMove = false;
                    int shiftCol;
                    for (shiftCol = col + 1; shiftCol < platform.GetLength(1) && platform[row, shiftCol] == EMPTY; shiftCol++)
                    {
                        canMove = true;
                    }

                    if (canMove)
                    {
                        platform[row, col] = EMPTY;
                        platform[row, shiftCol - 1] = ROUND_ROCK;
                    }
                }
            }
        }
    }

    private char[,] ReadPlatform()
    {
        var lines = Reader.ReadLines(this);
        var platform = new char[lines.Length, lines[0].Length];

        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                platform[row, col] = lines[row][col];
            }
        }

        return platform;
    }
}
