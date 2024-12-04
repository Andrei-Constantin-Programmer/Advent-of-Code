// Task: https://adventofcode.com/2022/day/21

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution21(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        var monkeys = ReadMonkeys(false);
        var root = FindMonkey(monkeys, "root");

        //Print(root, 0);

        Console.WriteLine(root.Value);
    }

    public override void SolveSecondPart()
    {
        var monkeys = ReadMonkeys(true);
        var root = FindMonkey(monkeys, "root");
        var human = FindMonkey(monkeys, "humn");

        Console.WriteLine(FindHumanCall(root, human));
    }

    private static void Print(Monkey monkey, int spaces)
    {
        for (int i = 0; i < spaces; i++)
            Console.Write(" ");
        Console.WriteLine(monkey.Name + " " + monkey.Value);

        if (monkey.LeftMonkey != null)
            Print(monkey.LeftMonkey, spaces + 1);
        if (monkey.RightMonkey != null)
            Print(monkey.RightMonkey, spaces + 1);
    }

    private static long FindHumanCall(Monkey root, Monkey human)
    {
        return FindHumanCall(root, human, 0, true);
    }

    private static long FindHumanCall(Monkey? currentMonkey, Monkey human, long valueToReach, bool isRoot)
    {
        if (currentMonkey == human)
        {
            return valueToReach;
        }
        if (currentMonkey == null || currentMonkey!.LeftMonkey == null || currentMonkey!.RightMonkey == null)
        {
            return 0;
        }

        var leftSideNeedsHuman = currentMonkey.LeftMonkey.DoesCallMonkey(human);
        var rightSideNeedsHuman = currentMonkey.RightMonkey.DoesCallMonkey(human);

        if (leftSideNeedsHuman)
        {
            var rightSideValue = currentMonkey.RightMonkey.Value;
            var leftOperand = CalculateLeftOperand(currentMonkey.Operator, valueToReach, rightSideValue);
            var newValueToReach = isRoot ? rightSideValue : leftOperand;

            return FindHumanCall(currentMonkey.LeftMonkey, human, newValueToReach, false);
        }
        if (rightSideNeedsHuman)
        {
            var leftSideValue = currentMonkey.LeftMonkey.Value;
            var rightOperand = CalculateRightOperand(currentMonkey.Operator, valueToReach, leftSideValue);
            var newValueToReach = isRoot ? leftSideValue : rightOperand;

            return FindHumanCall(currentMonkey.RightMonkey, human, newValueToReach, false);
        }

        throw new Exception("Human not found");
    }

    private List<Monkey> ReadMonkeys(bool separateRootMonkey)
    {
        var splitLines = Reader.ReadLines(this)
            .Select(line => line.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .ToList();

        List<Monkey> monkeys = splitLines
            .Select(elements => new Monkey(elements[0]))
            .ToList();

        for (var i = 0; i < monkeys.Count; i++)
        {
            var equation = splitLines[i][1].Trim();

            if (equation.Any(c => new char[] { '+', '-', '*', '/' }.Contains(c)))
            {
                var operands = equation.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                monkeys[i].LeftMonkey = FindMonkey(monkeys, operands[0]);
                monkeys[i].RightMonkey = FindMonkey(monkeys, operands[2]);

                if (separateRootMonkey && monkeys[i].Name == "root")
                {
                    monkeys[i].Operator = '-';
                }
                else
                {
                    monkeys[i].Operator = operands[1][0];
                }
            }
            else
            {
                monkeys[i].Operation = (_, _) => Convert.ToInt64(equation);
            }
        }

        return monkeys;
    }

    private static Monkey FindMonkey(IEnumerable<Monkey> monkeys, string name)
    {
        var monkey = monkeys.FirstOrDefault(monkey => monkey.Name == name);

        if (monkey == null)
            throw new ArgumentException($"Monkey {name} does not exist.");

        return monkey!;
    }

    private class Monkey
    {
        public string Name { get; }
        public Monkey? LeftMonkey { get; set; }
        public Monkey? RightMonkey { get; set; }

        public Func<long, long, long> Operation { get; set; }

        private char _operator;
        public char Operator
        {
            get => _operator;
            set
            {
                _operator = value;
                Operation = ChallengeSolution21.OperationFromOperator(_operator);
            }
        }
        public long Value => Operation(
            LeftMonkey == null ? 0 : LeftMonkey.Value,
            RightMonkey == null ? 0 : RightMonkey.Value);

        public Monkey(string name)
        {
            Name = name;
            Operation = (LeftMonkey, RightMonkey) => 0;
        }

        public bool DoesCallMonkey(Monkey monkey)
        {
            if (this == monkey)
                return true;
            if (LeftMonkey == monkey || RightMonkey == monkey)
                return true;
            if (LeftMonkey != null && LeftMonkey.DoesCallMonkey(monkey))
                return true;
            if (RightMonkey != null && RightMonkey.DoesCallMonkey(monkey))
                return true;

            return false;
        }
    }

    private static long CalculateLeftOperand(char op, long originalResult, long rightOperand) => op switch
    {
        '+' => OperationFromOperator('-')(originalResult, rightOperand),
        '-' => OperationFromOperator('+')(originalResult, rightOperand),
        '*' => OperationFromOperator('/')(originalResult, rightOperand),
        '/' => OperationFromOperator('*')(originalResult, rightOperand),

        _ => throw new ArgumentException("Invalid operator")
    };

    private static long CalculateRightOperand(char op, long originalResult, long leftOperand) => op switch
    {
        '+' => OperationFromOperator('-')(originalResult, leftOperand),
        '-' => OperationFromOperator('-')(leftOperand, originalResult),
        '*' => OperationFromOperator('/')(originalResult, leftOperand),
        '/' => OperationFromOperator('/')(leftOperand, originalResult),

        _ => throw new ArgumentException("Invalid operator")
    };

    private static Func<long, long, long> OperationFromOperator(char op) => op switch
    {
        '+' => (left, right) => left + right,
        '-' => (left, right) => left - right,
        '*' => (left, right) => left * right,
        '/' => (left, right) => left / right,

        _ => throw new ArgumentException("Invalid operator")
    };
}
