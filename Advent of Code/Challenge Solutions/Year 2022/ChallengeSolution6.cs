using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution6 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var buffer = ReadDatastreamBuffer();

            Console.WriteLine(FindFirstUniqueBufferPosition(buffer, 4));
        }

        public void SolveSecondPart()
        {
            var buffer = ReadDatastreamBuffer();

            Console.WriteLine(FindFirstUniqueBufferPosition(buffer, 14));
        }

        private int FindFirstUniqueBufferPosition(string buffer, int bufferSize)
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

        private bool ContainsDuplicateCharacter(char[] array)
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
            return String.Join("", File.ReadAllLines(GetFileString(FileType.Input, 2022, 6)));
        }
    }
}
