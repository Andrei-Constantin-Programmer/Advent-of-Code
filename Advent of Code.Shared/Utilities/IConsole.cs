namespace Advent_of_Code.Shared.Utilities;

public interface IConsole
{
    void WriteLine();
    void WriteLine(string message);
    void WriteLine(char[] message);
    void WriteLine(object obj);
    void Write(string message);
    void Write(object obj);
}
