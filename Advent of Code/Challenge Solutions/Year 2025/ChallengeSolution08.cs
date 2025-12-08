// Task: https://adventofcode.com/2025/day/8

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution08(IConsole console, ISolutionReader<ChallengeSolution08> reader)
    : ChallengeSolution<ChallengeSolution08>(console, reader)
{
    private const int CircuitsToConsider = 3;
    
    public override void SolveFirstPart()
    {
        var junctionBoxes = ReadJunctionBoxLocations(out var pairsToConnect);

        var topEdges = 
            GetEdges(junctionBoxes)
                .Take(pairsToConnect)
                .ToList();
        
        var circuits = BuildCircuits(junctionBoxes, topEdges);
        
        var topThreeCircuitsProduct = circuits
            .Select(cluster => cluster.Count)
            .OrderDescending()
            .Take(CircuitsToConsider)
            .Aggregate(1, (acc, x) => acc * x);
        
        Console.WriteLine($"Top {CircuitsToConsider} circuit sizes product: {topThreeCircuitsProduct}");
    }

    public override void SolveSecondPart()
    {
        var junctionBoxes = ReadJunctionBoxLocations(out _);

        var edges = 
            GetEdges(junctionBoxes)
            .ToList();

        var lastEdge = GetLastConnectingEdge(junctionBoxes, edges);
        var xProduct = (long)lastEdge.JunctionBox1.X * lastEdge.JunctionBox2.X;
        
        Console.WriteLine($"X coordinate product for last connection: {xProduct}");
    }

    private static Edge GetLastConnectingEdge(List<JunctionBox> junctionBoxes, List<Edge> edges)
    {
        Dictionary<JunctionBox, JunctionBox> parentOf = [];
        Dictionary<JunctionBox, int> rankOf = [];

        foreach (var jb in junctionBoxes)
        {
            parentOf[jb] = jb;
            rankOf[jb] = 0;
        }

        var components = junctionBoxes.Count;
        Edge? lastEdge = null;

        for (var eIndex = 0; eIndex < edges.Count && components > 1; eIndex++)
        {
            var edge = edges[eIndex];
            
            var rootOfJb1 = Find(edge.JunctionBox1);
            var rootOfJb2 = Find(edge.JunctionBox2);

            if (rootOfJb1 == rootOfJb2)
            {
                continue;
            }
            
            Union(rootOfJb1, rootOfJb2);
            components--;
            lastEdge = edge;
        }

        return lastEdge!;

        JunctionBox Find(JunctionBox junctionBox)
        {
            if (parentOf[junctionBox] != junctionBox)
            {
                parentOf[junctionBox] = Find(parentOf[junctionBox]);
            }

            return parentOf[junctionBox];
        }

        void Union(JunctionBox jb1, JunctionBox jb2)
        {
            var rootOfJb1 = Find(jb1);
            var rootOfJb2 = Find(jb2);
            if (rootOfJb1 == rootOfJb2)
            {
                return;
            }

            if (rankOf[rootOfJb1] < rankOf[rootOfJb2])
            {
                parentOf[rootOfJb1] = rootOfJb2;
            }
            else if (rankOf[rootOfJb1] > rankOf[rootOfJb2])
            {
                parentOf[rootOfJb2] = rootOfJb1;
            }
            else
            {
                parentOf[rootOfJb2] = rootOfJb1;
                rankOf[rootOfJb1]++;
            }
        }
    }

    private static List<HashSet<JunctionBox>> BuildCircuits(List<JunctionBox> junctionBoxes, List<Edge> topEdges)
    {
        var adjacencyList = InitialiseAdjacencyList(junctionBoxes, topEdges);

        HashSet<JunctionBox> visited = [];
        List<HashSet<JunctionBox>> circuits = [];

        foreach (var start in junctionBoxes)
        {
            if (!visited.Add(start))
            {
                continue;
            }

            HashSet<JunctionBox> circuit = [];

            Stack<JunctionBox> stack = [];
            stack.Push(start);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                circuit.Add(current);

                foreach (var neighbour in adjacencyList[current])
                {
                    if (visited.Add(neighbour))
                    {
                        stack.Push(neighbour);
                    }
                }
            }

            circuits.Add(circuit);
        }

        return circuits;
    }

    private static Dictionary<JunctionBox, List<JunctionBox>> InitialiseAdjacencyList(List<JunctionBox> junctionBoxes, List<Edge> topEdges)
    {
        var adjacencyList = junctionBoxes.ToDictionary(jb => jb, _ => new List<JunctionBox>());

        foreach (var (jb1, jb2, _) in topEdges)
        {
            adjacencyList[jb1].Add(jb2);
            adjacencyList[jb2].Add(jb1);
        }

        return adjacencyList;
    }

    private static IEnumerable<Edge> GetEdges(List<JunctionBox> junctionBoxes)
    {
        List<Edge> edges = [];

        for (var i = 0; i < junctionBoxes.Count - 1; i++)
        {
            for (var j = i + 1; j < junctionBoxes.Count; j++)
            {
                var jb1 = junctionBoxes[i];
                var jb2 = junctionBoxes[j];
                var distance = EuclideanDistanceSquared(jb1, jb2);
                
                edges.Add(new(jb1, jb2, distance));
            }
        }
        
        return edges.OrderBy(e => e.Distance);
    }
    
    // Nothing earned by taking the square root of this value
    private static long EuclideanDistanceSquared(JunctionBox jb1, JunctionBox jb2)
    {
        long x = jb1.X - jb2.X;
        long y = jb1.Y - jb2.Y;
        long z = jb1.Z - jb2.Z;
        
        return x * x + y * y + z * z;
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
            
            junctionBoxes.Add(new JunctionBox(coords[0], coords[1], coords[2]));
        }

        return junctionBoxes;
    }

    private record Edge(JunctionBox JunctionBox1, JunctionBox JunctionBox2, long Distance);

    private class JunctionBox(int x, int y, int z)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Z { get; } = z;
    }
}
