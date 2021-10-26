module FSharp.xUnit.TupleEqualityComparer


open System.Collections
open FSharp.xUnit
open System
open FSharp.Idioms
open Microsoft.FSharp.Reflection

let tryGet(ty: Type) =
    if FSharpType.IsTuple ty then
        Some(fun (loop:Type -> IEqualityComparer) ->
            let reader = TupleType.readTuple ty
            let elementTypes = FSharpType.GetTupleElements ty
            let loopElements = elementTypes |> Array.map(loop)
            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) = 
                        let a1 = reader ls1 |> Array.map(snd)
                        let a2 = reader ls2 |> Array.map(snd)
                        if a1.Length = a2.Length then
                            Array.zip3 loopElements a1 a2
                            |> Array.forall(fun(loopElement,a1,a2) -> loopElement.Equals(a1, a2))
                        else false
                    member this.GetHashCode(ls) = 
                        ls
                        |> reader
                        |> Array.zip loopElements
                        |> Array.map(fun(loopElement,a1) -> loopElement.GetHashCode(a1))
                        |> hash
            })
    else None


