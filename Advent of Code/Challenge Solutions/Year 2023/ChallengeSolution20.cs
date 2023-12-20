using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

internal class ChallengeSolution20 : ChallengeSolution
{
    private const char FLIP_FLOP_PREFIX = '%';
    private const char CONJUNCTION_PREFIX = '&';

    protected override void SolveFirstPart()
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

    protected override void SolveSecondPart()
    {
        var modules = ReadModules(out var broadcaster);
        var rxModule = modules
            .FirstOrDefault(mod => mod.Name == "rx") as TestModule
            ?? throw new NotImplementedException();
        var rxConjunction = (ConjunctionModule)modules
            .First(mod => mod.DestinationModules.Contains(rxModule));

        long buttonPress;

        Dictionary<IModule, long> cycleLengths = new();
        foreach (var inputModule in rxConjunction.InputModules.Keys)
        {
            cycleLengths.Add(inputModule, -1);
        }

        for (buttonPress = 1; ; buttonPress++)
        {
            if (cycleLengths.All(cycle => cycle.Value > -1))
            {
                break;
            }

            Queue<(IModule? sender, IModule receiver, bool strength)> pulses = new();
            pulses.Enqueue((null, broadcaster, false));

            while (pulses.TryDequeue(out var pulse))
            {
                if (pulse.sender is not null
                    && cycleLengths.ContainsKey(pulse.sender)
                    && cycleLengths[pulse.sender] == -1
                    && pulse.strength)
                {
                    cycleLengths[pulse.sender] = buttonPress;
                }

                if (pulse.receiver is FlipFlopModule flipFlop && !pulse.strength)
                {
                    flipFlop.IsActivated = !flipFlop.IsActivated;
                    foreach (var module in flipFlop.DestinationModules)
                    {
                        pulses.Enqueue((flipFlop, module, flipFlop.IsActivated));
                    }
                }
                else if (pulse.receiver is ConjunctionModule conjunction)
                {
                    conjunction.InputModules[pulse.sender!] = pulse.strength;
                    if (conjunction.InputModules.Values.All(pulse => pulse))
                    {
                        foreach (var module in conjunction.DestinationModules)
                        {
                            pulses.Enqueue((conjunction, module, false));
                        }
                    }
                    else
                    {
                        foreach (var module in conjunction.DestinationModules)
                        {
                            pulses.Enqueue((conjunction, module, true));
                        }
                    }
                }
                else if (pulse.receiver is BroadcasterModule broadcasterModule)
                {
                    foreach (var module in pulse.receiver.DestinationModules)
                    {
                        pulses.Enqueue((broadcasterModule, module, pulse.strength));
                    }
                }
            }
        }

        Console.WriteLine(cycleLengths.Values.Aggregate(1L, (x, y) => x * y));
    }

    private static (int lowPulses, int highPulses) FindPulseCounts(BroadcasterModule broadcaster)
    {
        var lowPulses = 0;
        var highPulses = 0;

        Queue<(IModule? sender, IModule receiver, bool strength)> pulses = new();
        pulses.Enqueue((null, broadcaster, false));

        while (pulses.TryDequeue(out var pulse))
        {
            if (pulse.strength)
            {
                highPulses++;
            }
            else
            {
                lowPulses++;
            }

            if (pulse.receiver is FlipFlopModule flipFlop && !pulse.strength)
            {
                flipFlop.IsActivated = !flipFlop.IsActivated;
                foreach (var module in flipFlop.DestinationModules)
                {
                    pulses.Enqueue((flipFlop, module, flipFlop.IsActivated));
                }
            }
            else if (pulse.receiver is ConjunctionModule conjunction)
            {
                conjunction.InputModules[pulse.sender!] = pulse.strength;
                if (conjunction.InputModules.Values.All(pulse => pulse))
                {
                    foreach (var module in conjunction.DestinationModules)
                    {
                        pulses.Enqueue((conjunction, module, false));
                    }
                }
                else
                {
                    foreach (var module in conjunction.DestinationModules)
                    {
                        pulses.Enqueue((conjunction, module, true));
                    }
                }
            }
            else if (pulse.receiver is BroadcasterModule broadcasterModule)
            {
                foreach (var module in pulse.receiver.DestinationModules)
                {
                    pulses.Enqueue((broadcasterModule, module, pulse.strength));
                }
            }
        }

        return (lowPulses, highPulses);
    }

    private List<IModule> ReadModules(out BroadcasterModule broadcaster)
    {
        var lines = Reader.ReadLines(this);
        List<IModule> modules = new();
        Dictionary<IModule, string[]> destinations = new();
        foreach (var line in lines)
        {
            IModule module = line[0] switch
            {
                FLIP_FLOP_PREFIX => new FlipFlopModule(line[1..line.IndexOf(' ')]),
                CONJUNCTION_PREFIX => new ConjunctionModule(line[1..line.IndexOf(' ')]),
                _ => new BroadcasterModule()
            };

            modules.Add(module);
            destinations.Add(module, line.Split("-> ")[1].Split(", "));
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

        foreach (ConjunctionModule conjunctionModule in modules.Where(module => module is ConjunctionModule))
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
        public bool LastSent { get; set; } = true;
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
