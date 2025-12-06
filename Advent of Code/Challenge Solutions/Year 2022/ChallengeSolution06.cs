// Task: https://adventofcode.com/2022/day/6

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution06(IConsole console, ISolutionReader<ChallengeSolution06> reader)
    : ChallengeSolution<ChallengeSolution06>(console, reader)
{
    public override void SolveFirstPart()
    {
        var buffer = ReadDatastreamBuffer();

        _console.WriteLine(FindFirstUniqueBufferPosition(buffer, 4));
    }

    public override void SolveSecondPart()
    {
        var buffer = ReadDatastreamBuffer();

        _console.WriteLine(FindFirstUniqueBufferPosition(buffer, 14));
    }

    private static int FindFirstUniqueBufferPosition(string buffer, int bufferSize)
    {
        char[] recent = new char[bufferSize];

        recent = buffer.Substring(0, bufferSize).ToCharArray();
        int count = bufferSize;
        while (ContainsDuplicateCharacter(recent))
        {
            for (int i = 0; i < bufferSize - 1; i++)
            {
                recent[i] = recent[i + 1];
            }

            recent[bufferSize - 1] = buffer[count];
            count++;
        }

        return count;
    }

    private static bool ContainsDuplicateCharacter(char[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (array[i] == array[j])
                {
                    return true;
                }
            }
        }

        return false;
    }

    private string ReadDatastreamBuffer()
    {
        return String.Join(Environment.NewLine, _reader.ReadLines());
    }
}
