using System.Text;

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution2 : ChallengeSolution
    {
        private string[] lines;

        public ChallengeSolution2()
        {
            lines = File.ReadAllLines(Utilities.GetFileString("input", 2020, 2));
        }

        public void SolveFirstPart()
        {
            int no = 0;

            foreach (string line in lines)
            {
                int index = 0;
                int min = ReadNumber(line);
                index += NumberOfDigits(min) + 1;
                int max = ReadNumber(line.Substring(index));
                index += NumberOfDigits(max) + 1;
                char c = line[index];
                index += 2;
                string sub = line.Substring(index);

                int nr = 0;
                for (int i = 0; i < sub.Length; i++)
                {
                    if (sub[i] == c)
                        nr++;
                }
                if (nr >= min && nr <= max)
                    no++;
            }

            Console.WriteLine(no);
        }

        public void SolveSecondPart()
        {
            int no = 0;

            foreach (string line in lines)
            {
                int index = 0;
                int min = ReadNumber(line);
                index += NumberOfDigits(min) + 1;
                int max = ReadNumber(line.Substring(index));
                index += NumberOfDigits(max) + 1;
                char c = line[index];
                index += 3;
                string sub = line.Substring(index);

                if ((sub[min - 1] == c && sub[max - 1] != c) || (sub[min - 1] != c && sub[max - 1] == c))
                    no++;
            }

            Console.WriteLine(no);
        }

        private static int NumberOfDigits(int x)
        {
            int nr = 0;
            do
            {
                nr++;
                x /= 10;
            } while (x > 0);
            return nr;
        }

        private static int ReadNumber(string sir)
        {
            StringBuilder builder = new StringBuilder();
            bool stop = false;
            for (int i = 0; i < sir.Length && !stop; i++)
                if (sir[i] >= '0' && sir[i] <= '9')
                    builder.Append(sir[i]);
                else
                    stop = true;
            return Convert.ToInt32(builder.ToString());
        }
    }
}
