// Task: https://adventofcode.com/2023/day/15

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution15(IConsole console, ISolutionReader<ChallengeSolution15> reader)
    : ChallengeSolution<ChallengeSolution15>(console, reader)
{
    public override void SolveFirstPart()
    {
        var stringsToHash = _reader
            .ReadLines()[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries);

        long sum = stringsToHash
            .Select(Hash)
            .Sum();

        _console.WriteLine(sum);
    }

    public override void SolveSecondPart()
    {
        var steps = _reader
            .ReadLines()[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries);

        Dictionary<int, List<Lens>> boxes = ComputeBoxesFromSteps(steps);

        var focusingPowerSum = 0;
        for (var boxIndex = 0; boxIndex < 256; boxIndex++)
        {
            if (!boxes.ContainsKey(boxIndex))
            {
                continue;
            }

            for (var lensIndex = 0; lensIndex < boxes[boxIndex].Count; lensIndex++)
            {
                focusingPowerSum += (boxIndex + 1)
                                  * (lensIndex + 1)
                                  * boxes[boxIndex][lensIndex].FocalLength;
            }
        }

        _console.WriteLine(focusingPowerSum);
    }

    private static Dictionary<int, List<Lens>> ComputeBoxesFromSteps(string[] steps)
    {
        Dictionary<int, List<Lens>> boxes = new();

        foreach (var step in steps)
        {
            var stepElements = step.Split(new char[] { '-', '=' }, StringSplitOptions.RemoveEmptyEntries);
            var (label, focalLength) = (stepElements[0], stepElements.Length == 2 ? int.Parse(stepElements[1]) : 0);
            var box = Hash(label);

            if (step.Contains('-'))
            {
                HandleDashOperation(box, label);
            }
            else
            {
                HandleEqualOperation(box, label, focalLength);
            }
        }

        return boxes;

        void HandleDashOperation(int box, string label)
        {
            if (boxes.ContainsKey(box))
            {
                boxes[box].RemoveAll(boxContents => boxContents.Label == label);
            }
        }

        void HandleEqualOperation(int box, string label, int focalLength)
        {
            if (!boxes.ContainsKey(box))
            {
                boxes.Add(box, new List<Lens> { new(label, focalLength) });
                return;
            }

            var indexOfLens = boxes[box].FindIndex(boxContents => boxContents.Label == label);
            if (indexOfLens == -1)
            {
                boxes[box].Add(new Lens(label, focalLength));
            }
            else
            {
                boxes[box][indexOfLens].FocalLength = focalLength;
            }
        }
    }

    private static int Hash(string stringToHash) => stringToHash
        .Aggregate(0, (hash, character) => (hash + character) * 17 % 256);

    private class Lens
    {
        public string Label { get; set; }
        public int FocalLength { get; set; }

        public Lens(string label, int focalLength)
        {
            Label = label;
            FocalLength = focalLength;
        }

        public override string ToString() => $"[{Label} {FocalLength}]";
    }
}
