// Task: https://adventofcode.com/2022/day/9

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution09 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            var commands = ReadCommands();

            var head = new Knot(new Coordinate(0, 0));
            var tail = new Knot(new Coordinate(0, 0));
            ComputeCommands(head, tail, commands);

            Console.WriteLine(tail.History.Count);
        }

        protected override void SolveSecondPart()
        {
            var commands = ReadCommands();
            var knots = CreateKnots();

            ComputeCommands(knots[0], knots.Skip(1).ToList(), commands);

            Console.WriteLine(knots.Last().History.Count);
        }

        private static void ComputeCommands(Knot head, Knot tail, List<Command> commands)
        {
            ComputeCommands(head, new List<Knot>() { tail }, commands);
        }

        private static void ComputeCommands(Knot head, List<Knot> trailingKnots, List<Command> commands)
        {
            foreach(var command in commands)
            {
                (head, trailingKnots) = MoveRope(head, trailingKnots, command);
            }
        }

        private static (Knot, List<Knot>) MoveRope(Knot head, List<Knot> trailingKnots, Command command)
        {
            if (command.Steps == 0)
            {
                return (head, trailingKnots);
            }

            head = MoveHead(head, command.Direction);
            for(int i = 0; i < trailingKnots.Count; i++)
            {
                if (i == 0)
                    trailingKnots[i] = MoveTail(head, trailingKnots[i]);
                else
                    trailingKnots[i] = MoveTail(trailingKnots[i - 1], trailingKnots[i]);
            }

            return MoveRope(head, trailingKnots, new Command(command.Direction, command.Steps - 1));
        }

        private static Knot MoveTail(Knot head, Knot tail)
        {
            if (AreTouching(head.Position, tail.Position))
                return tail;

            var newTailPosition = new Coordinate(tail.Position.Row, tail.Position.Column);

            var headRow = head.Position.Row;
            var headColumn = head.Position.Column;
            var tailRow = tail.Position.Row;
            var tailColumn = tail.Position.Column;

            if(headRow == tailRow)
            {
                if (headColumn > tailColumn)
                    newTailPosition.Column++;
                else if (headColumn < tailColumn)
                    newTailPosition.Column--;
            }
            else if(headColumn == tailColumn)
            {
                if (headRow > tailRow)
                    newTailPosition.Row++;
                else if (headRow < tailRow)
                    newTailPosition.Row--;
            }
            else
            {
                if(headRow < tailRow && headColumn < tailColumn)
                {
                    newTailPosition.Row--;
                    newTailPosition.Column--;
                }
                else if (headRow < tailRow && headColumn > tailColumn)
                {
                    newTailPosition.Row--;
                    newTailPosition.Column++;
                }
                else if (headRow > tailRow && headColumn < tailColumn)
                {
                    newTailPosition.Row++;
                    newTailPosition.Column--;
                }
                else
                {
                    newTailPosition.Row++;
                    newTailPosition.Column++;
                }
            }

            tail.History.Add(newTailPosition);

            return new Knot(newTailPosition, tail.History);
        }

        private static bool AreTouching(Coordinate head, Coordinate tail)
        {
            if (head == tail)
                return true;

            int horizontalDistance = head.Column - tail.Column;
            int verticalDistance = head.Row - tail.Row;

            if (Math.Abs(horizontalDistance) > 1)
                return false;
            if (Math.Abs(verticalDistance) > 1)
                return false;

            return true;
        }

        private static Knot MoveHead(Knot head, Direction direction)
        {
            var newHead = new Knot(new Coordinate(head.Position.Row, head.Position.Column));
            switch (direction)
            {
                case Direction.Up:
                    newHead.Position = new Coordinate(head.Position.Row - 1, head.Position.Column);
                    break;
                case Direction.Down:
                    newHead.Position = new Coordinate(head.Position.Row + 1, head.Position.Column);
                    break;
                case Direction.Left:
                    newHead.Position = new Coordinate(head.Position.Row, head.Position.Column - 1);
                    break;
                case Direction.Right:
                    newHead.Position = new Coordinate(head.Position.Row, head.Position.Column + 1);
                    break;
            }

            return newHead;
        }

        private static List<Knot> CreateKnots()
        {
            var knots = new List<Knot>();

            for(int i = 0; i < 10; i++)
                knots.Add(new Knot(new Coordinate(0, 0)));

            return knots;
        }

        private static List<Command> ReadCommands()
        {
            var commands = new List<Command>();
            using(TextReader read = Reader.GetInputFile(2022, 9))
            {
                string? line;
                while((line = read.ReadLine()) != null)
                {
                    var elements = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    commands.Add(new Command(
                        elements[0] switch
                        {
                            "R" => Direction.Right,
                            "L" => Direction.Left,
                            "U" => Direction.Up,
                            "D" => Direction.Down,
                            _ => throw new ArgumentException()
                        },
                        Convert.ToInt32(elements[1])));
                }
            }

            return commands;
        }

        private class Knot
        {
            public Coordinate Position { get; set; }
            public HashSet<Coordinate> History { get; set; }

            public Knot(Coordinate initialPosition)
            {
                Position = initialPosition;
                History = new HashSet<Coordinate>() { initialPosition };
            }

            public Knot(Coordinate position, HashSet<Coordinate> history)
            {
                Position = position;
                History = history;
            }
        }

        private record struct Coordinate(int Row, int Column);

        private record struct Command(Direction Direction, int Steps);

        private enum Direction
        {
            Right,
            Up,
            Left,
            Down
        }
    }
}
