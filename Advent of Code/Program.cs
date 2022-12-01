using Advent_of_Code.Challenge_Solutions;
using Advent_of_Code.Challenge_Solutions.Year_2022;
using Advent_of_Code.Reader;
using Advent_of_Code.Solution_Mapper;

namespace Advent_of_Code
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SolutionMapper mapper = new Year2022().SolutionMapper;
            ChallengeReader reader = new ChallengeReaderImplementation(mapper);
            var solution = reader.ReadChallenge();

            PrintSolution(solution);
        }


        static void PrintSolution(ChallengeSolution solution)
        {
            try
            {
                solution.SolveFirstPart();
                try
                {
                    solution.SolveSecondPart();
                    Console.WriteLine();
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("The second part of this challenge has not been solved yet.");
                    Console.WriteLine();
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("The first part of this challenge has not been solved yet.");
            }
        }
    }
}
