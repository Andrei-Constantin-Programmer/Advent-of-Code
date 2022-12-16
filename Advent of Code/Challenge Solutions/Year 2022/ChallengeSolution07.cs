// Task: https://adventofcode.com/2022/day/7

using System.Diagnostics;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution07 : ChallengeSolution
    {
        private const int MAX_SIZE = 100000;
        private const int AVAILABLE = 70000000;
        private const int NEEDED = 30000000;

        private Folder root;
        private List<Folder> folders;

        public ChallengeSolution07()
        {
            root = new Folder("/");
            folders = new List<Folder>() { root };
            ReadFileSystem();
        }

        public void SolveFirstPart()
        {            
            int sum = 0;

            sum += folders
                .Select(folder => folder.Size < MAX_SIZE ? folder.Size : 0)
                .Sum();

            Console.WriteLine(sum);
        }

        public void SolveSecondPart()
        {
            var orderedFolders = folders
                .OrderBy(folder => folder.Size);

            foreach(var folder in orderedFolders)
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
            using TextReader read = Reader.GetInputFile(2022, 7);

            var currentFolder = root;
            string? line;
            while ((line = read.ReadLine()) != null && line.Trim().Length > 0)
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
                        var newFolder = new Folder(elements[1], currentFolder);
                        currentFolder!.AddFile(newFolder);

                        folders.Add(newFolder);
                    }
                    else
                    {
                        currentFolder!.AddFile(new TextFile(elements[1], Convert.ToInt32(elements[0]), currentFolder));
                    }
                }
            }
        }
    }

    abstract class FileSystemObject
    {
        public string Name { get; }
        public Folder? Parent { get; }
        public abstract int Size { get; }

        public FileSystemObject(string name, Folder? parent = null)
        {
            Name = name;
            Parent = parent;
        }

        public virtual void Print()
        {
            if (Parent != null)
                Console.Write($" ");
        }
    }

    class TextFile : FileSystemObject
    {
        public override int Size { get; }

        public TextFile(string name, int size, Folder? parent = null) : base(name, parent)
        {
            Size = size;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"- {Name} (file, size = {Size})");
        }
    }

    class Folder : FileSystemObject
    {
        List<FileSystemObject> Files { get; init; }

        public override int Size => Files.Select(file => file.Size).Sum();

        public Folder(string name, Folder? parent = null) : base(name, parent)
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
            Console.WriteLine($"- {Name} (dir)");
            foreach(var file in Files)
            {
                var parent = Parent;
                while(parent != null)
                {
                    Console.Write(" ");
                    parent = parent.Parent;
                }
                file.Print();
            }
        }
    }
}
