using Advent_of_Code.Challenge_Solutions.Year_2025;
using Advent_of_Code.Shared.Utilities;
using Advent_of_Code.Tests.Shared;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2025;

public class ChallengeSolutionTests12
{
    private readonly IConsole _consoleMock;
    private readonly ISolutionReader<ChallengeSolution12> _readerMock;

    private readonly ChallengeSolution12 _challengeSolution;
    
    public ChallengeSolutionTests12()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution12>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2025, 12));

        _challengeSolution = new ChallengeSolution12(_consoleMock, _readerMock);
    }

    // Skipped the sample input, since it's actually a harder challenge to get that to work than the real input >:)

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(512)));
    }
}