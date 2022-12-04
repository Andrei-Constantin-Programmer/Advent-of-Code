using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019
{
    internal class ChallengeSolution2 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var opCodes = ReadOpCodes();
            opCodes[1] = 12;
            opCodes[2] = 2;

            opCodes = CalculateOpCodes(opCodes);

            Console.WriteLine(opCodes[0]);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private int[] ReadOpCodes()
        {
            return String.Join(",", File.ReadAllLines(GetFileString(FileType.Input, 2019, 2)))
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(stringValue => Convert.ToInt32(stringValue))
                .ToArray();
        }

        private int[] CalculateOpCodes(int[] opCodes)
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
    }
}
