using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Challenge_Solutions
{
    internal class Challenge1Solution : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(GetElfCaloriesList().First());
        }

        public void SolveSecondPart()
        {
            Console.WriteLine(GetElfCaloriesList().GetRange(0, 3).Sum());
        }

        private List<int> GetElfCaloriesList()
        {
            var elfCalories = new List<int>();
            using (TextReader read = Utilities.GetInputFile(2022, 1))
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
