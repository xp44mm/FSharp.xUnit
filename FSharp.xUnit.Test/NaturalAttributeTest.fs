namespace FSharp.xUnit

open System
open System.Collections.Generic

open Xunit
//open Xunit.Abstractions

open FSharp.Idioms.Literal

type NaturalAttributeTest(output: ITestOutputHelper) =
    [<Theory>]
    [<Natural(3)>]
    member _.``Natural Number Sequence``(i:int) =
        let expect = [0;1;2;3]
        let actual = [0;1;2;3]
        Should.equal expect.[i] actual.[i]
