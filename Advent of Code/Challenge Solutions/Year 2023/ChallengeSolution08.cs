namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution08 : ChallengeSolution
{
    protected override void SolveFirstPart()
    {
        var steps = 0;

        var (instructions, nodes) = ReadInput();

        Node currentNode = nodes.First(node => node.Label == "AAA");

        for (var i = 0; ; i++, steps++)
        {
            if (currentNode.Label == "ZZZ")
            {
                break;
            }

            if (i == instructions.Count)
            {
                i = 0;
            }

            currentNode = instructions[i] switch
            {
                Instruction.Left => currentNode.Left!,
                Instruction.Right => currentNode.Right!,

                _ => throw new Exception($"Unknown instruction {instructions[i]}"),
            };
        }

        Console.WriteLine(steps);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static (List<Instruction> instructions, List<Node> nodes) ReadInput()
    {
        Dictionary<Node, (string left, string right)> nodeMap = new();
        var lines = ReadInputLines();

        var instructions = lines[0]
            .Select(c => c == 'R' ? Instruction.Right : Instruction.Left)
            .ToList();

        foreach (var line in lines[2..])
        {
            var elements = line.Split(" = ");
            var childNodes = elements[1][1..^1].Split(", ");

            nodeMap.Add(new(elements[0]), (childNodes[0], childNodes[1]));
        }

        var nodes = nodeMap.Keys.ToList();
        foreach (var node in nodes)
        {
            var (left, right) = nodeMap[node];
            node.Left = nodes.First(n => n.Label == left);
            node.Right = nodes.First(n => n.Label == right);
        }

        return (instructions, nodes);
    }

    private static string[] ReadInputLines() => File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2023, 8));

    private class Node
    {
        public string Label { get; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }

        public Node(string label, Node? left = null, Node? right = null)
        {
            Label = label;
            Left = left;
            Right = right;
        }
    }

    private enum Instruction
    {
        Left, Right
    }
}
