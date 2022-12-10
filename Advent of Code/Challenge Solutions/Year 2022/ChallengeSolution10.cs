using Microsoft.Win32;
using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution10 : ChallengeSolution
    {
        private int sumSignalStrengths = 0;

        public void SolveFirstPart()
        {
            var cpu = new CPU();
            var commands = ReadCommands();

            foreach(var command in commands)
            {
                RunCommand(cpu, command);
            }

            Console.WriteLine(sumSignalStrengths);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private void RunCommand(CPU cpu, string command)
        {
            var elements = command.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            switch (elements[0])
            {
                case "noop":
                    cpu.Cycles++;
                    AddSignalStrengthIfInteresting(cpu);

                    break;
                case "addx":
                    cpu.Cycles++;
                    AddSignalStrengthIfInteresting(cpu);

                    cpu.Register += Convert.ToInt32(elements[1]);

                    cpu.Cycles++;
                    AddSignalStrengthIfInteresting(cpu);

                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void AddSignalStrengthIfInteresting(CPU cpu)
        {
            if (IsInterestingCycle(cpu.Cycles))
            {
                sumSignalStrengths += cpu.SignalStrength;
            }
        }

        private bool IsInterestingCycle(int cycle)
        {
            while (cycle > 20)
                cycle -= 40;

            return cycle == 20;
        }

        private string[] ReadCommands()
        {
            return File.ReadAllLines(GetFileString(Utilities.FileType.Input, 2022, 10));
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
}
