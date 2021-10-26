module FSharp.xUnit.SetEqualityComparer

open System.Collections
open FSharp.xUnit
open System
open FSharp.Idioms

let tryGet(ty: Type) =
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>> then
        Some(fun (loop:Type -> IEqualityComparer) ->
            let elementType = ty.GenericTypeArguments.[0]
            let reader = SetType.readSet ty
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
            })
    else None