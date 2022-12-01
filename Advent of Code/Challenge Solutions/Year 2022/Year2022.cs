using Advent_of_Code.Solution_Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class Year2022 : AdventYear
    {
        public SolutionMapper SolutionMapper { get; init; }

        public Year2022()
        {
            SolutionMapper = new Year2022SolutionMapper();
        }
    }
}
