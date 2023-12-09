using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019;

internal class ChallengeSolution04 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var inputNumbers = Reader.ReadLines(this)[0].Split('-').Select(int.Parse).ToArray();
        var rangeStart = inputNumbers[0];
        var rangeEnd = inputNumbers[1];

        Console.WriteLine(FindPasswordCount(rangeStart, rangeEnd));
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int FindPasswordCount(int rangeStart, int rangeEnd)
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
                                
                                var containsDouble = digit1 == digit2 || digit2 == digit3 || digit3 == digit4 || digit4 == digit5 || digit5 == digit6;

                                if (containsDouble)
                                {
                                    Console.WriteLine(number);
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
}
