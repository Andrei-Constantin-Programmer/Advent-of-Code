namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution15 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            var pairs = ReadSensorsAndBeacons();

            Console.WriteLine(BlockedPositions(2000000, pairs).Count);
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }

        private static ISet<(long x, long y)> BlockedPositions(long row, List<SensorBeaconPair> pairs)
        {
            var blockedPositions = new SortedSet<(long x, long y)>();

            foreach(var pair in pairs)
            {
                var distance = ManhattanDistance(pair.Sensor, pair.Beacon);

                var perpendicularBase = (pair.Sensor.x, row);
                var perpendicularDistanceToRow = ManhattanDistance(pair.Sensor, perpendicularBase);

                if(perpendicularDistanceToRow <= distance)
                {
                    var distanceToRow = perpendicularDistanceToRow;
                    blockedPositions.Add(perpendicularBase);

                    for(var i = 1; distanceToRow < distance; i++)
                    {
                        distanceToRow = ManhattanDistance(pair.Sensor, (pair.Sensor.x + i, row));
                        if (distanceToRow <= distance)
                        {
                            blockedPositions.Add((pair.Sensor.x + i, row));
                            blockedPositions.Add((pair.Sensor.x - i, row));
                        }
                    }
                }
            }

            blockedPositions = RemoveBeaconsFromSet(blockedPositions, pairs);

            return blockedPositions;
        }

        private static SortedSet<(long x, long y)> RemoveBeaconsFromSet(SortedSet<(long x, long y)> set, List<SensorBeaconPair> pairs)
        {
            var newSet = new SortedSet<(long x, long y)>(set);

            foreach(var pair in pairs)
            {
                newSet.Remove(pair.Beacon);
            }

            return newSet;
        }

        private static List<SensorBeaconPair> ReadSensorsAndBeacons()
        {
            var pairs = new List<SensorBeaconPair>();
            using TextReader read = Reader.GetInputFile(2022, 15);
            
            string? line;
            while ((line = read.ReadLine()) != null)
            {
                var values = line
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Contains('='))
                    .Select(x => 
                        Convert.ToInt64(
                            x.Replace(',', ' ')
                             .Replace(':', ' ')
                             .Trim()
                             .Split("=")
                             [1]))
                    .ToArray();

                (long x, long y) source = (values[0], values[1]);
                (long x, long y) beacon = (values[2], values[3]);

                pairs.Add(new SensorBeaconPair(source, beacon));
            }

            return pairs;
        }

        private static double ManhattanDistance((long x, long y) point1, (long x, long y) point2)
        {
            return (Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y));
        }

        private record struct SensorBeaconPair((long x, long y) Sensor, (long x, long y) Beacon);
    }
}
