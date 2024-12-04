using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests02
{
    protected IConsole _consoleMock;
    protected ISolutionReader<ChallengeSolution02> _readerMock;

    private readonly ChallengeSolution02 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "7 6 4 2 1",
        "1 2 7 8 9",
        "9 7 6 2 1",
        "1 3 2 4 5",
        "8 6 4 4 1",
        "1 3 6 7 9"
    ];

    public ChallengeSolutionTests02()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution02>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2024, 2));

        _challengeSolution = new ChallengeSolution02(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(2)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(326)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(4)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(381)));
    }
}
