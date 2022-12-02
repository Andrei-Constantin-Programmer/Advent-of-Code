using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class Challenge2Solution : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(
                ReadRockPaperScissorsRounds()
                .Select((round) => {
                    return GetScore(round.Item1, round.Item2); })
                .Sum());
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private List<(char, char)> ReadRockPaperScissorsRounds()
        {
            var rounds = new List<(char, char)>();
            using (TextReader read = Utilities.GetInputFile(2022, 2))
            {
                string? line;
                while((line = read.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    rounds.Add((line[0], line[2]));
                }
            }

            return rounds;
        }

        private int GetScore(char oponentChoiceCharacter, char yourChoiceCharacter)
        {
            var oponentChoice = GetChoiceForCharacter(oponentChoiceCharacter);
            var yourChoice = GetChoiceForCharacter(yourChoiceCharacter);

            if (oponentChoice == yourChoice)
                return 3 + (int)GetChoiceForCharacter(yourChoiceCharacter);

            if (GetWinner(oponentChoice, yourChoice) == oponentChoice)
                return (int)yourChoice;

            return 6 + (int)yourChoice;
        }

        private RockPaperScissorsChoice GetChoiceForCharacter(char character)
        {
            if (character == 'A' || character == 'X')
                return RockPaperScissorsChoice.Rock;
            if (character == 'B' || character == 'Y')
                return RockPaperScissorsChoice.Paper;
            if (character == 'C' || character == 'Z')
                return RockPaperScissorsChoice.Scissors;

            throw new ArgumentException("This character does not map to a rock-paper-scissors choice");
        }

        private RockPaperScissorsChoice GetWinner(RockPaperScissorsChoice oponentChoice, RockPaperScissorsChoice yourChoice)
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

        private enum RockPaperScissorsChoice
        {
            Rock = 1,
            Paper = 2,
            Scissors = 3,
        }
    }
}
