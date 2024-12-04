// Task: https://adventofcode.com/2022/day/17

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution17 : ChallengeSolution<ChallengeSolution17>
{
    private const int stackWidth = 7;
    private readonly Rock[] rocks;

    public ChallengeSolution17(IConsole console, ISolutionReader<ChallengeSolution17> reader) : base(console, reader)
    {
        rocks = CreateRocks();
    }

    public override void SolveFirstPart()
    {
        var stack = new Stack<char[]>();
        stack.Push(CreateStackBase());
        var air = CreateStackAirLine();

        for (int i = 0; i < 3; i++)
            stack.Push(air);

        for (int rockIndex = 0; rockIndex < 10; rockIndex++)
        {
            var rock = rocks[rockIndex % rocks.Length];

            foreach (var line in rock.Shape)
            {
                stack.Push(line);
            }

            for (int i = 0; i < 3; i++)
                stack.Push(air);
        }

        foreach (var x in stack)
            _console.WriteLine(x);

    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static char[] CreateStackAirLine()
    {
        return Enumerable.Repeat('.', stackWidth).ToArray();
    }

    private static char[] CreateStackBase()
    {
        return Enumerable.Repeat('-', stackWidth).ToArray();
    }

    private static Rock[] CreateRocks()
    {
        return new Rock[]
        {
            new Rock("Minus", new char[][] {
                new char[] { '#', '#', '#', '#' },
                new char[] { '.', '.', '.', '.' },
                new char[] { '.', '.', '.', '.' },
            }),
            new Rock("Plus", new char[][] {
                new char[] { '.', '#', '.' },
                new char[] { '#', '#', '#' },
                new char[] { '.', '#', '.' },
            }),
            new Rock("Corner", new char[][]
            {
                new char[] { '#', '#', '#' },
                new char[] { '.', '.', '#' },
                new char[] { '.', '.', '#' },
            }),
            new Rock("Pipe", new char[][]
            {
                new char[] { '#', '.', '.' },
                new char[] { '#', '.', '.' },
                new char[] { '#', '.', '.' },
            }),
            new Rock("Square", new char[][] {
                new char[] { '.', '.', '.' },
                new char[] { '#', '#', '.' },
                new char[] { '#', '#', '.' },
            }),
        };
    }

    private record struct Rock(string Name, char[][] Shape);
}
