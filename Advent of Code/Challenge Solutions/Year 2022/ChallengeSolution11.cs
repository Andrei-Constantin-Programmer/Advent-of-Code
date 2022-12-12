using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution11 : ChallengeSolution
    {
        private int limit;

        public void SolveFirstPart()
        {
            var monkeys = ReadMonkeys();
            Console.WriteLine(FindMonkeyBusiness(monkeys, 20, (x) => x / 3));
        }

        public void SolveSecondPart()
        {
            limit = 1;
            var monkeys = ReadMonkeys();
            Console.WriteLine(FindMonkeyBusiness(monkeys, 10000, (x) => x % limit));
        }

        private static long FindMonkeyBusiness(List<Monkey> monkeys, int rounds, Func<long, long> modifier)
        {
            var totalItemsInspected = new long[monkeys.Count];

            for(int round = 0; round < rounds; round++)
            {
                var inspectedThisRound = PlayRoundAndReturnInspected(monkeys, modifier);

                for (int monkeyIndex = 0; monkeyIndex < monkeys.Count; monkeyIndex++)
                {
                    totalItemsInspected[monkeyIndex] += inspectedThisRound[monkeyIndex];
                }
            }

            return totalItemsInspected
                .ToList()
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((x, y) => x * y);
        }

        private static long[] PlayRoundAndReturnInspected(List<Monkey> monkeys, Func<long, long> modifier)
        {
            var itemsInspected = new List<long>();
            foreach (var monkey in monkeys)
            {
                itemsInspected.Add(monkey.Items.Count);

                for (int i = 0; i < monkey.Items.Count; i++)
                {
                    var item = monkey.Items[i];
                    item = monkey.Operation(item);
                    item = modifier(item);
                    monkeys[monkey.ThrowToMonkey(item)].Items.Add(item);
                }
                monkey.Items.Clear();
            }

            return itemsInspected.ToArray();
        }

        private List<Monkey> ReadMonkeys()
        {
            var monkeys = new List<Monkey>();

            using(TextReader read = GetInputFile(2022, 11))
            {
                string? line;

                var currentItems = new List<long>();
                Func<long, long>? currentOperation = null;
                Func<long, int>? currentTest = null;

                while (true)
                {
                    line = read.ReadLine();
                    line = line?.Trim();

                    if(line == null || line.Length == 0)
                    {
                        monkeys.Add(new Monkey(currentItems, currentOperation!, currentTest!));
                        currentItems = new List<long>();
                        currentOperation = null;
                        currentTest = null;

                        if (line == null)
                            break;
                    }
                    else if(line.StartsWith("Starting items"))
                    {
                        currentItems.AddRange(line
                            .Split(":", StringSplitOptions.RemoveEmptyEntries)[1]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => Convert.ToInt64(x))
                            .ToList()
                            );
                    }
                    else if(line.StartsWith("Operation"))
                    {
                        var elements = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        var value = elements[5];

                        if (elements[4] == "+")
                            currentOperation = (old) => old + ((value == "old") ? old : Convert.ToInt32(value));
                        else
                            currentOperation = (old) => old * ((value == "old") ? old : Convert.ToInt32(value));
                    }
                    else if(line.StartsWith("Test"))
                    {
                        var trueLine = read.ReadLine()!.Trim();
                        var falseLine = read.ReadLine()!.Trim();
                        var divisibleBy = Convert.ToInt32(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());

                        var trueValue = Convert.ToInt32(trueLine!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());
                        var falseValue = Convert.ToInt32(falseLine!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());

                        limit *= divisibleBy;
                        currentTest = (value) => value % divisibleBy == 0 ? trueValue : falseValue;
                    }
                }
            }

            return monkeys;
        }

        private class Monkey
        {
            public List<long> Items { get; set; }
            public Func<long, long> Operation { get; init; }
            public Func<long, int> ThrowToMonkey { get; init; }

            public Monkey(List<long> items, Func<long, long> operation, Func<long, int> throwTest)
            {
                Items = items;
                Operation = operation;
                ThrowToMonkey = throwTest;
            }
        }
    }
}
