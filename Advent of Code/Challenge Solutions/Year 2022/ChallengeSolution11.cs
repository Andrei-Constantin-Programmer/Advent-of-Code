using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution11 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var monkeys = ReadMonkeys();

            Console.WriteLine(FindMonkeyBusiness(monkeys));
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private static int FindMonkeyBusiness(List<Monkey> monkeys)
        {
            var totalItemsInspected = new int[monkeys.Count];

            for(int round = 0; round < 20; round++)
            {
                var inspectedThisRound = PlayRoundAndReturnInspected(monkeys);

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

        private static int[] PlayRoundAndReturnInspected(List<Monkey> monkeys)
        {
            var itemsInspected = new List<int>();
            int x = 0;
            foreach (var monkey in monkeys)
            {
                itemsInspected.Add(monkey.Items.Count);

                for (int i = 0; i < monkey.Items.Count; i++)
                {
                    var item = monkey.Items[i];
                    item = monkey.Operation(item);
                    item /= 3;
                    monkeys[monkey.ThrowToMonkey(item)].Items.Add(item);
                }
                monkey.Items.Clear();

                x++;
            }

            return itemsInspected.ToArray();
        }

        private static List<Monkey> ReadMonkeys()
        {
            var monkeys = new List<Monkey>();

            using(TextReader read = GetInputFile(2022, 11))
            {
                string? line;

                var currentItems = new List<int>();
                Func<int, int>? currentOperation = null;
                Func<int, int>? currentTest = null;

                while (true)
                {
                    line = read.ReadLine();

                    if(line == null || line.Trim().Length == 0)
                    {
                        monkeys.Add(new Monkey(currentItems, currentOperation!, currentTest!));
                        currentItems = new List<int>();
                        currentOperation = null;
                        currentTest = null;

                        if (line == null)
                            break;
                    }
                    else if(line.Trim().StartsWith("Starting items"))
                    {
                        currentItems.AddRange(line
                            .Split(":", StringSplitOptions.RemoveEmptyEntries)[1]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => Convert.ToInt32(x))
                            .ToList()
                            );
                    }
                    else if(line.Trim().StartsWith("Operation"))
                    {
                        var elements = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        var value = elements[5];

                        if (elements[4] == "+")
                            currentOperation = (old) => old + ((value == "old") ? old : Convert.ToInt32(value));
                        else
                            currentOperation = (old) => old * ((value == "old") ? old : Convert.ToInt32(value));
                    }
                    else if(line.Trim().StartsWith("Test"))
                    {
                        var trueLine = read.ReadLine()!.Trim();
                        var falseLine = read.ReadLine()!.Trim();
                        var divisibleBy = Convert.ToInt32(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());
                        var trueValue = Convert.ToInt32(trueLine!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());
                        var falseValue = Convert.ToInt32(falseLine!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());

                        currentTest = (value) => value % divisibleBy == 0 ? trueValue : falseValue;
                    }
                }
            }

            return monkeys;
        }

        private class Monkey
        {
            public List<int> Items { get; set; }
            public Func<int, int> Operation { get; init; }
            public Func<int, int> ThrowToMonkey { get; init; }

            public Monkey(List<int> items, Func<int, int> operation, Func<int, int> throwTest)
            {
                Items = items;
                Operation = operation;
                ThrowToMonkey = throwTest;
            }
        }
    }
}
