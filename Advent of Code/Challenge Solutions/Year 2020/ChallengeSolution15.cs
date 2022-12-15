// Task: https://adventofcode.com/2020/day/15

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution15 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Solution(2020);
        }

        public void SolveSecondPart()
        {
            Solution(30000000);
        }

        private void Solution(int n)
        {
            string input = File.ReadAllText(Reader.GetFileString(Reader.FileType.Input, 2020, 15));
            string[] inputSeparated = input.Split(new string(","), StringSplitOptions.RemoveEmptyEntries);
            var spokenNumbers = new Dictionary<ulong, uint>();

            List<ulong> temp = new List<ulong>();

            foreach (string x in inputSeparated)
                temp.Add(Convert.ToUInt64(x));

            ulong last = temp[temp.Count - 1];
            temp.RemoveAt(temp.Count - 1);

            for (int i = 0; i < temp.Count; i++)
                spokenNumbers[temp[i]] = (uint)i + 1;

            for (uint turn = (uint)temp.Count + 1; turn < n; turn++)
            {
                if (spokenNumbers.TryGetValue(last, out uint lastTurn))
                {
                    uint current = turn - lastTurn;
                    spokenNumbers[last] = turn;
                    last = current;
                }
                else
                {
                    spokenNumbers[last] = turn;
                    last = 0;
                }
            }

            Console.WriteLine(last);
        }
    }
}
