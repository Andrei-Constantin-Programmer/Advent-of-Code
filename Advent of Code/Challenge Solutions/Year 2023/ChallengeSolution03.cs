using Advent_of_Code.Utilities;

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
        Dictionary<(int row, int column), List<int>> gears = new();

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

                ProcessGearNumberAddition(gears, row, digitSequence);
                digitSequence.Clear();
            }

            ProcessGearNumberAddition(gears, row, digitSequence);
        }

        int totalGearRatioSum = gears.Keys
            .Where(gearPos => gears[gearPos].Count == 2)
            .Select(gearPos => gears[gearPos].Aggregate((acc, x) => acc * x))
            .Sum();

        Console.WriteLine(totalGearRatioSum);
    }

    private int ProcessDigitSequence(int row, List<(int index, char character)> digitSequence)
        => digitSequence.Count > 0
            ? GetPartNumber(row, digitSequence)
            : 0;

    private int GetPartNumber(int row, List<(int index, char character)> digitSequence) =>
        digitSequence.Any(digit => IsAdjacentToSymbol(row, digit.index))
            ? int.Parse(string.Concat(digitSequence.Select(digit => digit.character)))
            : 0;
    
    private bool IsAdjacentToSymbol(int row, int column)
    {
        int rows = _engineSchematic.GetLength(0);
        int columns = _engineSchematic.GetLength(1);

        for (int neighbourRow = Math.Max(0, row - 1); neighbourRow <= Math.Min(rows - 1, row + 1); neighbourRow++)
        {
            for (int neighbourColumn = Math.Max(0, column - 1); neighbourColumn <= Math.Min(columns - 1, column + 1); neighbourColumn++)
            {
                if ((neighbourRow != row || neighbourColumn != column) 
                    && IsSymbol(_engineSchematic[neighbourRow, neighbourColumn]))
                {
                    return true;
                }
            }
        }

        return false;

        static bool IsSymbol(char character) => !char.IsDigit(character) && character != '.';
    }

    private void ProcessGearNumberAddition(Dictionary<(int row, int column), List<int>> gears, int row, List<(int index, char character)> digitSequence)
    {
        (int row, int column) gearPosition = (0, 0);
        if (!digitSequence.Any(digit => TryGetAdjacentGearPosition(row, digit.index, out gearPosition)))
        {
            return;
        }

        if (gears.ContainsKey(gearPosition))
        {
            gears[gearPosition].Add(GetPartNumber(row, digitSequence));
        }
        else
        {
            gears[gearPosition] = new List<int>() { GetPartNumber(row, digitSequence) };
        }
    }

    // We assume no numbers are adjacent to more than one gear
    private bool TryGetAdjacentGearPosition(int row, int column, out (int row, int column) gearPosition)
    {
        int rows = _engineSchematic.GetLength(0);
        int columns = _engineSchematic.GetLength(1);

        for (int neighbourRow = Math.Max(0, row - 1); neighbourRow <= Math.Min(rows - 1, row + 1); neighbourRow++)
        {
            for (int neighbourColumn = Math.Max(0, column - 1); neighbourColumn <= Math.Min(columns - 1, column + 1); neighbourColumn++)
            {
                if ((neighbourRow != row || neighbourColumn != column)
                    && _engineSchematic[neighbourRow, neighbourColumn] == '*')
                {
                    gearPosition = (neighbourRow, neighbourColumn);
                    return true;
                }
            }
        }

        gearPosition = (-1, -1);
        return false;
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