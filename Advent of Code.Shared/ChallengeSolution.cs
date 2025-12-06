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
    protected readonly IConsole Console;
    protected readonly ISolutionReader<TSolution> Reader;

    protected ChallengeSolution(IConsole console, ISolutionReader<TSolution> reader)
    {
        Console = console;
        Reader = reader;
    }

    public override void PrintSolution()
    {
        var watch = new Stopwatch();

        try
        {
            System.Console.WriteLine("First part:");

            watch.Start();
            SolveFirstPart();
            watch.Stop();

            System.Console.WriteLine("Time for solving first: " + TimeUtils.GetTimeElapsed(watch));
            System.Console.WriteLine();
            try
            {
                System.Console.WriteLine("Second part:");

                watch.Restart();
                SolveSecondPart();
                watch.Stop();

                System.Console.WriteLine("Time for solving second: " + TimeUtils.GetTimeElapsed(watch));
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
            catch (NotImplementedException)
            {
                System.Console.WriteLine("The second part of this challenge has not been solved yet.");
                System.Console.WriteLine();
            }
        }
        catch (NotImplementedException)
        {
            System.Console.WriteLine("The first part of this challenge has not been solved yet.");
            System.Console.WriteLine();
        }
    }

    public abstract void SolveFirstPart();
    public abstract void SolveSecondPart();
}