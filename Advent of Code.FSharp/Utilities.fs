module Utilities

// https://www.fssnip.net/4o/title/High-precedence-right-associative-backward-pipe
let inline (^<|) f a = f a


let failwithMalformedInput() = failwith "Malformed input"
