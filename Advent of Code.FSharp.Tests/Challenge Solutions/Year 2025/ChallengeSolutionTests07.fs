namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.Tests

open Advent_of_Code.Tests.Shared
open Xunit
open Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.ChallengeSolution07
open Advent_of_Code.FSharp.Tests.TestUtilities
open TestExtensions

module ChallengeSolutionTests07 =

    let getSampleInput() : string list =
        [
            ".......S.......";
            "...............";
            ".......^.......";
            "...............";
            "......^.^......";
            "...............";
            ".....^.^.^.....";
            "...............";
            "....^.^...^....";
            "...............";
            "...^.^...^.^...";
            "...............";
            "..^...^.....^..";
            "...............";
            ".^.^.^.^.^...^.";
            "..............."
        ]
        
    let getRealInput() : string list = TestHelpers.GetInputFileContents(2025, 7) |> List.ofArray

    [<Fact>]
    let ``SolveFirstPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 21 1
        
    [<Fact>]
    let ``SolveFirstPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 1638 1        

    [<Fact>]
    let ``SolveSecondPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 40 1

    [<Fact>]
    let ``SolveSecondPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 7759107121385L 1       