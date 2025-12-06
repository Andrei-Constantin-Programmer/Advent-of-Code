namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.Tests

open Advent_of_Code.Tests.Shared
open Xunit
open Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.ChallengeSolution01
open Advent_of_Code.FSharp.Tests.TestUtilities
open TestExtensions

module ChallengeSolutionTests01 =

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
        
    let getRealInput() : string list = TestHelpers.GetInputFileContents(2025, 1) |> List.ofArray

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
        writeLineFake.VerifyCalledContainingInt 1081 1        

    [<Fact>]
    let ``SolveSecondPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 18 1

    [<Fact>]
    let ``SolveSecondPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 6689 1       