// Task: https://adventofcode.com/2022/day/6

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution06 : ChallengeSolution
    {
        protected override void SolveFirstPart()
        {
            var buffer = ReadDatastreamBuffer();

            Console.WriteLine(FindFirstUniqueBufferPosition(buffer, 4));
        }

        protected override void SolveSecondPart()
        {
            var buffer = ReadDatastreamBuffer();

            Console.WriteLine(FindFirstUniqueBufferPosition(buffer, 14));
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

        private static string ReadDatastreamBuffer()
        {
            return String.Join("", File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2022, 6)));
        }
    }
}
