// Task: https://adventofcode.com/2022/day/10

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution10 : ChallengeSolution<ChallengeSolution10>
{
    private const int DISPLAY_WIDTH = 40, DISPLAY_HEIGHT = 6;
    private const char LIT = '#', DARK = ' ';

    private int sumSignalStrengths;
    private char[] flattenedDisplay;

    public ChallengeSolution10(IConsole console, ISolutionReader<ChallengeSolution10> reader) : base(console, reader)
    {
        sumSignalStrengths = 0;

        flattenedDisplay = new char[DISPLAY_HEIGHT * DISPLAY_WIDTH];
        for (int i = 0; i < flattenedDisplay.Length; i++)
            flattenedDisplay[i] = DARK;
    }

    public override void SolveFirstPart()
    {
        var cpu = new CPU();
        var commands = ReadCommands();

        foreach (var command in commands)
        {
            RunCommand(cpu, command, AddSignalStrengthIfInteresting);
        }

        Console.WriteLine(sumSignalStrengths);
    }

    public override void SolveSecondPart()
    {
        var cpu = new CPU();
        var commands = ReadCommands();

        foreach (var command in commands)
        {
            RunCommand(cpu, command, DrawCycle);
        }

        PrintDisplay();
    }

    private void PrintDisplay()
    {
        var display = UnflattenDisplay(flattenedDisplay);

        for (int i = 0; i < DISPLAY_HEIGHT; i++)
        {
            for (int j = 0; j < DISPLAY_WIDTH; j++)
            {
                Console.Write(display[i][j]);
            }
            Console.WriteLine();
        }
    }

    private static char[][] UnflattenDisplay(char[] flattenedDisplay)
    {
        char[][] display = new char[DISPLAY_HEIGHT][];

        for (int i = 0; i < DISPLAY_HEIGHT; i++)
        {
            display[i] = flattenedDisplay.ToList().GetRange(i * DISPLAY_WIDTH, DISPLAY_WIDTH).ToArray();
        }

        return display;
    }

    private void AddSignalStrengthIfInteresting(CPU cpu)
    {
        if (IsInterestingCycle(cpu.Cycles))
        {
            sumSignalStrengths += cpu.SignalStrength;
        }
    }

    private void DrawCycle(CPU cpu)
    {
        var currentPosition = cpu.Cycles - 1;

        if (Math.Abs(cpu.Register - currentPosition % 40) <= 1 && currentPosition < flattenedDisplay.Length)
        {
            flattenedDisplay[currentPosition] = LIT;
        }
    }

    private static void RunCommand(CPU cpu, string command, Action<CPU> cycleEvent)
    {
        var elements = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        switch (elements[0])
        {
            case "noop":
                cycleEvent(cpu);
                cpu.Cycles++;
                break;
            case "addx":
                cycleEvent(cpu);
                cpu.Cycles++;
                cycleEvent(cpu);
                cpu.Register += Convert.ToInt32(elements[1]);
                cpu.Cycles++;
                break;
            default:
                throw new ArgumentException("Command unrecognised");
        }
    }

    private static bool IsInterestingCycle(int cycle)
    {
        while (cycle > 20)
            cycle -= 40;

        return cycle == 20;
    }

    private string[] ReadCommands()
    {
        return Reader.ReadLines();
    }

    private class CPU
    {
        public int Register { get; set; }

        public int Cycles { get; set; }

        public int SignalStrength => Register * Cycles;

        public CPU()
        {
            Register = 1;
            Cycles = 1;
        }
    }
}
