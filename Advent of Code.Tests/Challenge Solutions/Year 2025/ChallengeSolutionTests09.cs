using Advent_of_Code.Challenge_Solutions.Year_2025;
using Advent_of_Code.Shared.Utilities;
using Advent_of_Code.Tests.Shared;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2025;

public class ChallengeSolutionTests09
{
    private readonly IConsole _consoleMock;
    private readonly ISolutionReader<ChallengeSolution09> _readerMock;

    private readonly ChallengeSolution09 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "7,1",
        "11,1",
        "11,7",
        "9,7",
        "9,5",
        "2,5",
        "2,3",
        "7,3",
    ];

    public ChallengeSolutionTests09()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution09>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2025, 9));

        _challengeSolution = new ChallengeSolution09(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(50)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(4749929916)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(24)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(1572047142)));
    }
}