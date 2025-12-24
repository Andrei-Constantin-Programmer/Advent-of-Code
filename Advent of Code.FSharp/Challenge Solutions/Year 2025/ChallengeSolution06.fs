// Task: https://adventofcode.com/2025/day/6

namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open System
open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution06 =

    let whitespace = ' '

    type Operation =
        | Addition
        | Multiplication

    module Operation =

        let ofString operator =
            match operator with
            | "+" -> Addition
            | "*" -> Multiplication
            | _ -> failwith $"Unknown operator {operator}"

        let ofChar operator =
            ofString $"{operator}"

        let inline apply a op b =
            match op with
            | Addition       -> a + b
            | Multiplication -> a * b

        let getIdentity op =
            match op with
            | Addition       -> 0
            | Multiplication -> 1

    type Problem = {
        Numbers: uint64 list
        Operation: Operation
    }

    module Problem =
        let compute problem =
            let op = problem.Operation
            let identity = (Operation.getIdentity op |> uint64)
            problem.Numbers
            |> List.fold (fun acc x -> Operation.apply acc op x) identity

    let removeLast list =
        match list with
        | [] -> []
        | _ -> list |> List.rev |> List.tail |> List.rev

    type Solution(getInput: unit -> string list, writeLine: string -> unit) =

        let readProblemsAsHuman() =
            let rec convertColumnToProblem numbers columnElements =
                match columnElements with
                | [] -> failwith "Line has no elements!"
                | [op] -> {
                        Numbers = numbers
                        Operation = op |> Operation.ofString
                    }
                | numberString :: remainingElements ->
                    let numbers = (numberString |> UInt64.Parse) :: numbers
                    remainingElements |> convertColumnToProblem numbers

            getInput()
            |> List.map (fun line -> 
                line.Split(whitespace, StringSplitOptions.RemoveEmptyEntries)
                |> List.ofArray)
            |> List.transpose
            |> List.map (convertColumnToProblem [])

        let readProblemsAsCephalopod() =
            let groupLinesByProblem lines =
                let groupsReversed, current =
                    lines
                    |> List.fold (fun (problemStrings, current) line ->
                        if line |> List.forall (fun (c: char) -> c = whitespace) then
                            match current with
                            | [] -> (problemStrings, [])
                            | _  -> (List.rev current :: problemStrings, [])
                        else 
                            (problemStrings, line :: current)
                    ) ([], [])

                let groupsReversed =
                    match current with
                    | [] -> groupsReversed
                    | _ -> List.rev current :: groupsReversed

                List.rev groupsReversed

            let lineGroupToProblem (group: char list list) =
                let operation = group[0] |> List.last |> Operation.ofChar
                let numbers =
                    group
                    |> List.map (fun line -> 
                        line
                        |> removeLast
                        |> List.map (
                            function
                            | c when c >= '0' && c <= '9' -> (int c - int '0') |> Some
                            | _ -> None))
                    |> List.map (
                        List.fold (fun acc digit -> 
                            match digit with
                            | Some d -> acc * 10 + d
                            | None   -> acc) 0)
                    |> List.map uint64

                { 
                    Numbers = numbers
                    Operation = operation 
                }

            getInput()
            |> List.map Seq.toList
            |> List.transpose
            |> groupLinesByProblem
            |> List.map lineGroupToProblem

        let computeGrandTotal (readProblems: unit -> Problem list) =
            readProblems() 
            |> List.sumBy Problem.compute
        
        member _.SolveFirstPart() =
            let total = computeGrandTotal readProblemsAsHuman
            writeLine $"Grand total: {total}"

        member _.SolveSecondPart() : unit =
            let total = computeGrandTotal readProblemsAsCephalopod
            writeLine $"Grand total: {total}"

    type ChallengeSolution06(console: IConsole, reader: ISolutionReader<ChallengeSolution06>) =
        inherit ChallengeSolution<ChallengeSolution06>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
