// Task: https://adventofcode.com/2023/day/4

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution04(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        double totalPointsSum = 0;

        foreach (var line in Reader.ReadLines(this))
        {
            var winningGameNumberCount = GetCard(line).MatchingValues;

            if (winningGameNumberCount == 0)
            {
                continue;
            }

            var pointValue = Math.Pow(2, winningGameNumberCount - 1);
            totalPointsSum += pointValue;
        }

        _console.WriteLine(totalPointsSum);
    }

    public override void SolveSecondPart()
    {
        var cards = Reader.ReadLines(this).Select(GetCard).ToList();

        Queue<Card> cardQueue = new(cards);
        int cardCount = 0;

        while (cardQueue.Count > 0)
        {
            var card = cardQueue.Dequeue();
            cardCount++;

            if (card.MatchingValues == 0)
            {
                continue;
            }

            for (var i = card.Number; i - card.Number < card.MatchingValues; i++)
            {
                cardQueue.Enqueue(cards[i]);
            }
        }

        _console.WriteLine(cardCount);
    }

    private static Card GetCard(string cardLine)
    {
        var (winningNumbers, gameNumbers) = GetNumbersFromCardLine(cardLine, out var cardNumber);
        return new Card(cardNumber, gameNumbers.Count(x => winningNumbers.Contains(x)));
    }

    private static (List<int>, List<int>) GetNumbersFromCardLine(string cardLine, out int cardNumber)
    {
        var cardElements = cardLine.Split(": ");
        cardNumber = int.Parse(cardElements[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1].Trim());

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

    private record Card(int Number, int MatchingValues);
}
