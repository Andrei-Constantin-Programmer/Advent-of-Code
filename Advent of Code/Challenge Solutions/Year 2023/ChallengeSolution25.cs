// Task: https://adventofcode.com/2023/day/25

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution25 : ChallengeSolution
{
    private static readonly Random _random = new();

    protected override void SolveFirstPart()
    {
        var graph = ReadGraph();

        //var (subgraphSize1, subgraphSize2) = GetPartialGraphSizesAfterDivision(graph);

        //Console.WriteLine(subgraphSize1 * subgraphSize2);
    }

    protected override void SolveSecondPart()
    {
        throw new NotImplementedException();
    }

    private Dictionary<string, List<string>> ReadGraph()
    {
        Dictionary<string, HashSet<string>> graph = new();

        var lines = Reader.ReadLines(this);
        foreach (var line in lines)
        {
            var elements = line.Split(": ");
            var name = elements[0];
            var connections = elements[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            graph.TryAdd(name, new());
            foreach (var connection in connections)
            {
                graph[name].Add(connection);
                graph.TryAdd(connection, new());
                graph[connection].Add(name);
            }
        }

        return graph.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
    }
}
