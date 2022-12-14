using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution12 : ChallengeSolution
    {
        private const char START = 'S', END = 'E';
        private readonly int[][] heightMap;
        private (int row, int column) endPosition;

        public ChallengeSolution12()
        {
            heightMap = ReadHeightMap();
            endPosition = GetPositionOf(0);
        }

        public void SolveFirstPart()
        {
            var distanceMap = CreateMatrix<int>(heightMap.Length, heightMap[0].Length);

            Console.WriteLine(ShortestPath(distanceMap, endPosition, LetterReversePositionInAlphabet('a') + 1));
        }

        public void SolveSecondPart()
        {
            var distanceMap = CreateMatrix<int>(heightMap.Length, heightMap[0].Length);

            Console.WriteLine(ShortestPath(distanceMap, endPosition, LetterReversePositionInAlphabet('a')));
        }

        private int ShortestPath(int[][] distanceMap, (int row, int column) position, int toBeReached)
        {
            int currentPath = distanceMap[position.row][position.column] + 1;

            int shortestPath = int.MaxValue;

            foreach (var neighbour in GetNeighbourPositions((position.row, position.column)))
            {   
                if (IsValidMove(neighbour))
                {
                    if (CanTraverse((position.row, position.column), neighbour) && IsShorterPath(distanceMap, currentPath, neighbour))
                    {
                        distanceMap[neighbour.row][neighbour.column] = currentPath;

                        if (heightMap[neighbour.row][neighbour.column] == toBeReached)
                            return currentPath;

                        shortestPath = Math.Min(shortestPath, ShortestPath(distanceMap, neighbour, toBeReached));
                    }
                }
            }

            return shortestPath;
        }

        private bool IsValidMove((int row, int column) position)
        {
            if (position.row < 0)
                return false;
            if (position.row >= heightMap.Length)
                return false;
            if (position.column < 0)
                return false;
            if (position.column >= heightMap[0].Length)
                return false;

            return true;
        }

        private bool CanTraverse((int row, int column) source, (int row, int column) destination)
        {
            return heightMap[destination.row][destination.column] - heightMap[source.row][source.column] <= 1;
        }

        private static bool IsShorterPath(int[][] distanceMap, int currentPath, (int row, int column) position)
        {
            return distanceMap[position.row][position.column] == 0 || currentPath < distanceMap[position.row][position.column];
        }

        private static (int row, int column)[] GetNeighbourPositions((int row, int column) position)
        {
            var neighbours = new List<(int, int)>();
            for (int direction = 1; direction <= 7; direction += 2)
            {
                neighbours.Add((position.row + ((direction % 3) - 1), position.column + ((direction / 3) - 1)));
            }

            return neighbours.ToArray();
        }

        private (int row, int column) GetPositionOf(int toBeFound)
        {
            for(int i = 0; i < heightMap.Length; i++)
                for(int j = 0; j < heightMap[i].Length; j++)
                    if(heightMap[i][j] == toBeFound)
                        return (i, j);

            throw new ArgumentException("The height map doesn't have a starting position");
        }

        private static int[][] ReadHeightMap()
        {
            return File.ReadAllLines(GetFileString(FileType.Input, 2022, 12))
                .Select(line => line
                    .ToCharArray()
                    .Select(c =>
                    {
                        if (c == START)
                        {
                            return LetterReversePositionInAlphabet('a') + 1;
                        }
                        else if (c == END)
                        {
                            return LetterReversePositionInAlphabet('z') - 1;
                        }
                        else
                        {
                            return LetterReversePositionInAlphabet(c);
                        }
                    }).ToArray())
                .ToArray();
        }

        private static int LetterReversePositionInAlphabet(char letter)
        {
            if (!char.IsLetter(letter))
                return -1;

            return 'z' + 1 - letter;
        }

        private static T[][] CreateMatrix<T>(int rows, int columns)
        {
            var matrix = new T[rows][];
            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new T[columns];
            }

            return matrix;
        }
    }
}
