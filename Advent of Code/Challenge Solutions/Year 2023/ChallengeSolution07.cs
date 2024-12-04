// Task: https://adventofcode.com/2023/day/7

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution07(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        _console.WriteLine(GetTotalWinnings(false));
    }

    public override void SolveSecondPart()
    {
        _console.WriteLine(GetTotalWinnings(true));
    }

    private int GetTotalWinnings(bool includeJokers)
    {
        var hands = ReadCamelCardsHands(includeJokers);

        hands = hands
            .OrderBy(hand => hand)
            .ToList();

        var totalWinnings = 0;
        for (var rank = 1; rank <= hands.Count; rank++)
        {
            totalWinnings += hands[rank - 1].Bid * rank;
        }

        return totalWinnings;
    }

    private List<Hand> ReadCamelCardsHands(bool includeJokers)
    {
        List<Hand> hands = new();

        foreach (var line in Reader.ReadLines(this))
        {
            var elements = line.Split(' ');
            var cards = elements[0]
                .Select(card => ParseCard(card, includeJokers))
                .ToArray();
            var bid = int.Parse(elements[1]);

            hands.Add(includeJokers ? new JokerHand(cards, bid) : new Hand(cards, bid));
        }

        return hands;
    }

    private static CamelCard ParseCard(char character, bool includeJokers) => character switch
    {
        '2' => CamelCard.Two,
        '3' => CamelCard.Three,
        '4' => CamelCard.Four,
        '5' => CamelCard.Five,
        '6' => CamelCard.Six,
        '7' => CamelCard.Seven,
        '8' => CamelCard.Eight,
        '9' => CamelCard.Nine,
        'T' => CamelCard.T,
        'J' => includeJokers ? CamelCard.Joker : CamelCard.J,
        'Q' => CamelCard.Q,
        'K' => CamelCard.K,
        'A' => CamelCard.A,

        _ => throw new ArgumentException($"Unknown Card label {character}"),
    };

    private record JokerHand : Hand
    {
        public JokerHand(CamelCard[] cards, int bid) : base(cards, bid, GetHandTypeWithJokers(cards)) { }

        protected static HandType GetHandTypeWithJokers(CamelCard[] cards)
        {
            var jokerCount = cards.Count(card => card == CamelCard.Joker);
            if (jokerCount == 0)
            {
                return GetHandType(cards);
            }

            var groupedCards = cards
                .Where(card => card != CamelCard.Joker)
                .GroupBy(card => card)
                .OrderBy(group => group.Count())
                .ToArray();

            bool isHighCard = groupedCards.Length == cards.Length - jokerCount;
            if (isHighCard)
            {
                return jokerCount switch
                {
                    1 => HandType.OnePair,
                    2 => HandType.ThreeOfAKind,
                    3 => HandType.FourOfAKind,
                    4 or 5 => HandType.FiveOfAKind,
                    _ => HandType.HighCard
                };
            }

            bool isOnePair = groupedCards.Length == cards.Length - jokerCount - 1;
            if (isOnePair)
            {
                var highestPair = groupedCards[0].Key;
                return jokerCount switch
                {
                    1 => HandType.ThreeOfAKind,
                    2 => HandType.FourOfAKind,
                    3 => HandType.FiveOfAKind,
                    _ => HandType.HighCard
                };
            }

            bool isTwoPairOrThreeOfAKind = groupedCards.Length == cards.Length - jokerCount - 2;
            if (isTwoPairOrThreeOfAKind)
            {
                var isTwoPair = groupedCards.Any(group => group.Count() == 2);

                return isTwoPair
                    ? HandType.FullHouse
                    : jokerCount switch
                    {
                        1 => HandType.FourOfAKind,
                        2 => HandType.FiveOfAKind,
                        _ => HandType.HighCard
                    };
            }

            return HandType.FiveOfAKind;
        }
    }

    private record Hand : IComparable<Hand>
    {
        public CamelCard[] Cards { get; }
        public int Bid { get; }
        public HandType Type { get; }

        public Hand(CamelCard[] cards, int bid) : this(cards, bid, GetHandType(cards)) { }

        protected Hand(CamelCard[] cards, int bid, HandType type)
        {
            if (cards.Length != 5)
            {
                throw new ArgumentException("There must be five cards in a hand");
            }

            Cards = cards;
            Bid = bid;
            Type = type;
        }

        public int CompareTo(Hand? other)
        {
            if (other is null)
            {
                return 1;
            }

            var typeDifference = Type.CompareTo(other.Type);
            if (typeDifference != 0)
            {
                return typeDifference;
            }

            var cardComparison = Cards
                .Zip(other.Cards, (card1, card2) => card1 - card2)
                .FirstOrDefault(difference => difference != 0);

            return cardComparison;
        }

        protected static HandType GetHandType(CamelCard[] cards)
        {
            var groupedCards = cards.GroupBy(card => card).ToArray();

            return groupedCards.Length switch
            {
                1 => HandType.FiveOfAKind,
                2 => groupedCards.Any(group => group.Count() == 4)
                    ? HandType.FourOfAKind
                    : HandType.FullHouse,
                3 => groupedCards.Any(group => group.Count() == 3)
                    ? HandType.ThreeOfAKind
                    : HandType.TwoPair,
                4 => HandType.OnePair,

                _ => HandType.HighCard,
            };
        }
    }

    private enum HandType
    {
        HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind
    }

    private enum CamelCard
    {
        Joker, Two, Three, Four, Five, Six, Seven, Eight, Nine, T, J, Q, K, A
    }
}
