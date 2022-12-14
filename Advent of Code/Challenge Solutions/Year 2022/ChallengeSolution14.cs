using Advent_of_Code.Challenge_Solutions.Year_2021;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution14 : ChallengeSolution
    {
        private const char ROCK = '#', AIR = '.', SAND = 'o';

        private int leftMost = int.MaxValue;
        private int rightMost = 0;
        private int topMost = 0;
        private int bottomMost = 0;

        public void SolveFirstPart()
        {
            var paths = ReadRockPaths();
            var cave = CreateCaveMap(paths);

            Console.WriteLine(GrainsToFillCave(cave));
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private int GrainsToFillCave(char[][] cave)
        {
            int grains = 0;
            bool hasSettled = false;
            while(!hasSettled)
            {
                (int x, int y) sandPosition = GetCavePosition((500, 0));

                while(true)
                {
                    if (OutOfCaveBounds(cave, sandPosition) || sandPosition.y + 1 >= cave.Length || (sandPosition.y + 1 < cave.Length && sandPosition.x - 1 < 0))
                    {
                        hasSettled = true;
                        break;
                    }

                    if (sandPosition.y + 1 < cave.Length && cave[sandPosition.y + 1][sandPosition.x] == AIR)
                    {
                        sandPosition = (sandPosition.x, sandPosition.y + 1);
                    }
                    else if (sandPosition.y + 1 < cave.Length && sandPosition.x - 1 >= 0 && cave[sandPosition.y + 1][sandPosition.x - 1] == AIR)
                    {
                        sandPosition = (sandPosition.x - 1, sandPosition.y + 1);
                    }
                    else if (sandPosition.y + 1 < cave.Length && sandPosition.x + 1 < cave[0].Length && cave[sandPosition.y + 1][sandPosition.x + 1] == AIR)
                    {
                        sandPosition = (sandPosition.x + 1, sandPosition.y + 1);
                    }
                    else
                    {
                        cave[sandPosition.y][sandPosition.x] = SAND;
                        grains++;
                        break;
                    }
                }
            }

            return grains;
        }

        private static bool OutOfCaveBounds(char[][] cave, (int x, int y) sandPosition)
        {
            if (sandPosition.x < 0 || sandPosition.x > cave[0].Length)
                return true;

            if (sandPosition.y < 0 || sandPosition.y > cave.Length)
                return true;

            return false;
        }

        private char[][] CreateCaveMap(List<(int x, int y)[]> paths)
        {
            var cave = new char[bottomMost + 1][];
            for(int i = 0; i < cave.Length; i++)
            {
                cave[i] = new char[rightMost - leftMost + 1];
                for(int j = 0; j < cave[i].Length; j++)
                {
                    cave[i][j] = AIR;
                }
            }

            foreach(var path in paths)
            {
                for(int pathIndex = 0; pathIndex < path.Length - 1; pathIndex++)
                {
                    var start = GetCavePosition(path[pathIndex]);
                    var end = GetCavePosition(path[pathIndex + 1]);

                    if(start.x > end.x)
                    {
                        (end, start) = (start, end);
                    }

                    if (start.y > end.y)
                    {
                        (end, start) = (start, end);
                    }

                    if(start.x == end.x)
                    {
                        for(var row = start.y; row <= end.y; row++)
                        {
                            cave[row][start.x] = ROCK;
                        }
                    }
                    else if(start.y == end.y)
                    {
                        for (var column = start.x; column <= end.x; column++)
                        {
                            cave[start.y][column] = ROCK;
                        }
                    }
                }
            }

            return cave;
        }

        private (int x, int y) GetCavePosition((int x, int y) position)
        {
            return (position.x - leftMost, position.y - topMost);
        }

        private List<(int x, int y)[]> ReadRockPaths()
        {
            var paths = new List<(int x, int y)[]>();

            using (TextReader read = GetInputFile(2022, 14))
            {
                string? line;
                while((line = read.ReadLine()) != null)
                {
                    paths.Add(line
                        .Split("->", StringSplitOptions.RemoveEmptyEntries)
                        .Select(point =>
                        {
                            var coordinates = point.Split(",", StringSplitOptions.RemoveEmptyEntries);
                            var x = Convert.ToInt32(coordinates[0]);
                            var y = Convert.ToInt32(coordinates[1]);

                            if (x < leftMost)
                                leftMost = x;
                            if (x > rightMost)
                                rightMost = x;
                            if (y > bottomMost)
                                bottomMost = y;

                            return (x, y);
                        })
                        .ToArray());
                }
            }

            return paths;
        }
    }
}
