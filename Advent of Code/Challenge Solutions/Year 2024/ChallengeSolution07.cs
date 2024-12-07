// Task: https://adventofcode.com/2024/day/7

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution07(IConsole console, ISolutionReader<ChallengeSolution07> reader)
    : ChallengeSolution<ChallengeSolution07>(console, reader)
{
    public override void SolveFirstPart()
    {
        var equations = ReadEquations();

        long totalCalibrationResult = 0;

        foreach (var equation in equations)
        {
            if (IsTrueEquation(equation))
            {
                totalCalibrationResult += equation.Result;
            }
            else
            {
                Console.WriteLine(equation.Result);
            }
        }

        _console.WriteLine($"Total Calibration Result: {totalCalibrationResult}");
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static bool IsTrueEquation(Equation equation)
    {
        var signs = new Sign[equation.Values.Length - 1];

        return VerifyEquationIsTrue(0, signs);

        bool VerifyEquationIsTrue(int position, Sign[] signs)
        {
            if (position == signs.Length)
            {
                return IsTrueEquation(signs);
            }

            signs[position] = Sign.Add;
            if (VerifyEquationIsTrue(position + 1, signs))
            {
                return true;
            }

            signs[position] = Sign.Multiply;
            if (VerifyEquationIsTrue(position + 1, signs))
            {
                return true;
            }

            return false;
        }

        bool IsTrueEquation(Sign[] signs)
        {
            var result = equation.Values[0];

            for (var i = 0; i < signs.Length; i++)
            {
                if (signs[i] == Sign.Add)
                {
                    result += equation.Values[i + 1];
                }
                else
                {
                    result *= equation.Values[i + 1];
                }
            }

            return result == equation.Result;
        }
    }

    private List<Equation> ReadEquations()
    {
        var equations = new List<Equation>();

        foreach (var line in _reader.ReadLines())
        {
            var parts = line.Split(": ");
            var result = long.Parse(parts[0]);
            var values = parts[1]
                .Split(" ")
                .Select(long.Parse)
                .ToArray();

            equations.Add(new Equation(result, values));
        }

        return equations;
    }

    private record struct Equation(long Result, long[] Values);

    private enum Sign
    {
        Add,
        Multiply,
    }
}
