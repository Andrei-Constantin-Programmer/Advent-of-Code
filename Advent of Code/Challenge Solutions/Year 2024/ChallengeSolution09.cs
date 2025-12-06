// Task: https://adventofcode.com/2024/day/9

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

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
        var checksum = ComputeChecksum(blocks);

        Console.WriteLine($"Checksum: {checksum}");
    }

    public override void SolveSecondPart()
    {
        var diskMap = ReadDiskMap();
        var blocks = GenerateBlocks(diskMap);

        CompactBlocksByFile(blocks, out var lastMemoryBlockEnd);
        var checksum = ComputeChecksum(blocks, lastMemoryBlockEnd + 1);

        Console.WriteLine($"Checksum: {checksum}");
    }

    private static long ComputeChecksum(int[] blocks, int end = 0)
    {
        long checksum = 0;

        if (end == 0)
        {
            end = Array.FindIndex(blocks, block => block == FREE_SPACE);
        }

        for (var blockId = 0; blockId < end; blockId++)
        {
            if (blocks[blockId] == FREE_SPACE)
            {
                continue;
            }

            checksum += blockId * (long)blocks[blockId];
        }

        return checksum;
    }

    private static void CompactBlocksByFile(int[] blocks, out int lastMemoryBlockEnd)
    {
        var rightIndex = FindNextMemoryBlock(blocks, blocks.Length - 1);
        lastMemoryBlockEnd = rightIndex;

        while (blocks[rightIndex] != 0)
        {
            var memoryBlockEnd = ComputeMemoryBlockEnd(blocks, rightIndex);

            if (!TryCompactBlockAsFile(blocks, ref rightIndex, memoryBlockEnd))
            {
                rightIndex = memoryBlockEnd;
            }

            rightIndex = FindNextMemoryBlock(blocks, rightIndex);
        }
    }

    private static bool TryCompactBlockAsFile(int[] blocks, ref int rightIndex, int memoryBlockEnd)
    {
        for (var leftIndex = 0; leftIndex < memoryBlockEnd;)
        {
            if (blocks[leftIndex] != FREE_SPACE)
            {
                leftIndex++;
                continue;
            }

            var freeBlockEnd = ComputeFreeBlockEnd(blocks, leftIndex);

            if (CanMoveBlockAsFile(freeBlockEnd, leftIndex, rightIndex, memoryBlockEnd))
            {
                MoveBlockAsFile(blocks, ref rightIndex, memoryBlockEnd, leftIndex);
                return true;
            }

            leftIndex = freeBlockEnd;
        }

        return false;
    }

    private static bool CanMoveBlockAsFile(int freeBlockEnd, int leftIndex, int rightIndex, int memoryBlockEnd)
        => freeBlockEnd - leftIndex >= rightIndex - memoryBlockEnd;

    private static void MoveBlockAsFile(int[] blocks, ref int rightIndex, int memoryBlockEnd, int leftIndex)
    {
        for (var left = leftIndex; rightIndex > memoryBlockEnd; left++, rightIndex--)
        {
            blocks[left] = blocks[rightIndex];
            blocks[rightIndex] = FREE_SPACE;
        }
    }

    private static void CompactBlocks(int[] blocks)
    {
        var leftIndex = 0;
        var rightIndex = blocks.Length - 1;

        while (leftIndex < rightIndex)
        {
            if (blocks[leftIndex] == FREE_SPACE)
            {
                rightIndex = FindNextMemoryBlock(blocks, rightIndex);

                blocks[leftIndex] = blocks[rightIndex];
                blocks[rightIndex--] = FREE_SPACE;
            }

            leftIndex++;
        }
    }

    private static int FindNextMemoryBlock(int[] blocks, int rightIndex)
    {
        while (blocks[rightIndex] == FREE_SPACE)
        {
            rightIndex--;
        }

        return rightIndex;
    }

    private static int ComputeMemoryBlockEnd(int[] blocks, int rightIndex)
    {
        var memoryBlockEnd = rightIndex;
        while (blocks[memoryBlockEnd] == blocks[rightIndex])
        {
            memoryBlockEnd--;
        }

        return memoryBlockEnd;
    }

    private static int ComputeFreeBlockEnd(int[] blocks, int leftIndex)
    {
        var freeBlockEnd = leftIndex;
        while (blocks[freeBlockEnd] == FREE_SPACE)
        {
            freeBlockEnd++;
        }

        return freeBlockEnd;
    }

    private static int[] GenerateBlocks(byte[] diskMap)
    {
        var blocks = Enumerable
            .Repeat(FREE_SPACE, diskMap.Length * 9)
            .ToArray();

        var currentBlockId = 0;
        var blockIndex = 0;

        for (var diskMapIndex = 0; diskMapIndex < diskMap.Length; diskMapIndex++)
        {
            var blockLength = diskMap[diskMapIndex];

            if (diskMapIndex % 2 == 0)
            {
                FillBlock(blocks, blockIndex, blockLength, currentBlockId);
                currentBlockId++;
            }

            blockIndex += blockLength;
        }

        return blocks;
    }

    private static void FillBlock(int[] blocks, int startIndex, int length, int blockId)
    {
        for (var i = 0; i < length; i++)
        {
            blocks[startIndex + i] = blockId;
        }
    }

    private byte[] ReadDiskMap() => Reader
        .ReadLines()[0]
        .Select(c => (byte)(c - '0'))
        .ToArray();
}
