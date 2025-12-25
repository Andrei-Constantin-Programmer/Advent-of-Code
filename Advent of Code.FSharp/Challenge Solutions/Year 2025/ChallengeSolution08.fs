// Task: https://adventofcode.com/2025/day/8

namespace Advent_of_Code.FSharp.Challenge_Solutions.Year_2025

open System
open System.Collections.Generic
open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

module ChallengeSolution08 =

    [<Literal>]
    let circuitsToConsider = 3

    type Location = {
        X: int64
        Y: int64
        Z: int64
    }
    
    type Distance = Distance of uint64
    module Distance =
        let value (Distance d) = d

    type JunctionBox = private JunctionBox of Location
    module JunctionBox =
        let create loc = JunctionBox loc
        let value (JunctionBox loc) = loc

    type Edge = private {
        junctionBox1: JunctionBox
        junctionBox2: JunctionBox
        distance: Distance
    }
    with
        member this.JunctionBox1 = this.junctionBox1
        member this.JunctionBox2 = this.junctionBox2
        member this.Distance = this.distance

    module Edge =
        let create jb1 jb2 =
            let computeEuclideanDistanceSquared loc1 loc2 =
                let pow2 x = x * x
                let dx = abs (loc1.X - loc2.X) |> pow2 |> uint64
                let dy = abs (loc1.Y - loc2.Y) |> pow2 |> uint64
                let dz = abs (loc1.Z - loc2.Z) |> pow2 |> uint64
                dx + dy + dz |> Distance

            {
                junctionBox1 = jb1
                junctionBox2 = jb2
                distance = computeEuclideanDistanceSquared (jb1 |> JunctionBox.value) (jb2 |> JunctionBox.value)
            }

    type Circuit = private Circuit of JunctionBox array
    module Circuit =
        let create junctionBoxes = Circuit junctionBoxes
        let count (Circuit circuit) = circuit |> Array.length

    type DisjointSetUnion<'a when 'a : equality> (items: 'a array) =
        let toMutableDictionary (pairs: ('k * 'v) array) =
            let dictionary = Dictionary<'k, 'v>(pairs.Length)
            for (k, v) in pairs do
                dictionary[k] <- v
            dictionary

        let parentOf, rankOf =
            items
            |> Array.map (fun x -> (x, x), (x, 0))
            |> Array.unzip
            |> fun (initParents, initRanks) -> 
                toMutableDictionary initParents, toMutableDictionary initRanks

        member _.Find (x: 'a) =
            let rec find x =
                let parent = parentOf[x]
                if parent = x then 
                    x
                else
                    let root = find parent
                    parentOf[x] <- root
                    root
            find x

        member this.Union (a: 'a) (b: 'a) =
            let rootA = this.Find a
            let rootB = this.Find b
            if rootA = rootB then
                false
            else
                if rankOf[rootA] < rankOf[rootB] then
                    parentOf[rootA] <- rootB
                elif rankOf[rootA] > rankOf[rootB] then
                    parentOf[rootB] <- rootA
                else
                    parentOf[rootB] <- rootA
                    rankOf[rootA] <- rankOf[rootA] + 1

                true

    type Solution(getInput: unit -> string list, writeLine: string -> unit) =

        let getJunctionBoxes() =
            let lines = getInput()
            let pairsToConnect = lines[0] |> Int32.Parse
            let junctionBoxes =
                lines
                |> List.skip 1
                |> List.map (fun line -> line.Split(',') |> Array.map Int64.Parse)
                |> List.map (
                    function 
                    | [|x; y; z|] ->
                        {
                            X = x
                            Y = y
                            Z = z
                        } |> JunctionBox.create
                    | line -> failwith $"Format of line {line} is wrong")
                |> Array.ofList
            (pairsToConnect, junctionBoxes)

        let getOrderedEdges junctionBoxes =
            let boxCount = junctionBoxes |> Array.length
            [|
                for i = 0 to boxCount - 2 do
                    for j = i + 1 to boxCount - 1 do
                        yield Edge.create junctionBoxes[i] junctionBoxes[j]
            |]
            |> Array.sortBy (_.Distance >> Distance.value)

        let buildCircuits junctionBoxes edges =
            let buildAdjacencyMap junctionBoxes (edges: Edge array) =
                let adjacencyMap = 
                    junctionBoxes
                    |> Array.map (fun jb -> jb, ResizeArray<JunctionBox>())
                    |> dict
                for edge in edges do
                    let jb1, jb2 = edge.JunctionBox1, edge.JunctionBox2
                    adjacencyMap[jb1].Add jb2
                    adjacencyMap[jb2].Add jb1

                adjacencyMap
                |> Seq.map (fun (KeyValue(jb, neighbours)) -> jb, neighbours.ToArray())
                |> dict

            let adjacencyMap = buildAdjacencyMap junctionBoxes edges
            let visited = HashSet<JunctionBox>()
            let circuits = ResizeArray<Circuit>()
            
            for start in junctionBoxes do
                if visited.Add start then
                    let stack = Stack<JunctionBox>()
                    let currentCircuit = ResizeArray<JunctionBox>()

                    stack.Push start
                    while stack.Count > 0 do
                        let jb = stack.Pop()
                        currentCircuit.Add jb

                        for neighbour in adjacencyMap[jb] do
                            if visited.Add neighbour then
                                stack.Push neighbour

                    currentCircuit.ToArray()
                    |> Circuit.create
                    |> circuits.Add

            circuits 
            |> List.ofSeq

        let getLastConnectingEdge junctionBoxes (edges: Edge array) =
            let disjointSetUnion = DisjointSetUnion(junctionBoxes)
            let mutable circuits = junctionBoxes.Length
            let mutable lastEdge = None

            let edgeCount = (edges |> Array.length)
            let mutable i = 0
            while i < edgeCount && circuits > 1 do
                let edge = edges[i]
                if disjointSetUnion.Union edge.JunctionBox1 edge.JunctionBox2 then
                    circuits <- circuits - 1
                    lastEdge <- Some edge
                i <- i + 1

            lastEdge
            |> Option.defaultWith (fun _ -> failwith "No connecting edge")

        member _.SolveFirstPart() =
            let pairsToConnect, junctionBoxes = getJunctionBoxes()
            
            let topCircuitsProduct =
                junctionBoxes
                |> getOrderedEdges
                |> Array.take pairsToConnect
                |> buildCircuits junctionBoxes
                |> List.map Circuit.count
                |> List.sortDescending
                |> List.take circuitsToConsider
                |> List.fold (fun acc x -> acc * (x |> uint64)) 1UL

            writeLine $"Top {circuitsToConsider} circuit sizes product: {topCircuitsProduct}"

        member _.SolveSecondPart() : unit =
            let _, junctionBoxes = getJunctionBoxes()

            let xProduct = 
                junctionBoxes
                |> getOrderedEdges
                |> getLastConnectingEdge junctionBoxes
                |> fun edge -> 
                    [|edge.JunctionBox1; edge.JunctionBox2|]
                    |> Array.map (JunctionBox.value >> _.X)
                    |> Array.fold (fun acc x -> acc * (x |> uint64)) 1UL
                    
            writeLine $"X coordinate product for last connection: {xProduct}"

    type ChallengeSolution08(console: IConsole, reader: ISolutionReader<ChallengeSolution08>) =
        inherit ChallengeSolution<ChallengeSolution08>(console, reader)

        let solution = Solution(reader.ReadLines >> List.ofArray, console.WriteLine)

        override this.SolveFirstPart() = solution.SolveFirstPart()
        override this.SolveSecondPart() = solution.SolveSecondPart()
