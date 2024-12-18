﻿// Task: https://adventofcode.com/2021/day/12

using Advent_of_Code.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution12(IConsole console, ISolutionReader<ChallengeSolution12> reader)
    : ChallengeSolution<ChallengeSolution12>(console, reader)
{
    private Dictionary<string, List<string>>? caveConnections;
    private int paths;

    public override void SolveFirstPart()
    {
        paths = 0;
        GetCaves();
        FindCaves("start", new List<string>());

        _console.WriteLine(paths);
    }

    private void FindCaves(string cave, List<string> prevCaves)
    {
        if (cave.Equals("end"))
        {
            paths++;
            return;
        }

        List<string> connections = caveConnections![cave];
        for (int i = 0; i < connections.Count; i++)
            if (connections[i] != "start" && ((IsSmall(connections[i]) && !prevCaves.Contains(connections[i])) || IsBig(connections[i])))
            {
                var newPrevCaves = new List<string>(prevCaves);
                newPrevCaves.Add(cave);
                FindCaves(connections[i], newPrevCaves);
            }
    }

    private List<string> smallCaves;

    public override void SolveSecondPart()
    {
        paths = 0;
        GetCaves();
        foreach (var cave in smallCaves)
            FindCavesSpecialRule("start", new List<string>(), cave);

        fullPaths.Sort();
        for (int i = 1; i < fullPaths.Count; i++)
            if (fullPaths[i].Equals(fullPaths[i - 1]))
            {
                fullPaths.RemoveAt(i);
                i--;
            }


        _console.WriteLine(fullPaths.Count);
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

        List<string> connections = caveConnections![cave];
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
        foreach (var line in _reader.ReadLines())
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

class CavePath : IComparable, IComparable<CavePath>
{
    public List<string> path { get; }

    public CavePath(List<string> path)
    {
        this.path = path;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null) return 1;
        CavePath? other = obj as CavePath;
        if (other == null)
            throw new ArgumentException("A Path object is required for comparison.", "obj");

        return CompareTo(other);
    }

    public int CompareTo([AllowNull] CavePath other)
    {
        if (other == null)
            return 1;

        if (other.path.Count != path.Count)
            return path.Count.CompareTo(other.path.Count);

        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].CompareTo(other.path[i]) != 0)
            {
                return path[i].CompareTo(other.path[i]);
            }
        }

        return 0;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        var other = obj as CavePath;
        if (other == null)
            return false;

        if (other.path.Count != path.Count)
            return false;

        for (int i = 0; i < path.Count; i++)
        {
            if (!path[i].Equals(other.path[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        string toReturn = "";
        foreach (var x in path)
            toReturn += x + " ";
        return toReturn;
    }
}
