// Task: https://adventofcode.com/2022/day/16

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution16 : ChallengeSolution<ChallengeSolution16>
{
    private readonly Dictionary<Valve, List<Valve>> adjacentValves;

    public ChallengeSolution16(IConsole console, ISolutionReader<ChallengeSolution16> reader) : base(console, reader)
    {
        adjacentValves = ReadValves();
    }

    public override void SolveFirstPart()
    {
        throw new NotImplementedException();

        _console.WriteLine(GetMaximumPressure(adjacentValves.Keys.First(), 30, 0));
    }

    private int GetMaximumPressure(Valve valve, int minutes, int accumulatedPressure)
    {
        if (minutes <= 0)
            return 0;

        _console.WriteLine(valve.Label);

        int maximumPressure = 0;
        foreach (var v in adjacentValves[valve])
        {
            int pressure = Math.Max(
                GetMaximumPressure(v, minutes - 1, accumulatedPressure),
                GetMaximumPressure(v, minutes - 2, accumulatedPressure + valve.Rate));

            if (pressure > maximumPressure)
                maximumPressure = pressure;
        }

        _console.WriteLine(maximumPressure);

        return accumulatedPressure + maximumPressure;
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private Dictionary<Valve, List<Valve>> ReadValves()
    {
        var valves = new List<Valve>();
        var valveNeighbours = new Dictionary<Valve, string[]>();

        foreach (var line in _reader.ReadLines())
        {
            var elements = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var valveLabel = elements[1];
            var valveRate = Convert.ToInt32(elements[4].Replace(';', ' ').Split('=')[1].Trim());
            var neighbourLabels = elements[9..].Select(label => label.Replace(',', ' ').Trim()).ToArray();

            valves.Add(new Valve(valveLabel, valveRate));
            valveNeighbours.Add(valves.Last(), neighbourLabels);
        }

        var valveDictionary = new Dictionary<Valve, List<Valve>>();

        foreach (var valve in valves)
        {
            valveDictionary.Add(valve, valves.Where(v => valveNeighbours[valve].Contains(v.Label)).ToList());
        }

        return valveDictionary;
    }

    private class Valve
    {
        public string Label { get; init; }
        public int Rate { get; init; }

        public Valve(string label, int rate)
        {
            Label = label;
            Rate = rate;
        }
    }
}
