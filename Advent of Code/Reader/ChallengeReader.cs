using Advent_of_Code.Challenge_Solutions;

namespace Advent_of_Code.Reader
{
    internal interface ChallengeReader
    {
        public int ReadYear();
        public ChallengeSolution ReadChallenge(int year);
    }
}
