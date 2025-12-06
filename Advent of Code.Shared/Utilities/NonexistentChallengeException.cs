namespace Advent_of_Code.Shared.Utilities;

[Serializable]
public class NonexistentChallengeException(int year, int day)
    : Exception($"Challenge {day} for year {year} does not exist");