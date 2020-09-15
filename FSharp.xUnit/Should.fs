module FSharp.xUnit.Should

open Xunit

let equal<'a> (expected:'a) actual = Assert.Equal<'a>(expected,actual)
