// Task: https://adventofcode.com/2023/day/1

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution01 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        int calibrationSum = 0;

        string? line;
        using TextReader read = Reader.GetInputFile(2023, 1);
        while ((line = read.ReadLine()) != null)
        {
            calibrationSum += ComputeCalibrationValue(line);
        }

        Console.WriteLine(calibrationSum);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int ComputeCalibrationValue(string line)
    {
        char firstDigit = line.First(c => char.IsDigit(c));
        char lastDigit = line.Last(c => char.IsDigit(c));

        string calibrationValue = $"{firstDigit}{lastDigit}";

        return int.Parse(calibrationValue);
    }
}
