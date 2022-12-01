using Advent_of_Code.Challenge_Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Solution_Mapper
{
    internal class SolutionMapperImplementation : SolutionMapper
    {
        private readonly Dictionary<int, ChallengeSolution> solutionMappings = InitialiseSolutionMappings();

        public ChallengeSolution GetChallengeSolution(int day)
        {
            return solutionMappings[day];
        }
    
        private static Dictionary<int, ChallengeSolution> InitialiseSolutionMappings()
        {
            return new Dictionary<int, ChallengeSolution>()
            {
                { 1, new Challenge1Solution() },
            };
        }
    }
}
