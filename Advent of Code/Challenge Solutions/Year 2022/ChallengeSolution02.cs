// Task: https://adventofcode.com/2022/day/2

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution02(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        _console.WriteLine(
            ReadRockPaperScissorsRounds()
            .Select((round) =>
            {
                return GetScorePartOne(round.Item1, round.Item2);
            })
            .Sum());
    }

    public override void SolveSecondPart()
    {
        _console.WriteLine(
            ReadRockPaperScissorsRounds()
            .Select((round) =>
            {
                return GetScorePartTwo(round.Item1, round.Item2);
            })
            .Sum());
    }

    private List<(char, char)> ReadRockPaperScissorsRounds()
    {
        var rounds = new List<(char, char)>();
        foreach (var line in Reader.ReadLines(this).Select(line => line.ToUpper()))
        {
            rounds.Add((line[0], line[2]));
        }

        return rounds;
    }

    private static RockPaperScissorsChoice GetWinner(RockPaperScissorsChoice oponentChoice, RockPaperScissorsChoice yourChoice)
    {
        return oponentChoice switch
        {
            RockPaperScissorsChoice.Rock => yourChoice switch
            {
                RockPaperScissorsChoice.Rock => oponentChoice,
                RockPaperScissorsChoice.Paper => yourChoice,
                RockPaperScissorsChoice.Scissors => oponentChoice,
                _ => throw new ArgumentException("Invalid rock paper scissors choices found")
            },

            RockPaperScissorsChoice.Paper => yourChoice switch
            {
                RockPaperScissorsChoice.Rock => oponentChoice,
                RockPaperScissorsChoice.Paper => oponentChoice,
                RockPaperScissorsChoice.Scissors => yourChoice,
                _ => throw new ArgumentException("Invalid rock paper scissors choices found")
            },

            RockPaperScissorsChoice.Scissors => yourChoice switch
            {
                RockPaperScissorsChoice.Rock => yourChoice,
                RockPaperScissorsChoice.Paper => oponentChoice,
                RockPaperScissorsChoice.Scissors => oponentChoice,
                _ => throw new ArgumentException("Invalid rock paper scissors choices found")
            },

            _ => throw new ArgumentException("Invalid rock paper scissors choices found")
        };
    }

    private static RockPaperScissorsChoice GetChoiceForCharacter(char character)
    {
        if (character == 'A' || character == 'X')
            return RockPaperScissorsChoice.Rock;
        if (character == 'B' || character == 'Y')
            return RockPaperScissorsChoice.Paper;
        if (character == 'C' || character == 'Z')
            return RockPaperScissorsChoice.Scissors;

        throw new ArgumentException("This character does not map to a rock-paper-scissors choice");
    }

    public static int GetScorePartOne(char oponentChoiceCharacter, char yourChoiceCharacter)
    {
        var oponentChoice = GetChoiceForCharacter(oponentChoiceCharacter);
        var yourChoice = GetChoiceForCharacter(yourChoiceCharacter);

        if (oponentChoice == yourChoice)
            return 3 + (int)yourChoice;

        if (GetWinner(oponentChoice, yourChoice) == oponentChoice)
            return (int)yourChoice;

        return 6 + (int)yourChoice;
    }

    public static int GetScorePartTwo(char oponentChoiceCharacter, char roundConclusion)
    {
        var oponentChoice = GetChoiceForCharacter(oponentChoiceCharacter);
        var yourChoice = oponentChoice switch
        {
            RockPaperScissorsChoice.Rock => roundConclusion switch
            {
                'X' => RockPaperScissorsChoice.Scissors,
                'Y' => RockPaperScissorsChoice.Rock,
                'Z' => RockPaperScissorsChoice.Paper,
                _ => throw new ArgumentException("Invalid round conclusion")
            },
            RockPaperScissorsChoice.Paper => roundConclusion switch
            {
                'X' => RockPaperScissorsChoice.Rock,
                'Y' => RockPaperScissorsChoice.Paper,
                'Z' => RockPaperScissorsChoice.Scissors,
                _ => throw new ArgumentException("Invalid round conclusion")
            },
            RockPaperScissorsChoice.Scissors => roundConclusion switch
            {
                'X' => RockPaperScissorsChoice.Paper,
                'Y' => RockPaperScissorsChoice.Scissors,
                'Z' => RockPaperScissorsChoice.Rock,
                _ => throw new ArgumentException("Invalid round conclusion")
            },
            _ => throw new ArgumentException("Invalid rock paper scissors choice found")
        };

        if (oponentChoice == yourChoice)
            return 3 + (int)yourChoice;

        if (GetWinner(oponentChoice, yourChoice) == oponentChoice)
            return (int)yourChoice;

        return 6 + (int)yourChoice;
    }

    private enum RockPaperScissorsChoice
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }
}
