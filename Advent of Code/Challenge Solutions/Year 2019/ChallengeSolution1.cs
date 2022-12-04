using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2019
{
    internal class ChallengeSolution1 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(ReadModuleMasses()
                .Select(mass => CalculateMassSum(mass))
                .Sum());
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private long CalculateMassSum(long moduleMass)
        {
            return moduleMass / 3 - 2;
        }

        private List<int> ReadModuleMasses()
        {
            return File.ReadAllLines(GetFileString(FileType.Input, 2019, 1))
                .Select(massString => Convert.ToInt32(massString))
                .ToList();
        }
    }
}
