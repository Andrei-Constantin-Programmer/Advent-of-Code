// Task: https://adventofcode.com/2025/day/2

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution02(IConsole console, ISolutionReader<ChallengeSolution02> reader)
    : ChallengeSolution<ChallengeSolution02>(console, reader)
{
    public override void SolveFirstPart()
    {
        var ranges = SplitRangesByDigitCount(ReadRanges());
        RemoveRangesWithOddCount(ranges);

        long invalidIdSum = 0;

        foreach (var range in ranges)
        {
            for (var i = long.Parse(range.Start); i <= long.Parse(range.End); i++)
            {
                if (IsFirstHalfEqualToTheSecondHalf(i))
                {
                    invalidIdSum += i;
                }
            }
        }

        _console.WriteLine($"Invalid ID sum: {invalidIdSum}");
    }

    public override void SolveSecondPart()
    {
        var ranges = SplitRangesByDigitCount(ReadRanges());

        long invalidIdSum = 0;

        foreach (var range in ranges)
        {
            for (var i = long.Parse(range.Start); i <= long.Parse(range.End); i++)
            {
                if (IsMadeOfRepeatedSequences(i))
                {
                    invalidIdSum += i;
                }
            }
        }

        _console.WriteLine($"Invalid ID sum: {invalidIdSum}");
    }

    private static bool IsFirstHalfEqualToTheSecondHalf(long number)
    {
        var digits = GetDigits(number);
        var digitCount = digits.Length;

        if (digitCount % 2 != 0)
        {
            return false;
        }

        var leftHalf = DigitArrayToNumber(digits[..(digitCount / 2)]);
        var rightHalf = DigitArrayToNumber(digits[(digitCount / 2)..]);

        return leftHalf == rightHalf;
    }

    private static bool IsMadeOfRepeatedSequences(long number)
    {
        var digits = GetDigits(number);
        var digitCount = digits.Length;

        for (var sequenceLength = 1; sequenceLength <= digitCount / 2; sequenceLength++)
        {
            if (digitCount % sequenceLength != 0)
            {
                continue;
            }

            var sequences = ChunkString(number.ToString(), sequenceLength).ToArray();
            if (sequences[1..].All(seq => seq == sequences[0]))
            {
                return true;
            }
        }

        return false;
    }

    private static List<Range> SplitRangesByDigitCount(List<Range> ranges)
    {
        List<Range> newRangeList = [];

        foreach (var range in ranges)
        {
            var start = range.Start;
            var end = range.End;

            while (start.Length < end.Length)
            {
                var smallRangeEnd = new string('9', start.Length);
                newRangeList.Add(new(start, smallRangeEnd));

                start = $"1{new string('0', start.Length)}";
            }

            newRangeList.Add(new(start, end));
        }

        return newRangeList;
    }

    private static void RemoveRangesWithOddCount(List<Range> ranges) => ranges.RemoveAll(range => range.Start.Length % 2 != 0);

    private static IEnumerable<string> ChunkString(string @string, int size)
    {
        for (var i = 0; i < @string.Length; i += size)
        {
            var x = "";
            try
            {
                x = @string.Substring(i, size);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            yield return x;
        }
    }

    private static int[] GetDigits(long number)
    {
        List<int> digitList = [];
        do
        {
            digitList.Add((int)(number % 10));
            number /= 10;
        } while (number > 0);

        return digitList.ToArray();
    }

    private static long DigitArrayToNumber(int[] array)
    {
        long number = 0;
        foreach (var x in array)
        {
            number = (number * 10) + x;
        }

        return number;
    }

    private List<Range> ReadRanges()
    {
        var input = _reader.ReadLines()[0];
        var rangeStrings = input.Split(',', StringSplitOptions.TrimEntries);

        List<Range> ranges = [];

        foreach (var range in rangeStrings)
        {
            var rangeElements = range.Split('-', StringSplitOptions.TrimEntries);
            var start = rangeElements[0];
            var end = rangeElements[1];

            ranges.Add(new(start, end));
        }

        return ranges;
    }

    private record struct Range(string Start, string End);
}
