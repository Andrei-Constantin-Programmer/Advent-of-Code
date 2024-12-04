// Task: https://adventofcode.com/2021/day/10

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2021;

public class ChallengeSolution10(IConsole console, ISolutionReader<ChallengeSolution10> reader)
    : ChallengeSolution<ChallengeSolution10>(console, reader)
{
    private static Dictionary<char, int> pointValues = new Dictionary<char, int>()
    {
        { ')', 3},
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137}
    };
    private static Dictionary<char, int> scoreValues = new Dictionary<char, int>()
    {
        {')', 1 },
        {']', 2 },
        {'}', 3 },
        {'>', 4 },
    };
    private static Dictionary<char, char> openCloseValues = new Dictionary<char, char>()
    {
        {'(', ')' },
        {'[', ']' },
        {'{', '}' },
        {'<', '>' },
    };

    public override void SolveFirstPart()
    {
        int points = 0;
        foreach (var line in _reader.ReadLines())
        {
            char? c = CorruptedChar(line);
            if (c != null)
            {
                points += pointValues[(char)c];
            }
        }

        _console.WriteLine(points);
    }

    public override void SolveSecondPart()
    {
        var scores = new List<long>();
        foreach (var line in _reader.ReadLines())
        {
            if (CorruptedChar(line) == null)
            {
                var stack = new Stack<char>();
                for (int i = 0; i < line.Length; i++)
                    if (openCloseValues.ContainsKey(line[i]))
                        stack.Push(line[i]);
                    else
                        stack.Pop();

                if (stack.Count > 0)
                {
                    long score = 0;
                    while (stack.Count > 0)
                        score = score * 5 + scoreValues[openCloseValues[stack.Pop()]];
                    scores.Add(score);
                }
            }
        }

        scores.Sort();
        _console.WriteLine(scores[(scores.Count - 1) / 2]);
    }

    private char? CorruptedChar(string line)
    {
        var stack = new Stack<char>();
        for (int i = 0; i < line.Length; i++)
        {
            if (openCloseValues.ContainsKey(line[i]))
                stack.Push(line[i]);
            else if (openCloseValues[stack.Pop()] != line[i])
            {
                return line[i];
            }
        }

        return null;
    }

}
