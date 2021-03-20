namespace FSharp.xUnit.EqualityComparerAdapters

open System
open System.Collections
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Literals

type NullableEqualityComparerAdapter() =
    static member Singleton = NullableEqualityComparerAdapter() :> EqualityComparerAdapter
    interface EqualityComparerAdapter with
        member this.filter ty = 
            ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>>

        member this.getEqualityComparer(loop,ty) =
            let elementType = ty.GenericTypeArguments.[0]
            Console.WriteLine(Render.stringify elementType)
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
            }

