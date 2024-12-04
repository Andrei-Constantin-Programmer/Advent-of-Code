// Task: https://adventofcode.com/2023/day/22

using Advent_of_Code.Utilities;

namespace Advent_of_Code.Challenge_Solutions.Year_2023;

public class ChallengeSolution22(IConsole console) : ChallengeSolution(console)
{
    public override void SolveFirstPart()
    {
        _console.WriteLine(
            ReadToppledCounts()
            .Count(toppleCount => toppleCount == 0));
    }

    public override void SolveSecondPart()
    {
        _console.WriteLine(
            ReadToppledCounts()
            .Sum());
    }

    private List<int> ReadToppledCounts()
    {
        var bricks = ReadBricks();
        var stabilisedBricks = DropBricks(bricks);
        return GetToppledCountsAfterDisintegration(stabilisedBricks);
    }

    private static List<int> GetToppledCountsAfterDisintegration(List<Brick> bricks)
    {
        List<int> toppledCounts = new();

        GetSupportBricks(bricks, out var supports, out var supported);

        foreach (var disintegratingBrick in bricks)
        {
            HashSet<Brick> removedBricks = new();
            Queue<Brick> brickRemovalQueue = new();
            brickRemovalQueue.Enqueue(disintegratingBrick);

            while (brickRemovalQueue.TryDequeue(out var removedBrick))
            {
                removedBricks.Add(removedBrick);

                var endangeredBricks = supported[removedBrick]
                    .Where(brick => removedBricks.ContainsAll(supports[brick]));

                foreach (var endangeredBrick in endangeredBricks)
                {
                    brickRemovalQueue.Enqueue(endangeredBrick);
                }
            }

            toppledCounts.Add(removedBricks.Count - 1);
        }

        return toppledCounts;
    }

    private static void GetSupportBricks(
        List<Brick> bricks,
        out Dictionary<Brick, HashSet<Brick>> supportBricks,
        out Dictionary<Brick, HashSet<Brick>> supportedBricks)
    {
        supportBricks = bricks.ToDictionary(brick => brick, _ => new HashSet<Brick>());
        supportedBricks = bricks.ToDictionary(brick => brick, _ => new HashSet<Brick>());

        for (var supporting = 0; supporting < bricks.Count - 1; supporting++)
        {
            for (var supported = supporting + 1; supported < supportedBricks.Count; supported++)
            {
                var supportingBrick = bricks[supporting];
                var supportedBrick = bricks[supported];

                if (AreVerticallyTouching(supportingBrick, supportedBrick)
                    && AreHorizontallyIntersecting(supportingBrick, supportedBrick))
                {
                    supportBricks[supportedBrick].Add(supportingBrick);
                    supportedBricks[supportingBrick].Add(supportedBrick);
                }
            }
        }
    }

    private static List<Brick> DropBricks(List<Brick> bricks)
    {
        var bricksAfterDrop = bricks
            .OrderBy(brick => brick.Bottom)
            .ToList();

        for (var level = 0; level < bricksAfterDrop.Count; level++)
        {
            var dropLevel = 1;
            for (var previousLevel = 0; previousLevel < level; previousLevel++)
            {
                if (AreHorizontallyIntersecting(bricksAfterDrop[level], bricksAfterDrop[previousLevel]))
                {
                    dropLevel = Math.Max(dropLevel, bricksAfterDrop[previousLevel].Top + 1);
                }
            }

            var levelsDropped = bricksAfterDrop[level].Bottom - dropLevel;
            bricksAfterDrop[level].Drop(levelsDropped);
        }

        return bricksAfterDrop;
    }

    private static bool AreVerticallyTouching(Brick supportingBrick, Brick supportedBrick)
        => supportingBrick.Top == supportedBrick.Bottom - 1;

    private static bool AreHorizontallyIntersecting(Brick brick1, Brick brick2)
        => AreIntersecting(brick1.X, brick2.X)
        && AreIntersecting(brick1.Y, brick2.Y);

    private static bool AreIntersecting(Range range1, Range range2)
        => range1.Start <= range2.End
        && range2.Start <= range1.End;

    private List<Brick> ReadBricks()
    {
        var lines = Reader.ReadLines(this);
        List<Brick> bricks = new();

        foreach (var line in lines)
        {
            var coordinates = line.Split('~');
            var startCoordinates = CoordinateCsvToNumericalList(coordinates[0]);
            var endCoordinates = CoordinateCsvToNumericalList(coordinates[1]);

            bricks.Add(new Brick(x: new(startCoordinates[0], endCoordinates[0]),
                                 y: new(startCoordinates[1], endCoordinates[1]),
                                 z: new(startCoordinates[2], endCoordinates[2])));
        }

        return bricks;

        static List<int> CoordinateCsvToNumericalList(string coordinates) => coordinates.Split(',').Select(int.Parse).ToList();
    }

    private record Brick
    {
        public Range X { get; private set; }
        public Range Y { get; private set; }
        public Range Z { get; private set; }

        public int Bottom => Z.Start;
        public int Top => Z.End;

        public Brick(Range x, Range y, Range z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Drop(int levels) => Z = new(Bottom - levels, Top - levels);
    }

    private record Range(int Start, int End);
}

public static class CollectionExtensions
{
    public static bool ContainsAll<T>(this ICollection<T> collection, ICollection<T> subCollection) =>
        subCollection.All(x => collection.Contains(x));
}