// Task: https://adventofcode.com/2023/day/6

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution06(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        var races = ReadRaceInformation();

        long winningPossibilityProduct = 1;
        foreach (var race in races)
        {
            winningPossibilityProduct *= ComputeWinniningPossibilityCount(race);
        }

        _console.WriteLine(winningPossibilityProduct);
    }

    public override void SolveSecondPart()
    {
        var lines = ReadInputLines();
        KeyValuePair<long, long> race = new(ParseCombinedInputLine(lines[0]), ParseCombinedInputLine(lines[1]));

        _console.WriteLine(ComputeWinniningPossibilityCount(race));
    }

    private static long ComputeWinniningPossibilityCount(KeyValuePair<long, long> race)
        => GetLastWinningCharge(race) - GetFirstWinningCharge(race) + 1;

    private static long GetFirstWinningCharge(KeyValuePair<long, long> race)
    {
        for (var index = 1; index < race.Key; index++)
        {
            if (ComputeDistanceTravelled(index, race.Key) > race.Value)
            {
                return index;
            }
        }

        return -1;
    }

    private static long GetLastWinningCharge(KeyValuePair<long, long> race)
    {
        for (var index = race.Key - 1; index >= 1; index--)
        {
            if (ComputeDistanceTravelled(index, race.Key) > race.Value)
            {
                return index;
            }
        }

        return -1;
    }

    private static long ComputeDistanceTravelled(long chargeTime, long timeAllowed) => (timeAllowed - chargeTime) * chargeTime;

    private Dictionary<long, long> ReadRaceInformation()
    {
        Dictionary<long, long> races = new();

        var lines = ReadInputLines();
        var times = ParseSplitInputLine(lines[0]);
        var distances = ParseSplitInputLine(lines[1]);
        for (var index = 0; index < times.Count; index++)
        {
            races.Add(times[index], distances[index]);
        }

        return races;
    }

    private string[] ReadInputLines() => Reader.ReadLines(this);

    private static long ParseCombinedInputLine(string line) => long.Parse(string.Concat(line
        .Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        ));

    private static List<long> ParseSplitInputLine(string line) => line
        .Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => long.Parse(x.Trim()))
        .ToList();
}
