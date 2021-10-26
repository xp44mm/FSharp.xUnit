module FSharp.xUnit.ArrayEqualityComparer

open System
open System.Collections
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Literals

let tryGet (ty: Type) =
    if ty.IsArray && ty.GetArrayRank() = 1 then
        Some(fun (loop:Type -> IEqualityComparer) ->
            let reader = ArrayType.readArray ty
            let elementType = ty.GetElementType()
            let loopElement = loop elementType
            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) = 
                        if ls1 = null && ls2 = null then
                            true
                        elif ls1 = null || ls2 = null then
                            false
                        else
                            let _, a1 = reader ls1
                            let _, a2 = reader ls2
                        
                            if a1.Length = a2.Length then
                                if Array.isEmpty a1 then
                                    true
                                else
                                    Array.zip a1 a2
                                    |> Array.forall(fun(b1,b2) -> loopElement.Equals(b1, b2))
                            else false
                    member this.GetHashCode(ls) = 
                        ls
                        |> reader
                        |> snd
                        |> Array.map(loopElement.GetHashCode)
                        |> hash
            })
    else None
