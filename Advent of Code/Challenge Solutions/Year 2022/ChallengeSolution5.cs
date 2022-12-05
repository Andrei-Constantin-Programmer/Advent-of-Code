using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution5 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var (stacks, moves) = ReadInput();

            OperateMoves(stacks, moves);

            Console.WriteLine(String.Join("", stacks.Select(stack => stack.Peek())));
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private void OperateMoves(List<Stack<char>> stacks, List<MoveOperation> moves)
        {
            foreach(var move in moves)
            {
                for (int i = 0; i < move.Quantity; i++)
                {
                    stacks[move.Destination].Push(stacks[move.Source].Pop());
                }
            }
        }

        private (List<Stack<char>>, List<MoveOperation>) ReadInput()
        {
            var stacks = new List<Stack<char>>();
            var moves = new List<MoveOperation>();

            using (TextReader read = GetInputFile(2022, 5))
            {
                var stackInput = new List<string>();
                string? line;
                while ((line = read.ReadLine()) != null && line.Trim().Length > 0)
                {
                    stackInput.Add(line);
                }

                stacks = GetStacksFromInput(stackInput);

                while ((line = read.ReadLine()) != null)
                {
                    var splitLine = line
                        .Split(new string[] { "move", "from", "to" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(value => Convert.ToInt32(value))
                        .ToList();
                    moves.Add(new MoveOperation(splitLine[1] - 1, splitLine[2] - 1, splitLine[0]));
                }
            }

            return (stacks, moves);
        }

        private List<Stack<char>> GetStacksFromInput(List<string> input)
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
}
