// Task: https://adventofcode.com/2023/day/9

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution09(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        _console.WriteLine(ComputePredictionSum(ExtrapolationDirection.Forwards));
    }

    public override void SolveSecondPart()
    {
        _console.WriteLine(ComputePredictionSum(ExtrapolationDirection.Backwards));
    }

    private long ComputePredictionSum(ExtrapolationDirection direction)
    {
        var lines = Reader.ReadLines(this);

        long predictionSum = 0;
        foreach (var line in lines)
        {
            var values = line.Split(' ').Select(long.Parse).ToList();
            var terminalElements = GetTerminalElements(values, direction);
            predictionSum += ComputePrediction(terminalElements, direction);
        }

        return predictionSum;
    }

    private static long ComputePrediction(List<long> terminalElements, ExtrapolationDirection? direction)
    {
        long prediction = 0;

        for (var i = terminalElements.Count - 1; i >= 0; i--)
        {
            prediction = direction switch
            {
                ExtrapolationDirection.Forwards => terminalElements[i] + prediction,
                ExtrapolationDirection.Backwards => terminalElements[i] - prediction,

                _ => throw new ArgumentException($"Unknown direction {direction}")
            };
        }

        return prediction;
    }

    private static List<long> GetTerminalElements(List<long> initialValues, ExtrapolationDirection direction)
    {
        var values = new List<long>(initialValues);
        List<long> terminalElements = new() { GetTerminalValue(values, direction) };

        while (values.Any(x => x != 0))
        {
            for (var i = values.Count - 1; i >= 1; i--)
            {
                values[i] -= values[i - 1];
            }

            values.RemoveAt(0);
            terminalElements.Add(GetTerminalValue(values, direction));
        }

        return terminalElements;
    }

    private static long GetTerminalValue(List<long> values, ExtrapolationDirection direction) => direction switch
    {
        ExtrapolationDirection.Forwards => values[^1],
        ExtrapolationDirection.Backwards => values[0],

        _ => throw new ArgumentException($"Unknown direction {direction}")
    };

    private enum ExtrapolationDirection
    {
        Backwards, Forwards
    }
}
