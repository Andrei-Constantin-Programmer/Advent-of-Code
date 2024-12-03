using Advent_of_Code.Challenge_Solutions;

namespace Advent_of_Code.Utilities;

internal class Reader
{
    public static string[] ReadLines(ChallengeSolution solution)
    {
        var (year, day) = GetYearAndDayFromSolution(solution);

        return File.ReadAllLines(GetFilePath(FileType.Input, year, day));
    }

    public static TextReader GetInputFile(ChallengeSolution solution)
    {
        var (year, day) = GetYearAndDayFromSolution(solution);

        return File.OpenText(GetFilePath(FileType.Input, year, day));
    }

    private static (int year, int day) GetYearAndDayFromSolution(ChallengeSolution solution)
    {
        Type solutionType = solution.GetType();
        var year = int.Parse(solutionType.Namespace![^4..]);
        var day = int.Parse(new string(
            solutionType.Name
            .Reverse()
            .TakeWhile(char.IsDigit)
            .Reverse()
            .ToArray()));

        return (year, day);
    }

    public static StreamWriter GetOutputFile(int year, int day) => new(GetFilePath(FileType.Output, year, day));

    private static string GetFilePath(FileType fileType, int year, int day) =>
        Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName + @$"\resources\{fileType.ToString().ToLower()}\{year}\{FormatDay(day)}.txt";

    public enum FileType
    {
        Input,
        Output,
    }

    public static string FormatDay(int day) => day < 10 ? $"0{day}" : $"{day}";
}
