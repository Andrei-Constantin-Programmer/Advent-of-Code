using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Shared.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests03
{
    private readonly IConsole _consoleMock;
    private readonly ISolutionReader<ChallengeSolution03> _readerMock;

    private readonly ChallengeSolution03 _challengeSolution;

    public ChallengeSolutionTests03()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution03>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2024, 3));

        _challengeSolution = new ChallengeSolution03(_consoleMock, _readerMock);
    }

    [Fact]
    public void SolveFirstPart_SampleInput()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(["xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"]);

        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(161)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(184576302)));
    }

    [Fact]
    public void SolveSecondPart_SampleInput()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(["xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"]);

        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(48)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();
        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(118173507)));
    }
}
