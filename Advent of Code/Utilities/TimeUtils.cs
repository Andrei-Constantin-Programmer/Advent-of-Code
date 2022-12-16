using System.Diagnostics;

namespace Advent_of_Code.Utilities
{
    internal class TimeUtils
    {
        public static string GetTimeElapsed(Stopwatch watch)
        {
            return watch.Elapsed.TotalMilliseconds + " ms";
        }
    }
}
