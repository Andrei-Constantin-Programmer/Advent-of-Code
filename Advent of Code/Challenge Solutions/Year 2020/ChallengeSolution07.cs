﻿// Task: https://adventofcode.com/2020/day/7

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020;

public class ChallengeSolution07(IConsole console, ISolutionReader<ChallengeSolution07> reader)
    : ChallengeSolution<ChallengeSolution07>(console, reader)
{
    public override void SolveFirstPart()
    {
        new FirstPart(_console).Solution(_reader.ReadLines());
    }

    public override void SolveSecondPart()
    {
        new SecondPart(_console).Solution(_reader.ReadLines());
    }

    class FirstPart(IConsole console)
    {
        private static List<Bag> allBags;
        private static List<Bag> validBags;
        private static string sg = "shiny gold";

        public void Solution(string[] rules)
        {
            validBags = new List<Bag>();
            allBags = new List<Bag>();
            foreach (string rule in rules)
            {
                string[] codes = rule.Split(new string[] { "contain" }, StringSplitOptions.RemoveEmptyEntries);
                Bag bag = new Bag(GetBagName(codes[0]), codes[1]);
                allBags.Add(bag);
            }
            foreach (Bag bag in allBags)
            {
                FillBag(bag);
            }

            foreach (Bag bag in allBags)
            {
                if (bag.HasChild(sg))
                    validBags.Add(bag);
            }

            console.WriteLine(validBags.Count);
        }

        private static Bag? FindBag(string bagName)
        {
            for (int i = 0; i < allBags.Count; i++)
                if (allBags[i].Name.Equals(bagName))
                    return allBags[i];
            return null;
        }

        private static void FillBag(Bag bag)
        {
            string[] children = bag.Rule.Split(new string[] { ".", "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string child in children)
            {
                string childName = GetChildName(child);
                Bag? childBag = FindBag(childName);
                if (childBag != null)
                    bag.AddChildBag(childBag);
            }
        }

        private static string GetChildName(string child)
        {
            if (child.Contains("no other"))
                return "";
            string name = child.Substring(2, child.IndexOf("bag") - 3);
            return name.Trim();
        }

        private static string GetBagName(string code)
        {
            string name = code.Substring(0, code.IndexOf("bags") - 1);
            return name.Trim();
        }


        class Bag
        {
            public string Name { get; set; }
            private List<Bag> ChildBags;
            public string Rule { get; set; }


            public Bag(string name, string rule)
            {
                Name = name;
                Rule = rule;
                ChildBags = new List<Bag>();
            }

            public void AddChildBag(Bag bag)
            {
                ChildBags.Add(bag);
            }

            public bool HasChild(string name)
            {
                bool found = false;
                foreach (Bag child in ChildBags)
                {
                    if (child.Name.Equals(name))
                        found = true;
                    else
                        if (child.HasChild(name))
                        found = true;
                }

                return found;
            }
        }
    }

    class SecondPart(IConsole console)
    {
        private static List<Bag> allBags;
        private static string sg = "shiny gold";

        public void Solution(string[] rules)
        {
            allBags = new List<Bag>();
            foreach (string rule in rules)
            {
                string[] codes = rule.Split(new string[] { "contain" }, StringSplitOptions.RemoveEmptyEntries);
                Bag bag = new Bag(GetBagName(codes[0]), codes[1]);
                allBags.Add(bag);
            }
            foreach (Bag bag in allBags)
            {
                FillBag(bag);
            }

            Bag? sgBag = FindBag(sg);

            console.WriteLine(sgBag!.SumOfBags());
        }

        private static Bag? FindBag(string bagName)
        {
            for (int i = 0; i < allBags.Count; i++)
                if (allBags[i].Name.Equals(bagName))
                    return allBags[i];
            return null;
        }

        private static void FillBag(Bag bag)
        {
            string[] children = bag.Rule.Split(new string[] { ".", "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string child in children)
            {
                int childQuantity = GetChildQuantity(child);
                string childName = GetChildName(child);
                Bag? childBag = FindBag(childName);
                if (childBag != null)
                    bag.AddChildBag(childBag, childQuantity);
            }
        }

        private static int GetChildQuantity(string child)
        {
            if (child.Contains("no other"))
                return 0;
            string number = child.Substring(1, 2);
            return Convert.ToInt32(number);
        }

        private static string GetChildName(string child)
        {
            if (child.Contains("no other"))
                return "";
            string name = child.Substring(2, child.IndexOf("bag") - 3);
            return name.Trim();
        }

        private static string GetBagName(string code)
        {
            string name = code.Substring(0, code.IndexOf("bags") - 1);
            return name.Trim();
        }

        class Bag
        {
            public string Name { get; set; }
            private Dictionary<Bag, int> ChildBags;
            public string Rule { get; set; }


            public Bag(string name, string rule)
            {
                Name = name;
                Rule = rule;
                ChildBags = new Dictionary<Bag, int>();
            }

            public void AddChildBag(Bag bag, int quantity)
            {
                ChildBags.Add(bag, quantity);
            }

            public Dictionary<Bag, int> GetChildren()
            {
                return ChildBags;
            }

            public int SumOfBags()
            {
                int s = 0;
                List<Bag> childList = new List<Bag>(ChildBags.Keys);

                foreach (Bag bag in childList)
                {
                    s += ChildBags[bag] * (bag.SumOfBags() + 1);
                }

                return s;
            }
        }
    }
}
