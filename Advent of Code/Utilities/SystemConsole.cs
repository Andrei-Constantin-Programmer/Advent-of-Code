namespace Advent_of_Code.Utilities;

public class SystemConsole : IConsole
{
    public void WriteLine() => Console.WriteLine();

    public void WriteLine(string message) => Console.WriteLine(message);
    public void WriteLine(char[] message) => Console.WriteLine(message);

    public void WriteLine(object obj) => Console.WriteLine(obj);

    public void Write(string message) => Console.Write(message);

    public void Write(object obj) => Console.Write(obj);
}
