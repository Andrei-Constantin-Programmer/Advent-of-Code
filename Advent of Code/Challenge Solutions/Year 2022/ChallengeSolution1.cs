using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution1 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(GetElfCaloriesList().First());
        }

        public void SolveSecondPart()
        {
            Console.WriteLine(GetElfCaloriesList().GetRange(0, 3).Sum());
        }

        private static List<int> GetElfCaloriesList()
        {
            var elfCalories = new List<int>();
            using (TextReader read = GetInputFile(2022, 1))
            {
                int currentCaloriesSum = 0;
                string? line;
                while ((line = read.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0)
                    {
                        elfCalories.Add(currentCaloriesSum);
                        currentCaloriesSum = 0;
                    }
                    else
                    {
                        currentCaloriesSum += Convert.ToInt32(line);
                    }
                }
            }

            elfCalories = elfCalories.OrderByDescending(x => x).ToList();
            return elfCalories;
        }
    }
}
