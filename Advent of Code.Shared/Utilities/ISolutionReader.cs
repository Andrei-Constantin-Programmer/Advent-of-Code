using System.Diagnostics.CodeAnalysis;

namespace Advent_of_Code.Shared.Utilities;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface ISolutionReader<TSolution> where TSolution : ChallengeSolution
{
    TextReader GetInputFile();
    string[] ReadLines();
}