namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution16 : ChallengeSolution
    {
        private Dictionary<Valve, List<Valve>> adjacentValves;

        public ChallengeSolution16()
        {
            adjacentValves = ReadValves();
        }

        protected override void SolveFirstPart()
        {
            throw new NotImplementedException();

            Console.WriteLine(GetMaximumPressure(adjacentValves.Keys.First(), 30, 0));
        }

        private int GetMaximumPressure(Valve valve, int minutes, int accumulatedPressure)
        {
            if (minutes <= 0)
                return 0;

            Console.WriteLine(valve.Label);

            int maximumPressure = 0;
            foreach(var v in adjacentValves[valve])
            {
                int pressure = Math.Max(
                    GetMaximumPressure(v, minutes - 1, accumulatedPressure), 
                    GetMaximumPressure(v, minutes - 2, accumulatedPressure + valve.Rate));

                if (pressure > maximumPressure)
                    maximumPressure = pressure;
            }

            Console.WriteLine(maximumPressure);

            return accumulatedPressure + maximumPressure;
        }

        protected override void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private static Dictionary<Valve, List<Valve>> ReadValves()
        {
            var valves = new List<Valve>();
            var valveNeighbours = new Dictionary<Valve, string[]>();

            using TextReader read = Reader.GetInputFile(2022, 16);

            string? line;
            while ((line = read.ReadLine()) != null)
            {
                var elements = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var valveLabel = elements[1];
                var valveRate = Convert.ToInt32(elements[4].Replace(';', ' ').Split('=')[1].Trim());
                var neighbourLabels = elements[9..].Select(label => label.Replace(',', ' ').Trim()).ToArray();

                valves.Add(new Valve(valveLabel, valveRate));
                valveNeighbours.Add(valves.Last(), neighbourLabels);
            }

            var valveDictionary = new Dictionary<Valve, List<Valve>>();

            foreach(var valve in valves)
            {
                valveDictionary.Add(valve, valves.Where(v => valveNeighbours[valve].Contains(v.Label)).ToList());
            }

            return valveDictionary;
        }

        private class Valve
        {
            public string Label { get; init; }
            public int Rate { get; init; }
            
            public Valve(string label, int rate)
            {
                Label = label;
                Rate = rate;
            }
        }
    }
}
