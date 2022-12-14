namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution04 : ChallengeSolution
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
            Console.WriteLine(
                ReadPairRanges()
                .Where(pair => OverlapsRange(pair.FirstRange, pair.SecondRange))
                .Count());
        }

        private static List<Pair> ReadPairRanges()
        {
            var pairs = new List<Pair>();

            using (TextReader read = Reader.GetInputFile(2022, 4))
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

        private static bool ContainsRange(Range firstRange, Range secondRange)
        {
            return firstRange.Minimum <= secondRange.Minimum && firstRange.Maximum >= secondRange.Maximum;
        }

        private static bool OverlapsRange(Range firstRange, Range secondRange)
        {
            if (firstRange.Minimum == secondRange.Minimum || firstRange.Minimum == secondRange.Maximum)
                return true;
            if (firstRange.Maximum == secondRange.Minimum || firstRange.Maximum == secondRange.Maximum)
                return true;

            if (firstRange.Minimum < secondRange.Minimum && firstRange.Maximum > secondRange.Minimum)
                return true;

            if (firstRange.Minimum > secondRange.Minimum && firstRange.Minimum < secondRange.Maximum)
                return true;

            return false;
        }

        record Pair(Range FirstRange, Range SecondRange);
        record Range(byte Minimum, byte Maximum);
    }
}
