namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution04 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        double totalPointsSum = 0;

        string? line;
        using TextReader read = Reader.GetInputFile(2023, 4);
        while ((line = read.ReadLine()) != null)
        {
            var (winningNumbers, gameNumbers) = GetNumbersFromCard(line);
            var winningGameNumberCount = gameNumbers.Count(x => winningNumbers.Contains(x));

            if (winningGameNumberCount == 0)
            {
                continue;
            }

            var pointValue = Math.Pow(2, winningGameNumberCount - 1);
            totalPointsSum += pointValue;
        }

        Console.WriteLine(totalPointsSum);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static (List<int>, List<int>) GetNumbersFromCard(string card)
    {
        var cardElements = card.Split(": ");
        var gameArea = cardElements[1].Split(" | ");
        var winningNumbers = NumberSequenceToList(gameArea[0]);
        var gameNumbers = NumberSequenceToList(gameArea[1]);

        return (winningNumbers, gameNumbers);
    }

    private static List<int> NumberSequenceToList(string numberSequence) => numberSequence
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        .Select(int.Parse)
        .ToList();
}
