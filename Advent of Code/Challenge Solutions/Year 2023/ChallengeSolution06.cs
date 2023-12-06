namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution06 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var races = ReadRaceInformation();

        var winningPossibilityProduct = 1;
        foreach (var race in races)
        {
            winningPossibilityProduct *= ComputeWinniningPossibilityCount(race);
        }

        Console.WriteLine(winningPossibilityProduct);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int ComputeWinniningPossibilityCount(KeyValuePair<int, int> race) 
        => GetLastWinningCharge(race) - GetFirstWinningCharge(race) + 1;

    private static int GetFirstWinningCharge(KeyValuePair<int, int> race)
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

    private static int GetLastWinningCharge(KeyValuePair<int, int> race)
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

    private static int ComputeDistanceTravelled(int chargeTime, int timeAllowed) => (timeAllowed - chargeTime) * chargeTime;

    private static Dictionary<int, int> ReadRaceInformation()
    {
        Dictionary<int, int> races = new();

        var lines = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2023, 6));
        var times = ParseInputLine(lines[0]);
        var distances = ParseInputLine(lines[1]);
        for (var index = 0; index < times.Count; index++)
        {
            races.Add(times[index], distances[index]);
        }

        return races;
    }

    private static List<int> ParseInputLine(string line) => line.Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();
}
