// Task: https://adventofcode.com/2022/day/5

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution05(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        var (stacks, moves) = ReadInput();

        OperateMovesCrateMover9000(stacks, moves);

        _console.WriteLine(String.Join("", stacks.Select(stack => stack.Peek())));
    }

    public override void SolveSecondPart()
    {
        var (stacks, moves) = ReadInput();

        OperateMovesCrateMover9001(stacks, moves);

        _console.WriteLine(String.Join("", stacks.Select(stack => stack.Peek())));
    }

    private static void OperateMovesCrateMover9000(List<Stack<char>> stacks, List<MoveOperation> moves)
    {
        foreach (var move in moves)
        {
            for (int i = 0; i < move.Quantity; i++)
            {
                stacks[move.Destination].Push(stacks[move.Source].Pop());
            }
        }
    }

    private static void OperateMovesCrateMover9001(List<Stack<char>> stacks, List<MoveOperation> moves)
    {
        foreach (var move in moves)
        {
            var crateBuffer = new Stack<char>();
            for (int i = 0; i < move.Quantity; i++)
            {
                crateBuffer.Push(stacks[move.Source].Pop());
            }

            foreach (var crate in crateBuffer)
            {
                stacks[move.Destination].Push(crate);
            }
        }
    }

    private (List<Stack<char>>, List<MoveOperation>) ReadInput()
    {
        var stacks = new List<Stack<char>>();
        var moves = new List<MoveOperation>();

        var lines = Reader.ReadLines(this);
        var movesPosition = lines.ToList().IndexOf(string.Empty);
        var stackInput = lines[..movesPosition].ToList();
        stacks = GetStacksFromInput(stackInput);

        var moveLines = lines[(movesPosition + 1)..];
        foreach (var line in moveLines)
        {
            var splitLine = line
                .Split(new string[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => Convert.ToInt32(value))
                .ToList();
            moves.Add(new MoveOperation(splitLine[1] - 1, splitLine[2] - 1, splitLine[0]));
        }

        return (stacks, moves);
    }

    private static List<Stack<char>> GetStacksFromInput(List<string> input)
    {
        var stacks = new List<Stack<char>>();

        int numberOfStacks = input
            .Last()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Count();

        for (int column = 0; column < numberOfStacks; column++)
        {
            var currentStack = new Stack<char>();

            for (int row = input.Count - 1; row >= 0; row--)
            {
                var character = input[row][column * 3 + column + 1];
                if (Char.IsLetter(character))
                    currentStack.Push(character);
            }

            stacks.Add(currentStack);
        }

        return stacks;
    }


    record MoveOperation(int Source, int Destination, int Quantity);
}
