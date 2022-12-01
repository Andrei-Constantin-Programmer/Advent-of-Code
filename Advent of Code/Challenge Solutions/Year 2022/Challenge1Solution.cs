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
            int maxCalories = 0;

            using (TextReader read = Utilities.GetInputFile(2022, 1))
            {
                int currentCaloriesSum = 0;
                string? line;
                while((line = read.ReadLine()) != null)
                {
                    line = line.Trim();
                    if(line.Length == 0)
                    {
                        if(maxCalories < currentCaloriesSum)
                            maxCalories = currentCaloriesSum;
                        currentCaloriesSum = 0;
                    }
                    else
                    {
                        currentCaloriesSum += Convert.ToInt32(line);
                    }
                }
            }

            Console.WriteLine(maxCalories);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }
    }
}
