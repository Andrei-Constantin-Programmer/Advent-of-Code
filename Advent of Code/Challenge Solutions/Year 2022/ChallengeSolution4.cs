using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution4 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(
                ReadPairRanges()
                .Where(pair => ContainsRange(pair.FirstRange, pair.SecondRange) || ContainsRange(pair.SecondRange, pair.FirstRange))
                .Count());
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private List<Pair> ReadPairRanges()
        {
            var pairs = new List<Pair>();

            using (TextReader read = GetInputFile(2022, 4))
            {
                string? line;
                while ((line = read.ReadLine()) != null)
                {
                    var ranges = line
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(elf =>
                        {
                            var rangeValues = elf.Split("-", StringSplitOptions.RemoveEmptyEntries);
                            return new Range(Convert.ToByte(rangeValues[0]), Convert.ToByte(rangeValues[1]));
                        })
                        .ToList();

                    pairs.Add(new Pair(ranges[0], ranges[1]));
                }
            }

            return pairs;
        }

        private bool ContainsRange(Range firstRange, Range secondRange)
        {
            return firstRange.Minimum <= secondRange.Minimum && firstRange.Maximum >= secondRange.Maximum;
        }

        record Pair(Range FirstRange, Range SecondRange);
        record Range(byte Minimum, byte Maximum);
    }
}
