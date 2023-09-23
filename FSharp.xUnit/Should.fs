module FSharp.xUnit.Should

open Xunit.Sdk
open FSharp.Literals

let equal<'a when 'a:equality > (expected:'a) actual = 
    if expected = actual then
        ()
    else
        let ex = Render.stringify expected
        let ac = Render.stringify actual
        raise <| EqualException.ForMismatchedValues(ex, ac)

let notEqual<'a when 'a:equality > (expected:'a) actual = 
    if expected = actual then
        let ex = Render.stringify expected
        let ac = Render.stringify actual
        raise <| NotEqualException.ForEqualValues(ex, ac)

