// Task: https://adventofcode.com/2021/day/9

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

class ChallengeSolution09 : ChallengeSolution
{
    private int rows, columns;

    public ChallengeSolution09(IConsole console) : base(console)
    {
        string[] lines = Reader.ReadLines(this);
        rows = lines.Length;
        columns = lines[0].Length;
    }

    public override void SolveFirstPart()
    {
        int[,] heightMap = ReadHeightMap();
        int sum = 0;
        var lowPoints = GetLowPoints(heightMap);

        foreach (var point in lowPoints)
        {
            sum += heightMap[point.Row, point.Column] + 1;
        }

        _console.WriteLine(sum);
    }

    public override void SolveSecondPart()
    {
        var heightMap = ReadHeightMap();
        var lowPoints = GetLowPoints(heightMap);

        int basins;
        CreateBasins(heightMap, lowPoints, out basins);

        _console.WriteLine(CalculateBasinSizes(heightMap, basins)
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate((x, y) => x * y));
    }

    private List<int> CalculateBasinSizes(int[,] heightMap, int basins)
    {
        var basinToSize = new Dictionary<int, int>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var basinNumber = heightMap[i, j];
                if (basinNumber < 0)
                {
                    if (basinToSize.ContainsKey(basinNumber))
                        basinToSize[basinNumber]++;
                    else
                        basinToSize.Add(basinNumber, 1);
                }
            }
        }

        return basinToSize.Values.ToList();
    }

    private void CreateBasins(int[,] heightMap, List<Coordinate> lowPoints, out int basins)
    {
        basins = 0;
        foreach (var lowPoint in lowPoints)
        {
            if (heightMap[lowPoint.Row, lowPoint.Column] < 9)
            {
                basins++;
                GetBasinMembers(heightMap, lowPoint, basins);
            }
        }
    }

    private HashSet<Coordinate> GetBasinMembers(int[,] heightMap, Coordinate startingPoint, int basinNumber)
    {
        var basinMembers = new HashSet<Coordinate>();
        if (heightMap[startingPoint.Row, startingPoint.Column] < 0)
            return basinMembers;

        basinMembers.Add(startingPoint);
        heightMap[startingPoint.Row, startingPoint.Column] = -basinNumber;

        foreach (var neighbour in GetNeighbourCoords(startingPoint))
        {
            if (InBounds(neighbour))
            {
                if (heightMap[neighbour.Row, neighbour.Column] < 9 && heightMap[neighbour.Row, neighbour.Column] >= 0)
                {
                    basinMembers.Add(neighbour);
                    foreach (var member in GetBasinMembers(heightMap, neighbour, basinNumber))
                        basinMembers.Add(member);
                }
            }
        }

        return basinMembers;
    }

    private List<Coordinate> GetLowPoints(int[,] heightMap)
    {
        var lowPoints = new List<Coordinate>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (IsLowPoint(heightMap, new Coordinate(i, j)))
                {
                    lowPoints.Add(new Coordinate(i, j));
                }
            }
        }

        return lowPoints;
    }

    private bool IsLowPoint(int[,] heightMap, Coordinate coord)
    {
        foreach (var neighbour in GetNeighbourCoords(coord))
        {
            if (InBounds(coord) && heightMap[neighbour.Row, neighbour.Column] <= heightMap[coord.Row, coord.Column])
            {
                return false;
            }
        }

        return true;
    }

    private List<Coordinate> GetNeighbourCoords(Coordinate coord)
    {
        var coords = new List<Coordinate>()
        {
            new Coordinate(coord.Row - 1, coord.Column),
            new Coordinate(coord.Row, coord.Column - 1),
            new Coordinate(coord.Row + 1, coord.Column),
            new Coordinate(coord.Row, coord.Column + 1)
        };

        for (int k = 0; k < coords.Count; k++)
            if (!InBounds(coords[k]))
            {
                coords.RemoveAt(k);
                k--;
            }

        return coords;
    }

    private bool InBounds(Coordinate coord)
    {
        return coord.Row >= 0 && coord.Row < rows && coord.Column >= 0 && coord.Column < columns;
    }

    private int[,] ReadHeightMap()
    {
        var heightMap = new int[rows, columns];
        var lines = Reader.ReadLines(this);
        for (int i = 0; i < rows; i++)
        {
            char[] heightsAsChar = lines[i].ToCharArray();
            byte[] heights = Array.ConvertAll(heightsAsChar, character => (byte)Char.GetNumericValue(character));
            for (int j = 0; j < columns; j++)
                heightMap[i, j] = heights[j];
        }

        return heightMap;
    }

    private record Coordinate(int Row, int Column);
}
