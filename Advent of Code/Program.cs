using Advent_of_Code.ChallengeReader;
using Advent_of_Code.Solution_Mapper;

ISolutionMapper _mapper = new SolutionMapper();
IChallengeReader _reader = new ChallengeReader(_mapper);

while (true)
{
    try
    {
        var year = _reader.ReadYear();
        Console.WriteLine();

        while (true)
        {
            try
            {
                var solution = _reader.ReadChallenge(year);
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
