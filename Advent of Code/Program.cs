using Advent_of_Code.Challenge_Solutions;
using Advent_of_Code.Utilities.ChallengeReader;
using Advent_of_Code.Solution_Mapper;
using System.Diagnostics;

namespace Advent_of_Code
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ISolutionMapper mapper = new SolutionMapper();
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
                            solution.PrintSolution();
                        } 
                        catch(Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
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

    }
}
