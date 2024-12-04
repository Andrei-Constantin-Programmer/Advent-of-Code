// Task: https://adventofcode.com/2023/day/21

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution21(IConsole console) : ChallengeSolution(console)
{
    private const int MAX_STEPS = 64;
    private const char PLOT = '.';
    private const char START = 'S';

    public override void SolveFirstPart()
    {
        var garden = ReadGarden(out var startPlot);
        var latestPlotsReached = FindLastPlotsReached(garden, startPlot);

        _console.WriteLine(latestPlotsReached);
    }

    private static int FindLastPlotsReached(string[] garden, Point startPlot)
    {
        Queue<Point> nextPlotsToVisit = new();
        HashSet<Point> lastPlotsReached = new();

        nextPlotsToVisit.Enqueue(startPlot);

        var stepsTaken = 0;
        var lastPlotsReachedCount = 0;
        while (nextPlotsToVisit.TryDequeue(out var step) && stepsTaken < MAX_STEPS)
        {
            foreach (var plot in GetNeighbouringPlots(garden, step))
            {
                lastPlotsReached.Add(plot);
            }

            if (nextPlotsToVisit.Count == 0)
            {
                nextPlotsToVisit = new Queue<Point>(lastPlotsReached);
                lastPlotsReachedCount = lastPlotsReached.Count;
                lastPlotsReached = new();
                stepsTaken++;
            }
        }

        return lastPlotsReachedCount;
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static List<Point> GetNeighbouringPlots(string[] garden, Point point)
    {
        List<Point> neighbours = new();
        if (point.Row - 1 >= 0 && IsPlot(garden[point.Row - 1][point.Column]))
        {
            neighbours.Add(new(point.Row - 1, point.Column));
        }
        if (point.Row + 1 < garden.Length && IsPlot(garden[point.Row + 1][point.Column]))
        {
            neighbours.Add(new(point.Row + 1, point.Column));
        }
        if (point.Column - 1 >= 0 && IsPlot(garden[point.Row][point.Column - 1]))
        {
            neighbours.Add(new(point.Row, point.Column - 1));
        }
        if (point.Column + 1 < garden[0].Length && IsPlot(garden[point.Row][point.Column + 1]))
        {
            neighbours.Add(new(point.Row, point.Column + 1));
        }

        return neighbours;
    }

    private static bool IsPlot(char gardenPlot) => gardenPlot is PLOT or START;

    private string[] ReadGarden(out Point start)
    {
        var lines = Reader.ReadLines(this);
        start = new(-1, -1);

        for (var row = 0; row < lines.Length && start.Row == -1; row++)
        {
            for (var col = 0; col < lines[row].Length && start.Row == -1; col++)
            {
                if (lines[row][col] == START)
                {
                    start = new(row, col);
                }
            }
        }

        return lines;
    }

    private record Point(int Row, int Column);
}
