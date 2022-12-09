namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution9 : ChallengeSolution
    {
        private HashSet<Coordinate> visitedCoordinates;

        public ChallengeSolution9()
        {
            visitedCoordinates = new HashSet<Coordinate>();
        }

        public void SolveFirstPart()
        {
            var commands = ReadCommands();

            var (head, tail) = ComputeCommands(commands);

            Console.WriteLine(visitedCoordinates.Count);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private (Coordinate head, Coordinate tail) ComputeCommands(List<Command> commands)
        {
            var head = new Coordinate(0, 0);
            var tail = new Coordinate(0, 0);
            visitedCoordinates.Add(tail);

            foreach(var command in commands)
            {
                (head, tail) = MoveRope(head, tail, command);
            }

            return (head, tail);
        }

        private (Coordinate, Coordinate) MoveRope(Coordinate head, Coordinate tail, Command command)
        {
            if (command.Steps == 0)
            {
                return (head, tail);
            }

            head = MoveHead(head, command.Direction);
            tail = MoveTail(head, tail);

            return MoveRope(head, tail, new Command(command.Direction, command.Steps - 1));
        }

        private Coordinate MoveTail(Coordinate head, Coordinate tail)
        {
            if (AreTouching(head, tail))
                return tail;

            var newTail = new Coordinate(tail.Row, tail.Column);
            if(head.Row == tail.Row)
            {
                if (head.Column > tail.Column)
                    newTail.Column++;
                else if (head.Column < tail.Column)
                    newTail.Column--;
            }
            else if(head.Column == tail.Column)
            {
                if (head.Row > tail.Row)
                    newTail.Row++;
                else if (head.Row < tail.Row)
                    newTail.Row--;
            }
            else
            {
                if(head.Row < tail.Row && head.Column < tail.Column)
                {
                    newTail.Row--;
                    newTail.Column--;
                }
                else if (head.Row < tail.Row && head.Column > tail.Column)
                {
                    newTail.Row--;
                    newTail.Column++;
                }
                else if (head.Row > tail.Row && head.Column < tail.Column)
                {
                    newTail.Row++;
                    newTail.Column--;
                }
                else
                {
                    newTail.Row++;
                    newTail.Column++;
                }
            }

            visitedCoordinates.Add(newTail);

            return newTail;
        }

        private bool AreTouching(Coordinate head, Coordinate tail)
        {
            if (head == tail)
                return true;

            int horizontalDistance = head.Column - tail.Column;
            int verticalDistance = head.Row - tail.Row;

            if (!(horizontalDistance >= -1 && horizontalDistance <= 1))
                return false;
            if (!(verticalDistance >= -1 && verticalDistance <= 1))
                return false;

            return true;
        }

        private Coordinate MoveHead(Coordinate head, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Coordinate(head.Row - 1, head.Column);
                case Direction.Down:
                    return new Coordinate(head.Row + 1, head.Column);
                case Direction.Left:
                    return new Coordinate(head.Row, head.Column - 1);
                case Direction.Right:
                    return new Coordinate(head.Row, head.Column + 1);
            }

            return head;
        }

        private List<Command> ReadCommands()
        {
            var commands = new List<Command>();
            using(TextReader read = Utilities.GetInputFile(2022, 9))
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
