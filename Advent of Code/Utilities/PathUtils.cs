namespace Advent_of_Code.Utilities;

public static class PathUtils
{
    public static StreamWriter GetOutputFile(int year, int day) => new(GetFilePath(FileType.Output, year, day));

    public static string GetFilePath(FileType fileType, int year, int day) =>
        Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.FullName
            + @$"\resources\{fileType.ToString().ToLower()}\{year}\{FormatDay(day)}.txt";

    public static string FormatDay(int day) => day < 10 ? $"0{day}" : $"{day}";

    public enum FileType
    {
        Input,
        Output,
    }
}
