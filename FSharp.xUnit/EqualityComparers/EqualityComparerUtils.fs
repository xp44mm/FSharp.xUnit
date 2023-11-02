module FSharp.xUnit.EqualityComparers.EqualityComparerUtils

open System
open System.Collections
open System.Collections.Generic
open FSharp.Idioms
open FSharp.xUnit.EqualityComparers.EqualityComparerCases

let cases = [
    ArrayEqualityComparerCase
    TupleEqualityComparerCase
    RecordEqualityComparerCase
    ListEqualityComparerCase
    SetEqualityComparerCase
    MapEqualityComparerCase
    UnionEqualityComparerCase
    SeqEqualityComparerCase
    NullableEqualityComparerCase
]

let rec mainTryGetDynamic (getters: list<Type->EqualityComparerCase>) (ty:Type) =
    let pickedTryGet =
        getters
        |> Seq.tryPick(fun cs -> 
            let x = cs ty
            if x.finder then
                Some x.get
            else None
            )
        |> Option.defaultValue(fun loop -> {
            new IEqualityComparer with
                member this.Equals(ls1,ls2) = ls1 = ls2
                member this.GetHashCode(ls) = hash ls
        })
    pickedTryGet (mainTryGetDynamic getters)

let toGenericIEqualityComparer<'T> (getters: list<Type->EqualityComparerCase>) = 
    let ty = typeof<'T>
    let comp = mainTryGetDynamic getters ty
    {
        new IEqualityComparer<'T> with
            member this.Equals(x,y) = comp.Equals(x,y)
            member this.GetHashCode(x) = comp.GetHashCode x
    }
