using Advent_of_Code.Utilities;
using System.Diagnostics;

namespace Advent_of_Code.Challenge_Solutions;

public abstract class ChallengeSolution
{
    protected readonly IConsole _console;

    public ChallengeSolution(IConsole console)
    {
        _console = console;
    }

    public void PrintSolution()
    {
        var watch = new Stopwatch();

        try
        {
            _console.WriteLine("First part:");

            watch.Start();
            SolveFirstPart();
            watch.Stop();

            _console.WriteLine("Time for solving first: " + TimeUtils.GetTimeElapsed(watch));
            _console.WriteLine();
            try
            {
                _console.WriteLine("Second part:");

                watch.Start();
                SolveSecondPart();
                watch.Stop();

                _console.WriteLine("Time for solving second: " + TimeUtils.GetTimeElapsed(watch));
                _console.WriteLine();
                _console.WriteLine();
            }
            catch (NotImplementedException)
            {
                _console.WriteLine("The second part of this challenge has not been solved yet.");
                _console.WriteLine();
            }
        }
        catch (NotImplementedException)
        {
            _console.WriteLine("The first part of this challenge has not been solved yet.");
            _console.WriteLine();
        }
    }

    public abstract void SolveFirstPart();
    public abstract void SolveSecondPart();
}
