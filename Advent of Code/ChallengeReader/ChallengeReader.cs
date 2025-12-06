using Advent_of_Code.Services;
using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Advent_of_Code.ChallengeReader;

internal class ChallengeReader : IChallengeReader
{
    private const int FirstChallengeDay = 1;
    private const int LastChallengeDayPre2025 = 25;
    private const int LastChallengeDayPost2025 = 12;

    private readonly ISolutionMapper _csMapper;
    private readonly ISolutionMapper _fsMapper;

    public ChallengeReader(
        [FromKeyedServices(CSharpLang.ServiceKey)]
        ISolutionMapper csMapper,
        [FromKeyedServices(FSharpLang.ServiceKey)]
        ISolutionMapper fsMapper)
    {
        _csMapper = csMapper;
        _fsMapper = fsMapper;
    }

    public Lang ReadLang()
    {
        while (true)
        {
            PrintQuit();
            Console.WriteLine("Choose implementation language: ");
            
            try
            {
                var line = Console.ReadLine()!;
                CheckQuitSymbol(line);

                Lang? lang = line.Trim().ToLower() switch
                {
                    "" or "c#" or "csharp" => Lang.CSharp,
                    "f#" or "fsharp" => Lang.FSharp,
                    _ => null
                };

                if (lang is not null)
                {
                    return lang.Value;
                }

                Console.WriteLine($"Language chosen {line} is not valid.");
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

    public int ReadYear(Lang lang = Lang.CSharp)
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

                if (DoesYearExist(year, lang))
                {
                    return year;
                }

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

    public ChallengeSolution ReadChallenge(int year, Lang lang = Lang.CSharp)
    {
        var lastChallengeDay = GetLastChallengeDay(year);
        while (true)
        {
            PrintQuit();
            Console.WriteLine($"Lang chosen: {lang.ToDisplayString()}");
            Console.WriteLine($"Year chosen: {year}");
            Console.WriteLine($"Select a challenge ({FirstChallengeDay}-{lastChallengeDay}): ");
            try
            {
                var challengeDay = ReadChallengeDay(lastChallengeDay);
                var solution = GetChallengeSolution(year, challengeDay, lang);

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
                Console.WriteLine(
                    $"Sorry, this is an invalid challenge. Please choose a number between {FirstChallengeDay} and {lastChallengeDay}.");
                Console.WriteLine();
            }
        }
    }

    private static int ReadChallengeDay(int lastChallengeDay)
    {
        var line = Console.ReadLine()!;
        CheckQuitSymbol(line);
        var challengeDay = Convert.ToInt32(line);

        if (challengeDay < FirstChallengeDay || challengeDay > lastChallengeDay)
        {
            throw new ArgumentException($"The challenge must be between {FirstChallengeDay} and {lastChallengeDay}");
        }

        return challengeDay;
    }

    private ChallengeSolution GetChallengeSolution(int year, int challengeDay, Lang lang)
    {
        try
        {
            var solution = lang switch
            {
                Lang.CSharp => _csMapper.GetChallengeSolution(year, challengeDay),
                Lang.FSharp => _fsMapper.GetChallengeSolution(year, challengeDay),
                _ => throw new ArgumentException($"No mapper found for language {lang}")
            };

            return solution;
        }
        catch (Exception)
        {
            throw new ArgumentException($"There is no solution yet for challenge number {challengeDay}");
        }
    }

    private bool DoesYearExist(int year, Lang lang) =>
        lang switch
        {
            Lang.CSharp => _csMapper.DoesYearExist(year),
            Lang.FSharp => _fsMapper.DoesYearExist(year),
            _ => throw new ArgumentException($"No mapper found for language {lang}")
        };

    private static int GetLastChallengeDay(int year) => year >= 2025
        ? LastChallengeDayPost2025
        : LastChallengeDayPre2025;

    private static void PrintQuit() => Console.WriteLine("Press 'Q' or 'quit' to close this menu.");

    private static void CheckQuitSymbol(string symbol) => _ = symbol.ToLower() switch
    {
        "q" or "quit" => throw new QuitMenuException(),
        _ => 0
    };
}