namespace Advent_of_Code
{
    internal class Reader
    {
        public static TextReader GetInputFile(int year, int day)
        {
            return File.OpenText(GetFilePath(FileType.Input, year, day));
        }

        public static StreamWriter GetOutputFile(int year, int day)
        {
            return new StreamWriter(GetFilePath(FileType.Output, year, day));
        }

        public static string GetFilePath(FileType fileType, int year, int day)
        {
            return Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName + @$"\resources\{fileType.ToString().ToLower()}\{year}_{day}.txt";
        }

        public enum FileType
        {
            Input,
            Output,
        }
    }
}
