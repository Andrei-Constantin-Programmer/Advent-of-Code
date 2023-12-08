// Task: https://adventofcode.com/2021/day/12

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021
{
    class ChallengeSolution12 : ChallengeSolution
    {
        private Dictionary<string, List<string>>? caveConnections;
        private int paths;

        protected override void SolveFirstPart()
        {
            paths = 0;
            GetCaves();
            FindCaves("start", new List<string>());

            Console.WriteLine(paths);
        }

        private void FindCaves(string cave, List<string> prevCaves)
        {
            if (cave.Equals("end"))
            {
                paths++;
                return;
            }

            List<string> connections = caveConnections[cave];
            for (int i = 0; i < connections.Count; i++)
                if (connections[i] != "start" && ((IsSmall(connections[i]) && !prevCaves.Contains(connections[i])) || IsBig(connections[i])))
                {
                    var newPrevCaves = new List<string>(prevCaves);
                    newPrevCaves.Add(cave);
                    FindCaves(connections[i], newPrevCaves);          
                }
        }


        private List<string> smallCaves;

        protected override void SolveSecondPart()
        {
            paths = 0;
            GetCaves();
            foreach(var cave in smallCaves)
                FindCavesSpecialRule("start", new List<string>(), cave);

            fullPaths.Sort();
            for (int i = 1; i < fullPaths.Count; i++)
                if (fullPaths[i].Equals(fullPaths[i-1]))
                {
                    fullPaths.RemoveAt(i);
                    i--;
                }


            Console.WriteLine(fullPaths.Count);
        }


        private List<CavePath> fullPaths = new List<CavePath>();
        private void FindCavesSpecialRule(string cave, List<string> prevCaves, string visitedTwice)
        {
            if (cave.Equals("end"))
            {
                prevCaves.Add("end");
                fullPaths.Add(new CavePath(prevCaves));
                return;
            }

            List<string> connections = caveConnections[cave];
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i] != "start" && (IsSmall(connections[i]) && (!prevCaves.Contains(connections[i]) || (prevCaves.FindAll(x => x.Equals(connections[i])).Count == 1) && visitedTwice == connections[i])) || IsBig(connections[i]))
                {
                    var newPrevCaves = new List<string>(prevCaves);
                    newPrevCaves.Add(cave);
                    FindCavesSpecialRule(connections[i], newPrevCaves, visitedTwice);
                }
            }
        }

        private void GetCaves()
        {
            caveConnections = new Dictionary<string, List<string>>();
            smallCaves = new List<string>();
            using (TextReader read = Reader.GetInputFile(2021, 12))
            {
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    string[] caves = line.Split("-", StringSplitOptions.RemoveEmptyEntries);
                    if (caveConnections.ContainsKey(caves[0]))
                        caveConnections[caves[0]].Add(caves[1]);
                    else
                        caveConnections.Add(caves[0], new List<string> { caves[1] });

                    if (caveConnections.ContainsKey(caves[1]))
                        caveConnections[caves[1]].Add(caves[0]);
                    else
                        caveConnections.Add(caves[1], new List<string> { caves[0] });

                    if (!smallCaves.Contains(caves[0]) && IsSmall(caves[0]))
                        smallCaves.Add(caves[0]);
                    if (!smallCaves.Contains(caves[1]) && IsSmall(caves[1]))
                        smallCaves.Add(caves[1]);
                }
            }
        }

        private bool IsSmall(string caveName)
        {
            foreach (char c in caveName.ToCharArray())
                if (c < 'a' || c > 'z')
                    return false;

            return true;
        }

        private bool IsBig(string caveName)
        {
            return !IsSmall(caveName);
        }
    }

    class CavePath
    {
        public List<string> path { get; }

        public CavePath(List<string> path)
        {
            this.path = path;
        }
    }
}
