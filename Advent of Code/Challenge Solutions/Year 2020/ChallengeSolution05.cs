// Task: https://adventofcode.com/2020/day/5

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution05 : ChallengeSolution
    {
        private string[] seats;
        private static int[,] plane = new int[128, 8];

        public ChallengeSolution05()
        {
            seats = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2020, 5));
        }

        protected override void SolveFirstPart()
        {
            string[] mySeats = new string[seats.Length];
            seats.CopyTo(mySeats, 0);
            FillPlane(mySeats);
            Console.WriteLine(GetHighestID(mySeats));
        }

        protected override void SolveSecondPart()
        {
            string[] mySeats = new string[seats.Length];
            seats.CopyTo(mySeats, 0);
            FillPlane(mySeats);

            using (StreamWriter write = Reader.GetOutputFile(2020, 5))
            {
                for (int i = 0; i < 128; i++)
                {
                    for (int j = 0; j < 8; j++)
                        write.Write(plane[i, j] + " ");
                    write.WriteLine();
                }
            }

            Console.WriteLine("Look in file /resources/output/2020_5.txt");
        }

        private static int GetCorrect(string searchString, char lowChar, int low, int high)
        {
            int med = (low + high) / 2;

            for (int i = 0; i < searchString.Length; i++)
            {
                double m = ((double)low + (double)high) / 2;
                double frac = m - Math.Truncate(m);
                if (frac == 0)
                    med = (low + high) / 2;
                else
                {
                    med = searchString[i] == lowChar ? (low + high) / 2 : (low + high) / 2 + 1;
                }

                if (searchString[i] == lowChar)
                {
                    high = med;
                }
                else
                    low = med;
            }
            return med;
        }

        private static int GetHighestID(string[] seats)
        {
            int max = 0;
            foreach (string seat in seats)
            {
                int row = GetCorrect(seat.Substring(0, 7), 'F', 0, 127);
                int column = GetCorrect(seat.Substring(7, 3), 'L', 0, 7);
                int id = row * 8 + column;
                if (id > max)
                    max = id;
            }

            return max;
        }

        private static void FillPlane(string[] seats)
        {
            foreach (string seat in seats)
            {
                int row = GetCorrect(seat.Substring(0, 7), 'F', 0, 127);
                int column = GetCorrect(seat.Substring(7, 3), 'L', 0, 7);
                plane[row, column] = 1;
            }
        }
    }
}
