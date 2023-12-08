// Task: https://adventofcode.com/2021/day/14

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution14 : ChallengeSolution
    {
        private Dictionary<string, char> insertionRules;
        private Dictionary<char, long> appearances;
        private Dictionary<string, long> pairAppearances;
        private List<char> polymer;

        protected override void SolveFirstPart()
        {
            ReadInput();
            for(int step=0; step<10; step++)
            {
                for(int i=0; i<polymer.Count-1; i++)
                {
                    string pair = polymer[i] + "" + polymer[i + 1];
                    if(insertionRules.ContainsKey(pair))
                    {
                        char c = insertionRules[pair];
                        polymer.Insert(i+1, c);
                        AddAppearance(c, 1);
                        
                        i++;
                    }
                }
            }

            var sorted = (from entry in appearances orderby entry.Value ascending select entry).ToList();

            Console.WriteLine(sorted[sorted.Count-1].Value - sorted[0].Value);
        }

        protected override void SolveSecondPart()
        {
            ReadInput();
            for(int step=0; step<40; step++)
            {
                var newPairAppearances = new Dictionary<string, long>(pairAppearances);

                foreach(var pairApp in pairAppearances)
                {
                    if (insertionRules.ContainsKey(pairApp.Key))
                    {
                        char c = insertionRules[pairApp.Key];
                        string newPair1 = pairApp.Key[0] + "" + c;
                        string newPair2 = c + "" + pairApp.Key[1];
                        
                        newPairAppearances[pairApp.Key]-=pairApp.Value;
                        AddPair(newPairAppearances, newPair1, pairApp.Value);
                        AddPair(newPairAppearances, newPair2, pairApp.Value);
                        AddAppearance(c, pairApp.Value);
                    }
                }

                foreach (var pairApp in newPairAppearances)
                    if (pairApp.Value <= 0)
                        RemovePair(newPairAppearances, pairApp.Key);

                pairAppearances = newPairAppearances;
            }
            
            var sorted = (from entry in appearances orderby entry.Value ascending select entry).ToList();
            Console.WriteLine(sorted[sorted.Count - 1].Value - sorted[0].Value);
        }

        private void AddAppearance(char c, long value)
        {
            if (appearances.ContainsKey(c))
                appearances[c]+=value;
            else
                appearances.Add(c, value);
        }

        private void AddPair(Dictionary<string, long> appearances, string pair, long value)
        {
            if (appearances.ContainsKey(pair))
                appearances[pair]+=value;
            else
                appearances.Add(pair, value);
        }

        private void RemovePair(Dictionary<string, long> appearances, string pair)
        {
            if (appearances.ContainsKey(pair))
                appearances.Remove(pair);
        }

        private void ReadInput()
        {
            insertionRules = new Dictionary<string, char>();
            appearances = new Dictionary<char, long>();
            pairAppearances = new Dictionary<string, long>();
            var lines = Reader.ReadLines(this);
            polymer = new List<char>(lines[0].ToCharArray());
            foreach (var c in polymer)
            {
                if (appearances.ContainsKey(c))
                    appearances[c]++;
                else
                    appearances.Add(c, 1);
            }

            for (int i = 0; i < polymer.Count - 1; i++)
            {
                string pair = polymer[i] + "" + polymer[i + 1];
                AddPair(pairAppearances, pair, 1);
            }

            foreach (var line in lines[2..])
            {
                string[] result = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
                insertionRules.Add(result[0], result[1][0]);
            }
        }
    }
}
