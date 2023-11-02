module FSharp.xUnit.Should

open FSharp.Idioms.Literal
open Xunit.Sdk

let equal<'a when 'a:equality> (expected:'a) actual =
    if expected <> actual then
        let ex = stringify expected
        let ac = stringify actual
        EqualException.ForMismatchedValues(ex, ac)
        |> raise

let notEqual<'a when 'a:equality> (expected:'a) actual = 
    if expected = actual then
        let ex = stringify expected
        let ac = stringify actual
        NotEqualException.ForEqualValues(ex, ac)
        |> raise

