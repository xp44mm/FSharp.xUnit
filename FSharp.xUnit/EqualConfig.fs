﻿namespace FSharp.xUnit

open Xunit.Sdk
open FSharp.Idioms.Literal

type EqualConfig<'t>(equals:'t*'t->bool) =
    member _.equal expected actual = 
        if equals(expected, actual) |> not then
            let ex = stringify expected
            let ac = stringify actual
            EqualException.ForMismatchedValues(ex, ac)
            |> raise

    member _.notEqual expected actual = 
        if equals(expected, actual) then
            let ex = stringify expected
            let ac = stringify actual
            NotEqualException.ForEqualValues(ex, ac)
            |> raise
    

