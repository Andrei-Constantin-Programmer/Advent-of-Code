using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution19 : ChallengeSolution
{
    private const char ACCEPTED = 'A';
    private const char REJECTED = 'R';
    private static readonly int _minimumRating = 0;
    private static readonly int _maximumRating = 4001;

    protected override void SolveFirstPart()
    {
        var (workflows, parts) = ReadInput();

        var startWorkflow = workflows.First(w => w.Label == "in");
        long ratingNumberSum = 0;
        foreach (var part in parts)
        {
            var isAccepted = GetAcceptance(part, startWorkflow, workflows) == ACCEPTED;
            if (isAccepted)
            {
                ratingNumberSum += part.Xmas;
            }
        }

        Console.WriteLine(ratingNumberSum);
    }

    protected override void SolveSecondPart()
    {
        var (workflows, _) = ReadInput();

        var startWorkflow = workflows.First(w => w.Label == "in");
        var ranges = GetFittingRanges(startWorkflow, workflows);

        long possibleCombinations = 0;
        foreach (var (lowBound, highBound) in ranges)
        {
            var x = highBound.X - lowBound.X - 1;
            var m = highBound.M - lowBound.M - 1;
            var a = highBound.A - lowBound.A - 1;
            var s = highBound.S - lowBound.S - 1;
            possibleCombinations += x * m * a * s;
        }

        Console.WriteLine(possibleCombinations);
    }

    private static List<(Part lowBound, Part highBound)> GetFittingRanges(Workflow workflow, List<Workflow> workflows)
    {
        List<(Part lowBound, Part highBound)> ranges = new();

        (Part lowBound, Part highBound) currentBounds = (new(_minimumRating, _minimumRating, _minimumRating, _minimumRating), new(_maximumRating, _maximumRating, _maximumRating, _maximumRating));

        foreach (var rule in workflow.Rules)
        {
            List<(Part lowBound, Part highBound)> nextRanges = GetNextRanges(rule, workflows);

            foreach ((Part lowBound, Part highBound) in nextRanges)
            {
                if (rule.Category is not null)
                {
                    ModifyBounds(rule, lowBound, highBound);
                }

                lowBound.X = Math.Max(lowBound.X, currentBounds.lowBound.X);
                lowBound.M = Math.Max(lowBound.M, currentBounds.lowBound.M);
                lowBound.A = Math.Max(lowBound.A, currentBounds.lowBound.A);
                lowBound.S = Math.Max(lowBound.S, currentBounds.lowBound.S);

                highBound.X = Math.Min(highBound.X, currentBounds.highBound.X);
                highBound.M = Math.Min(highBound.M, currentBounds.highBound.M);
                highBound.A = Math.Min(highBound.A, currentBounds.highBound.A);
                highBound.S = Math.Min(highBound.S, currentBounds.highBound.S);

                if (lowBound.X < highBound.X
                    && lowBound.M < highBound.M
                    && lowBound.A < highBound.A
                    && lowBound.S < highBound.S)
                {
                    ranges.Add((lowBound, highBound));
                }
            }

            if (rule.Symbol == '>')
            {
                ModifyBound(rule, currentBounds.highBound, Math.Min, +1);
            }
            else
            {
                ModifyBound(rule, currentBounds.lowBound, Math.Max, -1);
            }
        }

        return ranges;
    }

    private static void ModifyBounds(Rule rule, Part lowBound, Part highBound)
    {
        if (rule.Symbol == '>')
        {
            ModifyBound(rule, lowBound, Math.Max);
        }
        else
        {
            ModifyBound(rule, highBound, Math.Min);
        }
    }

    private static void ModifyBound(Rule rule, Part bound, Func<long, long, long> chooserFunction, int modifier = 0)
    {
        switch (rule.Category)
        {
            case 'x':
                bound.X = chooserFunction(bound.X, rule.ConditionValue!.Value) + modifier;
                break;
            case 'm':
                bound.M = chooserFunction(bound.M, rule.ConditionValue!.Value) + modifier;
                break;
            case 'a':
                bound.A = chooserFunction(bound.A, rule.ConditionValue!.Value) + modifier;
                break;
            case 's':
                bound.S = chooserFunction(bound.S, rule.ConditionValue!.Value) + modifier;
                break;
        }
    }

    private static List<(Part lowBound, Part highBound)> GetNextRanges(Rule rule, List<Workflow> workflows) => rule.Destination switch
    {
        "A" => new() { (new(_minimumRating, _minimumRating, _minimumRating, _minimumRating), new(_maximumRating, _maximumRating, _maximumRating, _maximumRating)) },
        "R" => new(),
        _ => GetFittingRanges(workflows.First(w => w.Label == rule.Destination), workflows)
    };

    private static char GetAcceptance(Part part, Workflow workflow, List<Workflow> workflows)
    {
        foreach (var rule in workflow.Rules)
        {
            if (rule.Condition(part))
            {
                return rule.Destination switch
                {
                    "A" => ACCEPTED,
                    "R" => REJECTED,
                    _ => GetAcceptance(part, workflows.First(w => w.Label == rule.Destination), workflows)
                };
            }
        }

        return REJECTED;
    }

    private (List<Workflow> workflows, List<Part> parts) ReadInput()
    {
        var lines = Reader.ReadLines(this);
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

                    rule = new(category, symbol, value, destination);
                }
                else
                {
                    rule = new(null, null, null, ruleString);
                }

                newWorkflow.Rules.Add(rule);
            }

            workflows.Add(newWorkflow);
        }

        return workflows;
    }

    private class Workflow
    {
        public string Label { get; }
        public List<Rule> Rules { get; }

        public Workflow(string label)
        {
            Label = label;
            Rules = new();
        }
    }

    private class Rule
    {
        public Predicate<Part> Condition { get; }
        public char? Category { get; }
        public char? Symbol { get; }
        public int? ConditionValue { get; }
        public string Destination { get; }

        public Rule(char? category, char? symbol, int? conditionValue, string destination)
        {
            Category = category;
            Destination = destination;
            Symbol = symbol;
            ConditionValue = conditionValue;

            Condition = category is null || symbol is null || conditionValue is null
                ? _ => true
                : GetRuleCondition(category.Value, symbol.Value, conditionValue.Value);
        }
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

        static Predicate<long> GetMathematicalCondition(char symbol, long value) => symbol switch
        {
            '<' => rating => rating < value,
            '>' => rating => rating > value,

            _ => throw new ArgumentException($"Unknown symbol {symbol}")
        };
    }

    private class Part
    {
        public long X { get; set; }
        public long M { get; set; }
        public long A { get; set; }
        public long S { get; set; }

        public long Xmas => X + M + A + S;

        public Part(long x, long m, long a, long s)
        {
            X = x;
            M = m;
            A = a;
            S = s;
        }

        public override string ToString() => $"{X} {M} {A} {S}";
    }
}
