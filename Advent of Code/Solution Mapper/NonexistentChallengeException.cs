namespace Advent_of_Code.Solution_Mapper;

[Serializable]
public class NonexistentChallengeException : Exception
{
    public NonexistentChallengeException(int year, int day) : base($"Challenge {day} for year {year} does not exist") { }
}
