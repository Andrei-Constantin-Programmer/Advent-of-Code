using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution15 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var stringsToHash = Reader.ReadLines(this)[0].Split(',', StringSplitOptions.RemoveEmptyEntries);

        long sum = stringsToHash.Select(Hash).Sum();

        Console.WriteLine(sum);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static int Hash(string stringToHash) => stringToHash
        .Aggregate(0, (hash, character) => (hash + character) * 17 % 256);
}
