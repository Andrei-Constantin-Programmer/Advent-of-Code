// Task: https://adventofcode.com/2025/day/10

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;
using Microsoft.Z3;

namespace Advent_of_Code.Challenge_Solutions.Year_2025;

public class ChallengeSolution10(IConsole console, ISolutionReader<ChallengeSolution10> reader)
    : ChallengeSolution<ChallengeSolution10>(console, reader)
{
    private const char LightOn = '#';
    
    public override void SolveFirstPart()
    {
        var machines = ReadMachines();

        var fewestButtonPresses = machines
            .Sum(ButtonPressesToCorrectLightConfiguration);

        Console.WriteLine($"Fewest button presses: {fewestButtonPresses}");
    }

    public override void SolveSecondPart()
    {
        var machines = ReadMachines();

        var fewestButtonPresses = machines
            .Sum(ButtonPressesToCorrectJoltage);

        Console.WriteLine($"Fewest button presses: {fewestButtonPresses}");
    }

    private static int ButtonPressesToCorrectJoltage(Machine machine)
    {
        using Context z3Context = new();
        using var optimisationContext = z3Context.MkOptimize();

        var pressesPerButton = new IntExpr[machine.Buttons.Count];

        ConstrainPressCountsToBeNonNegative();
        ConstrainSolutionsToMatchRequirements();
        MinimiseNumberOfPresses();

        optimisationContext.Check();

        var model = optimisationContext.Model;
        
        var sum = 0;
        foreach (var press in pressesPerButton)
        {
            sum += ((IntNum)model.Evaluate(press)).Int;
        }

        return sum;

        void ConstrainPressCountsToBeNonNegative()
        {
            for (var buttonIndex = 0; buttonIndex < pressesPerButton.Length; buttonIndex++)
            {
                pressesPerButton[buttonIndex] = z3Context.MkIntConst($"presses_{buttonIndex}");
                optimisationContext.Add(z3Context.MkGe(pressesPerButton[buttonIndex], z3Context.MkInt(0)));
            }
        }

        void ConstrainSolutionsToMatchRequirements()
        {
            for (var joltReqIndex = 0; joltReqIndex < machine.JoltageRequirements.Length; joltReqIndex++)
            {
                ArithExpr joltValue = z3Context.MkInt(0);

                for (var buttonIndex = 0; buttonIndex < pressesPerButton.Length; buttonIndex++)
                {
                    if (machine.Buttons[buttonIndex].Contains(joltReqIndex))
                    {
                        joltValue = z3Context.MkAdd(joltValue, pressesPerButton[buttonIndex]);
                    }
                }
            
                optimisationContext.Add(z3Context.MkEq(joltValue, z3Context.MkInt(machine.JoltageRequirements[joltReqIndex])));
            }
        }

        void MinimiseNumberOfPresses()
        {
            ArithExpr total = z3Context.MkInt(0);
            foreach (var buttonPresses in pressesPerButton)
            {
                total = z3Context.MkAdd(total, buttonPresses);
            }
            
            optimisationContext.MkMinimize(total);
        }
    }
    
    private static int ButtonPressesToCorrectLightConfiguration(Machine machine)
    {
        for (var presses = 0; presses < machine.Buttons.Count; presses++)
        {
            if (TryReachMachineConfiguration(presses, 0, 0, new bool[machine.LightDiagram.Length]))
            {
                return presses;
            }
        }

        return -1;
        
        bool TryReachMachineConfiguration(int pressCount, int startIndex, int pressesSoFar, bool[] diagramSoFar)
        {
            if (pressesSoFar == pressCount)
            {
                return machine.MatchesDiagram(diagramSoFar);
            }

            var minimumPressesRequired = machine.Buttons.Count + pressesSoFar + 1 - pressCount;
            for (var buttonIndex = startIndex; buttonIndex < minimumPressesRequired; buttonIndex++)
            {
                var button = machine.Buttons[buttonIndex];
                ApplyButtonToLightDiagram(button, diagramSoFar);
                
                if (TryReachMachineConfiguration(pressCount, buttonIndex + 1, pressesSoFar + 1, diagramSoFar))
                {
                    return true;
                }
                
                ApplyButtonToLightDiagram(button, diagramSoFar); // Unpress button
            }

            return false;
        }
    }

    private static void ApplyButtonToLightDiagram(int[] button, bool[] lightDiagram)
    {
        foreach (var light in button)
        {
            lightDiagram[light] = !lightDiagram[light];
        }
    }

    private List<Machine> ReadMachines()
    {
        var lines = Reader.ReadLines();

        List<Machine> machines = new(lines.Length);
        
        foreach (var line in lines)
        {
            var elems = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var lightDiagram = elems[0][1..^1]
                .Select(c => c == LightOn)
                .ToArray();
            
            var buttons = elems[1..^1]
                .Select(buttonString => buttonString[1..^1]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse)
                    .ToArray())
                .ToList();

            var joltageRequirements = elems[^1][1..^1]
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray();
            
            machines.Add(new Machine(lightDiagram, buttons, joltageRequirements));
        }

        return machines;
    }

    private class Machine(bool[] lightDiagram, List<int[]> buttons, int[] joltageRequirements)
    {
        public bool[] LightDiagram { get; } = lightDiagram;

        public List<int[]> Buttons { get; } = buttons;

        public int[] JoltageRequirements { get; } = joltageRequirements;

        public bool MatchesDiagram(bool[] diagram)
        {
            if (diagram.Length != LightDiagram.Length)
            {
                return false;
            }

            for (var i = 0; i < LightDiagram.Length; i++)
            {
                if (diagram[i] != LightDiagram[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
