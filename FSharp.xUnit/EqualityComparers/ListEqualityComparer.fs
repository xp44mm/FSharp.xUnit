module FSharp.xUnit.ListEqualityComparer

open System
open System.Collections
open FSharp.xUnit
open FSharp.Idioms

let tryGet(ty: Type) =
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<list<_>> then
        Some(fun (loop:Type -> IEqualityComparer) ->
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
            })

    else None