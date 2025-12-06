module Advent_of_Code.FSharp.Utilities

open System

// https://www.fssnip.net/4o/title/High-precedence-right-associative-backward-pipe
let inline (^<|) f a = f a

let parseInt : string -> int option =
    Int32.TryParse
    >> function
        | true, value -> Some value
        | false, _    -> None

let failwithMalformedInput() = failwith "Malformed input"
