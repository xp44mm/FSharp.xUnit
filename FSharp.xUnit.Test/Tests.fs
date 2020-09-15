module Tests

open System
open Xunit
open FSharp.xUnit

[<Fact>]
let ``My test`` () =
    Should.equal 1 1
