// Task: https://adventofcode.com/2024/day/7

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution07(IConsole console, ISolutionReader<ChallengeSolution07> reader)
    : ChallengeSolution<ChallengeSolution07>(console, reader)
{
    public override void SolveFirstPart()
    {
        var equations = ReadEquations();

        var totalCalibrationResult = CalculateTotalCalibrationResult(equations);

        _console.WriteLine($"Total Calibration Result: {totalCalibrationResult}");
    }

    public override void SolveSecondPart()
    {
        var equations = ReadEquations();

        var totalCalibrationResult = CalculateTotalCalibrationResult(equations, true);

        _console.WriteLine($"Total Calibration Result: {totalCalibrationResult}");
    }

    private static long CalculateTotalCalibrationResult(List<Equation> equations, bool allowConcatenation = false)
    {
        long totalCalibrationResult = 0;

        foreach (var equation in equations)
        {
            if (IsTrueEquation(equation, allowConcatenation))
            {
                totalCalibrationResult += equation.Result;
            }
        }

        return totalCalibrationResult;
    }

    private static bool IsTrueEquation(Equation equation, bool allowConcatenation = false)
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

            if (allowConcatenation)
            {
                signs[position] = Sign.Concatenate;
                if (VerifyEquationIsTrue(position + 1, signs))
                {
                    return true;
                }
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
                else if (signs[i] == Sign.Multiply)
                {
                    result *= equation.Values[i + 1];
                }
                else if (signs[i] == Sign.Concatenate)
                {
                    result = long.Parse($"{result}{equation.Values[i + 1]}");
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
        Concatenate
    }
}
