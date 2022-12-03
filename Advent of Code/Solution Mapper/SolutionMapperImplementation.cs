﻿using Advent_of_Code.Challenge_Solutions;
using Year2022 = Advent_of_Code.Challenge_Solutions.Year_2022;
using Year2021 = Advent_of_Code.Challenge_Solutions.Year_2021;

namespace Advent_of_Code.Solution_Mapper
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
                    [1] = new Year2022.Challenge1Solution(),
                    [2] = new Year2022.Challenge2Solution(),
                },
                [2021] = new Dictionary<int, ChallengeSolution>()
                {
                    [1] = new Year2021.ChallengeSolution1(),
                    [2] = new Year2021.ChallengeSolution2(),
                    [3] = new Year2021.ChallengeSolution3(),
                    [4] = new Year2021.ChallengeSolution4(),
                    [5] = new Year2021.ChallengeSolution5(),
                    [6] = new Year2021.ChallengeSolution6(),
                    [7] = new Year2021.ChallengeSolution7(),
                    [8] = new Year2021.ChallengeSolution8(),
                    [9] = new Year2021.ChallengeSolution9(),
                    [10] = new Year2021.ChallengeSolution10(),
                    [11] = new Year2021.ChallengeSolution11(),
                    [12] = new Year2021.ChallengeSolution12(),
                    [13] = new Year2021.ChallengeSolution13(),
                    [14] = new Year2021.ChallengeSolution14(),
                    [15] = new Year2021.ChallengeSolution15(),
                    [16] = new Year2021.ChallengeSolution16(),
                },
            };
        }
    }
}