using Advent_of_Code.Challenge_Solutions.Year_2025;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2025;

public class ChallengeSolutionTests05
{
    protected IConsole _consoleMock;
    protected ISolutionReader<ChallengeSolution05> _readerMock;

    private readonly ChallengeSolution05 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "3-5",
        "10-14",
        "16-20",
        "12-18",
        "",
        "1",
        "5",
        "8",
        "11",
        "17",
        "32"
    ];

    public ChallengeSolutionTests05()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution05>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2025, 5));

        _challengeSolution = new ChallengeSolution05(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(3)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(567)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(14)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(354149806372909)));
    }
}