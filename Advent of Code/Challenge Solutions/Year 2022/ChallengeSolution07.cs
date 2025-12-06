// Task: https://adventofcode.com/2022/day/7

using Advent_of_Code.Shared;
using Advent_of_Code.Shared.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022;

public class ChallengeSolution07 : ChallengeSolution<ChallengeSolution07>
{
    private const int MAX_SIZE = 100000;
    private const int AVAILABLE = 70000000;
    private const int NEEDED = 30000000;

    private Folder root;
    private List<Folder> folders;

    public ChallengeSolution07(IConsole console, ISolutionReader<ChallengeSolution07> reader) : base(console, reader)
    {
        root = new Folder(Console, "/");
        folders = new List<Folder>() { root };
        ReadFileSystem();
    }

    public override void SolveFirstPart()
    {
        int sum = 0;

        sum += folders
            .Select(folder => folder.Size < MAX_SIZE ? folder.Size : 0)
            .Sum();

        Console.WriteLine(sum);
    }

    public override void SolveSecondPart()
    {
        var orderedFolders = folders
            .OrderBy(folder => folder.Size);

        foreach (var folder in orderedFolders)
        {
            var used = root.Size - folder.Size;
            if (AVAILABLE - used >= NEEDED)
            {
                Console.WriteLine(folder.Size);
                return;
            }
        }

        Console.WriteLine(root);
    }

    private void ReadFileSystem()
    {
        var currentFolder = root;
        foreach (var line in Reader.ReadLines())
        {
            var elements = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (elements[0] == "$")
            {
                if (elements[1] == "cd")
                {
                    currentFolder = elements[2] switch
                    {
                        "/" => root,
                        ".." => currentFolder!.Parent!,
                        var name => currentFolder!.FindFolder(elements[2])
                    };
                }
            }
            else
            {
                if (elements[0] == "dir")
                {
                    var newFolder = new Folder(Console, elements[1], currentFolder);
                    currentFolder!.AddFile(newFolder);

                    folders.Add(newFolder);
                }
                else
                {
                    currentFolder!.AddFile(new TextFile(Console, elements[1], Convert.ToInt32(elements[0]), currentFolder));
                }
            }
        }
    }
}

abstract class FileSystemObject
{
    protected readonly IConsole _console;

    public string Name { get; }
    public Folder? Parent { get; }
    public abstract int Size { get; }

    public FileSystemObject(IConsole console, string name, Folder? parent = null)
    {
        _console = console;

        Name = name;
        Parent = parent;
    }

    public virtual void Print()
    {
        if (Parent != null)
            _console.Write($" ");
    }
}

class TextFile : FileSystemObject
{
    public override int Size { get; }

    public TextFile(IConsole console, string name, int size, Folder? parent = null) : base(console, name, parent)
    {
        Size = size;
    }

    public override void Print()
    {
        base.Print();
        _console.WriteLine($"- {Name} (file, size = {Size})");
    }
}

class Folder : FileSystemObject
{
    List<FileSystemObject> Files { get; init; }

    public override int Size => Files.Select(file => file.Size).Sum();

    public Folder(IConsole console, string name, Folder? parent = null) : base(console, name, parent)
    {
        Files = new List<FileSystemObject>();
    }

    public void AddFile(FileSystemObject file)
    {
        Files.Add(file);
    }

    public Folder? FindFolder(string name)
    {
        return Files
            .Where(file => file.GetType() == typeof(Folder))
            .FirstOrDefault(folder => folder.Name == name)
            as Folder;
    }

    public override void Print()
    {
        base.Print();
        _console.WriteLine($"- {Name} (dir)");
        foreach (var file in Files)
        {
            var parent = Parent;
            while (parent != null)
            {
                _console.Write(" ");
                parent = parent.Parent;
            }
            file.Print();
        }
    }
}
