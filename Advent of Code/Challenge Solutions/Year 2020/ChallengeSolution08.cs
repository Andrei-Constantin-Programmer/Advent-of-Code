// Task: https://adventofcode.com/2020/day/8

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution08 : ChallengeSolution
    {
        private static List<Instruction> instructions = new List<Instruction>();
        private int accumulator;

        public ChallengeSolution08()
        {
            var lines = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2020, 8));

            foreach (string instruction in lines)
            {
                string[] codes = instruction.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                int instr = 0;
                switch (codes[0].ToUpper())
                {
                    case "ACC":
                        instr = Instruction.ACC;
                        break;
                    case "JMP":
                        instr = Instruction.JMP;
                        break;
                    default:
                        instr = Instruction.NOP;
                        break;
                }

                instructions.Add(new Instruction(instr, Convert.ToInt32(codes[1])));
            }
        }

        protected override void SolveFirstPart()
        {
            accumulator = 0;

            bool gotCorrect = false;
            for (int i = 0; i < instructions.Count && !gotCorrect; i++)
            {
                Instruction instruction = instructions[i];
                if (instruction.type != Instruction.ACC)
                {
                    accumulator = 0;
                    gotCorrect = !RunGame();
                }
            }

            Console.WriteLine(accumulator);
        }

        protected override void SolveSecondPart()
        {
            accumulator = 0;

            bool gotCorrect = false;
            for (int i = 0; i < instructions.Count && !gotCorrect; i++)
            {
                Instruction instruction = instructions[i];
                if (instruction.type != Instruction.ACC)
                {
                    accumulator = 0;
                    instruction.type = instruction.type == Instruction.NOP ? Instruction.JMP : Instruction.NOP;
                    gotCorrect = !RunGame();
                    instruction.type = instruction.type == Instruction.NOP ? Instruction.JMP : Instruction.NOP;
                }
            }

            Console.WriteLine(accumulator);
        }


        private bool RunGame()
        {
            bool reachedLoop = false;
            foreach (Instruction i in instructions)
                i.Reset();
            for (int i = 0; i < instructions.Count && !reachedLoop; i++)
            {
                if (instructions[i].IsExecuted())
                {
                    reachedLoop = true;
                }
                else
                {
                    instructions[i].Execute();
                    int command = instructions[i].type;
                    int value = instructions[i].GetValue();
                    switch (command)
                    {
                        case Instruction.ACC:
                            accumulator += value;
                            break;
                        case Instruction.JMP:
                            i += value - 1;
                            break;
                    }
                }
            }

            return reachedLoop;
        }

        class Instruction
        {
            public int type { get; set; }
            public const int ACC = 1, JMP = 2, NOP = 0;
            int value;
            bool executed;


            public Instruction(int type, int value)
            {
                this.type = type;
                this.value = value;
                executed = false;
            }

            public void Execute()
            {
                executed = true;
            }

            public void Reset()
            {
                executed = false;
            }

            public int GetValue()
            {
                return value;
            }

            public bool IsExecuted()
            {
                return executed;
            }
        }
    }
}
