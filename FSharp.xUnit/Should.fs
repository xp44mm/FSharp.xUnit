module FSharp.xUnit.Should

open Xunit.Sdk
open FSharp.Idioms.Literal
open FSharp.Idioms.EqualityComparers

let config<'t> = EqualConfig<'t>(EqualityComparerUtils.equal)

let equal<'a when 'a: equality> (expected: 'a) actual = config<'a>.equal expected actual

let notEqual<'a when 'a: equality> (expected: 'a) actual = config<'a>.notEqual expected actual
