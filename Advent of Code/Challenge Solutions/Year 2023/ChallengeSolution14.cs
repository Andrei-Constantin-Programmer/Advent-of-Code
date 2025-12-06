// Task: https://adventofcode.com/2023/day/14

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution14(IConsole console, ISolutionReader<ChallengeSolution14> reader)
    : ChallengeSolution<ChallengeSolution14>(console, reader)
{
    private const char ROUND_ROCK = 'O';
    private const char EMPTY = '.';

    public override void SolveFirstPart()
    {
        var platform = ReadPlatform();
        TiltNorth(platform);

        _console.WriteLine(GetTotalLoad(platform));
    }

    public override void SolveSecondPart()
    {
        var platform = ReadPlatform();
        CharArrayComparer comparer = new();
        HashSet<char[,]> cycles = new(comparer);

        do
        {
            var platformCopy = new char[platform.GetLength(0), platform.GetLength(1)];
            Array.Copy(platform, platformCopy, platform.Length);
            cycles.Add(platformCopy);
            PerformTiltCycle(platform);
        } while (!cycles.Contains(platform));

        var repeatingIndex = cycles
            .ToList()
            .FindIndex(x => comparer.Equals(x, platform));
        var repeatingCount = cycles.Count - repeatingIndex;
        var billionthCycleIndex = (1_000_000_000 - repeatingIndex) % repeatingCount;

        _console.WriteLine(GetTotalLoad(cycles.ToArray()[repeatingIndex..].ElementAt(billionthCycleIndex)));
    }

    private static long GetTotalLoad(char[,] platform)
    {
        var totalLoad = 0;
        for (var row = 0; row < platform.GetLength(0); row++)
        {
            for (var col = 0; col < platform.GetLength(1); col++)
            {
                if (platform[row, col] == ROUND_ROCK)
                {
                    totalLoad += platform.GetLength(0) - row;
                }
            }
        }

        return totalLoad;
    }

    private static void PerformTiltCycle(char[,] platform)
    {
        TiltNorth(platform);
        TiltWest(platform);
        TiltSouth(platform);
        TiltEast(platform);
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
        var lines = _reader.ReadLines();
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

    private class CharArrayComparer : IEqualityComparer<char[,]>
    {
        public bool Equals(char[,]? x, char[,]? y)
        {
            if (x is null || y is null)
            {
                return x == y;
            }

            if (x.GetLength(0) != y.GetLength(0)
                || x.GetLength(1) != y.GetLength(1))
            {
                return false;
            }

            for (var row = 0; row < x.GetLength(0); row++)
            {
                for (var col = 0; col < x.GetLength(1); col++)
                {
                    if (x[row, col] != y[row, col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetHashCode(char[,] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var hash = 17;

            for (var row = 0; row < obj.GetLength(0); row++)
            {
                for (var col = 0; col < obj.GetLength(1); col++)
                {
                    hash = (hash * 31) + obj[row, col].GetHashCode();
                }
            }

            return hash;
        }
    }
}
