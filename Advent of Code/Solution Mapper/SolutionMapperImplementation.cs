using Advent_of_Code.Challenge_Solutions;
using Year2019 = Advent_of_Code.Challenge_Solutions.Year_2019;
using Year2020 = Advent_of_Code.Challenge_Solutions.Year_2020;
using Year2021 = Advent_of_Code.Challenge_Solutions.Year_2021;
using Year2022 = Advent_of_Code.Challenge_Solutions.Year_2022;

namespace Advent_of_Code.Solution_Mapper
{
    internal class SolutionMapperImplementation : SolutionMapper
    {
        private readonly Dictionary<int, Func<int, ChallengeSolution>> yearMappings;

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
            return yearMappings[year](day);
        }

        private static Dictionary<int, Func<int, ChallengeSolution>> InitialiseYearMappings()
        {
            return new Dictionary<int, Func<int, ChallengeSolution>>()
            {
                [2019] = (day) => day switch
                {
                    1 => new Year2019.ChallengeSolution01(),
                    2 => new Year2019.ChallengeSolution02(),

                    _ => throw new Exception("Challenge unexistent")
                },
                [2020] = (day) => day switch
                {
                    1 => new Year2020.ChallengeSolution01(),
                    2 => new Year2020.ChallengeSolution02(),
                    3 => new Year2020.ChallengeSolution03(),
                    4 => new Year2020.ChallengeSolution04(),
                    5 => new Year2020.ChallengeSolution05(),
                    6 => new Year2020.ChallengeSolution06(),
                    7 => new Year2020.ChallengeSolution07(),
                    8 => new Year2020.ChallengeSolution08(),
                    9 => new Year2020.ChallengeSolution09(),
                    10 => new Year2020.ChallengeSolution10(),
                    11 => new Year2020.ChallengeSolution11(),
                    12 => new Year2020.ChallengeSolution12(),
                    13 => new Year2020.ChallengeSolution13(),
                    14 => new Year2020.ChallengeSolution14(),
                    15 => new Year2020.ChallengeSolution15(),
                    16 => new Year2020.ChallengeSolution16(),

                    _ => throw new Exception("Challenge unexistent")
                },
                [2021] = (day) => day switch
                {
                    1 => new Year2021.ChallengeSolution01(),
                    2 => new Year2021.ChallengeSolution02(),
                    3 => new Year2021.ChallengeSolution03(),
                    4 => new Year2021.ChallengeSolution04(),
                    5 => new Year2021.ChallengeSolution05(),
                    6 => new Year2021.ChallengeSolution06(),
                    7 => new Year2021.ChallengeSolution07(),
                    8 => new Year2021.ChallengeSolution08(),
                    9 => new Year2021.ChallengeSolution09(),
                    10 => new Year2021.ChallengeSolution10(),
                    11 => new Year2021.ChallengeSolution11(),
                    12 => new Year2021.ChallengeSolution12(),
                    13 => new Year2021.ChallengeSolution13(),
                    14 => new Year2021.ChallengeSolution14(),
                    15 => new Year2021.ChallengeSolution15(),
                    16 => new Year2021.ChallengeSolution16(),

                    _ => throw new Exception("Challenge unexistent")
                },
                [2022] = (day) => day switch
                {
                    1 => new Year2022.ChallengeSolution01(),
                    2 => new Year2022.ChallengeSolution02(),
                    3 => new Year2022.ChallengeSolution03(),
                    4 => new Year2022.ChallengeSolution04(),
                    5 => new Year2022.ChallengeSolution05(),
                    6 => new Year2022.ChallengeSolution06(),
                    7 => new Year2022.ChallengeSolution07(),
                    8 => new Year2022.ChallengeSolution08(),
                    9 => new Year2022.ChallengeSolution09(),
                    10 => new Year2022.ChallengeSolution10(),
                    11 => new Year2022.ChallengeSolution11(),
                    12 => new Year2022.ChallengeSolution12(),
                    13 => new Year2022.ChallengeSolution13(),
                    14 => new Year2022.ChallengeSolution14(),
                    15 => new Year2022.ChallengeSolution15(),
                    16 => new Year2022.ChallengeSolution16(),

                    _ => throw new Exception("Challenge unexistent")
                },
            };
        }
    }
}
