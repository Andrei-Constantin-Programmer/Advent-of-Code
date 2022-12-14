using System.Text;
using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution14 : ChallengeSolution
    {
        private const char ROCK = '#', AIR = '.', SAND = 'o';
        private List<(int x, int y)[]> paths;
        private readonly int leftMargin, rightMargin, bottomMargin;
        private const int startingColumn = 500;
        private const int floorExtension = 500;

        public ChallengeSolution14()
        {
            paths = ReadRockPaths(out leftMargin, out rightMargin, out bottomMargin);
        }

        public void SolveFirstPart()
        {
            var cave = CreateCaveMap(paths, leftMargin, rightMargin, bottomMargin);
            Console.WriteLine(GrainsToFillCave(cave, leftMargin));
        }

        public void SolveSecondPart()
        {
            var cave = AddFloor(CreateCaveMap(paths, leftMargin, rightMargin, bottomMargin));
            Console.WriteLine(GrainsToPlugSource(cave, leftMargin));
        }

        private static int GrainsToPlugSource(char[][] cave, int leftMost)
        {
            return GrainsToReachGoal(cave, GetCavePosition((startingColumn + floorExtension / 2, 0), leftMost), true);
        }

        private static int GrainsToFillCave(char[][] cave, int leftMargin)
        {
            return GrainsToReachGoal(cave, GetCavePosition((startingColumn, 0), leftMargin), false);
        }

        private static int GrainsToReachGoal(char[][] cave, (int x, int y) startingPosition, bool hasFloor)
        {
            int grains = 0;
            bool hasSettled = false;
            while (!hasSettled)
            {
                (int x, int y) sandPosition = startingPosition;

                if(hasFloor && cave[sandPosition.y][sandPosition.x] == SAND)
                {
                    grains--;
                    hasSettled = true;
                }

                while (!MoveSandAndReturnIfSettled(cave, sandPosition, out sandPosition))
                {
                    if (!hasFloor && sandPosition.y + 1 >= cave.Length || (sandPosition.y + 1 < cave.Length && sandPosition.x - 1 < 0))
                    {
                        grains--;
                        hasSettled = true;
                        break;
                    }
                }

                grains++;
            }

            return grains;
        }

        private static void PrintCave(char[][] cave)
        {
            for(int i = 0; i < cave.Length; i++)
            {
                Console.SetCursorPosition(0, i);
                var builder = new StringBuilder();
                for(int j = 0; j < cave[i].Length; j++)
                {
                    if (cave[i][j] == AIR)
                        builder.Append(" ");
                    else
                        builder.Append(cave[i][j]);
                }
                Console.Write(builder.ToString());
            }
        }

        private static bool MoveSandAndReturnIfSettled(char[][] cave, (int x, int y) sandPosition, out (int x, int y) newSandPosition)
        {
            if (sandPosition.y + 1 < cave.Length && cave[sandPosition.y + 1][sandPosition.x] == AIR)
            {
                newSandPosition = (sandPosition.x, sandPosition.y + 1);
            }
            else if (sandPosition.y + 1 < cave.Length && sandPosition.x - 1 >= 0 && cave[sandPosition.y + 1][sandPosition.x - 1] == AIR)
            {
                newSandPosition = (sandPosition.x - 1, sandPosition.y + 1);
            }
            else if (sandPosition.y + 1 < cave.Length && sandPosition.x + 1 < cave[0].Length && cave[sandPosition.y + 1][sandPosition.x + 1] == AIR)
            {
                newSandPosition = (sandPosition.x + 1, sandPosition.y + 1);
            }
            else
            {
                cave[sandPosition.y][sandPosition.x] = SAND;
                newSandPosition = sandPosition;
                return true;
            }

            return false;
        }
        
        private static char[][] AddFloor(char[][] cave)
        {
            var newCave = new char[cave.Length + 2][];
            
            for(int i = 0; i < newCave.Length; i++)
            {
                newCave[i] = new char[cave[0].Length + floorExtension];
            }

            newCave = CreateEmptyCave(newCave.Length, newCave[0].Length);

            for(int i = 0; i < cave.Length; i++)
            {
                for(int j = 0; j < cave[0].Length; j++)
                {
                    newCave[i][j + (floorExtension / 2)] = cave[i][j];
                }
            }

            for(int x = 0; x < newCave[0].Length; x++)
            {
                newCave[^1][x] = ROCK;
            }

            return newCave;
        }

        private static char[][] CreateCaveMap(List<(int x, int y)[]> paths, int leftMargin, int rightMargin, int bottomMargin)
        {
            var cave = CreateEmptyCave(bottomMargin + 1, rightMargin - leftMargin + 1);

            foreach(var path in paths)
            {
                for(int pathIndex = 0; pathIndex < path.Length - 1; pathIndex++)
                {
                    var start = GetCavePosition(path[pathIndex], leftMargin);
                    var end = GetCavePosition(path[pathIndex + 1], leftMargin);

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

        private static char[][] CreateEmptyCave(int rows, int columns)
        {
            var cave = new char[rows][];
            for (int i = 0; i < cave.Length; i++)
            {
                cave[i] = new char[columns];
                for (int j = 0; j < cave[i].Length; j++)
                {
                    cave[i][j] = AIR;
                }
            }

            return cave;
        }

        private static (int x, int y) GetCavePosition((int x, int y) position, int leftMargin)
        {
            return (position.x - leftMargin, position.y);
        }

        private static List<(int x, int y)[]> ReadRockPaths(out int leftMargin, out int rightMargin, out int bottomMargin)
        {
            int leftMost = int.MaxValue;
            int rightMost = 0;
            int bottomMost = 0;

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

            leftMargin = leftMost;
            rightMargin = rightMost;
            bottomMargin = bottomMost;

            return paths;
        }
    }
}
