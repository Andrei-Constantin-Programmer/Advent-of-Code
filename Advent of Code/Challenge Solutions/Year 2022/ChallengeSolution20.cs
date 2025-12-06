// Task: https://adventofcode.com/2022/day/20

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution20(IConsole console, ISolutionReader<ChallengeSolution20> reader)
    : ChallengeSolution<ChallengeSolution20>(console, reader)
{
    private const long DECRYPTION_KEY = 811589153;

    public override void SolveFirstPart()
    {
        var arrangement = ReadInitialArrangement();

        arrangement = UnmixArrangement(arrangement, 1);
        arrangement = RotateArrangementToZero(arrangement);

        _console.WriteLine(GetGroveCoordinatesSum(arrangement));
    }

    public override void SolveSecondPart()
    {
        var arrangement = ReadInitialArrangement();
        ApplyDecriptionKey(arrangement);

        arrangement = UnmixArrangement(arrangement, 10);
        arrangement = RotateArrangementToZero(arrangement);


        _console.WriteLine(GetGroveCoordinatesSum(arrangement));
    }

    private static List<EncryptedValue> UnmixArrangement(List<EncryptedValue> arrangement, byte timesToUnmix)
    {
        var newArrangement = new List<EncryptedValue>(arrangement);

        for (var mix = 0; mix < timesToUnmix; mix++)
        {
            foreach (var element in arrangement)
            {
                var index = newArrangement.IndexOf(element);

                var nextPosition = GetNextPosition(newArrangement, index);

                MoveElements(newArrangement, index, nextPosition);
                newArrangement[nextPosition] = element;
            }
        }

        return newArrangement;
    }

    private static int GetNextPosition(List<EncryptedValue> arrangement, int currentPosition)
    {
        var nextPosition = currentPosition + arrangement[currentPosition].Value;

        while (nextPosition <= 0 || nextPosition >= arrangement.Count)
        {
            if (nextPosition <= 0)
            {
                nextPosition += Math.Max(1, Math.Abs(nextPosition) / (arrangement.Count - 1)) * (arrangement.Count - 1);
            }
            else if (nextPosition >= arrangement.Count)
            {
                nextPosition -= (nextPosition / arrangement.Count) * (arrangement.Count - 1);
            }
        }

        return (int)nextPosition;
    }

    private static void MoveElements(List<EncryptedValue> arrangement, int startPosition, int endPosition)
    {
        if (startPosition < endPosition)
        {
            for (var i = startPosition; i < endPosition; i++)
            {
                arrangement[i] = arrangement[i + 1];
            }
        }
        else
        {
            for (var i = startPosition; i > endPosition; i--)
            {
                arrangement[i] = arrangement[i - 1];
            }
        }
    }

    private static List<EncryptedValue> RotateArrangementToZero(List<EncryptedValue> arrangement)
    {
        var zeroIndex = arrangement.IndexOf(arrangement.First(x => x.Value == 0));

        var array = arrangement.ToArray();
        var elementsToValue = array[0..zeroIndex];
        var elementsFromValue = array[zeroIndex..];

        return elementsFromValue.Concat(elementsToValue).ToList();
    }

    private static long GetGroveCoordinatesSum(List<EncryptedValue> arrangement)
    {
        return
            arrangement[1000 % arrangement.Count].Value +
            arrangement[2000 % arrangement.Count].Value +
            arrangement[3000 % arrangement.Count].Value;
    }

    private static void ApplyDecriptionKey(List<EncryptedValue> arrangement)
    {
        foreach (var element in arrangement)
            element.Value *= DECRYPTION_KEY;
    }

    private List<EncryptedValue> ReadInitialArrangement()
    {
        return _reader.ReadLines()
            .Select(x => new EncryptedValue(Convert.ToInt16(x)))
            .ToList();
    }

    private class EncryptedValue
    {
        public long Value { get; set; }

        public EncryptedValue(long value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
