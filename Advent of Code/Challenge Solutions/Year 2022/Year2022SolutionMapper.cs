using Advent_of_Code.Solution_Mapper;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class Year2022SolutionMapper : SolutionMapper
    {
        private readonly Dictionary<int, ChallengeSolution> solutionMappings = initialiseSolutionMappings();

        public ChallengeSolution GetChallengeSolution(int day)
        {
            return solutionMappings[day];
        }

        private static Dictionary<int, ChallengeSolution> initialiseSolutionMappings()
        {
            return new Dictionary<int, ChallengeSolution>()
            {
                { 1, new Challenge1Solution() },
                { 2, new Challenge2Solution() },
            };
        }
    }
}
