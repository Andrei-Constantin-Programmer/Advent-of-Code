namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.Tests

module ChallengeSolutionTests01 =

    open System
    open Xunit
    open TestUtilities
    open TestExtensions
    open Challenge_Solutions.Year_2025.ChallengeSolution01

    let getSampleInput() : string list =
        [
            "R1000"
            "L68"
            "L30"
            "R48"
            "L5"
            "R60"
            "L55"
            "L1"
            "L99"
            "R14"
            "L282"
        ]

    
    [<Fact>]
    let ``SolveFirstPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = ChallengeSolution01(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 3 1


    [<Fact>]
    let ``SolveSecondPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = ChallengeSolution01(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 18 1
