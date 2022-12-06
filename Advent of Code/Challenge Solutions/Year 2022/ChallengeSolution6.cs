using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution6 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var buffer = ReadDatastreamBuffer();

            char[] recent = new char[4];

            recent = buffer.Substring(0, 4).ToCharArray();
            int count = 4;
            while(ContainsDuplicateCharacter(recent))
            {
                for(int i = 0; i < 3; i++)
                {
                    recent[i] = recent[i + 1];
                }

                recent[3] = buffer[count];
                count++;
            }

            Console.WriteLine(count);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
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
