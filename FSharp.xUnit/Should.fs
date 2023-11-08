module FSharp.xUnit.Should

open Xunit.Sdk
open FSharp.Idioms.Literal
open FSharp.Idioms.EqualityCheckers

let config<'t> = EqualConfig<'t>(EqualityCheckerUtils.equals)

let equal<'a when 'a:equality> (expected:'a) actual =
    config<'a>.equal expected actual
let notEqual<'a when 'a:equality> (expected:'a) actual = 
    config<'a>.notEqual expected actual
