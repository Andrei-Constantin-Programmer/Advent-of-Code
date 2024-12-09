// Task: https://adventofcode.com/2024/day/9

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2024;

public class ChallengeSolution09(IConsole console, ISolutionReader<ChallengeSolution09> reader)
    : ChallengeSolution<ChallengeSolution09>(console, reader)
{
    private const int FREE_SPACE = -1;

    public override void SolveFirstPart()
    {
        var diskMap = ReadDiskMap();
        var blocks = GenerateBlocks(diskMap);

        CompactBlocks(blocks);

        long checksum = 0;
        for (var blockId = 0; blocks[blockId] != FREE_SPACE; blockId++)
        {
            checksum += blockId * (long)blocks[blockId];
        }

        _console.WriteLine($"Checksum: {checksum}");
    }

    private static void CompactBlocks(int[] blocks)
    {
        var leftIndex = 0;
        var rightIndex = blocks.Length - 1;

        while (leftIndex < rightIndex)
        {
            if (blocks[leftIndex] == FREE_SPACE)
            {
                while (blocks[rightIndex] == FREE_SPACE)
                {
                    rightIndex--;
                }

                blocks[leftIndex] = blocks[rightIndex];
                blocks[rightIndex--] = FREE_SPACE;
            }

            leftIndex++;
        }
    }

    private static int[] GenerateBlocks(byte[] diskMap)
    {
        var blocks = Enumerable
                    .Repeat(FREE_SPACE, diskMap.Length * 9)
                    .ToArray();

        var id = 0;
        var blockId = 0;

        for (var diskMapIndex = 0; diskMapIndex < diskMap.Length; diskMapIndex++)
        {
            if (diskMapIndex % 2 == 0)
            {
                for (var i = 0; i < diskMap[diskMapIndex]; i++)
                {
                    blocks[blockId++] = id;
                }

                id++;
            }
            else
            {
                blockId += diskMap[diskMapIndex];
            }
        }

        return blocks;
    }

    public override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private byte[] ReadDiskMap()
    {
        var lines = _reader.ReadLines();
        return lines[0]
            .Select(c => (byte)(c - '0'))
            .ToArray();
    }
}
