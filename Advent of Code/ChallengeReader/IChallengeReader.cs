using Advent_of_Code.Challenge_Solutions;

namespace Advent_of_Code.ChallengeReader;

internal interface IChallengeReader
{
    public int ReadYear();
    public ChallengeSolution ReadChallenge(int year);
}
