// Task: https://adventofcode.com/2020/day/16

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution16 : ChallengeSolution
    {
        private static List<Rule> rules = new List<Rule>();

        private static Ticket myTicket = new Ticket();

        private static List<Ticket> nearbyTickets = new List<Ticket>();

        private static List<Ticket> validTickets = new List<Ticket>();

        private static Dictionary<int, string> fieldPositions = new Dictionary<int, string>();

        public ChallengeSolution16()
        {
            string[] lines = File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2020, 16));

            int nearbyTicketsPos = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Contains("or"))
                {
                    string[] ruleCode = line.Split(new string(":"), StringSplitOptions.RemoveEmptyEntries);
                    string[] intervals = ruleCode[1].Split(new string("or"), StringSplitOptions.RemoveEmptyEntries);
                    string[] firstInterval = intervals[0].Split(new string("-"), StringSplitOptions.RemoveEmptyEntries);
                    string[] secondInterval = intervals[1].Split(new string("-"), StringSplitOptions.RemoveEmptyEntries);

                    string rule = ruleCode[0];
                    int firstLower = Convert.ToInt32(firstInterval[0]);
                    int firstUpper = Convert.ToInt32(firstInterval[1]);
                    int secondLower = Convert.ToInt32(secondInterval[0]);
                    int secondUpper = Convert.ToInt32(secondInterval[1]);

                    rules.Add(new Rule(rule, firstLower, firstUpper, secondLower, secondUpper));
                }
                else if (line.Contains("your ticket"))
                {
                    string[] values = lines[i + 1].Split(new string(","), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string value in values)
                        myTicket.fields.Add(Convert.ToInt32(value));
                }
                else if (line.Contains("nearby tickets"))
                {
                    nearbyTicketsPos = i + 1;
                    break;
                }
            }

            for (int i = nearbyTicketsPos; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(new string(","), StringSplitOptions.RemoveEmptyEntries);
                Ticket ticket = new Ticket();
                foreach (string value in values)
                    ticket.fields.Add(Convert.ToInt32(value));
                nearbyTickets.Add(ticket);
            }
        }

        public void SolveFirstPart()
        {
            int sum = 0;
            foreach (Ticket ticket in nearbyTickets)
                foreach (int field in ticket.fields)
                {
                    bool valid = false;
                    foreach (Rule rule in rules)
                        if (rule.isValid(field))
                            valid = true;
                    if (!valid)
                        sum += field;
                }
            Console.WriteLine(sum);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();

            foreach (Ticket ticket in nearbyTickets)
            {
                bool validTicket = true;
                foreach (int field in ticket.fields)
                {
                    bool valid = false;
                    foreach (Rule rule in rules)
                        if (rule.isValid(field))
                            valid = true;
                    if (!valid)
                        validTicket = false;
                }
                if (validTicket)
                    validTickets.Add(ticket);
            }

            foreach (Ticket validTicket in validTickets)
            {
                foreach (int field in validTicket.fields)
                    Console.Write(field + " ");
                Console.WriteLine();
            }
        }

        class Rule
        {
            public string ruleName { get; set; }

            public int firstLower { get; set; }

            public int firstUpper { get; set; }

            public int secondLower { get; set; }

            public int secondUpper { get; set; }

            public Rule(string name, int firstLower, int firstUpper, int secondLower, int secondUpper)
            {
                ruleName = name;
                this.firstLower = firstLower;
                this.firstUpper = firstUpper;
                this.secondLower = secondLower;
                this.secondUpper = secondUpper;
            }

            public bool isValid(int x)
            {
                if (x >= firstLower && x <= firstUpper)
                    return true;
                if (x >= secondLower && x <= secondUpper)
                    return true;
                return false;
            }
        }

        class Ticket
        {
            public List<int> fields = new List<int>();
        }
    }
}
