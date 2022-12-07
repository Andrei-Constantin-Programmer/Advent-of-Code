using static Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2022
{
    internal class ChallengeSolution7 : ChallengeSolution
    {
        public void SolveFirstPart()
        {
            ReadFileSystem().Print();
        }

        public void SolveSecondPart()
        {
            throw new NotImplementedException();
        }


        private Folder ReadFileSystem()
        {
            var root = new Folder("/");

            using (TextReader read = GetInputFile(2022, 7))
            {
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
                                var name => currentFolder.FindFolder(elements[2])
                            };
                        }
                    }
                    else
                    {
                        if (elements[0] == "dir")
                        {
                            currentFolder!.AddFile(new Folder(elements[1], currentFolder));
                        }
                        else
                        {
                            currentFolder!.AddFile(new TextFile(elements[1], Convert.ToInt32(elements[0]), currentFolder));
                        }
                    }
                }
            }

            return root;
        }
    }

    abstract class FileSystemObject
    {
        public string Name { get; }
        public Folder? Parent { get; }

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
        public int Size { get; init; }

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
