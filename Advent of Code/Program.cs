using Advent_of_Code.Reader;
using Advent_of_Code.Solution_Mapper;

namespace Advent_of_Code
{
    interface ChallengeSolution
    {
        void SolveFirstPart();
        void SolveSecondPart();
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SolutionMapper mapper = new SolutionMapperImplementation();
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
                }
                catch (NotImplementedException)
                {
                    Console.WriteLine("The second part of this challenge has not been solved yet.");
                }
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("The first part of this challenge has not been solved yet.");
            }
        }
    }
}
