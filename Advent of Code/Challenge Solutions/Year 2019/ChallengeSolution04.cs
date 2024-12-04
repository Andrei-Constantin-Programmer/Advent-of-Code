// Task: https://adventofcode.com/2019/day/4

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019;

internal class ChallengeSolution04 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var (rangeStart, rangeEnd) = ReadRange();
        Console.WriteLine(FindPasswordCount(rangeStart, rangeEnd, condition));

        static bool condition(int[] digits) => digits
            .Zip(digits.Skip(1), (a, b) => a == b)
            .Any(isEqualAdjacency => isEqualAdjacency);
    }

    protected override void SolveSecondPart()
    {
        var (rangeStart, rangeEnd) = ReadRange();
        Console.WriteLine(FindPasswordCount(rangeStart, rangeEnd, condition));

        static bool condition(int[] digits)
        {
            for (var i = 0; i < digits.Length - 1; i++)
            {
                if (digits[i] == digits[i + 1])
                {
                    var equalityCount = 2;
                    while (i < digits.Length - 2 && digits[++i] == digits[i + 1])
                    {
                        equalityCount++;
                    }

                    if (equalityCount == 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    private static int FindPasswordCount(int rangeStart, int rangeEnd, Predicate<int[]> condition)
    {
        var passwordCount = 0;

        var firstDigit = rangeStart / 100000;
        for (var digit1 = firstDigit; digit1 <= 9; digit1++)
        {
            for (var digit2 = digit1; digit2 <= 9; digit2++)
            {
                for (var digit3 = digit2; digit3 <= 9; digit3++)
                {
                    for (var digit4 = digit3; digit4 <= 9; digit4++)
                    {
                        for (var digit5 = digit4; digit5 <= 9; digit5++)
                        {
                            for (var digit6 = digit5; digit6 <= 9; digit6++)
                            {
                                var number = int.Parse($"{digit1}{digit2}{digit3}{digit4}{digit5}{digit6}");

                                if (number < rangeStart)
                                {
                                    continue;
                                }

                                if (number > rangeEnd)
                                {
                                    return passwordCount;
                                }
                               
                                if (condition(number.ToString().Select(c => c - '0').ToArray()))
                                {
                                    passwordCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        return passwordCount;
    }

    private (int, int) ReadRange()
    {
        var inputNumbers = Reader.ReadLines(this)[0].Split('-').Select(int.Parse).ToArray();
        var rangeStart = inputNumbers[0];
        var rangeEnd = inputNumbers[1];

        return (rangeStart, rangeEnd);
    }
}
