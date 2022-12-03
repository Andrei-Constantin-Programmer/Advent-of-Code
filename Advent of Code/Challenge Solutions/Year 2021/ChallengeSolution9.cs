using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution9 : ChallengeSolution
    {
        private int n, m;

        public ChallengeSolution9()
        {
            string[] lines = File.ReadAllLines(GetFileString(FileType.Input, 2021, 9));
            n = lines.Length;
            m = lines[0].Length;
        }

        public void SolveFirstPart()
        {
            int[,] heightMap = ReadHeightMap();
            using (TextReader read = GetInputFile(2021, 9))
            {
                for (int i = 0; i < n; i++)
                {
                    char[] heightsAsChar = read.ReadLine().ToCharArray();
                    byte[] heights = Array.ConvertAll(heightsAsChar, character => (byte)Char.GetNumericValue(character));
                    for (int j = 0; j < m; j++)
                        heightMap[i, j] = heights[j];
                }

                int sum = 0;
                var lowPoints = GetLowPoints(heightMap);

                foreach (var point in lowPoints)
                {
                    sum += heightMap[point.Key, point.Value] + 1;
                }

                Console.WriteLine(sum);
            }
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();

            int[,] heightMap = ReadHeightMap();

            var lowPoints = GetLowPoints(heightMap);
            var basinSizes = new List<int>();

            int[,] basinMap = (int[,])heightMap.Clone();

            foreach (var point in lowPoints)
            {
                int size = 1;
                basinMap[point.Key, point.Value] = 9;
                var neighbours = GetNeighbourLowPoints(basinMap, point);
                
                while(neighbours.Count>0)
                {
                    var newNeighbours = new List<KeyValuePair<int, int>>();

                    size += neighbours.Count;
                    foreach(var neighbour in neighbours)
                    {
                        basinMap[neighbour.Key, neighbour.Value] = 9;
                        newNeighbours.AddRange(GetNeighbourLowPoints(basinMap, neighbour));
                    }

                    neighbours = newNeighbours;
                }

                basinSizes.Add(size);
            }

            basinSizes = basinSizes.OrderByDescending(x => x).ToList();

            foreach(var x in basinSizes)
                Console.WriteLine(x);

            Console.WriteLine(basinSizes[0]*basinSizes[1]*basinSizes[2]);
        }

        private int[,] ReadHeightMap()
        {
            var heightMap = new int[n, m];
            using (TextReader read = GetInputFile(2021, 9))
            {
                for (int i = 0; i < n; i++)
                {
                    char[] heightsAsChar = read.ReadLine().ToCharArray();
                    byte[] heights = Array.ConvertAll(heightsAsChar, character => (byte)Char.GetNumericValue(character));
                    for (int j = 0; j < m; j++)
                        heightMap[i, j] = heights[j];
                }

                return heightMap;
            }
        }

        private List<KeyValuePair<int, int>> GetLowPoints(int[,] heightMap)
        {
            var lowPoints = new List<KeyValuePair<int, int>>();

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (IsLowPoint(heightMap, new KeyValuePair<int, int>(i, j)))
                    {
                        lowPoints.Add(new KeyValuePair<int, int>(i, j));
                    }
                }

            return lowPoints;
        }

        private List<KeyValuePair<int, int>> GetNeighbourLowPoints(int[,] heightMap, KeyValuePair<int, int> coord)
        {
            var neighbours = new List<KeyValuePair<int, int>>();
            var coords = GetNeighbourCoords(coord);
            foreach (var x in coords)
            {
                if (InBounds(x) && IsLowPoint(heightMap, x))
                    neighbours.Add(x);
            }

            return neighbours;
        }

        private bool IsLowPoint(int[,] heightMap, KeyValuePair<int, int> coord)
        {
            int i = coord.Key, j = coord.Value;
            int x = heightMap[i, j];
            bool isLowPoint = true;
            if (i - 1 >= 0 && heightMap[i - 1, j] <= x)
                isLowPoint = false;
            if (j - 1 >= 0 && heightMap[i, j - 1] <= x)
                isLowPoint = false;
            if (i + 1 < n && heightMap[i + 1, j] <= x)
                isLowPoint = false;
            if (j + 1 < m && heightMap[i, j + 1] <= x)
                isLowPoint = false;

            return isLowPoint;
        }

        private List<KeyValuePair<int, int>> GetNeighbourCoords(KeyValuePair<int, int> coord)
        {
            int i = coord.Key, j = coord.Value;
            var coords = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(i - 1, j), new KeyValuePair<int, int>(i, j - 1), new KeyValuePair<int, int>(i + 1, j), new KeyValuePair<int, int>(i, j + 1) };

            for (int k = 0; k < coords.Count; k++)
                if (!InBounds(coords[k]))
                {
                    coords.RemoveAt(k);
                    k--;
                }

            return coords;
        }

        private bool InBounds(KeyValuePair<int, int> coord)
        {
            return coord.Key >= 0 && coord.Key < n && coord.Value >= 0 && coord.Value < m;
        }
    }
}
