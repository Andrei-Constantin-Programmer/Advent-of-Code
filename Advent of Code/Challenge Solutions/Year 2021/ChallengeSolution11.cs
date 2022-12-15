﻿// Task: https://adventofcode.com/2021/day/11

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution11 : ChallengeSolution
    {
        private const int n = 10;
        private int[,]? energy;
        private int[,] originalEnergy;

        public ChallengeSolution11()
        {
            originalEnergy = new int[n, n];
            using (TextReader read = Reader.GetInputFile(2021, 11))
            {
                for (int i = 0; i < n; i++)
                {
                    int[] energyLevels = Array.ConvertAll(read.ReadLine().ToCharArray(), character => (int)Char.GetNumericValue(character));
                    for (int j = 0; j < n; j++)
                        originalEnergy[i, j] = energyLevels[j];
                }
            }
        }

        public void SolveFirstPart()
        {
            energy = (int[,])originalEnergy.Clone();
            int flashes = 0;
            for(int step=0; step<100; step++)
            {
                var check = new bool[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!check[i, j] && energy[i, j]!=-1)
                        {
                            energy[i, j]++;
                            check[i, j] = true;
                        }
                        if (energy[i, j] > 9)
                        {
                            flashes++;
                            energy[i, j] = -1;
                            Flash(i, j);

                            i = -1;
                            break;
                        }
                    }
                }

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        energy[i, j] = energy[i, j]==-1?0:energy[i, j];
            }

            Console.WriteLine(flashes);
        }

        public void SolveSecondPart()
        {
            energy = (int[,])originalEnergy.Clone();
            int step = 0;
            bool allFlashed = false;
            while (!allFlashed)
            {
                step++;
                var check = new bool[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!check[i, j] && energy[i, j] != -1)
                        {
                            energy[i, j]++;
                            check[i, j] = true;
                        }
                        if (energy[i, j] > 9)
                        {
                            energy[i, j] = -1;
                            Flash(i, j);

                            i = -1;
                            break;
                        }
                    }
                }


                int flashes = 0;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        if (energy[i, j] == -1)
                        {
                            flashes++;
                            energy[i, j] = 0;
                        }
                    }

                if (flashes == n * n)
                {
                    allFlashed = true;
                }
            }

            Console.WriteLine(step);
        }

        private void Flash(int i, int j)
        {
            if (i - 1 >= 0 && j - 1 >= 0 && energy[i-1, j-1]!=-1)
                energy[i-1, j-1]++;
            if (i - 1 >= 0 && j + 1 < n && energy[i - 1, j + 1] != -1)
                energy[i - 1, j + 1]++;
            if (i + 1 < n && j - 1 >= 0 && energy[i + 1, j - 1] != -1)
                energy[i + 1, j - 1]++;
            if (i + 1 < n && j + 1 < n && energy[i + 1, j + 1] != -1)
                energy[i + 1, j + 1]++;
            if (i - 1 >= 0 && energy[i - 1, j] != -1)
                energy[i - 1, j]++;
            if (i + 1 < n && energy[i + 1, j] != -1)
                energy[i + 1, j]++;
            if (j - 1 >= 0 && energy[i, j - 1] != -1)
                energy[i, j-1]++;
            if (j + 1 < n && energy[i, j + 1] != -1)
                energy[i, j + 1]++;
        }
    }
}
