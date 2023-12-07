namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution07 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        List<Hand> hands = new();

        string? line;
        using TextReader read = Reader.GetInputFile(2023, 7);
        while ((line = read.ReadLine()) != null)
        {
            var elements = line.Split(' ');
            var cards = elements[0]
                .Select(card => ParseCard(card, includeJokers: false))
                .ToArray();
            var bid = int.Parse(elements[1]);

            hands.Add(new(cards, bid));
        }

        hands = hands.OrderBy(hand => hand).ToList();

        var totalWinnings = 0;
        for (var rank = 1; rank <= hands.Count; rank++)
        {
            totalWinnings += hands[rank - 1].Bid * rank;
        }

        Console.WriteLine(totalWinnings);
    }

    protected override void SolveSecondPart()
    {
        List<JokerHand> hands = new();

        string? line;
        using TextReader read = Reader.GetInputFile(2023, 7);
        while ((line = read.ReadLine()) != null)
        {
            var elements = line.Split(' ');
            var cards = elements[0]
                .Select(card => ParseCard(card, includeJokers: true))
                .ToArray();
            var bid = int.Parse(elements[1]);

            hands.Add(new(cards, bid));
        }

        hands = hands.OrderBy(hand => hand).ToList();

        var totalWinnings = 0;
        for (var rank = 1; rank <= hands.Count; rank++)
        {
            totalWinnings += hands[rank - 1].Bid * rank;
        }

        Console.WriteLine(totalWinnings);

    }

    private static Card ParseCard(char character, bool includeJokers) => character switch
    {
        '2' => Card.Two,
        '3' => Card.Three,
        '4' => Card.Four,
        '5' => Card.Five,
        '6' => Card.Six,
        '7' => Card.Seven,
        '8' => Card.Eight,
        '9' => Card.Nine,
        'T' => Card.T,
        'J' => includeJokers ? Card.Joker : Card.J,
        'Q' => Card.Q,
        'K' => Card.K,
        'A' => Card.A,

        _ => throw new ArgumentException($"Unknown Card label {character}"),
    };

    private record JokerHand : Hand
    {
        public JokerHand(Card[] cards, int bid) : base(cards, bid, GetHandTypeWithJokers(cards)) { }

        protected static HandType GetHandTypeWithJokers(Card[] cards)
        {
            var jokerCount = cards.Count(card => card == Card.Joker);
            if (jokerCount == 0)
            {
                return GetHandType(cards);
            }

            var groupedCards = cards
                .Where(card => card != Card.Joker)
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
        public Card[] Cards { get; }
        public int Bid { get; }
        public HandType Type { get; }

        public Hand(Card[] cards, int bid) : this(cards, bid, GetHandType(cards)) { }

        protected Hand(Card[] cards, int bid, HandType type)
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

        protected static HandType GetHandType(Card[] cards)
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

    private enum Card
    {
        Joker, Two, Three, Four, Five, Six, Seven, Eight, Nine, T, J, Q, K, A
    }
}
