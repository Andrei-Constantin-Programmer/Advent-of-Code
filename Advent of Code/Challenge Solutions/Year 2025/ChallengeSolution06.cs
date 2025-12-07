// Task: https://adventofcode.com/2025/day/6

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution06(IConsole console, ISolutionReader<ChallengeSolution06> reader)
    : ChallengeSolution<ChallengeSolution06>(console, reader)
{
    private const char Whitespace = ' ';
    private const char Separator = '|';
    
    public override void SolveFirstPart()
    {
        var problems = ReadProblemsHuman();

        var grandTotal = problems.Sum(p => p.Solve());
        
        Console.WriteLine($"Grand total: {grandTotal}");
    }

    public override void SolveSecondPart()
    {
        var problems = ReadProblemsCephalopod();

        var grandTotal = problems.Sum(p => p.Solve());
        
        Console.WriteLine($"Grand total: {grandTotal}");
    }

    private List<Problem> ReadProblemsCephalopod()
    {
        List<Problem> problems = [];

        var lines = ReadWorkSheetWithWhitespace();

        for (var col = 0; col < lines[0].Length; col++)
        {
            var numbers = GetAllVerticalNumbers(lines, col);
            var operation = GetOperationForColumn(lines, col);
            
            problems.Add(new(numbers, operation)); 
        }
            
        return problems;
    }

    private static long[] GetAllVerticalNumbers(string[][] lines, int col)
    {
        List<long> numbers = [];

        var numericRows = lines.SkipLast(1).Select(row => row[col]).ToArray();

        for (var numericCol = numericRows[0].Length - 1; numericCol >= 0; numericCol--)
        {
            numbers.Add(ComputeNumberFromNumericRowsAndColumn(numericRows, numericCol));
        }

        return numbers.ToArray();
    }

    private static long ComputeNumberFromNumericRowsAndColumn(string[] numericRows, int numericCol)
    {
        List<char> numberAsDigits = [];

        foreach (var row in numericRows)
        {
            var character = numericCol < row.Length ? row[numericCol] : Whitespace;
            if (character is Whitespace)
            {
                continue;
            }
                    
            numberAsDigits.Add(character);
        }
        
        return long.Parse(new string(numberAsDigits.ToArray()));
    }

    private string[][] ReadWorkSheetWithWhitespace()
    {
        var lines = Reader
            .ReadLines()
            .Select(line => line.ToCharArray())
            .ToArray();
        
        for (var charCol = 0; charCol < lines[0].Length; charCol++)
        {
            if (!lines.All(line => line[charCol] is Whitespace))
            {
                continue;
            }

            foreach (var line in lines)
            {
                line[charCol] = Separator;
            }
        }

        var newLines = lines.Select(line => new string(line)).ToArray();
        
        return newLines
            .Select(line => line.Split(Separator))
            .ToArray();
    }

    private List<Problem> ReadProblemsHuman()
    {
        List<Problem> problems = [];

        var lines = ReadWorksheetWithNoWhitespace();

        for (var col = 0; col < lines[0].Length; col++)
        {
            var numbers = new long[lines.Length - 1];
            for (var row = 0; row < lines.Length - 1; row++)
            {
                numbers[row] = long.Parse(lines[row][col]);
            }

            var operation = GetOperationForColumn(lines, col);
            
            problems.Add(new(numbers, operation));
        }

        return problems;
    }

    private static Operation GetOperationForColumn(string[][] lines, int col) => (Operation) lines[^1][col][0];

    private string[][] ReadWorksheetWithNoWhitespace() =>
        Reader.ReadLines()
            .Select(line => line.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

    private record Problem(long[] Numbers, Operation Operation)
    {
        public long Solve() =>
            Operation switch
            {
                Operation.Addition => Numbers.Sum(),
                Operation.Multiplication => Numbers.Aggregate(1L, (acc, n) => acc * n),
                
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    private enum Operation
    {
        Addition = '+',
        Multiplication = '*'
    }
}
