using Advent_of_Code.Challenge_Solutions.Year_2023;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2023;

public class ChallengeSolutionTests21
{
    protected IConsole _consoleMock;
    protected ISolutionReader<ChallengeSolution21> _readerMock;

    private readonly ChallengeSolution21 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "...........",
        ".....###.#.",
        ".###.##..#.",
        "..#.#...#..",
        "....#.#....",
        ".##..S####.",
        ".##..#...#.",
        ".......##..",
        ".##.#.####.",
        ".##..##.##.",
        "...........",
    ];

    public ChallengeSolutionTests21()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution21>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2023, 21));

        _challengeSolution = new ChallengeSolution21(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is(42));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is(3642));
    }

    [Fact(Skip = "Takes too long")]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is(608603023105276));
    }
}
