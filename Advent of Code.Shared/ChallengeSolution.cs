using System.Diagnostics;
using Advent_of_Code.Shared.Utilities;
using JetBrains.Annotations;

namespace Advent_of_Code.Shared;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
public abstract class ChallengeSolution
{
    public abstract void PrintSolution();
}

public abstract class ChallengeSolution<TSolution> : ChallengeSolution
    where TSolution : ChallengeSolution
{
    protected readonly IConsole _console;
    protected readonly ISolutionReader<TSolution> _reader;

    protected ChallengeSolution(IConsole console, ISolutionReader<TSolution> reader)
    {
        _console = console;
        _reader = reader;
    }

    public override void PrintSolution()
    {
        var watch = new Stopwatch();

        try
        {
            Console.WriteLine("First part:");

            watch.Start();
            SolveFirstPart();
            watch.Stop();

            Console.WriteLine("Time for solving first: " + TimeUtils.GetTimeElapsed(watch));
            Console.WriteLine();
            try
            {
                Console.WriteLine("Second part:");

                watch.Restart();
                SolveSecondPart();
                watch.Stop();

                Console.WriteLine("Time for solving second: " + TimeUtils.GetTimeElapsed(watch));
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("The second part of this challenge has not been solved yet.");
                Console.WriteLine();
            }
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("The first part of this challenge has not been solved yet.");
            Console.WriteLine();
        }
    }

    public abstract void SolveFirstPart();
    public abstract void SolveSecondPart();
}