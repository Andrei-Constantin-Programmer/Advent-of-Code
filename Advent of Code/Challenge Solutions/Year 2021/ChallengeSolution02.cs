// Task: https://adventofcode.com/2021/day/2

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution02 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            int depth = 0, horizontal = 0;
            using (TextReader read = Reader.GetInputFile(2021, 2))
            {
                string line = "";
                while ((line = read.ReadLine()) != null)
                {
                    string[] command = line.Split(" ");
                    int x = Convert.ToInt32(command[1]);
                    if (command[0].Equals("forward"))
                        horizontal += x;
                    else if (command[0].Equals("down"))
                        depth += x;
                    else if (command[0].Equals("up"))
                        depth -= x;
                }
            }

            Console.WriteLine(depth*horizontal);
        }

        protected override void SolveSecondPart()
        {
            int depth = 0, horizontal = 0, aim = 0;

            using (TextReader read = Reader.GetInputFile(2021, 2))
            {
                string line = "";
                while ((line = read.ReadLine()) != null)
                {
                    string[] command = line.Split(" ");
                    int x = Convert.ToInt32(command[1]);
                    if (command[0].Equals("forward"))
                    {
                        horizontal += x;
                        depth += x*aim;
                    }
                    else if (command[0].Equals("down"))
                        aim += x;
                    else if (command[0].Equals("up"))
                        aim -= x;
                }
            }

            Console.WriteLine(depth * horizontal);
        }
    }
}
