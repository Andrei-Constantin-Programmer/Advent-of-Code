// Task: https://adventofcode.com/2020/day/14

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution14 : ChallengeSolution
    {
        private string[] lines;

        public ChallengeSolution14()
        {
            lines = File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2020, 14));
        }

        protected override void SolveFirstPart()
        {
            memory = new string[100000];
            string mask = "";
            foreach (string line in lines)
            {
                string[] values = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                string code = values[0];
                string value = values[1];
                if (code.Contains("mask"))
                {
                    mask = value.Trim();
                }
                else
                {
                    int address = Convert.ToInt32(code.Substring(4, values[0].Length - 6));
                    FillMemory(mask, address, Convert.ToInt64(value));
                }
            }

            long sum = 0;
            foreach (string code in memory)
                if (!String.IsNullOrEmpty(code))
                    if (code.Contains('1'))
                        sum += ToDecimal(code);
            Console.WriteLine(sum);
        }

        protected override void SolveSecondPart()
        {
            memory2 = new Dictionary<long, string>();
            string mask = "";
            foreach (string line in lines)
            {
                string[] values = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                string code = values[0];
                string value = values[1];
                if (code.Contains("mask"))
                {
                    mask = value.Trim();
                }
                else
                {
                    int address = Convert.ToInt32(code.Substring(4, values[0].Length - 6));
                    FillMemory2(mask, address, Convert.ToInt64(value));
                }
            }

            long sum2 = 0;
            foreach (long index in memory2.Keys)
                if (!String.IsNullOrEmpty(memory2[index]))
                {
                    sum2 += ToDecimal(memory2[index]);
                }
            Console.WriteLine(sum2);
        }

        private static string[] memory;

        private static Dictionary<long, string> memory2;

        private static char[] ToBinary(long x)
        {
            string aux = Convert.ToString(x, 2);

            char[] binary = new char[36];
            for (int i = 0; i < binary.Length; i++)
                binary[i] = '0';
            for (int i = aux.Length - 1, j = binary.Length - 1; i >= 0; i--, j--)
                binary[j] = aux[i];

            return binary;
        }

        private static long ToDecimal(string x)
        {
            return Convert.ToInt64(x, 2);
        }

        private static void FillMemory(string mask, int address, long value)
        {
            char[] binary = ToBinary(value);
            for (int i = 0; i < mask.Length; i++)
                if (mask[i] != 'X')
                    binary[i] = mask[i];
            string bin = new string(binary);
            memory[address] = bin;
        }

        public static IEnumerable<string> GetCombinations(string input)
        {
            int first = input.IndexOf('X');
            if (first == -1)
                return new string[] { input };

            string prefix = input.Substring(0, first);
            string suffix = input.Substring(first + 1);

            var recursiveCombinations = GetCombinations(suffix);

            return from chr in "01" from recSuffix in recursiveCombinations select prefix + chr + recSuffix;
        }

        private static void FillMemory2(string mask, int address, long value)
        {
            string bin = new string(ToBinary(value));

            char[] baseAddress = ToBinary(address);
            for (int i = 0; i < mask.Length; i++)
                if (mask[i] != '0')
                    baseAddress[i] = mask[i];


            IEnumerable<string> combinations = GetCombinations(new string(baseAddress));

            foreach (string c in combinations)
            {
                long index = ToDecimal(c);
                memory2[index] = bin;
            }
        }
    }
}
