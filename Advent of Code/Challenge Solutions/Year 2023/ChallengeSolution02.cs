using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution02 : ChallengeSolution
{
    private const int MAX_RED_CUBES = 12;
    private const int MAX_GREEN_CUBES = 13;
    private const int MAX_BLUE_CUBES = 14;

    protected override void SolveFirstPart()
    {
        int totalIdSum = 0;

        foreach (var line in Reader.ReadLines(this))
        {
            Game game = ReadGameFromInputLine(line);

            bool isGamePossible = game.CubeSets.All(cubeSet => cubeSet.All(CubeGroupIsPossible));

            if (isGamePossible)
            {
                totalIdSum += game.Id;
            }
        }

        Console.WriteLine(totalIdSum);
    }

    protected override void SolveSecondPart()
    {
        long totalPowerSum = 0;

        foreach (var line in Reader.ReadLines(this))
        {
            Game game = ReadGameFromInputLine(line);

            long minimumSetPower = game.CubeSets
                .SelectMany(cubeSet => cubeSet)
                .GroupBy(cubeGroup => cubeGroup.Color)
                .Select(colorGrouping => colorGrouping.Max(cubeGroup => cubeGroup.Count))
                .Aggregate((acc, x) => acc * x);

            totalPowerSum += minimumSetPower;
        }

        Console.WriteLine(totalPowerSum);
    }

    private static bool CubeGroupIsPossible(CubeGroup cubeGroup) => cubeGroup.Color switch
    {
        CubeColor.Red => cubeGroup.Count <= MAX_RED_CUBES,
        CubeColor.Green => cubeGroup.Count <= MAX_GREEN_CUBES,
        CubeColor.Blue => cubeGroup.Count <= MAX_BLUE_CUBES,

        _ => throw new ArgumentException($"Unknown color {nameof(cubeGroup.Color)}")
    };

    private static Game ReadGameFromInputLine(string line)
    {
        List<HashSet<CubeGroup>> cubeSets = new();

        var elements = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
        var gameId = int.Parse(elements[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        var cubeSetStrings = elements[1].Split(";", StringSplitOptions.RemoveEmptyEntries);

        foreach (var cubeSetString in cubeSetStrings)
        {
            var cubeSet = cubeSetString
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .Select(group =>
                {
                    string[] cubeGroupStrings = group.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    return new CubeGroup(int.Parse(cubeGroupStrings[0]), GetCubeColor(cubeGroupStrings[1]));
                })
                .ToHashSet();

            cubeSets.Add(cubeSet);
        }

        return new Game(gameId, cubeSets);
    }

    private record Game(int Id, List<HashSet<CubeGroup>> CubeSets);

    private record CubeGroup(int Count, CubeColor Color);

    private enum CubeColor { Red, Green, Blue };

    private static CubeColor GetCubeColor(string cubeColorString)
    {
        return cubeColorString switch
        {
            "red" => CubeColor.Red,
            "green" => CubeColor.Green,
            "blue" => CubeColor.Blue,

            _ => throw new ArgumentException($"Unknown color {cubeColorString}")
        };
    }
}
