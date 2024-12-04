using Advent_of_Code.Utilities;

namespace Advent_of_Code.Tests;

public static class TestHelpers
{
    public static string[] GetInputFileContents(int year, int day)
    {
        var inputFilePath = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName
            + @$"\Advent of Code\resources\input\{year}\{PathUtils.FormatDay(day)}.txt";

        return File.ReadAllLines(inputFilePath);
    }

    public static bool ContainsInt(this string s, int x) => s.Contains(x.ToString());
}
