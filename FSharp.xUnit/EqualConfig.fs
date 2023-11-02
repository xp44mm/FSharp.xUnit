namespace FSharp.xUnit

open System

open Xunit.Sdk
open FSharp.Idioms.Literal
open FSharp.xUnit.EqualityComparers

type EqualConfig(getters: list<Type -> EqualityComparerCase> ) =

    member this.equal<'a> (expected:'a) actual = 
        let comparer = EqualityComparerUtils.toGenericIEqualityComparer<'a> getters
        if comparer.Equals(expected,actual) then
            ()
        else
            let ex = stringify expected
            let ac = stringify actual
            EqualException.ForMismatchedValues(ex, ac)
            |> raise

    member this.notEqual<'a> (expected:'a) actual = 
        let comparer = EqualityComparerUtils.toGenericIEqualityComparer<'a> getters
        if comparer.Equals(expected,actual) then
            let ex = stringify expected
            let ac = stringify actual
            NotEqualException.ForEqualValues(ex, ac)
            |> raise
    
    new() = EqualConfig(EqualityComparerUtils.cases)

    [<Obsolete("EqualConfig [...;yield! EqualityComparerUtils.cases]")>]
    static member ``override`` (newAdapters:list<Type -> EqualityComparerCase>) = 
        EqualConfig [
            yield! newAdapters 
            yield! EqualityComparerUtils.cases
        ]
