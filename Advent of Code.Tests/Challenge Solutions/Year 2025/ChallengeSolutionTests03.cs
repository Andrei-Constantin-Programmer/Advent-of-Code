using Advent_of_Code.Challenge_Solutions.Year_2025;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2025;

public class ChallengeSolutionTests03
{
    protected IConsole _consoleMock;
    protected ISolutionReader<ChallengeSolution03> _readerMock;

    private readonly ChallengeSolution03 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "987654321111111",
        "811111111111119",
        "234234234234278",
        "818181911112111"
    ];

    public ChallengeSolutionTests03()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution03>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2025, 3));

        _challengeSolution = new ChallengeSolution03(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(357)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(17155)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(3121910778619)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(169685670469164)));
    }
}
