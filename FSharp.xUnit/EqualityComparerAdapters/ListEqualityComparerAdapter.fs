namespace FSharp.xUnit.EqualityComparerAdapters

open System
open System.Collections
open FSharp.xUnit
open FSharp.Idioms

type ListEqualityComparerAdapter() =
    static member Singleton = ListEqualityComparerAdapter() :> EqualityComparerAdapter
    interface EqualityComparerAdapter with
        member this.filter ty = 
            ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<list<_>>

        member this.getEqualityComparer(loop,ty) =
            let elementType = ty.GenericTypeArguments.[0]
            let readlist = ListType.readList ty
            let loopElement = loop elementType
            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) =
                        let _,ls1 = readlist ls1
                        let _,ls2 = readlist ls2
                        if ls1.Length = ls2.Length then
                            (ls1,ls2)
                            ||> Array.zip
                            |> Array.forall(fun(e1,e2)-> loopElement.Equals(e1,e2))
                        else false
                    member this.GetHashCode(ls) = 
                        ls
                        |> readlist
                        |> snd
                        |> Array.map(loopElement.GetHashCode)
                        |> hash
            }

