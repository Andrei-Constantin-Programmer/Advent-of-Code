namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.Tests

open Advent_of_Code.Tests.Shared
open Xunit
open Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.ChallengeSolution03
open Advent_of_Code.FSharp.Tests.TestUtilities
open TestExtensions

module ChallengeSolutionTests03 =

    let getSampleInput() : string list =
        [
            "987654321111111";
            "811111111111119";
            "234234234234278";
            "818181911112111"
        ]
        
    let getRealInput() : string list = TestHelpers.GetInputFileContents(2025, 3) |> List.ofArray

    [<Fact>]
    let ``SolveFirstPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 357 1
        
    [<Fact>]
    let ``SolveFirstPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 17155 1        

    [<Fact>]
    let ``SolveSecondPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 3121910778619L 1

    [<Fact>]
    let ``SolveSecondPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 169685670469164L 1       