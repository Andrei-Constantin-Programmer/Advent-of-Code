using Advent_of_Code.Challenge_Solutions;
using Year2019 = Advent_of_Code.Challenge_Solutions.Year_2019;
using Year2020 = Advent_of_Code.Challenge_Solutions.Year_2020;
using Year2021 = Advent_of_Code.Challenge_Solutions.Year_2021;
using Year2022 = Advent_of_Code.Challenge_Solutions.Year_2022;
using Year2023 = Advent_of_Code.Challenge_Solutions.Year_2023;

namespace Advent_of_Code.Solution_Mapper;

internal class SolutionMapper : ISolutionMapper
{
    private readonly Dictionary<int, Func<int, ChallengeSolution>> _yearMappings;

    public SolutionMapper()
    {
        _yearMappings = InitialiseYearMappings();
    }

    public bool DoesYearExist(int year)
    {
        return _yearMappings.ContainsKey(year);
    }

    public ChallengeSolution GetChallengeSolution(int year, int day)
    {
        return _yearMappings[year](day);
    }

    private static Dictionary<int, Func<int, ChallengeSolution>> InitialiseYearMappings()
    {
        return new Dictionary<int, Func<int, ChallengeSolution>>()
        {
            [2019] = (day) => day switch
            {
                1 => new Year2019.ChallengeSolution01(),
                2 => new Year2019.ChallengeSolution02(),

                _ => throw new NonexistentChallengeException(2019, day)
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

                _ => throw new NonexistentChallengeException(2020, day)
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

                _ => throw new NonexistentChallengeException(2021, day)
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
                17 => new Year2022.ChallengeSolution17(),
                20 => new Year2022.ChallengeSolution20(),
                21 => new Year2022.ChallengeSolution21(),
                25 => new Year2022.ChallengeSolution25(),

                _ => throw new NonexistentChallengeException(2022, day)
            },
            [2023] = (day) => day switch
            {
                1 => new Year2023.ChallengeSolution01(),
                2 => new Year2023.ChallengeSolution02(),
                3 => new Year2023.ChallengeSolution03(),

                _ => throw new NonexistentChallengeException(2023, day)
            }
        };
    }
}
