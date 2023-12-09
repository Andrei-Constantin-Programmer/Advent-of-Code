using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution09 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        Console.WriteLine(ComputePredictionSum(ExtrapolationDirection.Forwards));
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private long ComputePredictionSum(ExtrapolationDirection direction)
    {
        var lines = Reader.ReadLines(this);

        long predictionSum = 0;
        foreach (var line in lines)
        {
            var values = line.Split(' ').Select(long.Parse).ToList();
            var lastElements = GetLastElements(values);

            predictionSum += ComputePrediction(lastElements, direction);
        }

        return predictionSum;
    }

    private static long ComputePrediction(List<long> finalElements, ExtrapolationDirection? direction)
    {
        long prediction = 0;
        
        for (var i = finalElements.Count - 1; i >= 0; i--)
        {
            prediction = direction switch
            {
                ExtrapolationDirection.Backwards => prediction - finalElements[i],
                _ => prediction + finalElements[i]
            };
        }

        return prediction;
    }

    private static List<long> GetLastElements(List<long> initialValues)
    {
        var values = new List<long>(initialValues);
        List<long> lastElements = new() { values[^1] };

        while (values.Any(x => x != 0))
        {
            for (var i = values.Count - 1; i >= 1; i--)
            {
                values[i] -= values[i - 1];
            }

            values.RemoveAt(0);
            lastElements.Add(values[^1]);
        }

        return lastElements;
    }

    private enum ExtrapolationDirection
    {
        Backwards, Forwards
    }
}
