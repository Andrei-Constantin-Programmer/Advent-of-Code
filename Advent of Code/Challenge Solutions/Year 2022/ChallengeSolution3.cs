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
            throw new NotImplementedException();
        }

        private string[] ReadRucksacks()
        {
            var rucksackList = new List<string>();

            using (TextReader read = Utilities.GetInputFile(2022, 3))
            {
                string? line;
                while ((line = read.ReadLine()) != null)
                {
                    rucksackList.Add(line);
                }
            }

            return rucksackList.ToArray();
        }


        private char GetCommonItemType(string rucksack)
        {
            var firstCompartment = rucksack.Substring(0, rucksack.Length / 2);
            var secondCompartment = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);

            return firstCompartment.First(c => secondCompartment.Contains(c));
        }

        private int GetItemTypePriority(char itemType)
        {
            if (Char.IsLower(itemType))
                return itemType - 'a' + 1;
            if (Char.IsUpper(itemType))
                return itemType - 'A' + 27;

            return 0;
        }
    }
}
