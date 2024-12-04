// Task: https://adventofcode.com/2023/day/19

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution19 : ChallengeSolution
{
    private const char ACCEPTED = 'A';
    private const char REJECTED = 'R';
    private const char GREATER_THAN = '>';
    private const string STARTING_WORKFLOW = "in";
    
    private static readonly int _minimumRating = 0;
    private static readonly int _maximumRating = 4001;

    protected override void SolveFirstPart()
    {
        var (workflows, parts) = ReadInput();
        var startWorkflow = workflows.First(w => w.Label == STARTING_WORKFLOW);

        var ratingNumberSum = parts
            .Where(part => GetAcceptance(part, startWorkflow, workflows) == ACCEPTED)
            .Sum(part => part.Xmas);
        Console.WriteLine(ratingNumberSum);
    }

    protected override void SolveSecondPart()
    {
        var (workflows, _) = ReadInput();
        var startWorkflow = workflows.First(w => w.Label == STARTING_WORKFLOW);

        var ranges = GetFittingRanges(startWorkflow, workflows);
        var possibleCombinations = ranges.Sum(range => range.CombinationCount);
        Console.WriteLine(possibleCombinations);
    }

    private static List<RatingRange> GetFittingRanges(Workflow workflow, List<Workflow> workflows)
    {
        List<RatingRange> ranges = new();

        RatingRange workflowRange = new(
            new(_minimumRating, _minimumRating, _minimumRating, _minimumRating),
            new(_maximumRating, _maximumRating, _maximumRating, _maximumRating));

        foreach (var rule in workflow.Rules)
        {
            var nextRanges = GetNextRanges(rule, workflows);

            foreach (var range in nextRanges)
            {
                if (rule.Category is not null)
                {
                    ModifyRangeToFitRule(rule, range);
                }

                LimitRangeToWorkflowRange(range, workflowRange);

                if (range.IsValid)
                {
                    ranges.Add(range);
                }
            }

            UpdateWorkflowRangeToFitRule(rule, workflowRange);
        }

        return ranges;

        static void LimitRangeToWorkflowRange(RatingRange range, RatingRange workflowRange)
        {
            range.Start.X = Math.Max(range.Start.X, workflowRange.Start.X);
            range.Start.M = Math.Max(range.Start.M, workflowRange.Start.M);
            range.Start.A = Math.Max(range.Start.A, workflowRange.Start.A);
            range.Start.S = Math.Max(range.Start.S, workflowRange.Start.S);

            range.End.X = Math.Min(range.End.X, workflowRange.End.X);
            range.End.M = Math.Min(range.End.M, workflowRange.End.M);
            range.End.A = Math.Min(range.End.A, workflowRange.End.A);
            range.End.S = Math.Min(range.End.S, workflowRange.End.S);
        }
    }

    private static void UpdateWorkflowRangeToFitRule(Rule rule, RatingRange workflowRange)
    {
        if (rule.Symbol == GREATER_THAN)
        {
            ModifyBound(rule, workflowRange.End, Math.Min, +1);
        }
        else
        {
            ModifyBound(rule, workflowRange.Start, Math.Max, -1);
        }
    }

    private static void ModifyRangeToFitRule(Rule rule, RatingRange range)
    {
        if (rule.Symbol == GREATER_THAN)
        {
            ModifyBound(rule, range.Start, Math.Max);
        }
        else
        {
            ModifyBound(rule, range.End, Math.Min);
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
            default:
                break;
        }
    }

    private static List<RatingRange> GetNextRanges(Rule rule, List<Workflow> workflows) => rule.Destination[0] switch
    {
        ACCEPTED => new()
        {
            new RatingRange(
                new Part(_minimumRating, _minimumRating, _minimumRating, _minimumRating),
                new Part(_maximumRating, _maximumRating, _maximumRating, _maximumRating))
        },
        REJECTED => new(),
        _ => GetFittingRanges(workflows.First(w => w.Label == rule.Destination), workflows)
    };

    private static char GetAcceptance(Part part, Workflow workflow, List<Workflow> workflows)
    {
        foreach (var rule in workflow.Rules)
        {
            if (rule.Condition(part))
            {
                return rule.Destination[0] switch
                {
                    ACCEPTED => ACCEPTED,
                    REJECTED => REJECTED,
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

    private record RatingRange(Part Start, Part End)
    {
        public long CombinationCount =>
            (End.X - Start.X - 1) *
            (End.M - Start.M - 1) *
            (End.A - Start.A - 1) *
            (End.S - Start.S - 1);

        public bool IsValid =>
               Start.X < End.X
            && Start.M < End.M
            && Start.A < End.A
            && Start.S < End.S;
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
