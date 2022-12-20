namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution20 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            var arrangement = ReadInitialArrangement();

            try
            {
                arrangement = UnmixArrangement(arrangement);
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            arrangement = RotateBitListToValue(arrangement, 0);

            Console.WriteLine(GetGroveCoordinatesSum(arrangement));
        }

        protected override void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private static int GetGroveCoordinatesSum(List<Bit> arrangement)
        {
            return
                arrangement[1000 % arrangement.Count].Value +
                arrangement[2000 % arrangement.Count].Value +
                arrangement[3000 % arrangement.Count].Value;
        }

        private static List<Bit> RotateBitListToValue(List<Bit> list, short value)
        {
            var valueIndex = list.IndexOf(list.First(x => x.Value == value));

            var array = list.ToArray();
            var elementsToValue = array[0..valueIndex];
            var elementsFromValue = array[valueIndex..];

            return elementsFromValue.Concat(elementsToValue).ToList();
        }

        private static List<Bit> UnmixArrangement(List<Bit> arrangement)
        {
            var newArrangement = new List<Bit>(arrangement);

            foreach(var element in arrangement)
            {
                var index = newArrangement.IndexOf(element);

                var nextPosition = GetNextPosition(newArrangement, index);

                MoveElements(newArrangement, index, nextPosition);
                newArrangement[nextPosition] = element;
            }

            return newArrangement;
        }

        private static void MoveElements(List<Bit> arrangement, int startPosition, int endPosition)
        {
            if (startPosition < endPosition)
            {
                for (int j = startPosition; j < endPosition; j++)
                {
                    arrangement[j] = arrangement[j + 1];
                }
            }
            else
            {
                for (int j = startPosition; j > endPosition; j--)
                {
                    arrangement[j] = arrangement[j - 1];
                }
            }
        }

        private static int GetNextPosition(List<Bit> arrangement, int currentPosition)
        {
            var nextPosition = currentPosition + arrangement[currentPosition].Value;

            while(nextPosition <= 0 || nextPosition >= arrangement.Count)
            {
                if (nextPosition <= 0)
                {
                    nextPosition += arrangement.Count - 1;
                }
                else if (nextPosition >= arrangement.Count)
                {
                    nextPosition -= (arrangement.Count - 1);
                }
            }

            return nextPosition;
        }

        private static List<Bit> ReadInitialArrangement()
        {
            return File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2022, 20))
                .Select(x => new Bit(Convert.ToInt16(x)))
                .ToList();
        }

        private class Bit
        {
            public short Value { get; init; }

            public Bit(short value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}
