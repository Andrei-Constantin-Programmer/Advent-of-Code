namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.Tests

open Advent_of_Code.Tests.Shared
open Xunit
open Advent_of_Code.FSharp.Challenge_Solutions.Year_2025.ChallengeSolution08
open Advent_of_Code.FSharp.Tests.TestUtilities
open TestExtensions

module ChallengeSolutionTests08 =

    let getSampleInput() : string list =
        [
            "10";
            "162,817,812";
            "57,618,57";
            "906,360,560";
            "592,479,940";
            "352,342,300";
            "466,668,158";
            "542,29,236";
            "431,825,988";
            "739,650,466";
            "52,470,668";
            "216,146,977";
            "819,987,18";
            "117,168,530";
            "805,96,715";
            "346,949,466";
            "970,615,88";
            "941,993,340";
            "862,61,35";
            "984,92,344";
            "425,690,689"
        ]
        
    let getRealInput() : string list = TestHelpers.GetInputFileContents(2025, 8) |> List.ofArray

    [<Fact>]
    let ``SolveFirstPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 40 1
        
    [<Fact>]
    let ``SolveFirstPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveFirstPart()

        // Assert
        writeLineFake.VerifyCalledContainingInt 75680 1        

    [<Fact>]
    let ``SolveSecondPart with sample input`` () =
        // Arrange
        let writeLineFake = WriteLineFake()

        let challengeSolution = Solution(getSampleInput, writeLineFake.WriteLine)

        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 25272 1

    [<Fact>]
    let ``SolveSecondPart with real input``() =
        // Arrange
        let writeLineFake = WriteLineFake()
        
        let challengeSolution = Solution(getRealInput, writeLineFake.WriteLine)
    
        // Act
        challengeSolution.SolveSecondPart()

        // Assert
        writeLineFake.VerifyCalledContainingLong 8995844880L 1       