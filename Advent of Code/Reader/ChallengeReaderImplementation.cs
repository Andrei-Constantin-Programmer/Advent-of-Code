using Advent_of_Code.Challenge_Solutions;
using Advent_of_Code.Solution_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Reader
{
    internal class ChallengeReaderImplementation : ChallengeReader
    {
        private const int FIRST_CHALLENGE_DAY = 1;
        private const int LAST_CHALLENGE_DAY = 25;

        private SolutionMapper mapper;

        public ChallengeReaderImplementation(SolutionMapper mapper)
        {
            this.mapper = mapper;
        }

        public ChallengeSolution ReadChallenge()
        {
            while (true)
            {
                Console.WriteLine("Select a challenge (1-25): ");
                try
                {
                    int challengeDay = ReadChallengeDay();
                    ChallengeSolution solution = GetChallengeSolution(challengeDay);

                    return solution;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Sorry, this is an invalid challenge. Please choose a number between {FIRST_CHALLENGE_DAY} and {LAST_CHALLENGE_DAY}.");
                }
            }
        }

        private int ReadChallengeDay()
        {
            int challengeDay = Convert.ToInt32(Console.ReadLine());
            if (challengeDay < FIRST_CHALLENGE_DAY || challengeDay > LAST_CHALLENGE_DAY)
            {
                throw new ArgumentException($"The challenge must be between {FIRST_CHALLENGE_DAY} and {LAST_CHALLENGE_DAY}");
            }

            return challengeDay;
        }

        private ChallengeSolution GetChallengeSolution(int challengeDay)
        {
            try
            {
                var solution = mapper.GetChallengeSolution(challengeDay);
                return solution;
            }
            catch (Exception)
            {
                throw new ArgumentException($"There is no solution yet for challenge number {challengeDay}");
            }
        }
    }
}
