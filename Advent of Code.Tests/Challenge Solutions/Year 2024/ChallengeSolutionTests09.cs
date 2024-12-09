using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests09
{
    protected IConsole _consoleMock;
    protected ISolutionReader<ChallengeSolution09> _readerMock;

    private readonly ChallengeSolution09 _challengeSolution;

    private readonly string[] _sampleInputSmall = ["12345"];
    private readonly string[] _sampleInputMedium = ["2333133121414131402"];

    public ChallengeSolutionTests09()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution09>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2024, 9));

        _challengeSolution = new ChallengeSolution09(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(60)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(1928)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(6320029754031)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(132)));
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(2858)));
    }

    [Fact]
    public void SolveSecondPart_RealInput()
    {
        // Act
        _challengeSolution.SolveSecondPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsLong(6347435485773)));
    }
}
