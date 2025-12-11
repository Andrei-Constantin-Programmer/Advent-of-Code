// Task: https://adventofcode.com/2025/day/5

namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open System
open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution05 =

    type Range =
        { Start: int64; End: int64 }
        with
        member this.Count =
            this.End - this.Start + 1L
        
        member this.Contains value =
            this.Start <= value && value <= this.End
            
        static member tryMerge (range1: Range) (range2: Range) : Range option =
            match range1, range2 with
            | r1, r2 when r1.Start > r2.End || r1.End < r2.Start -> None
            | _ ->
                let start = min range1.Start range2.Start
                let rangeEnd = max range1.End range2.End
                Some { Start = start; End = rangeEnd }
    
    type Solution(getInput: unit -> string list, writeLine: string -> unit) =

        let foldRanges (acc: Range list) (range: Range) =
            match acc with
            | [] -> [range]
            | last :: remaining
                ->  match Range.tryMerge last range with
                    | Some merged -> merged :: remaining
                    | None -> range :: acc
        
        let readRangesAndIngredients() : Range list * int64 list =
            let lines = getInput()
            let splitIndex = lines |> List.findIndex String.IsNullOrWhiteSpace
            let rangeLines, ingredientLines = List.splitAt splitIndex lines
            
            let ranges = rangeLines
                         |> List.map (fun (line: string)
                                        -> line.Split('-')
                                            |> Array.map int64
                                            |> fun array -> { Start = array[0]; End = array[1] })
                         |> List.sortBy _.Start
                         |> List.fold foldRanges []
            let ingredients = ingredientLines |> List.skip 1 |> List.map int64
            (ranges, ingredients)
                        
        member _.SolveFirstPart() =
            let ranges, ingredients = readRangesAndIngredients()
            
            let freshIngredients =
                ingredients
                |> List.filter (fun ingredient
                                 -> ranges
                                    |> List.exists _.Contains(ingredient))
                |> List.length
                
            writeLine $"Fresh ingredients: {freshIngredients}"
        
        member _.SolveSecondPart() : unit =
            let ranges, _ = readRangesAndIngredients()
            
            let freshIngredients =
                ranges
                |> List.map _.Count
                |> List.sum
            
            writeLine $"Fresh ingredients: {freshIngredients}"

    type ChallengeSolution05(console: IConsole, reader: ISolutionReader<ChallengeSolution05>) =
        inherit ChallengeSolution<ChallengeSolution05>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
