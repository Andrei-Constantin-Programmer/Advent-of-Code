namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution03 =

    type Solution(getInput: unit -> string list, writeLine: string -> unit) =
            
        let computeOutputJoltage (digitCount: int) (banks: string list) =
            let rec computeBatteryJoltage (digitCount: int) (fromIndex: int) (joltage: int64) (battery: char array) : int64 =
                match digitCount with
                | 0 -> joltage
                | _ ->
                    let searchWindow = battery[fromIndex..(battery.Length - digitCount)]
                    let maxIndex, maxJoltage = searchWindow
                                              |> Array.mapi (fun i jolt -> (i, int64 jolt - int64 '0'))
                                              |> Array.maxBy snd
                    computeBatteryJoltage (digitCount - 1) (fromIndex + maxIndex + 1) (joltage * 10L + maxJoltage) battery                    
                
            banks
            |> List.map _.ToCharArray()
            |> List.map (computeBatteryJoltage digitCount 0 0)
            |> List.sum
        
        member _.SolveFirstPart() =
            let banks = getInput()
            let outputJoltage = computeOutputJoltage 2 banks
                
            writeLine $"Output Joltage: {outputJoltage}"

        member _.SolveSecondPart() : unit =
            let banks = getInput()
            let outputJoltage = computeOutputJoltage 12 banks
                
            writeLine $"Output Joltage: {outputJoltage}"

    type ChallengeSolution03(console: IConsole, reader: ISolutionReader<ChallengeSolution03>) =
        inherit ChallengeSolution<ChallengeSolution03>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
