namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution17 : ChallengeSolution
    {
        private readonly Rock[] rocks;

        public ChallengeSolution17()
        {
            rocks = CreateRocks();

            foreach(var rock in rocks)
            {
                for(int i = 0; i < rock.Shape.Length; i++)
                {
                    for(int j = 0; j < rock.Shape[i].Length; j++)
                        Console.Write(rock.Shape[i][j]);
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        protected override void SolveFirstPart()
        {
            throw new NotImplementedException();
        }

        protected override void SolveSecondPart()
        {
            throw new NotImplementedException();
        }


        private static Rock[] CreateRocks()
        {
            return new Rock[]
            {
                new Rock("Minus", new char[][] { new char[] { '#', '#', '#', '#' } }),
                new Rock("Plus", new char[][] { 
                    new char[] { '.', '#', '.' },
                    new char[] { '#', '#', '#' },
                    new char[] { '.', '#', '.' },
                }),
                new Rock("Corner", new char[][]
                {
                    new char[] { '.', '.', '#' },
                    new char[] { '.', '.', '#' },
                    new char[] { '#', '#', '#' },
                }),
                new Rock("Pipe", new char[][]
                {
                    new char[] { '#' },
                    new char[] { '#' },
                    new char[] { '#' },
                }),
                new Rock("Square", new char[][] {
                    new char[] { '#', '#' },
                    new char[] { '#', '#' },
                }),
            };
        }

        private record struct Rock(string Name, char[][] Shape);
    }
}
