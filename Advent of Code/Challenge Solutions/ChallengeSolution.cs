using Advent_of_Code.Utilities;
using System.Diagnostics;

namespace Advent_of_Code.Challenge_Solutions;

public abstract class ChallengeSolution
{
    public ChallengeSolution() { }

    public void PrintSolution()
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

                watch.Start();
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

    protected abstract void SolveFirstPart();
    protected abstract void SolveSecondPart();
}
