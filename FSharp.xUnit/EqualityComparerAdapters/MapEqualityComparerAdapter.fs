﻿namespace FSharp.xUnit.EqualityComparerAdapters

open System.Collections
open FSharp.xUnit
open System
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type MapEqualityComparerAdapter() =
    static member Singleton = MapEqualityComparerAdapter() :> EqualityComparerAdapter

    interface EqualityComparerAdapter with
        member this.filter ty = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>>
        member this.getEqualityComparer(loop,ty) =
            let reader = MapType.readMap ty
            let elementType = FSharpType.MakeTupleType(ty.GenericTypeArguments)
            let loopElement = loop elementType
            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) = 
                        let _, a1 = reader ls1
                        let _, a2 = reader ls2
                        if a1.Length = a2.Length then
                            Array.zip a1 a2
                            |> Array.forall(fun(a1,a2) -> loopElement.Equals(a1, a2))
                        else false
                    member this.GetHashCode(ls) = 
                        ls
                        |> reader
                        |> snd
                        |> Array.map(loopElement.GetHashCode)
                        |> hash
            }

