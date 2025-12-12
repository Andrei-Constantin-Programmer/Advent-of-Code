// Task: https://adventofcode.com/2025/day/12

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution12(IConsole console, ISolutionReader<ChallengeSolution12> reader)
    : ChallengeSolution<ChallengeSolution12>(console, reader)
{
    private const char PresentChar = '#';
    private const char Colon = ':';
    private const char RegionDimensionsSeparator = 'x';
    
    public override void SolveFirstPart()
    {
        var (presents, regions) = ReadPresentsAndRegions();

        var validRegions = 0;

        foreach (var region in regions)
        {
            var minimumRequiredSpace = presents
                .Sum(present => region.CountByPresent[present] * present.UnitCount);

            // Greedy, this is as if you tore up the presents into single-unit chunks and saw if they fit that way in the grid,
            // ignoring present borders and such. Sorry for the ruined presents, elves!
            if (minimumRequiredSpace <= region.TotalUnitCount)
            {
                validRegions++;
            }
        }

        Console.WriteLine($"Regions under trees where all presents fit: {validRegions}");
    }

    public override void SolveSecondPart()
    {
        Console.WriteLine("There is no second part to this challenge!");
    }

    private (List<Present>, List<Region>) ReadPresentsAndRegions()
    {
        var lines = Reader.ReadLines();

        var presents = ReadPresentsFromInput(lines, out var regionStartIndex);

        var regions = ReadRegionsFromInput(lines, presents, regionStartIndex);

        return (presents, regions);
    }

    private static List<Present> ReadPresentsFromInput(string[] lines, out int i)
    {
        List<Present> presents = [];
        
        List<bool[]> presentShapeRows = [];
        for (i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Contains(RegionDimensionsSeparator))
            {
                break;
            }
            
            if (line.Contains(Colon))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                presents.Add(new Present(presentShapeRows));
                presentShapeRows = [];
                continue;
            }

            presentShapeRows.Add(line
                .Select(c => c == PresentChar)
                .ToArray());
        }

        return presents;
    }
    
    private static List<Region> ReadRegionsFromInput(string[] lines, List<Present> presents, int regionStartIndex)
    {
        List<Region> regions = [];
        for (var i = regionStartIndex; i < lines.Length; i++)
        {
            var line = lines[i];
            var elems = line.Split(Colon, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
            var regionSize = elems[0]
                .Split(RegionDimensionsSeparator)
                .Select(int.Parse)
                .ToArray();

            var presentCounts = elems[1]
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            Dictionary<Present, int> countByPresent = [];
            for (var presentIndex = 0; presentIndex < presents.Count; presentIndex++)
            {
                countByPresent.Add(presents[presentIndex], presentCounts[presentIndex]);
            }
            
            regions.Add(new Region(regionSize[0], regionSize[1], countByPresent));
        }

        return regions;
    }

    private record Region(int Rows, int Cols, Dictionary<Present, int> CountByPresent)
    {
        public readonly int TotalUnitCount = Rows * Cols;
    }

    private class Present
    {
        public int UnitCount { get; }
        
        public Present(List<bool[]> shapeRows)
        {
            var shape = new bool[shapeRows.Count, shapeRows[0].Length];
            for (var row = 0; row < shape.GetLength(0); row++)
            {
                for (var col = 0; col < shape.GetLength(1); col++)
                {
                    shape[row, col] = shapeRows[row][col];
                }
            }
            
            UnitCount = shape
                .Cast<bool>()
                .Count(isPresent => isPresent);
        }
    }
}