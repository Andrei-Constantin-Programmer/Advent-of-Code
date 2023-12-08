using Advent_of_Code.Solution_Mapper;
using Advent_of_Code.Utilities.ChallengeReader;

ISolutionMapper mapper = new SolutionMapper();
ChallengeReader reader = new ChallengeReaderImplementation(mapper);

while (true)
{
    try
    {
        var year = reader.ReadYear();
        Console.WriteLine();

        while (true)
        {
            try
            {
                var solution = reader.ReadChallenge(year);
                Console.WriteLine();
                solution.PrintSolution();
            }
            catch (QuitMenuException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                break;
            }
        }
    }
    catch (Exception)
    {
        break;
    }
}
