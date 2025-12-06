namespace Advent_of_Code.Shared.Utilities;

public interface ISolutionMapper
{
    public ChallengeSolution GetChallengeSolution(int year, int day);
    public bool DoesYearExist(int year);
}