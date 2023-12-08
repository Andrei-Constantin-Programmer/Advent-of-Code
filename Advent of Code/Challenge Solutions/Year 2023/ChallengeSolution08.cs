using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution08 : ChallengeSolution
{
    private const string START_LABEL = "AAA";
    private const string END_LABEL = "ZZZ";

    protected override void SolveFirstPart()
    {
        var (instructions, nodes) = ReadInput();

        Console.WriteLine(ComputeStepsFromSourceToDestination(instructions, nodes));
    }

    protected override void SolveSecondPart()
    {
        var (instructions, nodes) = ReadInput();

        var steps = ComputeStepsFromSourcesToDestinations(
            instructions,
            nodes,
            startingCondition: node => node.Label.EndsWith('A'),
            endCondition: node => node.Label.EndsWith('Z')
        );

        Console.WriteLine(steps);
    }

    private static long ComputeStepsFromSourceToDestination(List<Instruction> instructions, List<Node> nodes)
    {
        Node startingNode = nodes.First(node => node.Label == START_LABEL);

        var steps = ComputeStepsToCondition(instructions, startingNode, node => node.Label == END_LABEL);

        return steps;
    }

    private static long ComputeStepsFromSourcesToDestinations(
        List<Instruction> instructions,
        List<Node> nodes,
        Func<Node, bool> startingCondition,
        Func<Node, bool> endCondition)
    {
        var startingNodes = nodes
            .Where(startingCondition)
            .ToList();

        var stepsToZ = startingNodes
            .Select(node => ComputeStepsToCondition(instructions, node, endCondition));

        return stepsToZ.Aggregate(1L, (lcm, x) => LowestCommonMultiple(lcm, x));
    }

    private static long ComputeStepsToCondition(List<Instruction> instructions, Node startingNode, Func<Node, bool> condition)
    {
        var currentNode = startingNode;
        var steps = 0;

        while (!condition(currentNode))
        {
            currentNode = instructions
                .Select(instruction => instruction switch
                {
                    Instruction.Left => currentNode.Left!,
                    Instruction.Right => currentNode.Right!,
                    _ => throw new Exception($"Unknown instruction {instruction}"),
                })
                .ElementAt(steps % instructions.Count);

            steps++;
        }

        return steps;
    }

    private static long LowestCommonMultiple(long a, long b) => (a * b) / GreatestCommonDivisor(a, b);
    private static long GreatestCommonDivisor(long a, long b) => a == 0 ? b : GreatestCommonDivisor(b % a, a);

    private (List<Instruction> instructions, List<Node> nodes) ReadInput()
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

    private string[] ReadInputLines() => Reader.ReadLines(this);

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
