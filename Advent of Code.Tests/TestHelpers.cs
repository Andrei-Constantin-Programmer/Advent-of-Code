using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Tests;

public static class TestHelpers
{
    public const string NotYetFinished = "Challenge not yet solved";

    public static string[] GetInputFileContents(int year, int day)
    {
        var inputFilePath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName
                            + @$"\resources\input\{year}\{PathUtils.FormatDay(day)}.txt";

        return File.ReadAllLines(inputFilePath);
    }

    public static bool ContainsInt(this string s, int x) => s.Contains(x.ToString());

    public static bool ContainsLong(this string s, long x) => s.Contains(x.ToString());
}