namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution01 : ChallengeSolution
    {
        private static int[] a;

        public ChallengeSolution01()
        {
            string[] lines = File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2020, 1));
            a = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
                a[i] = Convert.ToInt32(lines[i]);
        }

        public void SolveFirstPart()
        {
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = i; j < a.Length; j++)
                {
                    if (a[i] + a[j] == 2020)
                        Console.WriteLine(a[i] * a[j]);
                }
            }
        }

        public void SolveSecondPart()
        {
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = i; j < a.Length; j++)
                { 
                    for (int l = j; l < a.Length; l++)
                    {
                        if (a[i] + a[j] + a[l] == 2020)
                            Console.WriteLine(a[i] * a[j] * a[l]);
                    }
                }
            }
        }
    }
}
