// Task: https://adventofcode.com/2025/day/7

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution07(IConsole console, ISolutionReader<ChallengeSolution07> reader)
    : ChallengeSolution<ChallengeSolution07>(console, reader)
{
    private const char BeamStart = 'S';
    private const char Splitter = '^';
    
    public override void SolveFirstPart()
    {
        var lines = Reader.ReadLines();
        var splittersByRow = ReadTachyonManifold(lines, out var beamStart);

        HashSet<int> beamColumns = [beamStart.Col];
        var beamSplitCount = 0;

        for (var row = 1; row < lines.Length; row++)
        {
            HashSet<int> newBeamColumns = [];

            foreach (var beamColumn in beamColumns)
            {
                if (beamColumn < 0 || beamColumn >= lines[row].Length)
                {
                    continue;
                }

                if (!splittersByRow[row].Contains(beamColumn))
                {
                    newBeamColumns.Add(beamColumn);
                    continue;
                }

                beamSplitCount++;
                newBeamColumns.Add(beamColumn - 1);
                newBeamColumns.Add(beamColumn + 1);
            }
            
            beamColumns = newBeamColumns;
        }
        
        Console.WriteLine($"Beam split: {beamSplitCount} times");
    }

    public override void SolveSecondPart()
    {
        var lines = Reader.ReadLines();
        var splittersByRow = ReadTachyonManifold(lines, out var beamStart);

        var timelines = ComputeTimelines(lines, splittersByRow, [], beamStart.Row + 1, beamStart.Col);
        
        Console.WriteLine($"Active timelines: {timelines}");
    }
    
    private static long ComputeTimelines(
        string[] lines, 
        Dictionary<int, List<int>> splittersByRow,
        Dictionary<(int, int), long> timelinesFromRowCol,
        int currentRow,
        int tachyonColumn)
    {
        if (tachyonColumn < 0 || tachyonColumn > lines.Length)
        {
            return 0;
        }
        
        if (currentRow >= splittersByRow.Count)
        {
            return 1;
        }

        if (!splittersByRow[currentRow].Contains(tachyonColumn))
        {
            if (timelinesFromRowCol.TryGetValue((currentRow, tachyonColumn), out var timelines))
            {
                return timelines;
            }
            
            timelines = ComputeTimelines(lines, splittersByRow, timelinesFromRowCol, currentRow + 1, tachyonColumn); 
            timelinesFromRowCol.Add((currentRow, tachyonColumn), timelines);
            return timelines;
        }

        var timelinesLeft = ComputeTimelines(lines, splittersByRow, timelinesFromRowCol, currentRow + 1, tachyonColumn - 1);
        var timelinesRight = ComputeTimelines(lines, splittersByRow, timelinesFromRowCol, currentRow + 1, tachyonColumn + 1);

        return timelinesLeft + timelinesRight;
    }

    private static Dictionary<int, List<int>> ReadTachyonManifold(string[] lines, out Location beamStart)
    {
        Dictionary<int, List<int>> splittersByRow = [];
        beamStart = new(0, 0);

        for (var row = 0; row < lines.Length; row++)
        {
            splittersByRow.Add(row, []);
            for (var col = 0; col < lines[row].Length; col++)
            {
                switch (lines[row][col])
                {
                    case BeamStart:
                        beamStart = new Location(row, col);
                        continue;
                    case Splitter:
                        splittersByRow[row].Add(col);
                        break;
                }
            }
        }

        return splittersByRow;
    }

    private record struct Location(int Row, int Col);
}
