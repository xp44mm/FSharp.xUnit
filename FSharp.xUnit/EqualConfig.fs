namespace FSharp.xUnit

open FSharp.xUnit
open Xunit.Sdk
open FSharp.Literals
open System
open System.Collections

type EqualConfig(getters: seq<Type -> ((Type -> IEqualityComparer) -> IEqualityComparer) option>) =
    new() = EqualConfig(EqualityComparer.getters)

    static member ``override`` (newAdapters:seq<Type -> ((Type -> IEqualityComparer) -> IEqualityComparer) option>) = 
        let getters = newAdapters |> Seq.append <| EqualityComparer.getters
        EqualConfig(getters)

    member this.equal<'a> (expected:'a) actual = 
        let comparer = EqualityComparer.toGenericIEqualityComparer<'a> getters
        if comparer.Equals(expected,actual) then
            ()
        else
            let ex = Render.stringify expected
            let ac = Render.stringify actual
            raise <| EqualException.ForMismatchedValues(ex, ac)

    member this.notEqual<'a> (expected:'a) actual = 
        let comparer = EqualityComparer.toGenericIEqualityComparer<'a> getters
        if comparer.Equals(expected,actual) then
            let ex = Render.stringify expected
            let ac = Render.stringify actual
            raise <| NotEqualException.ForEqualValues(ex, ac)
    
