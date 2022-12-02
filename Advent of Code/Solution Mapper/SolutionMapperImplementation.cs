using Advent_of_Code.Solution_Mapper;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class SolutionMapperImplementation : SolutionMapper
    {
        private readonly Dictionary<int, Dictionary<int, ChallengeSolution>> yearMappings;

        public SolutionMapperImplementation()
        {
            yearMappings = InitialiseYearMappings();
        }

        public bool DoesYearExist(int year)
        {
            return yearMappings.ContainsKey(year);
        }

        public ChallengeSolution GetChallengeSolution(int year, int day)
        {
            return yearMappings[year][day];
        }

        private Dictionary<int, Dictionary<int, ChallengeSolution>> InitialiseYearMappings()
        {
            return new Dictionary<int, Dictionary<int, ChallengeSolution>>()
            {
                [2022] = new Dictionary<int, ChallengeSolution>()
                {
                    [1] = new Year_2022.Challenge1Solution(),
                    [2] = new Year_2022.Challenge2Solution()
                },
                [2021] = new Dictionary<int, ChallengeSolution>()
                {

                },
            };
        }
    }
}
