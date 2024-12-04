
using Advent_of_Code.Challenge_Solutions;

namespace Advent_of_Code.Utilities;

public interface ISolutionReader<TSolution>
    where TSolution : ChallengeSolution
{
    TextReader GetInputFile();
    string[] ReadLines();
}