// Task: https://adventofcode.com/2023/day/1

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution01(IConsole console, ISolutionReader<ChallengeSolution01> reader)
    : ChallengeSolution<ChallengeSolution01>(console, reader)
{
    private readonly Dictionary<string, char> _digitWords = new()
    {
        { "zero", '0' },
        { "one", '1' },
        { "two", '2' },
        { "three", '3' },
        { "four", '4' },
        { "five", '5' },
        { "six", '6' },
        { "seven", '7' },
        { "eight", '8' },
        { "nine", '9' }
    };

    public override void SolveFirstPart()
    {
        int calibrationSum = ComputeCalibrationSum(false);
        _console.WriteLine(calibrationSum);
    }

    public override void SolveSecondPart()
    {
        int calibrationSum = ComputeCalibrationSum(true);
        _console.WriteLine(calibrationSum);
    }

    private int ComputeCalibrationSum(bool includeWords)
    {
        int calibrationSum = 0;

        foreach (var line in _reader.ReadLines())
        {
            calibrationSum += ComputeCalibrationValue(line, includeWords);
        }

        return calibrationSum;
    }

    private int ComputeCalibrationValue(string line, bool doIncludeWords)
    {
        char firstDigit = line.FirstOrDefault(char.IsDigit);
        char lastDigit = line.LastOrDefault(char.IsDigit);

        if (doIncludeWords)
        {
            firstDigit = FindFirstDigitWord(line, firstDigit);
            lastDigit = FindLastDigitWord(line, lastDigit);
        }

        string calibrationValue = $"{firstDigit}{lastDigit}";

        return int.TryParse(calibrationValue, out int calibrationNumber) ? calibrationNumber : 0;
    }

    private char FindFirstDigitWord(string line, char currentFirstDigit)
    {
        var firstDigitIndex = line.IndexOf(currentFirstDigit);

        foreach (var word in _digitWords.Keys)
        {
            var firstWordIndex = line.IndexOf(word);
            if (firstDigitIndex == -1 || (firstWordIndex > -1 && firstDigitIndex > firstWordIndex))
            {
                firstDigitIndex = firstWordIndex;
                currentFirstDigit = _digitWords[word];
            }
        }

        return currentFirstDigit;
    }

    private char FindLastDigitWord(string line, char currentLastDigit)
    {
        var lastDigitIndex = line.LastIndexOf(currentLastDigit);

        foreach (var word in _digitWords.Keys)
        {
            var lastWordIndex = line.LastIndexOf(word);
            if (lastDigitIndex == -1 || (lastWordIndex > -1 && lastDigitIndex < lastWordIndex))
            {
                lastDigitIndex = lastWordIndex;
                currentLastDigit = _digitWords[word];
            }
        }

        return currentLastDigit;
    }
}
