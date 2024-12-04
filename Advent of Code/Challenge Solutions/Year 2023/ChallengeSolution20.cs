// Task: https://adventofcode.com/2023/day/20

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution20(IConsole console) : ChallengeSolution(console)
{
    private const char FLIP_FLOP_PREFIX = '%';
    private const char CONJUNCTION_PREFIX = '&';
    private const int HIGH_PULSE_NOT_FOUND = -1;

    public override void SolveFirstPart()
    {
        _ = ReadModules(out var broadcaster);
        (long totalLowPulses, long totalHighPulses) = (0, 0);

        for (var buttonPress = 0; buttonPress < 1000; buttonPress++)
        {
            var (lowPulses, highPulses) = FindPulseCounts(broadcaster);

            totalLowPulses += lowPulses;
            totalHighPulses += highPulses;
        }

        Console.WriteLine(totalLowPulses * totalHighPulses);
    }

    public override void SolveSecondPart()
    {
        var modules = ReadModules(out var broadcaster);
        var rxModule = modules
            .FirstOrDefault(mod => mod.Name == "rx") as TestModule
            ?? throw new NotImplementedException();

        var rxInput = (ConjunctionModule)modules
            .First(mod => mod.DestinationModules.Contains(rxModule));

        var inputPresses = FindPressesToHighPulse(broadcaster, rxInput.InputModules.Keys);

        var pressesToLowPulseToRx = inputPresses.Values.Aggregate(1L, (aggregator, presses) => aggregator * presses);
        Console.WriteLine(pressesToLowPulseToRx);
    }

    private static Dictionary<IModule, long> FindPressesToHighPulse(BroadcasterModule broadcaster, IEnumerable<IModule> inputModules)
    {
        Dictionary<IModule, long> pressesToHighPulse = new();
        foreach (var inputModule in inputModules)
        {
            pressesToHighPulse.Add(inputModule, HIGH_PULSE_NOT_FOUND);
        }

        long buttonPress;
        for (buttonPress = 1; ; buttonPress++)
        {
            if (pressesToHighPulse.All(cycle => cycle.Value > HIGH_PULSE_NOT_FOUND))
            {
                break;
            }

            Queue<Pulse> pulses = new();
            pulses.Enqueue(new(null, broadcaster, false));

            while (pulses.TryDequeue(out var pulse))
            {
                if (pulse.Sender is not null
                    && pressesToHighPulse.ContainsKey(pulse.Sender)
                    && pressesToHighPulse[pulse.Sender] == HIGH_PULSE_NOT_FOUND
                    && pulse.Strength)
                {
                    pressesToHighPulse[pulse.Sender] = buttonPress;
                }

                HandleButtonPress(pulses, pulse);
            }
        }

        return pressesToHighPulse;
    }

    private static (int lowPulses, int highPulses) FindPulseCounts(BroadcasterModule broadcaster)
    {
        var lowPulses = 0;
        var highPulses = 0;

        Queue<Pulse> pulses = new();
        pulses.Enqueue(new(null, broadcaster, false));

        while (pulses.TryDequeue(out var pulse))
        {
            if (pulse.Strength)
            {
                highPulses++;
            }
            else
            {
                lowPulses++;
            }

            HandleButtonPress(pulses, pulse);
        }

        return (lowPulses, highPulses);
    }

    private static void HandleButtonPress(Queue<Pulse> pulses, Pulse pulse)
    {
        switch (pulse.Receiver)
        {
            case FlipFlopModule flipFlopModule:
                if (pulse.Strength)
                {
                    return;
                }

                flipFlopModule.IsActivated = !flipFlopModule.IsActivated;
                foreach (var module in flipFlopModule.DestinationModules)
                {
                    pulses.Enqueue(new(flipFlopModule, module, flipFlopModule.IsActivated));
                }

                break;
            case ConjunctionModule conjunctionModule:
                conjunctionModule.InputModules[pulse.Sender!] = pulse.Strength;
                var pulseStrength = !conjunctionModule.InputModules.Values.All(pulse => pulse);
                foreach (var module in conjunctionModule.DestinationModules)
                {
                    pulses.Enqueue(new(conjunctionModule, module, pulseStrength));
                }

                break;
            case BroadcasterModule broadcasterModule:
                foreach (var module in pulse.Receiver.DestinationModules)
                {
                    pulses.Enqueue(new(broadcasterModule, module, pulse.Strength));
                }

                break;

            default:
                break;
        }
    }

    private List<IModule> ReadModules(out BroadcasterModule broadcaster)
    {
        var lines = Reader.ReadLines(this);
        List<IModule> modules = new();
        Dictionary<IModule, HashSet<string>> destinations = new();
        foreach (var line in lines)
        {
            IModule module = line[0] switch
            {
                FLIP_FLOP_PREFIX => new FlipFlopModule(line[1..line.IndexOf(' ')]),
                CONJUNCTION_PREFIX => new ConjunctionModule(line[1..line.IndexOf(' ')]),
                _ => new BroadcasterModule()
            };

            modules.Add(module);
            destinations.Add(module, new(line.Split("-> ")[1].Split(", ")));
        }

        modules.AddRange(destinations.Values
            .SelectMany(names => names)
            .Where(name => !modules.Any(mod => mod.Name == name))
            .Select(name => new TestModule(name)));

        foreach (var module in modules)
        {
            if (module is TestModule)
            {
                continue;
            }

            module.DestinationModules = destinations[module]
                .Where(destination => !string.IsNullOrEmpty(destination))
                .Select(destination => modules.First(module => module.Name == destination))
                .ToList();
        }

        foreach (ConjunctionModule conjunctionModule in modules
            .Where(module => module is ConjunctionModule)
            .Cast<ConjunctionModule>())
        {
            foreach (var module in modules)
            {
                if (module.DestinationModules.Contains(conjunctionModule))
                {
                    conjunctionModule.InputModules.Add(module, false);
                }
            }
        }

        broadcaster = (BroadcasterModule)modules.First(module => module is BroadcasterModule);
        return modules;
    }

    private record Pulse(IModule? Sender, IModule Receiver, bool Strength);

    private class BroadcasterModule : IModule
    {
        public string Name { get; } = "broadcaster";
        public List<IModule> DestinationModules { get; set; } = new();
    }

    private class ConjunctionModule : IModule
    {
        public string Name { get; }
        public List<IModule> DestinationModules { get; set; } = new();
        public Dictionary<IModule, bool> InputModules { get; set; } = new();

        public ConjunctionModule(string name)
        {
            Name = name;
        }
    }

    private class FlipFlopModule : IModule
    {
        public string Name { get; }
        public bool IsActivated { get; set; } = false;
        public List<IModule> DestinationModules { get; set; } = new();

        public FlipFlopModule(string name)
        {
            Name = name;
        }
    }

    private class TestModule : IModule
    {
        public string Name { get; }
        public List<IModule> DestinationModules { get; set; } = new();

        public TestModule(string name)
        {
            Name = name;
        }
    }

    private interface IModule
    {
        public string Name { get; }
        public List<IModule> DestinationModules { get; set; }
    }
}
