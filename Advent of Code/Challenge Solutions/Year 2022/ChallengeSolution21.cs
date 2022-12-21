namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution21 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            var monkeys = ReadMonkeys(false);
            Console.WriteLine(FindMonkey(monkeys, "root").Value());
        }

        protected override void SolveSecondPart()
        {
            var monkeys = ReadMonkeys(true);
            var root = FindMonkey(monkeys, "root");
            var human = FindMonkey(monkeys, "humn");
            
            var i = 0;
            for (i = 0; root.Value() != 0; i++)
            {
                if(i % 7919 == 0)
                    Console.WriteLine(i);
                human.Value = () => i;
            }

            Console.WriteLine(i);
        }

        private static List<Monkey> ReadMonkeys(bool separateRootMonkey)
        {
            var lines = File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2022, 21)).Select(line => line.Split(':', StringSplitOptions.RemoveEmptyEntries)).ToList();

            List<Monkey> monkeys = lines
                .Select(elements => new Monkey(elements[0]))
                .ToList();

            for (int i = 0; i < monkeys.Count; i++)
            {
                var equation = lines[i][1].Trim();
                if (equation.Any(c => new char[] { '+', '-', '*', '/' }.Contains(c)))
                {
                    var operands = equation.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    var leftMonkey = FindMonkey(monkeys, operands[0]);
                    var rightMonkey = FindMonkey(monkeys, operands[2]);

                    if (separateRootMonkey && monkeys[i].Name == "root")
                    {
                        monkeys[i].Value = () => leftMonkey.Value() - rightMonkey.Value();
                    }
                    else
                    {
                        monkeys[i].Value = operands[1] switch
                        {
                            "+" => () => leftMonkey.Value() + rightMonkey.Value(),
                            "-" => () => leftMonkey.Value() - rightMonkey.Value(),
                            "*" => () => leftMonkey.Value() * rightMonkey.Value(),
                            "/" => () => leftMonkey.Value() / rightMonkey.Value(),

                            _ => throw new ArgumentException("Invalid operator")
                        };
                    }
                }
                else
                {
                    monkeys[i].Value = () => Convert.ToInt32(equation);
                }
            }

            return monkeys;
        }

        private static Monkey FindMonkey(IEnumerable<Monkey> monkeys, string name)
        {
            var monkey = monkeys.FirstOrDefault(monkey => monkey.Name == name);

            if (monkey == null)
                throw new ArgumentException("Inexistant monkey");

            return monkey!;
        }

        private class Monkey
        {
            public string Name { get; }
            public Func<long> Value { get; set; }

            public Monkey(string name)
            {
                Name = name;
                Value = () => 0;
            }
        }
    }
}
