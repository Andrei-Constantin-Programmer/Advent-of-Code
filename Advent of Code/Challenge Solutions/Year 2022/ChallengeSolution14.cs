using System.Data.Common;
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
            try
            {
                var cave = CreateCaveMap(paths);

                for(int i = 0; i < cave.Length; i++)
                {
                    for(int j = 0; j < cave[i].Length; j++)
                        Console.Write(cave[i][j] + " ");
                    Console.WriteLine();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
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
                    var start = path[pathIndex];
                    var end = path[pathIndex + 1];

                    if(start.x > end.x)
                    {
                        (end, start) = (start, end);
                    }

                    if (start.y > end.y)
                    {
                        (end, start) = (start, end);
                    }

                    Console.WriteLine(start + " " + end);

                    if(start.x == end.x)
                    {
                        for(var row = start.y - topMost; row <= end.y - topMost; row++)
                        {
                            cave[row][start.x - leftMost] = ROCK;
                        }
                    }
                    else if(start.y == end.y)
                    {
                        for (var column = start.x - leftMost; column <= end.x - leftMost; column++)
                        {
                            cave[start.y - topMost][column] = ROCK;
                        }
                    }
                }
            }

            return cave;
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
