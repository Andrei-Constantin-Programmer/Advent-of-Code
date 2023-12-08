// Task: https://adventofcode.com/2021/day/16

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution16 : ChallengeSolution
    {
        private Dictionary<char, string> hexToBinary = new Dictionary<char, string>()
        {
            {'0', "0000"},
            {'1', "0001" },
            {'2', "0010" },
            {'3', "0011" },
            {'4', "0100" },
            {'5', "0101" },
            {'6', "0110" },
            {'7', "0111" },
            {'8', "1000" },
            {'9', "1001" },
            {'A', "1010" },
            {'B', "1011" },
            {'C', "1100" },
            {'D', "1101" },
            {'E', "1110" },
            {'F', "1111" },
        };

        protected override void SolveFirstPart()
        {
            throw new NotImplementedException();
            using(TextReader read = Reader.GetInputFile(2021, 16))
            {
                string transmission = read.ReadLine();
                string binary = ConvertToBinary(transmission);
            }
        }

        protected override void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private string ConvertToBinary(string transmission)
        {
            string binary = "";
            foreach (var hex in transmission)
                binary += hexToBinary[hex];

            return binary;
        }

    }
}
