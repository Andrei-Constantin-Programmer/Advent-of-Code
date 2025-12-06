using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Shared.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests12
{
    private readonly IConsole _consoleMock;
    private readonly ISolutionReader<ChallengeSolution12> _readerMock;

    private readonly ChallengeSolution12 _challengeSolution;

    private readonly string[] _sampleInputSmall =
    [
        "AAAA",
        "BBCD",
        "BBCC",
        "EEEC",
    ];

    private readonly string[] _sampleInputMedium =
    [
        "OOOOO",
        "OXOXO",
        "OOOOO",
        "OXOXO",
        "OOOOO",
    ];

    private readonly string[] _sampleInputLarge =
    [
        "RRRRIICCFF",
        "RRRRIICCCF",
        "VVRRRCCFFF",
        "VVRCCCJFFF",
        "VVVVCJJCFE",
        "VVIVCCJJEE",
        "VVIIICJJEE",
        "MIIIIIJJEE",
        "MIIISIJEEE",
        "MMMISSJEEE",
    ];

    public ChallengeSolutionTests12()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution12>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2024, 12));

        _challengeSolution = new ChallengeSolution12(_consoleMock, _readerMock);
    }

    [Fact]
    public void SolveFirstPart_SampleInputSmall()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInputSmall);

        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(140)));
    }

    [Fact]
    public void SolveFirstPart_SampleInputMedium()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInputMedium);

        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(772)));
    }

    [Fact]
    public void SolveFirstPart_SampleInputLarge()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInputLarge);

        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(1930)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(1533024)));
    }

    [Fact]
    public void SolveSecondPart_SampleInputSmall()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInputSmall);

        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(80)));
    }

    [Fact]
    public void SolveSecondPart_SampleInputMedium()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInputMedium);

        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(436)));
    }

    [Fact]
    public void SolveSecondPart_SampleLarge()
    {
        // Arrange
        _readerMock.ReadLines()
            .Returns(_sampleInputLarge);

        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(1206)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(910066)));
    }
}
