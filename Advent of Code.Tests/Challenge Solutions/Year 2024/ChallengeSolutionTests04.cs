﻿using Advent_of_Code.Challenge_Solutions.Year_2024;
using Advent_of_Code.Utilities;
using NSubstitute;

namespace Advent_of_Code.Tests.Challenge_Solutions.Year_2024;

public class ChallengeSolutionTests04
{
    protected IConsole _consoleMock;
    protected ISolutionReader<ChallengeSolution04> _readerMock;

    private readonly ChallengeSolution04 _challengeSolution;

    private readonly string[] _sampleInput =
    [
        "MMMSXXMASM",
        "MSAMXMSMSA",
        "AMXSXMAAMM",
        "MSAMASMSMX",
        "XMASAMXAMM",
        "XXAMMXXAMA",
        "SMSMSASXSS",
        "SAXAMASAAA",
        "MAMMMXMMMM",
        "MXMXAXMASX",
    ];

    public ChallengeSolutionTests04()
    {
        _consoleMock = Substitute.For<IConsole>();
        _readerMock = Substitute.For<ISolutionReader<ChallengeSolution04>>();

        _readerMock.ReadLines()
            .Returns(TestHelpers.GetInputFileContents(2024, 4));

        _challengeSolution = new ChallengeSolution04(_consoleMock, _readerMock);
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
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(18)));
    }

    [Fact]
    public void SolveFirstPart_RealInput()
    {
        // Act
        _challengeSolution.SolveFirstPart();

        // Assert
        _consoleMock.Received().WriteLine(Arg.Is<string>(s => s.ContainsInt(2567)));
    }
}
