module FSharp.xUnit.NullableEqualityComparer

open System
open System.Collections
open FSharp.Idioms
open FSharp.Literals

let tryGet (ty: Type) =
    if ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>> then
        Some(fun (loop:Type -> IEqualityComparer) -> 
            let elementType = ty.GenericTypeArguments.[0]
            let loopElement = loop elementType
            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) =
                        if ls1 = null && ls2 = null then
                            true
                        elif ls1 = null || ls2 = null then
                            false
                        else
                            loopElement.Equals(ls1,ls2)
                    member this.GetHashCode(ls) = 
                        if ls = null then 
                            Array.empty 
                        else 
                            loopElement.GetHashCode ls |> Array.singleton
                        |> hash
            })
    else None
