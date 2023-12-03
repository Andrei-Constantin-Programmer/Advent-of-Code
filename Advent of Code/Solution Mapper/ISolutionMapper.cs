using Advent_of_Code.Challenge_Solutions;

namespace Advent_of_Code.Solution_Mapper;

internal interface ISolutionMapper
{
    public ChallengeSolution GetChallengeSolution(int year, int day);
    public bool DoesYearExist(int year);
}
