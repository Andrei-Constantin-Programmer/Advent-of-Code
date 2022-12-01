using Advent_of_Code.Challenge_Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Solution_Mapper
{
    internal interface SolutionMapper
    {
        public ChallengeSolution GetChallengeSolution(int day);
    }
}
