using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests01
{
    protected IConsole _consoleMock;

    private readonly ChallengeSolution01 _challengeSolution01;

    public ChallengeSolutionTests01()
    {
        _consoleMock = Substitute.For<IConsole>();

        _challengeSolution01 = new ChallengeSolution01(_consoleMock);
    }

    [Fact]
    public void SolveFirstPart_SampleInput()
    {
        // Arrange


        // Act

        // Assert
    }
}
