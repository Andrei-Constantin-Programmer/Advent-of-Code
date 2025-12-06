namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open Advent_of_Code.FSharp.Utilities
open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution02 =

    type Range =
        {
            Start: string
            End: string
        }
    
    type Solution(getInput: unit -> string list, writeLine: string -> unit) =
        
        let readRanges() : Range array =
            getInput()
            |> List.head
            |> fun input -> input.Split ','
            |> Array.map
                ^<| fun range -> range.Split '-'
                    |> Array.map (fun splitRange ->
                        {
                            Start = splitRange[0]
                            End = splitRange[1]
                        })
                    
        let getSingleSizeRanges (ranges: Range array) : Range array =
            
            let rec getSubRanges (start: string) (final: string) : Range list =
                match start.Length < final.Length with
                | false -> [{ Start = start; End = final }]
                | true ->
                    let newStart = "1" + String.replicate start.Length "0"
                    let subRangeEnd = String.replicate start.Length "9"
                    [{ Start = start; End = subRangeEnd }] @ getSubRanges newStart final        
            
            ranges
            |> Array.collect (fun range ->
                getSubRanges range.Start range.End
                |> Array.ofList)

        let asDigits (number: int64) : int64 list =
            let rec asDigits (number: int64) : int64 list =
                match number with
                | 0L -> []
                | _ ->
                    let digit = number % 10L
                    (asDigits (number / 10L)) @ [digit]
                    
            match number with
            | 0L -> [0L]
            | _ -> asDigits number 
                
        let rec toNumber (digits: int64 list) : int64 =
            match digits with
            | [] -> 0
            | d::ds ->
                let number = toNumber ds
                number * 10L + d
                
        let areHalvesEqual (number: int64) : bool =
            let digits = number |> asDigits
            
            match digits.Length % 2 with
            | 0 ->
                digits
                |> List.splitAt (List.length digits / 2)
                |> fun (h1, h2) -> (toNumber h1, toNumber h2)
                |> fun (h1, h2) -> h1 = h2
            | _ -> false
            
        let isMadeOfRepeatedSequences (number: int64) : bool =
            let chunkNumber (number: int64) (sequenceLength: int) : string array =
                number
                |> string
                |> Seq.chunkBySize sequenceLength
                |> Seq.map System.String
                |> Seq.toArray
                
            let digits = number |> asDigits
            
            [|1..(digits.Length / 2)|]
            |> Array.filter (fun seqLength -> digits.Length % seqLength = 0)
            |> Array.map (chunkNumber number)
            |> Array.exists (fun chunks -> chunks |> Array.forall (fun chunk -> chunk = chunks[0]))
        
        let computeInvalidIdSum (isInvalid: int64 -> bool) : int64 =
            readRanges()
            |> getSingleSizeRanges
            |> Array.map (fun range ->
                [|(range.Start |> int64)..(range.End |> int64)|]
                |> Array.filter isInvalid
                |> Array.sum)
            |> Array.sum
        
        member _.SolveFirstPart() =
            let invalidIdSum = computeInvalidIdSum areHalvesEqual
            writeLine $"Invalid ID sum: {invalidIdSum}"

        member _.SolveSecondPart() : unit =
            let invalidIdSum = computeInvalidIdSum isMadeOfRepeatedSequences
            writeLine $"Invalid ID sum: {invalidIdSum}"

    type ChallengeSolution02(console: IConsole, reader: ISolutionReader<ChallengeSolution02>) =
        inherit ChallengeSolution<ChallengeSolution02>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
