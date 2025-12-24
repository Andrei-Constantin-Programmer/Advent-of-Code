// Task: https://adventofcode.com/2025/day/7

namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution07 =

    type Coordinates = {
        Row: int
        Col: int
    }

    type BeamStart = private BeamStart of Coordinates
    module BeamStart =
        let create coords = BeamStart coords
        let value (BeamStart coords) = coords

    type Splitter = private Splitter of Coordinates
    module Splitter =
        let create coords = Splitter coords
        let value (Splitter coords) = coords

    type TachyonSpace =
        | BeamStart of BeamStart
        | Splitter of Splitter
        | Empty

    module TachyonSpace =
        let getTachyonSpace coords c =
            match c with
            | 'S' -> BeamStart.create coords |> BeamStart
            | '^' -> Splitter.create coords |> Splitter
            | '.' -> Empty
            | _   -> failwith $"Unknown tachyon space {c}"

    type TachyonManifold = {
        BeamStart: BeamStart
        Splitters: Splitter list
        Height: int
    }

    module TachyonManifold =
        let getSplitterCoordinates manifold =
            manifold.Splitters
            |> List.map Splitter.value
            |> Set.ofList

    type Solution(getInput: unit -> string list, writeLine: string -> unit) =

        let readTachyonManifold() =
            let lines =
                getInput() 
                |> List.map Seq.toList

            let beamStartOption, splitters =
                lines
                |> List.mapi (fun rowIndex row ->
                    row
                    |> List.mapi (fun colIndex tachyonSpace -> 
                        let coords = { Row = rowIndex; Col = colIndex }
                        TachyonSpace.getTachyonSpace coords tachyonSpace))
                |> List.collect id
                |> List.fold (fun (beamStartOption, splitters) tachyonSpace ->
                    match tachyonSpace with
                    | BeamStart beam ->
                        match beamStartOption with
                        | None -> Some beam, splitters
                        | Some _ -> failwithf "Multiple beam start coordinates found"
                    | Splitter splitter -> 
                        beamStartOption, splitter :: splitters
                    | _ -> 
                        beamStartOption, splitters
                    ) (None, [])

            let beamStart = 
                beamStartOption
                |> Option.defaultWith (fun _ -> failwith "No beam start found")

            {
                BeamStart = beamStart
                Splitters = splitters
                Height = lines.Length
            }

        member _.SolveFirstPart() =

            let collectBeamSplits tachyonManifold =
                let splitterCoordinates = 
                    tachyonManifold
                    |> TachyonManifold.getSplitterCoordinates

                let rec collectBeamSplitsInner currentRow beamColumns splitCount =
                    if currentRow >= tachyonManifold.Height then
                        set [], splitCount
                    else
                        beamColumns
                        |> Set.fold (fun (accBeams, accSplits) beamCol ->
                            if splitterCoordinates |> Set.contains { Row = currentRow; Col = beamCol } then
                                let beamLeft =  beamCol - 1
                                let beamRight = beamCol + 1
                                accBeams |> Set.add beamLeft |> Set.add beamRight, accSplits + 1
                            else
                                accBeams |> Set.add beamCol, accSplits
                            ) (set [], splitCount)
                        |> fun (nextBeams, count) -> nextBeams, count
                        ||> collectBeamSplitsInner (currentRow + 1)
                
                let startBeam = tachyonManifold.BeamStart |> BeamStart.value

                collectBeamSplitsInner startBeam.Row (set [startBeam.Col]) 0
                |> snd

            let beamSplitCount = 
                readTachyonManifold()
                |> collectBeamSplits

            writeLine $"Beam split: {beamSplitCount} times"

        member _.SolveSecondPart() : unit =

            let computeTimelines tachyonManifold =
                let splitterCoordinates = 
                    tachyonManifold
                    |> TachyonManifold.getSplitterCoordinates

                let rec computeTimelinesInner currentRow tachyonColumn timelineMemory =
                    let currentCoords = { Row = currentRow; Col = tachyonColumn }

                    if currentRow >= tachyonManifold.Height then
                        1L, timelineMemory
                    elif splitterCoordinates |> Set.contains currentCoords then
                        let timelinesLeft, memoryLeft = computeTimelinesInner (currentRow + 1) (tachyonColumn - 1) timelineMemory
                        let timelinesRight, memoryRight = computeTimelinesInner (currentRow + 1) (tachyonColumn + 1) memoryLeft
                        timelinesLeft + timelinesRight, memoryRight
                    else
                        timelineMemory |> Map.tryFind currentCoords
                        |>  function
                            | Some timelines -> timelines, timelineMemory
                            | None -> 
                                let timelines, memoryDownstream = computeTimelinesInner (currentRow + 1) tachyonColumn timelineMemory
                                timelines, memoryDownstream |> Map.add currentCoords timelines
                            

                let startBeam = tachyonManifold.BeamStart |> BeamStart.value

                computeTimelinesInner startBeam.Row startBeam.Col (Map.empty)
                |> fst

            let timelineCount =
                readTachyonManifold()
                |> computeTimelines

            writeLine $"Active timelines: {timelineCount}"

    type ChallengeSolution07(console: IConsole, reader: ISolutionReader<ChallengeSolution07>) =
        inherit ChallengeSolution<ChallengeSolution07>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
