// Task: https://adventofcode.com/2025/day/8


using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution08(IConsole console, ISolutionReader<ChallengeSolution08> reader)
    : ChallengeSolution<ChallengeSolution08>(console, reader)
{
    public override void SolveFirstPart()
    {
        var junctionBoxes = ReadJunctionBoxLocations(out var pairsToConnect);
        HydrateJunctionBoxesWithDistanceToOthers(junctionBoxes);

        HydrateJunctionBoxesWithTheTopConnectedPairs(pairsToConnect, junctionBoxes);

        var circuitLengths = ComputeCircuitLengths(junctionBoxes);

        var topThreeCircuits = circuitLengths
            .OrderDescending()
            .Take(3)
            .Aggregate(1, (acc, x) => acc * x);
        
        Console.WriteLine($"Top 3 circuit sizes: {topThreeCircuits}");
    }

    public override void SolveSecondPart()
    {

    }
    
    private static List<int> ComputeCircuitLengths(List<JunctionBox> junctionBoxes)
    {
        List<int> circuitLengths = [];
        HashSet<JunctionBox> alreadyChecked = [];
        foreach (var junctionBox in junctionBoxes)
        {
            if (alreadyChecked.Contains(junctionBox))
            {
                continue;
            }

            circuitLengths.Add(1 + junctionBox.Neighbours.Count);
            alreadyChecked.Add(junctionBox);
            foreach (var jb in junctionBox.Neighbours)
            {
                alreadyChecked.Add(jb);
            }
        }

        return circuitLengths;
    }

    private static void HydrateJunctionBoxesWithTheTopConnectedPairs(int pairsToConnect, List<JunctionBox> junctionBoxes)
    {
        HashSet<(JunctionBox, JunctionBox)> alreadyConnected = [];
        for (var pairsConnected = 0; pairsConnected < pairsToConnect; pairsConnected++)
        {
            var newConnection = junctionBoxes
                .SelectMany(
                    jb => jb.DistanceTo,
                    (jb, kvp) => new
                    {
                        JunctionBox = jb,
                        Neighbour = kvp.Key,
                        Distance = kvp.Value
                    })
                .Where(x => !alreadyConnected.Contains((x.JunctionBox, x.Neighbour)))
                .MinBy(x => x.Distance)!;

            var jb1 = newConnection.JunctionBox;
            var jb2 = newConnection.Neighbour;
            
            foreach (var neighbour in jb2.Neighbours)
            {
                jb1.Neighbours.Add(neighbour);
                alreadyConnected.Add((jb1, neighbour));
                alreadyConnected.Add((neighbour, jb1));
            }
            
            foreach (var neighbour in jb1.Neighbours)
            {
                jb2.Neighbours.Add(neighbour);
                alreadyConnected.Add((jb2, neighbour));
                alreadyConnected.Add((neighbour, jb2));
            }
            
            jb1.Neighbours.Add(jb2);
            jb2.Neighbours.Add(jb1);
            alreadyConnected.Add((jb1, jb2));
            alreadyConnected.Add((jb2, jb1));
        }
    }
    
    private static void HydrateJunctionBoxesWithDistanceToOthers(List<JunctionBox> junctionBoxes)
    {
        for (var i = 0; i < junctionBoxes.Count - 1; i++)
        {
            for (var j = i + 1; j < junctionBoxes.Count; j++)
            {
                var distance = EuclideanDistance(junctionBoxes[i], junctionBoxes[j]);
                junctionBoxes[i].DistanceTo.Add(junctionBoxes[j], distance);
                junctionBoxes[j].DistanceTo.Add(junctionBoxes[i], distance);
            }
        }
    }
    
    private static double EuclideanDistance(JunctionBox jb1, JunctionBox jb2)
    {
        var x = jb1.X - jb2.X;
        var y = jb1.Y - jb2.Y;
        var z = jb1.Z - jb2.Z;
        
        return Math.Sqrt(x * x + y * y + z * z);
    }

    private List<JunctionBox> ReadJunctionBoxLocations(out int pairsToConnect)
    {
        var lines = Reader.ReadLines();
        pairsToConnect = int.Parse(lines[0]);
        List<JunctionBox> junctionBoxes = [];

        foreach (var line in lines[1..])
        {
            var coords = line
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            
            junctionBoxes.Add(new JunctionBox(coords[0], coords[1], coords[2], [], []));
        }

        return junctionBoxes;
    }
    
    private record JunctionBox(int X, int Y, int Z, Dictionary<JunctionBox, double> DistanceTo, HashSet<JunctionBox> Neighbours);
}
