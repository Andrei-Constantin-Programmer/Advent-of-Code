// Task: https://adventofcode.com/2021/day/7

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution07 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            Solution(false);
        }

        protected override void SolveSecondPart()
        {
            Solution(true);
        }

        private void Solution(bool increasingFuel)
        {
            List<int> initialPositions = new List<int>(Array.ConvertAll(Reader.GetInputFile(2021, 7).ReadLine().Split(",", StringSplitOptions.RemoveEmptyEntries), int.Parse));
            Dictionary<int, int> fishByPosition = new Dictionary<int, int>();
            foreach (var x in initialPositions)
            {
                if (fishByPosition.ContainsKey(x))
                    fishByPosition[x]++;
                else
                    fishByPosition.Add(x, 1);
            }

            int minFuel = int.MaxValue, minPos = -1;
            for (int i = 0; i < fishByPosition.Count; i++)
            {
                int sum = 0;
                if(increasingFuel)
                    foreach (var x in fishByPosition)
                        sum += x.Value * GetUsedFuel(i, x.Key);
                else
                    foreach (var x in fishByPosition)
                        sum += x.Value * Math.Abs(x.Key - i);

                if (sum < minFuel)
                {
                    minFuel = sum;
                    minPos = i;
                }
            }

            Console.WriteLine(minFuel);
        }

        private int GetUsedFuel(int start, int end)
        {
            if (start > end)
            {
                int aux = start;
                start = end;
                end = aux;
            }

            int no = 1, sum = 0;
            while (start < end)
            {
                sum += no;
                no++;
                start++;
            }

            return sum;
        }
    }
}
