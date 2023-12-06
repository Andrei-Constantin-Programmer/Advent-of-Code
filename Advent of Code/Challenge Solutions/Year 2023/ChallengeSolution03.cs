namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution03 : ChallengeSolution
{
    private readonly char[,] _engineSchematic;

    public ChallengeSolution03()
    {
        _engineSchematic = ReadEngineSchematic();
    }

    protected override void SolveFirstPart()
    {
        int totalPartNumberSum = 0;

        for (int row = 0; row < _engineSchematic.GetLength(0); row++)
        {
            List<(int index, char character)> digitSequence = new();

            for (int column = 0; column < _engineSchematic.GetLength(1); column++)
            {
                if (char.IsDigit(_engineSchematic[row, column]))
                {
                    digitSequence.Add((column, _engineSchematic[row, column]));
                    continue;
                }

                totalPartNumberSum += ProcessDigitSequence(row, digitSequence);
                digitSequence.Clear();
            }

            totalPartNumberSum += ProcessDigitSequence(row, digitSequence);
        }

        Console.WriteLine(totalPartNumberSum);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private int ProcessDigitSequence(int row, List<(int index, char character)> digitSequence)
        => digitSequence.Count > 0
            ? GetPartNumber(row, digitSequence)
            : 0;

    private int GetPartNumber(int row, List<(int index, char character)> currentNumber) =>
        currentNumber.Any(digit => IsAdjacentToSymbol(row, digit.index))
            ? int.Parse(string.Concat(currentNumber.Select(digit => digit.character)))
            : 0;

    private bool IsAdjacentToSymbol(int i, int j)
    {
        int rows = _engineSchematic.GetLength(0);
        int columns = _engineSchematic.GetLength(1);

        for (int x = Math.Max(0, i - 1); x <= Math.Min(rows - 1, i + 1); x++)
        {
            for (int y = Math.Max(0, j - 1); y <= Math.Min(columns - 1, j + 1); y++)
            {
                if ((x != i || y != j) && IsSymbol(_engineSchematic[x, y]))
                {
                    return true;
                }
            }
        }

        return false;

        static bool IsSymbol(char character) => !char.IsDigit(character) && character != '.';
    }

    private static char[,] ReadEngineSchematic()
    {
        string[] lines = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2023, 3));

        char[,] schematic = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                schematic[i, j] = lines[i][j];
            }
        }

        return schematic;
    }
}