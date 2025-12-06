using Advent_of_Code.Challenge_Solutions.Year_2025;
using Advent_of_Code.Shared.Utilities;
using Advent_of_Code.Tests.Shared;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2025;

public class ChallengeSolutionTests02
{
    private readonly IConsole _consoleMock;
    private readonly ISolutionReader<ChallengeSolution02> _readerMock;

    private readonly ChallengeSolution02 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124"
    ];

    public ChallengeSolutionTests02()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution02>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2025, 2));

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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(1227775554)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(18893502033)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(4174379265)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(26202168557)));
    }
}
