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
            SolutionMapper mapper = new SolutionMapperImplementation();
            ChallengeReader reader = new ChallengeReaderImplementation(mapper);
            
            while(true)
            {
                try
                {
                    var year = reader.ReadYear();
                    Console.WriteLine();

                    while(true)
                    {
                        try
                        {
                            var solution = reader.ReadChallenge(year);
                            Console.WriteLine();
                            PrintSolution(solution);
                        } 
                        catch(Exception)
                        {
                            break;
                        }
                    }
                }
                catch(Exception)
                {
                    break;
                }
            }
        }


        static void PrintSolution(ChallengeSolution solution)
        {
            try
            {
                Console.WriteLine("First part:");
                solution.SolveFirstPart();
                try
                {
                    Console.WriteLine("Second part:");
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
