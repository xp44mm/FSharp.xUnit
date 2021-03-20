module FSharp.xUnit.EqualityComparer

open FSharp.xUnit.EqualityComparerAdapters
open FSharp.Literals
open System
open System.Collections
open System.Collections.Generic

let rec mainDynamic (adapters:seq<EqualityComparerAdapter>) (ty:Type) =
    Console.WriteLine(Render.stringify ty)
    let method =
        adapters
        |> Seq.tryFind(fun x -> x.filter ty)
        |> Option.map(fun x -> x.getEqualityComparer)
        |> Option.defaultValue(fun (loop,ty) -> {
            new IEqualityComparer with
                member this.Equals(ls1,ls2) = ls1 = ls2
                member this.GetHashCode(ls) = hash ls
        })
    method(mainDynamic adapters, ty)

/// 自动装配
let Automata<'T> (customs:seq<EqualityComparerAdapter>) = 
    let ty = typeof<'T>
    let comp = mainDynamic customs ty
    {
        new IEqualityComparer<'T> with
            member this.Equals(x,y) = comp.Equals(x,y)
            member this.GetHashCode(x) = comp.GetHashCode x
    }

let adapters = [
    NullableEqualityComparerAdapter.Singleton
    ArrayEqualityComparerAdapter.Singleton
    TupleEqualityComparerAdapter.Singleton
    RecordEqualityComparerAdapter.Singleton
    ListEqualityComparerAdapter.Singleton
    SetEqualityComparerAdapter.Singleton
    MapEqualityComparerAdapter.Singleton
    UnionEqualityComparerAdapter.Singleton
    SeqEqualityComparerAdapter.Singleton
]