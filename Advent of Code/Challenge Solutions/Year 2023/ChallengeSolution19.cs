using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution19 : ChallengeSolution
{
    private const char ACCEPTED = 'A';
    private const char REJECTED = 'R';

    protected override void SolveFirstPart()
    {
        var lines = Reader.ReadLines(this);
        var (workflows, parts) = ReadInput(lines);


    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private static (List<Workflow> workflows, List<Part> parts) ReadInput(string[] lines)
    {
        var workflowLines = lines
                    .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
                    .ToList();
        var partLines = lines
            .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
            .Skip(1)
            .ToList();
        var workflows = GetWorkflows(workflowLines);
        var parts = GetParts(partLines);

        return (workflows, parts);
    }

    private static List<Part> GetParts(List<string> partLines)
    {
        List<Part> parts = new();
        foreach (var partLine in partLines)
        {
            var ratings = partLine[1..^1]
                .Split(',')
                .Select(rating =>
                {
                    var equalIndex = rating.IndexOf('=');
                    return int.Parse(rating[(equalIndex + 1)..]);
                })
                .ToArray();

            parts.Add(new Part(ratings[0], ratings[1], ratings[2], ratings[3]));
        }

        return parts;
    }

    private static List<Workflow> GetWorkflows(List<string> workflowLines)
    {
        List<Workflow> workflows = new();
        foreach (var workflowLine in workflowLines)
        {
            var bracketIndex = workflowLine.IndexOf('{');
            var label = workflowLine[..bracketIndex];
            var workflowBody = workflowLine[(bracketIndex + 1)..^1];
            var ruleStrings = workflowBody.Split(',');

            Workflow newWorkflow = new(label);
            foreach (var ruleString in ruleStrings)
            {
                Rule rule;
                int separatorPosition;

                if ((separatorPosition = ruleString.IndexOf(':')) > -1)
                {
                    var category = ruleString[0];
                    var symbol = ruleString[1];
                    var value = int.Parse(ruleString[2..separatorPosition]);
                    var destination = ruleString[(separatorPosition + 1)..];

                    rule = new(GetRuleCondition(category, symbol, value), destination);
                }
                else
                {
                    rule = new(_ => true, ruleString);
                }

                newWorkflow.Rules.Enqueue(rule);
            }

            workflows.Add(newWorkflow);
        }

        return workflows;
    }

    private static Predicate<Part> GetRuleCondition(char category, char symbol, int value)
    {
        var mathematicalCondition = GetMathematicalCondition(symbol, value);

        return category switch
        {
            'x' => part => mathematicalCondition(part.X),
            'm' => part => mathematicalCondition(part.M),
            'a' => part => mathematicalCondition(part.A),
            's' => part => mathematicalCondition(part.S),

            _ => throw new ArgumentException($"Unknown category {category}")
        };

        static Predicate<int> GetMathematicalCondition(char symbol, int value) => symbol switch
        {
            '<' => rating => rating < value,
            '>' => rating => rating > value,

            _ => throw new ArgumentException($"Unknown symbol {symbol}")
        };
    }

    private class Workflow
    {
        public string Label { get; }
        public Queue<Rule> Rules { get; }

        public Workflow(string label)
        {
            Label = label;
            Rules = new();
        }
    }

    private class Rule
    {
        public Predicate<Part> Condition { get; }
        public string Destination { get; }

        public Rule(Predicate<Part> condition, string destination)
        {
            Condition = condition;
            Destination = destination;
        }
    }

    private record Part(int X, int M, int A, int S);
}
