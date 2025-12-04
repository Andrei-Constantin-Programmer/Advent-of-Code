module TestUtilities

open System
open System.IO
open Xunit
open Utilities
    
type WriteLineFake() =
    
    let printed = ref List.empty<string>

    member _.WriteLine (message: string) =
        printed.Value <- printed.Value @ [message]

    member _.CallsLike (predicate: string -> bool) : int =
        printed.Value 
        |> List.filter predicate
        |> List.length

    member this.CallsExactlyWith (message: string) : int =
        this.CallsLike ^<| fun msg -> msg = message

    member this.CallsContaining (partMessage: string) =
        this.CallsLike ^<| fun msg -> msg.Contains(partMessage)

    member this.GetCalls () : string list =
        printed.Value

module TestExtensions =
    
    let callAssertion expectedCalls receivedCalls getCalls = 
        match expectedCalls, receivedCalls, (expectedCalls - receivedCalls) with
        | _, _, 0         -> Assert.True(true)
        | 1, callCount, _ -> Assert.True(false, $"Expected one call, received {callCount} calls. All calls: {getCalls()}")
        | _, 1, _         -> Assert.True(false, $"Expected {expectedCalls} calls, received one call. All calls: {getCalls()}")
        | _               -> Assert.True(false, $"Expected {expectedCalls} calls, received {receivedCalls} calls. All calls: {getCalls()}")

    type WriteLineFake with
        member this.VerifyCalledExactlyWith (message: string) (calls: int) =
            let receivedCalls = this.CallsExactlyWith message
            callAssertion calls receivedCalls this.GetCalls
            
        member this.VerifyCalledContaining (message: string) (calls: int) =
            let receivedCalls = this.CallsContaining message
            callAssertion calls receivedCalls this.GetCalls

        member this.VerifyCalledContainingInt (x: int) (calls: int) =
            this.VerifyCalledContaining $"{x}" calls

        member this.VerifyCalledContainingLong (x: int64) (calls: int) =
            this.VerifyCalledContaining $"{x}" calls
