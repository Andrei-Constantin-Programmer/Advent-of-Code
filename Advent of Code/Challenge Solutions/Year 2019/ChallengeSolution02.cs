// Task: https://adventofcode.com/2019/day/2

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019
{
    internal class ChallengeSolution02 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            var opCodes = ReadOpCodes();
            Console.WriteLine(ResultForOpCodeCalculation(opCodes, 12, 2));
        }

        protected override void SolveSecondPart()
        {
            for(byte noun = 0; noun < 99; noun++)
            {
                for (byte verb = 0; verb < 99; verb++)
                {
                    var opCodes = ReadOpCodes();
                    var result = ResultForOpCodeCalculation(opCodes, noun, verb);

                    if (result == 19_690_720)
                    {
                        Console.WriteLine(100 * noun + verb);
                        return;
                    }
                }
            }
        }

        private long ResultForOpCodeCalculation(long[] opCodes, byte noun, byte verb)
        {
            opCodes[1] = noun;
            opCodes[2] = verb;

            return CalculateOpCodes(opCodes)[0];
        }

        private long[] CalculateOpCodes(long[] opCodes)
        {
            for (var i = 0; opCodes[i] != 99; i += 4)
            {
                opCodes[opCodes[i + 3]] = opCodes[i] switch
                {
                    1 => opCodes[opCodes[i + 1]] + opCodes[opCodes[i + 2]],
                    2 => opCodes[opCodes[i + 1]] * opCodes[opCodes[i + 2]],
                    _ => throw new ArgumentException()
                };
            }

            return opCodes;
        }

        private long[] ReadOpCodes()
        {
            return string.Join(",", File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2019, 2)))
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(stringValue => Convert.ToInt64(stringValue))
                .ToArray();
        }
    }
}
