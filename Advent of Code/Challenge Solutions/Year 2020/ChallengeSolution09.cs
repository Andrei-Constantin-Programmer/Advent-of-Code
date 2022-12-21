// Task: https://adventofcode.com/2020/day/9

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution09 : ChallengeSolution
    {
        private List<long> numbers;
        private string[] lines;

        public ChallengeSolution09()
        {
            numbers = new List<long>();
            lines = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2020, 9));
            for (int i = 0; i < 25; i++)
            {
                numbers.Add(Convert.ToInt64(lines[i]));
            }
        }

        protected override void SolveFirstPart()
        {
            bool found = false;
            int pos = 0;
            long number = 0;
            for (int i = 25; i < lines.Length && !found; i++)
            {
                long x = Convert.ToInt64(lines[i]);
                if (!IsSumOfTwo(x, i))
                {
                    found = true;
                    Console.WriteLine(x);
                }
                else
                    numbers.Add(x);
            }
        }

        protected override void SolveSecondPart()
        {
            bool found = false;
            int pos = 0;
            long number = 0;
            for (int i = 25; i < lines.Length && !found; i++)
            {
                long x = Convert.ToInt64(lines[i]);
                if (!IsSumOfTwo(x, i))
                {
                    found = true;
                    pos = i;
                    number = x;
                }
                else
                    numbers.Add(x);
            }

            Console.WriteLine(GetContiguousSet(number, pos));
        }


        private bool IsSumOfTwo(long number, int index)
        {
            bool found = false;

            for (int i = index - 25; i < index - 1 && !found; i++)
                for (int j = i + 1; j < index && !found; j++)
                {
                    //Console.WriteLine(numbers[i]+" + "+numbers[j]+" = "+(numbers[i]+numbers[j]));
                    if (numbers[i] + numbers[j] == number)
                        found = true;
                }

            return found;
        }

        private long GetContiguousSet(long number, int index)
        {
            long min = -1, max = -1;
            bool found = false;

            for (int i = 0; i < numbers.Count && !found; i++)
            {
                min = max = numbers[i];
                int j = i + 1;
                long sum = numbers[i];
                while (sum < number && j < numbers.Count)
                {
                    sum += numbers[j];
                    if (numbers[j] > max)
                        max = numbers[j];
                    if (numbers[j] < min)
                        min = numbers[j];
                    j++;
                }
                if (sum == number)
                {
                    found = true;
                }
            }

            return min + max;
        }
    }
}
