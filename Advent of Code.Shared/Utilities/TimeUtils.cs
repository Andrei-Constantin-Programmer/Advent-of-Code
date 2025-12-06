using System.Diagnostics;

namespace Advent_of_Code.Shared.Utilities;

internal static class TimeUtils
{
    public static string GetTimeElapsed(Stopwatch watch) => watch.Elapsed.TotalMilliseconds + " ms";
}
