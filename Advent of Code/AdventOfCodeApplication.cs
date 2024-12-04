using Advent_of_Code.ChallengeReader;

namespace Advent_of_Code;

internal class AdventOfCodeApplication
{
    private readonly IChallengeReader _reader;

    public AdventOfCodeApplication(IChallengeReader reader)
    {
        _reader = reader;
    }

    public void Run()
    {
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
    }
}
