module Advent_of_Code.FSharp.FSharpSolutionMapper

open System
open System.Reflection
open Microsoft.Extensions.DependencyInjection
open Advent_of_Code.FSharp.Utilities
open Advent_of_Code.Shared
open Advent_of_Code.Shared.Utilities

type FSharpSolutionMapper(console: IConsole, serviceProvider: IServiceProvider) =
    
    [<Literal>]
    let yearNamespacePrefix = "Advent_of_Code.FSharp.Challenge_Solutions.Year_"
    
    let years = Assembly.GetExecutingAssembly().GetTypes()
                    |> Array.filter (fun t -> t.Namespace <> null && t.Namespace.StartsWith(yearNamespacePrefix))
                    |> Array.map (fun t -> t.Namespace.Split('_') |> Array.last |> parseInt)
                    |> Array.choose id
                    |> Array.distinct
                    
    let getChallengeYearNamespace(year: int) = $"{yearNamespacePrefix}{year}"
    
    interface ISolutionMapper with
        member _.DoesYearExist(year) : bool = years |> Array.contains year
        
        member _.GetChallengeSolution(year, day) : ChallengeSolution =
            let challengeClassName = $"ChallengeSolution{PathUtils.FormatDay day}"
            let fullClassName = $"{getChallengeYearNamespace year}.{challengeClassName}+{challengeClassName}"
                
            let solutionType = Type.GetType fullClassName
                                |> function
                                | null -> raise (NonexistentChallengeException(year, day))
                                | t -> t
                         
            let readerType = typedefof<ISolutionReader<_>>.MakeGenericType(solutionType)
            let reader = serviceProvider.GetRequiredService(readerType)
            
            let instance = Activator.CreateInstance(solutionType, console, reader)
            
            match instance with
            | :? ChallengeSolution as sol -> sol
            | _ -> failwith $"The F# solution for challenge {year}_{day} is malformed"
