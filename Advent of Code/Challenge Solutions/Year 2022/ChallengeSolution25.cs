// Task: https://adventofcode.com/2022/day/25

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution25 : ChallengeSolution
{
    private readonly List<long> fuelRequirements;

    public ChallengeSolution25(IConsole console) : base(console)
    {
        fuelRequirements = ReadFuelRequirements();
    }

    public override void SolveFirstPart()
    {
        var fuelRequirementsSum = fuelRequirements.Sum();

        Console.WriteLine(fuelRequirementsSum);
        Console.WriteLine(DecimalToSnafu(fuelRequirementsSum));
    }

    public override void SolveSecondPart()
    {
        Console.WriteLine("There is no second part to this challenge!");
    }

    private List<long> ReadFuelRequirements()
    {
        var fuelRequirements = new List<long>();
        foreach (var line in Reader.ReadLines(this))
        {
            fuelRequirements.Add(SnafuToDecimal(line));
        }

        return fuelRequirements;
    }

    private static string DecimalToSnafu(long decimalNumber)
    {
        return new string(DecimalToSnafuReverse(decimalNumber).Reverse().ToArray());
    }

    private static string DecimalToSnafuReverse(long decimalNumber)
    {
        if (decimalNumber == 0)
            return "";

        var snafu = (decimalNumber % 5) switch
        {
            0 => "0" + DecimalToSnafuReverse(decimalNumber / 5),
            1 => "1" + DecimalToSnafuReverse(decimalNumber / 5),
            2 => "2" + DecimalToSnafuReverse(decimalNumber / 5),
            3 => "=" + DecimalToSnafuReverse(decimalNumber / 5 + 1),
            4 => "-" + DecimalToSnafuReverse(decimalNumber / 5 + 1),

            _ => throw new Exception("Error when dividing by 5"),
        };

        return snafu;
    }

    private static long SnafuToDecimal(string snafuNumber)
    {
        if (snafuNumber == "0")
            return 0;

        long number = 0;
        for (var i = 0; i < snafuNumber.Length; i++)
        {
            var value = (long)Math.Pow(5, i);
            number += value * SnafuDigitToDecimal(snafuNumber[snafuNumber.Length - 1 - i]);
        }

        return number;
    }

    private static long SnafuDigitToDecimal(char snafuDigit) => snafuDigit switch
    {
        '2' => 2,
        '1' => 1,
        '0' => 0,
        '-' => -1,
        '=' => -2,

        _ => throw new ArgumentException($"Unrecognised SNAFU digit {snafuDigit}")
    };
}
