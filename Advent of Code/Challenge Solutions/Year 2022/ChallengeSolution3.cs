using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution3 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            Console.WriteLine(ReadRucksacks()
                .Select(rucksack => GetItemTypePriority(GetCommonItemType(rucksack)))
                .Sum()
                );
        }

        public void SolveSecondPart()
        {
            int sum = 0;
            var rucksacks = ReadRucksacks();
            for(int i = 0; i < rucksacks.Length; i+=3)
            {
                sum += GetItemTypePriority(GetCommonItemTypePerGroup(rucksacks[i], rucksacks[i + 1], rucksacks[i + 2]));
            }

            Console.WriteLine(sum);
        }

        private static string[] ReadRucksacks()
        {
            var rucksackList = new List<string>();

            using (TextReader read = GetInputFile(2022, 3))
            {
                string? line;
                while ((line = read.ReadLine()) != null)
                {
                    rucksackList.Add(line);
                }
            }

            return rucksackList.ToArray();
        }

        private static char GetCommonItemType(string rucksack)
        {
            var firstCompartment = rucksack.Substring(0, rucksack.Length / 2);
            var secondCompartment = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);

            return firstCompartment.First(c => secondCompartment.Contains(c));
        }

        private static char GetCommonItemTypePerGroup(string firstRucksack, string secondRucksack, string thirdRucksack)
        {
            return firstRucksack.First(c => secondRucksack.Contains(c) && thirdRucksack.Contains(c));
        }

        private static int GetItemTypePriority(char itemType)
        {
            if (Char.IsLower(itemType))
                return itemType - 'a' + 1;
            if (Char.IsUpper(itemType))
                return itemType - 'A' + 27;

            return 0;
        }
    }
}
