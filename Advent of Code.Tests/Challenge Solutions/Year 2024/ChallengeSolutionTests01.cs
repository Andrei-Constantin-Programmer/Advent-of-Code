using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Shared.Utilities;
using Advent_of_Code.Tests.Shared;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests01
{
    private readonly IConsole _consoleMock;
    private readonly ISolutionReader<ChallengeSolution01> _readerMock;

    private readonly ChallengeSolution01 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "3   4",
        "4   3",
        "2   5",
        "1   3",
        "3   9",
        "3   3"
    ];

    public ChallengeSolutionTests01()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution01>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2024, 1));

        _challengeSolution = new ChallengeSolution01(_consoleMock, _readerMock);
    }

    [Fact]
    public void SolveFirstPart_SampleInput()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInput);

        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(11)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(1603498)));
    }

    [Fact]
    public void SolveSecondPart_SampleInput()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInput);

        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(31)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(25574739)));
    }
}
