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
                            solution.PrintSolution();
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

    }
}
