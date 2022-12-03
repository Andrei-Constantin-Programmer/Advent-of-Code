namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution1 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            int no = 0;
            using (TextReader read = Utilities.GetInputFile(2021, 1))
            {
                int prev = Convert.ToInt32(read.ReadLine());
                int x = 0;
                while ((x = Convert.ToInt32(read.ReadLine())) != 0)
                {
                    if (x > prev)
                        no++;
                    prev = x;
                }
            }

            Console.WriteLine(no);
        }

        public void SolveSecondPart()
        {
            List<int> depths = new List<int>(Array.ConvertAll(File.ReadAllLines(Utilities.GetFileString("input", 2021, 1)), int.Parse));
            List<int> sums = new List<int>(depths);
            int highestSum = 0;
            for (int i = 0; i < depths.Count; i++)
            {
                if (highestSum > 1)
                    sums[highestSum - 2] += depths[i];
                if (highestSum > 0)
                    sums[highestSum - 1] += depths[i];
                sums[highestSum] += depths[i];
                highestSum++;
            }

            int no = 0;
            for (int i = 1; i < highestSum; i++)
                if (sums[i] > sums[i - 1])
                    no++;

            Console.WriteLine(no);
        }
    }
}
