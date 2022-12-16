// Task: https://adventofcode.com/2020/day/13

namespace Advent_of_Code.Challenge_Solutions.Year_2020
{
    internal class ChallengeSolution13 : ChallengeSolution
    {
        private static long timestamp, firstTimestamp, earliestTimestamp;
        private static int[] buses;
        private static string[] lines;
        private static string[] busesIds;

        public ChallengeSolution13()
        {
            lines = File.ReadAllLines(Reader.GetFileString(Reader.FileType.Input, 2020, 13));
            busesIds = lines[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected override void SolveFirstPart()
        {
            var busIds = new string[busesIds.Length];
            busesIds.CopyTo(busIds, 0);
            firstTimestamp = timestamp = Convert.ToInt32(lines[0]);
            var list = new List<string>(busIds);
            list.RemoveAll(item => item[0] == 'x');
            busIds = list.ToArray();

            buses = new int[busIds.Length];

            int i = 0;
            foreach (string id in busIds)
            {
                buses[i] = Convert.ToInt32(id);
                i++;
            }

            Console.WriteLine(GetEarliestBus() * (earliestTimestamp - firstTimestamp));
        }

        protected override void SolveSecondPart()
        {
            var busIds = new string[busesIds.Length];
            busesIds.CopyTo(busIds, 0);
            buses = new int[busIds.Length];
            int k = 0;
            foreach (string id in busIds)
            {
                if (id == "x")
                    buses[k] = 0;
                else
                    buses[k] = Convert.ToInt32(id);
                k++;
            }

            long timestamp = 0;
            long time = buses[0];
            for (int i = 1; i < buses.Length; i++)
            {
                if (buses[i] != 0)
                {
                    long newTimestamp = buses[i];
                    bool found = false;
                    while (!found)
                    {
                        timestamp += time;
                        if ((timestamp + i) % newTimestamp == 0)
                        {
                            time *= newTimestamp;
                            found = true;
                        }
                    }
                }
            }

            Console.WriteLine(timestamp);
        }

        private static int GetEarliestBus()
        {
            int earliest = -1;
            while (earliest == -1)
            {
                bool found = false;
                for (int i = 0; i < buses.Length && !found; i++)
                {
                    if (timestamp % buses[i] == 0)
                    {
                        found = true;
                        earliest = buses[i];
                        earliestTimestamp = timestamp;
                    }
                }

                timestamp++;
            }

            return earliest;
        }

    }
}
