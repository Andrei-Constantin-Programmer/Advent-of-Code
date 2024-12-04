namespace Advent_of_Code.Utilities;

public interface IConsole
{
    void WriteLine();
    void WriteLine(string message);
    void WriteLine(object obj);
    void Write(string message);
    void Write(object obj);
}
