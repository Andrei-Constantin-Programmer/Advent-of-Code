﻿// Task: https://adventofcode.com/2022/day/3

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution03(IConsole console, ISolutionReader<ChallengeSolution03> reader)
    : ChallengeSolution<ChallengeSolution03>(console, reader)
{
    public override void SolveFirstPart()
    {
        _console.WriteLine(ReadRucksacks()
            .Select(rucksack => GetItemTypePriority(GetCommonItemType(rucksack)))
            .Sum()
            );
    }

    public override void SolveSecondPart()
    {
        int sum = 0;
        var rucksacks = ReadRucksacks();
        for (int i = 0; i < rucksacks.Length; i += 3)
        {
            sum += GetItemTypePriority(GetCommonItemTypePerGroup(rucksacks[i], rucksacks[i + 1], rucksacks[i + 2]));
        }

        _console.WriteLine(sum);
    }

    private string[] ReadRucksacks() => _reader.ReadLines();

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
