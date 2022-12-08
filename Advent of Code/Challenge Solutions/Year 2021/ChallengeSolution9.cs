using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution9 : ChallengeSolution
    {
        private int rows, columns;

        public ChallengeSolution9()
        {
            string[] lines = File.ReadAllLines(GetFileString(FileType.Input, 2021, 9));
            rows = lines.Length;
            columns = lines[0].Length;
        }

        public void SolveFirstPart()
        {
            int[,] heightMap = ReadHeightMap();
            int sum = 0;
            var lowPoints = GetLowPoints(heightMap);

            foreach (var point in lowPoints)
            {
                sum += heightMap[point.row, point.column] + 1;
            }

            Console.WriteLine(sum);
        }

        public void SolveSecondPart()
        {
            var heightMap = ReadHeightMap();
            var lowPoints = GetLowPoints(heightMap);

            var basinHeights = CalculateBasinSizes(heightMap, lowPoints);

            Console.WriteLine(basinHeights
                .OrderBy(x => x)
                .Take(3)
                .Aggregate((x, y) => x * y));
        }

        private List<int> CalculateBasinSizes(int[,] heightMap, List<(int row, int column)> lowPoints)
        {
            var basinHeights = new List<int>();

            foreach (var lowPoint in lowPoints)
            {
                if (heightMap[lowPoint.row, lowPoint.column] < 9)
                {
                    int basinSize;
                    CreateBasin(heightMap, lowPoint, out basinSize);
                    for (var i = 0; i < rows; i++)
                    {
                        for (var j = 0; j < columns; j++)
                        {
                            Console.Write(heightMap[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    basinHeights.Add(basinSize);
                }
            }

            return basinHeights;
        }

        private void CreateBasin(int[,] heightMap, (int row, int column) point, out int basinSize)
        {
            basinSize = 1;
            heightMap[point.row, point.column] = 9;
            var neighbours = GetNeighbourLowPoints(heightMap, point);

            while(neighbours.Count > 0)
            {
                var newNeighbours = new List<(int row, int column)>();

                basinSize += neighbours.Count;
                foreach(var neighbour in neighbours)
                {
                    heightMap[neighbour.row, neighbour.column] = 9;
                    newNeighbours.AddRange(GetNeighbourLowPoints(heightMap, neighbour));
                }

                neighbours = newNeighbours;
            }
        }

        private List<(int row, int column)> GetNeighbourLowPoints(int[,] heightMap, (int row, int column) coord)
        {
            var neighbours = new List<(int row, int column)>();
            var coords = GetNeighbourCoords(coord);
            foreach (var x in coords)
            {
                if (InBounds(x) && IsLowPoint(heightMap, x))
                    neighbours.Add(x);
            }

            return neighbours;
        }

        private List<(int row, int column)> GetLowPoints(int[,] heightMap)
        {
            var lowPoints = new List<(int row, int column)>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (IsLowPoint(heightMap, (i, j)))
                    {
                        lowPoints.Add((i, j));
                    }
                }
            }

            return lowPoints;
        }

        private bool IsLowPoint(int[,] heightMap, (int row, int column) coord)
        {
            foreach(var neighbour in GetNeighbourCoords(coord))
            {
                if (InBounds(coord) && heightMap[neighbour.row, neighbour.column] <= heightMap[coord.row, coord.column])
                {
                    return false;
                }
            }

            return true;
        }

        private List<(int row, int column)> GetNeighbourCoords((int row, int column) coord)
        {
            var coords = new List<(int row, int column)>() 
            { 
                (coord.row - 1, coord.column), 
                (coord.row, coord.column - 1), 
                (coord.row + 1, coord.column), 
                (coord.row, coord.column + 1) 
            };

            for (int k = 0; k < coords.Count; k++)
                if (!InBounds(coords[k]))
                {
                    coords.RemoveAt(k);
                    k--;
                }

            return coords;
        }

        private bool InBounds((int row, int column) coord)
        {
            return coord.row >= 0 && coord.row < rows && coord.column >= 0 && coord.column < columns;
        }

        private int[,] ReadHeightMap()
        {
            var heightMap = new int[rows, columns];
            using (TextReader read = GetInputFile(2021, 9))
            {
                for (int i = 0; i < rows; i++)
                {
                    char[] heightsAsChar = read.ReadLine().ToCharArray();
                    byte[] heights = Array.ConvertAll(heightsAsChar, character => (byte)Char.GetNumericValue(character));
                    for (int j = 0; j < columns; j++)
                        heightMap[i, j] = heights[j];
                }

                return heightMap;
            }
        }

    }
}
