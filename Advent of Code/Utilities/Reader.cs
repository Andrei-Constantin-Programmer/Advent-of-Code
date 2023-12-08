using Advent_of_Code.Challenge_Solutions;

namespace Advent_of_Code.Utilities;

internal class Reader
{
    public static string[] ReadLines(ChallengeSolution solution)
    {
        Type solutionType = solution.GetType();
        var year = int.Parse(solutionType.Namespace![^4..]);
        var day = int.Parse(new string(
            solutionType.Name
            .Reverse()
            .TakeWhile(char.IsDigit)
            .Reverse()
            .ToArray()));

        return File.ReadAllLines(GetFilePath(FileType.Input, year, day));
    }

    public static TextReader GetInputFile(int year, int day) => File.OpenText(GetFilePath(FileType.Input, year, day));

    public static StreamWriter GetOutputFile(int year, int day) => new(GetFilePath(FileType.Output, year, day));

    public static string GetFilePath(FileType fileType, int year, int day) =>
        Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName + @$"\resources\{fileType.ToString().ToLower()}\{year}_{day}.txt";

    public enum FileType
    {
        Input,
        Output,
    }
}
