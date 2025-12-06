namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open System
open Advent_of_Code.FSharp.Utilities
open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution01 =

    [<Literal>]
    let UpperDialLimit : int = 100

    [<Literal>]
    let StartingDialPosition : int = 50

    [<Literal>]
    let Zero = 0

    type Rotation =
    | Left of int
    | Right of int

    let distanceOf : Rotation -> int = function
    | Left x -> x
    | Right x -> x

    type Solution(getInput: unit -> string list, writeLine: string -> unit) =

        let readRotations() : Rotation list =
            getInput()
            |> List.map 
                ^<| (Seq.toList
                    >> function
                    | direction::distanceAsString -> 
                        (direction, distanceAsString 
                                    |> List.toArray
                                    |> String
                                    |> int)
                    | _ -> failwithMalformedInput())
            |> List.map 
                ^<| fun (direction, distance) -> 
                    match direction with
                    | 'L' -> Left distance
                    | 'R' -> Right distance
                    | _   -> failwithMalformedInput()

        let turnDial (dialPosition: int) (rotation: Rotation) =
            match rotation with
            | Left distance  -> dialPosition - distance
            | Right distance -> dialPosition + distance
            |> fun dial -> ((dial % UpperDialLimit) + UpperDialLimit) % UpperDialLimit

        member _.SolveFirstPart() =
            
            let rec computePassword (rotations: Rotation list) (dial: int) : int =
                match rotations with
                | [] -> 0
                | rotation::remaining ->
                    let newDialPosition = turnDial dial rotation
                    let password = newDialPosition
                                   |> computePassword remaining

                    match newDialPosition with
                    | Zero -> password + 1
                    | _ -> password

            let rotations = readRotations()
            let password = computePassword rotations StartingDialPosition
            writeLine $"Password: {password}"

        member _.SolveSecondPart() : unit =

            let rec computePassword (rotations: Rotation list) (dial: int) : int =
                match rotations with
                | [] -> 0
                | rotation::remaining ->
                    let distance = distanceOf rotation
                    let fullRotations = distance / UpperDialLimit
                    let remainder = distance - (fullRotations * UpperDialLimit)

                    let hasRotationMovedPastZero = function
                    | Left _ -> dial <= remainder
                    | Right _ -> dial >= UpperDialLimit - remainder

                    let movedPastZeroAddition = if dial <> Zero && hasRotationMovedPastZero rotation then 1 else 0

                    let password = turnDial dial rotation 
                                   |> computePassword remaining

                    password + fullRotations + movedPastZeroAddition

            let rotations = readRotations()
            let password = computePassword rotations StartingDialPosition
            writeLine $"Password: {password}"

    type ChallengeSolution01(console: IConsole, reader: ISolutionReader<ChallengeSolution01>) =
        inherit ChallengeSolution<ChallengeSolution01>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()