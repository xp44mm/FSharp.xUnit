namespace FSharp.xUnit.EqualityComparerAdapters

open System.Collections
open FSharp.xUnit
open System
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type RecordEqualityComparerAdapter() =
    static member Singleton = RecordEqualityComparerAdapter() :> EqualityComparerAdapter

    interface EqualityComparerAdapter with
        member this.filter ty = FSharpType.IsRecord ty
        member this.getEqualityComparer(loop,ty) =
            let piFields = FSharpType.GetRecordFields ty
            let reader = FSharpValue.PreComputeRecordReader ty

            let loopFields = 
                piFields 
                |> Array.map(fun pi -> loop pi.PropertyType)

            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) = 
                        let a1 = reader ls1
                        let a2 = reader ls2
                        Array.zip3 loopFields a1 a2
                        |> Array.forall(fun(loopField,a1,a2) -> loopField.Equals(a1, a2))
                    member this.GetHashCode(ls) = 
                        ls
                        |> reader
                        |> Array.zip loopFields
                        |> Array.map(fun(loopField,a1) -> loopField.GetHashCode(a1))
                        |> hash
            }

