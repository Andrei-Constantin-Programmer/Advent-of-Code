using Advent_of_Code.Challenge_Solutions;
using Advent_of_Code.Solution_Mapper;

namespace Advent_of_Code.ChallengeReader;

internal partial class ChallengeReaderImplementation : IChallengeReader
{
    private const int FIRST_CHALLENGE_DAY = 1;
    private const int LAST_CHALLENGE_DAY = 25;

    private ISolutionMapper mapper;

    public ChallengeReaderImplementation(ISolutionMapper mapper)
    {
        this.mapper = mapper;
    }

    public int ReadYear()
    {
        while (true)
        {
            PrintQuit();
            Console.WriteLine("Choose a year: ");
            try
            {
                var line = Console.ReadLine()!;
                CheckQuitSymbol(line);
                var year = Convert.ToInt32(line);

                if (mapper.DoesYearExist(year))
                    return year;

                Console.WriteLine($"There are no solutions found for year {year}.");
                Console.WriteLine();
            }
            catch (QuitMenuException)
            {
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine("The value you have inputted is invalid.");
                Console.WriteLine();
            }
        }
    }

    public ChallengeSolution ReadChallenge(int year)
    {
        while (true)
        {
            PrintQuit();
            Console.WriteLine($"Year chosen: {year}");
            Console.WriteLine($"Select a challenge ({FIRST_CHALLENGE_DAY}-{LAST_CHALLENGE_DAY}): ");
            try
            {
                var challengeDay = ReadChallengeDay();
                ChallengeSolution solution = GetChallengeSolution(year, challengeDay);

                return solution;
            }
            catch (QuitMenuException)
            {
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine($"Sorry, this is an invalid challenge. Please choose a number between {FIRST_CHALLENGE_DAY} and {LAST_CHALLENGE_DAY}.");
                Console.WriteLine();
            }
        }
    }

    private int ReadChallengeDay()
    {
        var line = Console.ReadLine()!;
        CheckQuitSymbol(line);
        var challengeDay = Convert.ToInt32(line);
        if (challengeDay < FIRST_CHALLENGE_DAY || challengeDay > LAST_CHALLENGE_DAY)
        {
            throw new ArgumentException($"The challenge must be between {FIRST_CHALLENGE_DAY} and {LAST_CHALLENGE_DAY}");
        }

        return challengeDay;
    }

    private ChallengeSolution GetChallengeSolution(int year, int challengeDay)
    {
        try
        {
            var solution = mapper.GetChallengeSolution(year, challengeDay);
            return solution;
        }
        catch (Exception)
        {
            throw new ArgumentException($"There is no solution yet for challenge number {challengeDay}");
        }
    }

    private void PrintQuit()
    {
        Console.WriteLine("Press 'Q' or 'quit' to close this menu.");
    }

    private void CheckQuitSymbol(string symbol)
    {
        symbol = symbol.ToLower();
        _ = symbol switch
        {
            "q" => throw new QuitMenuException(),
            "quit" => throw new QuitMenuException(),
            _ => 0
        };
    }
}
