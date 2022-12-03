namespace Advent_of_Code
{
    internal class Utilities
    {
        public static TextReader GetInputFile(int year, int day)
        {
            return File.OpenText(GetFileString(FileType.Input, year, day));
        }

        public static StreamWriter GetOutputFile(int year, int day)
        {
            return new StreamWriter(GetFileString(FileType.Output, year, day));
        }

        public static string GetFileString(FileType fileType, int year, int day)
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @$"\resources\{fileType.ToString().ToLower()}\{year}_{day}.txt";
        }

        public enum FileType
        {
            Input,
            Output,
        }
    }
}
