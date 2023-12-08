// Task: https://adventofcode.com/2022/day/15

using Advent_of_Code.Utilities;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution15 : ChallengeSolution
    {
        private const bool IsTesting = false;
        private readonly ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 12 };

        protected override void SolveFirstPart()
        {
            var pairs = ReadSensorsAndBeacons();

            var row = IsTesting ? 10 : 2000000;

            Console.WriteLine(GetBlockedPositions(row, pairs).Count);
        }

        protected override void SolveSecondPart()
        {
            var sensorBeaconPairs = ReadSensorsAndBeacons();
            var diamonds = CreateDiamonds(sensorBeaconPairs);

            var maximum = IsTesting ? 20 : 4000000;

            Console.WriteLine(GetTuningFrequency(FindDistressBeacon(diamonds, maximum)));
        }

        private (long, long) FindDistressBeacon(List<BlockedDiamond> diamonds, long maximum)
        {
            var outliners = GetOutliners(diamonds, maximum);

            foreach (var outliner in outliners)
            {
                if (IsDistressBeacon(diamonds, outliner))
                {
                    return outliner;
                }
            }

            throw new Exception("Distress beacon not found.");
        }

        private static bool IsDistressBeacon(List<BlockedDiamond> diamonds, (long, long) position)
        {
            foreach (var diamond in diamonds)
            {
                if (diamond.Contains(position))
                {
                    return false;
                }
            }

            return true;
        }
        
        private List<(long, long)> GetOutliners(List<BlockedDiamond> diamonds, long maximum)
        {
            var outlines = GetAllOutlines(diamonds, maximum);

            var outliners = new ConcurrentBag<(long, long)>();

            foreach(var outline in outlines)
            {
                Parallel.ForEach(outline, parallelOptions, outliner =>
                {
                    outliners.Add(outliner);
                });
            }

            return outliners.ToList();
        }

        private List<ISet<(long, long)>> GetAllOutlines(List<BlockedDiamond> diamonds, long maximum)
        {
            var outlines = new ConcurrentBag<ISet<(long, long)>>();

            Parallel.ForEach(diamonds, parallelOptions, diamond =>
            {
                outlines.Add(GetDiamondOutline(diamond, maximum));
            });

            return outlines.ToList();
        }

        private static ISet<(long, long)> GetDiamondOutline(BlockedDiamond diamond, long maximum)
        {
            var outline = new HashSet<(long, long)>();
            for (long i = diamond.Top - 1, j = diamond.Center.x; i <= diamond.Center.y; i++, j++)
            {
                if(i > 0 && j > 0 && i <= maximum && j <= maximum)
                    outline.Add((j, i));
            }
            for (long j = diamond.Right + 1, i = diamond.Center.y; j >= diamond.Center.x; j--, i++)
            {
                if (i > 0 && j > 0 && i <= maximum && j <= maximum)
                    outline.Add((j, i));
            }
            for (long i = diamond.Bottom + 1, j = diamond.Center.x; i >= diamond.Center.y; i--, j--)
            {
                if (i > 0 && j > 0 && i <= maximum && j <= maximum)
                    outline.Add((j, i));
            }
            for (long j = diamond.Left - 1, i = diamond.Center.y; j <= diamond.Center.x; j++, i--)
            {
                if (i > 0 && j > 0 && i <= maximum && j <= maximum)
                    outline.Add((j, i));
            }

            return outline;
        }

        private static long GetTuningFrequency((long x, long y) beacon)
        {
            return beacon.x * 4000000 + beacon.y;
        }

        private static ISet<(long, long)> GetBlockedPositions(long row, List<SensorBeaconPair> sensorBeaconPairs)
        {
            var blockedPositions = new SortedSet<(long x, long y)>();

            foreach(var pair in sensorBeaconPairs)
            {
                var distance = pair.Distance;

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

            blockedPositions = RemoveBeaconsFromSet(blockedPositions, sensorBeaconPairs);

            return blockedPositions;
        }

        private static SortedSet<(long, long)> RemoveBeaconsFromSet(SortedSet<(long x, long y)> set, List<SensorBeaconPair> sensorBeaconPairs)
        {
            var newSet = new SortedSet<(long x, long y)>(set);

            foreach(var pair in sensorBeaconPairs)
            {
                newSet.Remove(pair.Beacon);
            }

            return newSet;
        }

        private static List<BlockedDiamond> CreateDiamonds(List<SensorBeaconPair> sensorBeaconPairs)
        {
            var diamonds = new List<BlockedDiamond>();

            foreach(var pair in sensorBeaconPairs)
            {
                var diamond = new BlockedDiamond(
                        pair.Sensor,
                        pair.Sensor.y - (long)pair.Distance,
                        pair.Sensor.y + (long)pair.Distance,
                        pair.Sensor.x - (long)pair.Distance,
                        pair.Sensor.x + (long)pair.Distance
                    );

                diamonds.Add(diamond);
            }

            return diamonds;
        }

        private List<SensorBeaconPair> ReadSensorsAndBeacons()
        {
            var sensorBeaconPairs = new List<SensorBeaconPair>();
            foreach (var line in Reader.ReadLines(this))
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

                sensorBeaconPairs.Add(new SensorBeaconPair(source, beacon));
            }

            return sensorBeaconPairs;
        }

        private record struct BlockedDiamond((long x, long y) Center, long Top, long Bottom, long Left, long Right)
        {
            public bool Contains((long x, long y) point)
            {
                if(point.y < Center.y)
                {
                    if(point.x > Center.x)
                    {
                        return CalculateDirection((Center.x, Top), (Right, Center.y), point) <= 0;
                    }
                    else
                    {
                        return CalculateDirection((Center.x, Top), (Left, Center.y), point) >= 0;
                    }
                }
                else
                {
                    if (point.x > Center.x)
                    {
                        return CalculateDirection((Center.x, Bottom), (Right, Center.y), point) >= 0;
                    }
                    else
                    {
                        return CalculateDirection((Center.x, Bottom), (Left, Center.y), point) <= 0;
                    }
                }
            }

            private static long CalculateDirection((long x, long y) linePointA, (long x, long y) linePointB, (long x, long y) externalPoint)
            {
                return (externalPoint.x - linePointA.x) * (linePointB.y - linePointA.y) - (externalPoint.y - linePointA.y) * (linePointB.x - linePointA.x);
            }
        }

        private record struct SensorBeaconPair((long x, long y) Sensor, (long x, long y) Beacon)
        {
            public double Distance => ManhattanDistance(Sensor, Beacon);
        }

        private static double ManhattanDistance((long x, long y) point1, (long x, long y) point2)
        {
            return (Math.Abs(point1.x - point2.x) + Math.Abs(point1.y - point2.y));
        }
    }
}
