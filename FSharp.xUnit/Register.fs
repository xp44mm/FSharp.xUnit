﻿namespace FSharp.xUnit

open FSharp.xUnit
open Xunit.Sdk
open FSharp.Literals

type Register(adapters:seq<EqualityComparerAdapter>) =
    new() = Register(EqualityComparer.adapters)

    static member ``override`` (newAdapters:seq<EqualityComparerAdapter>) = 
        let adapters = newAdapters |> Seq.append <| EqualityComparer.adapters
        Register(adapters)

    member this.equal<'a> (expected:'a) actual = 
        let comparer = EqualityComparer.Automata<'a> adapters
        if comparer.Equals(expected,actual) then
            ()
        else
            let ex = Render.stringify expected
            let ac = Render.stringify actual
            raise <| EqualException(ex, ac)

    member this.notEqual<'a> (expected:'a) actual = 
        let comparer = EqualityComparer.Automata<'a> adapters
        if comparer.Equals(expected,actual) then
            let ex = Render.stringify expected
            let ac = Render.stringify actual
            raise <| NotEqualException(ex, ac)
    
