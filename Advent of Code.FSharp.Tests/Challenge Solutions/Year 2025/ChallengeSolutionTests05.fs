namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.Tests

open Advent_of_Code.Tests.Shared
open Xunit
open Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.ChallengeSolution05
open Advent_of_Code.FSharp.Tests.TestUtilities
open TestExtensions

module ChallengeSolutionTests05 =

    let getSampleInput() : string list =
        [
            "3-5";
            "10-14";
            "16-20";
            "12-18";
            "";
            "1";
            "5";
            "8";
            "11";
            "17";
            "32"
        ]
        
    let getRealInput() : string list = TestHelpers.GetInputFileContents(2025, 5) |> List.ofArray

    [<Fact>]
    let ``SolveFirstPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 3 1
        
    [<Fact>]
    let ``SolveFirstPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 567 1        

    [<Fact>]
    let ``SolveSecondPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 14 1

    [<Fact>]
    let ``SolveSecondPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 354149806372909L 1       