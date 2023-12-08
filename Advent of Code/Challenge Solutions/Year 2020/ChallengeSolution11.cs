// Task: https://adventofcode.com/2020/day/11

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution11 : ChallengeSolution
    {
        private string[] lines;

        public ChallengeSolution11()
        {
            lines = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2020, 11));
        }

        protected override void SolveFirstPart()
        {
            new FirstPart(lines).Solution();
        }

        protected override void SolveSecondPart()
        {
            new SecondPart(lines).Solution();
        }

        class FirstPart
        {
            private char[][] seats;
            private char[][] prevSeats;
            private string[] lines;

            public FirstPart(string[] lines)
            {
                this.lines = lines;
            }

            public void Solution()
            {
                seats = new char[lines.Length][];
                prevSeats = new char[lines.Length][];
                for (int i = 0; i < seats.Length; i++)
                {
                    seats[i] = new char[lines[i].Length];
                    prevSeats[i] = new char[lines[i].Length];
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    for (int j = 0; j < line.Length; j++)
                        seats[i][j] = line[j];
                }

                while (!Compare(prevSeats, seats))
                {
                    prevSeats = CopyArray(seats);
                    seats = DoOneRound();
                }

                int nr = 0;
                for (int i = 0; i < seats.Length; i++)
                    for (int j = 0; j < seats[i].Length; j++)
                        if (seats[i][j] == '#')
                            nr++;
                Console.WriteLine(nr);
            }

            private int CountNeighbour(int i, int j, char c)
            {
                int nr = 0;

                if (i - 1 >= 0)
                {
                    if (j - 1 >= 0)
                    {
                        if (seats[i - 1][j - 1] == c)
                            nr++;
                    }
                    if (seats[i - 1][j] == c)
                        nr++;
                    if (j + 1 < seats[i].Length)
                    {
                        if (seats[i - 1][j + 1] == c)
                            nr++;
                    }
                }
                if (i + 1 < seats.Length)
                {
                    if (j - 1 >= 0)
                    {
                        if (seats[i + 1][j - 1] == c)
                            nr++;
                    }
                    if (seats[i + 1][j] == c)
                        nr++;
                    if (j + 1 < seats[i].Length)
                    {
                        if (seats[i + 1][j + 1] == c)
                            nr++;
                    }
                }

                if (j - 1 >= 0)
                {
                    if (seats[i][j - 1] == c)
                        nr++;
                }
                if (j + 1 < seats[i].Length)
                {
                    if (seats[i][j + 1] == c)
                        nr++;
                }

                return nr;
            }

            private char[][] CopyArray(char[][] source)
            {
                return source.Select(s => s.ToArray()).ToArray();
            }


            private char[][] DoOneRound()
            {
                char[][] changedSeats = new char[seats.Length][];

                changedSeats = CopyArray(seats);

                for (int i = 0; i < seats.Length; i++)
                {
                    string row = new string(seats[i]);
                    for (int j = 0; j < row.Length; j++)
                    {
                        if (row[j] == 'L')
                        {
                            /*Console.Write(i + " " + j+" ");
                            Console.WriteLine(countNeighbour(i, j, '#'));*/
                            if (CountNeighbour(i, j, '#') == 0)
                            {
                                changedSeats[i][j] = '#';
                            }
                        }
                        else if (row[j] == '#')
                        {
                            //Console.WriteLine("#");
                            if (CountNeighbour(i, j, '#') >= 4)
                                changedSeats[i][j] = 'L';
                        }
                    }
                }

                return changedSeats;
            }

            private bool Compare(char[][] first, char[][] second)
            {
                bool same = true;

                if (first.Length != second.Length)
                    return false;

                for (int i = 0; i < first.Length && same; i++)
                {
                    if (first[i].Length != second[i].Length)
                        return false;
                    for (int j = 0; j < first[i].Length && same; j++)
                    {
                        char elem2 = second[i][j];
                        char elem1 = first[i][j];
                        if (elem1 != elem2)
                            same = false;
                    }
                }

                return same;
            }
        }

        class SecondPart
        {
            private char[][] seats;
            private char[][] prevSeats;
            private string[] lines;

            public SecondPart(string[] lines)
            {
                this.lines = lines;
            }

            public void Solution()
            {
                string[] lines = File.ReadAllLines(Reader.GetFilePath(Reader.FileType.Input, 2020, 11));
                seats = new char[lines.Length][];
                prevSeats = new char[lines.Length][];
                for (int i = 0; i < seats.Length; i++)
                {
                    seats[i] = new char[lines[i].Length];
                    prevSeats[i] = new char[lines[i].Length];
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    for (int j = 0; j < line.Length; j++)
                        seats[i][j] = line[j];
                }

                while (!Compare(prevSeats, seats))
                {
                    prevSeats = CopyArray(seats);
                    seats = DoOneRound();
                }

                int nr = 0;
                for (int i = 0; i < seats.Length; i++)
                    for (int j = 0; j < seats[i].Length; j++)
                        if (seats[i][j] == '#')
                            nr++;
                Console.WriteLine(nr);
            }

            private int CountSeats(int i, int j, int c)
            {
                int nr = 0;

                bool found = false;
                for (int k = i - 1; k >= 0 && !found; k--)
                    if (seats[k][j] != '.')
                    {
                        found = true;
                        if (seats[k][j] == c)
                            nr++;
                    }

                found = false;
                for (int k = i - 1, l = j - 1; k >= 0 && l >= 0 && !found; k--, l--)
                    if (seats[k][l] != '.')
                    {
                        found = true;
                        if (seats[k][l] == c)
                            nr++;
                    }

                found = false;
                for (int k = i - 1, l = j + 1; k >= 0 && l < seats[i].Length && !found; k--, l++)
                    if (seats[k][l] != '.')
                    {
                        found = true;
                        if (seats[k][l] == c)
                            nr++;
                    }

                found = false;
                for (int l = j - 1; l >= 0 && !found; l--)
                    if (seats[i][l] != '.')
                    {
                        found = true;
                        if (seats[i][l] == c)
                            nr++;
                    }

                found = false;
                for (int l = j + 1; l < seats[i].Length && !found; l++)
                    if (seats[i][l] != '.')
                    {
                        found = true;
                        if (seats[i][l] == c)
                            nr++;
                    }

                found = false;
                for (int k = i + 1; k < seats.Length && !found; k++)
                    if (seats[k][j] != '.')
                    {
                        found = true;
                        if (seats[k][j] == c)
                            nr++;
                    }

                found = false;
                for (int k = i + 1, l = j - 1; k < seats.Length && l >= 0 && !found; k++, l--)
                    if (seats[k][l] != '.')
                    {
                        found = true;
                        if (seats[k][l] == c)
                            nr++;
                    }

                found = false;
                for (int k = i + 1, l = j + 1; k < seats.Length && l < seats[i].Length && !found; k++, l++)
                    if (seats[k][l] != '.')
                    {
                        found = true;
                        if (seats[k][l] == c)
                            nr++;
                    }

                return nr;
            }

            private char[][] CopyArray(char[][] source)
            {
                return source.Select(s => s.ToArray()).ToArray();
            }

            private char[][] DoOneRound()
            {
                char[][] changedSeats = new char[seats.Length][];

                changedSeats = CopyArray(seats);

                for (int i = 0; i < seats.Length; i++)
                {
                    string row = new string(seats[i]);
                    for (int j = 0; j < row.Length; j++)
                    {
                        if (row[j] == 'L')
                        {
                            if (CountSeats(i, j, '#') == 0)
                            {
                                changedSeats[i][j] = '#';
                            }
                        }
                        else if (row[j] == '#')
                        {
                            if (CountSeats(i, j, '#') >= 5)
                                changedSeats[i][j] = 'L';
                        }
                    }
                }

                return changedSeats;
            }

            private bool Compare(char[][] first, char[][] second)
            {
                bool same = true;

                if (first.Length != second.Length)
                    return false;

                for (int i = 0; i < first.Length && same; i++)
                {
                    if (first[i].Length != second[i].Length)
                        return false;
                    for (int j = 0; j < first[i].Length && same; j++)
                    {
                        char elem2 = second[i][j];
                        char elem1 = first[i][j];
                        if (elem1 != elem2)
                            same = false;
                    }
                }

                return same;
            }
        }
    }
}
