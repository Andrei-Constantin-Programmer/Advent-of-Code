using Advent_of_Code.Services;
using Advent_of_Code.Shared;

namespace Advent_of_Code.ChallengeReader;

internal interface IChallengeReader
{
    public Lang ReadLang();
    public int ReadYear(Lang lang = Lang.CSharp);
    public ChallengeSolution ReadChallenge(int year, Lang lang = Lang.CSharp);
}