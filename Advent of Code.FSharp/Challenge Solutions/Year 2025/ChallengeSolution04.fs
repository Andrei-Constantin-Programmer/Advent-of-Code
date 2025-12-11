// Task: https://adventofcode.com/2025/day/4

namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities
open Advent_of_Code.FSharp.Utilities

module ChallengeSolution04 =

    [<Literal>]
    let maxAdjacentPaperRolls = 4
    
    type Floor =
        | PaperRoll
        | Empty
        
    type Grid = Floor[,]
    
    type PaperRollLocation = int * int
    
    type Solution(getInput: unit -> string list, writeLine: string -> unit) =
        
        let readFloorGrid() : Grid =
            getInput()
            |> Seq.map
                ^<| Seq.map (function
                        | '@' -> PaperRoll
                        | _   -> Empty
                    )
            |> array2D
            
        let getAllPaperRolls (grid: Grid) : PaperRollLocation list =
            grid
            |> Array2D.mapi (fun row col value ->
                             match value with
                             | PaperRoll -> Some (row, col)
                             | _ -> None)
            |> Seq.cast<PaperRollLocation option>
            |> Seq.choose id
            |> List.ofSeq
            
        let getNeighbouringPaperRolls (grid: Grid) (row: int, col: int) =
            [
                (row - 1, col)
                (row + 1, col)
                (row, col - 1)
                (row, col + 1)
                (row - 1, col - 1)
                (row - 1, col + 1)
                (row + 1, col - 1)
                (row + 1, col + 1)
            ]
            |> List.filter (fun (r, c) -> r >= 0 && r < (grid |> Array2D.length1)
                                           && c >= 0 && c < (grid |> Array2D.length2)
                                           && grid[r, c] = PaperRoll)
            
        let isAccessible (grid: Grid) (roll: PaperRollLocation) =
            getNeighbouringPaperRolls grid roll
                |> List.length < maxAdjacentPaperRolls 
            
        let removeAllAccessibleRolls (grid: Grid) =
            let rec removeAllAccessibleInner removedSoFar =
                let rollsToRemove =
                    grid
                    |> getAllPaperRolls
                    |> List.filter (isAccessible grid)
                
                match rollsToRemove with
                | [] -> removedSoFar
                | _ -> rollsToRemove
                       |> List.iter (fun (row, col) -> grid[row, col] <- Empty)
                       
                       removeAllAccessibleInner (removedSoFar + rollsToRemove.Length)
            
            removeAllAccessibleInner 0
                        
        member _.SolveFirstPart() =
            let grid = readFloorGrid()

            let reachableRolls =
                grid
                |> getAllPaperRolls
                |> List.filter (isAccessible grid)
                |> List.length
                
            writeLine $"Reachable paper rolls: {reachableRolls}"
        
        member _.SolveSecondPart() : unit =
            let grid = readFloorGrid()
            let removed = removeAllAccessibleRolls grid
            
            writeLine $"Removed paper rolls: {removed}"

    type ChallengeSolution04(console: IConsole, reader: ISolutionReader<ChallengeSolution04>) =
        inherit ChallengeSolution<ChallengeSolution04>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
