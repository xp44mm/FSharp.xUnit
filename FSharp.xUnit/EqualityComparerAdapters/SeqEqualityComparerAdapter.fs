namespace FSharp.xUnit.EqualityComparerAdapters

open System
open System.Collections
open System.Collections.Generic

open FSharp.xUnit
open FSharp.Idioms

type SeqEqualityComparerAdapter() =
    static member Singleton = SeqEqualityComparerAdapter() :> EqualityComparerAdapter

    interface EqualityComparerAdapter with
        member this.filter ty = 
            ty.IsGenericType && ty.GenericTypeArguments.Length = 1 &&
            typedefof<seq<_>>.MakeGenericType(ty.GenericTypeArguments).IsAssignableFrom ty

        member this.getEqualityComparer(loop,ty) =
            let loopElement = loop ty.GenericTypeArguments.[0]

            let seq = typedefof<seq<_>>.MakeGenericType(ty.GenericTypeArguments)
            let enumerator = typedefof<IEnumerator<_>>.MakeGenericType(ty.GenericTypeArguments)
            let mMoveNext = typeof<IEnumerator>.GetMethod("MoveNext")

            let mGetEnumerator = seq.GetMethod("GetEnumerator")
            let pCurrent = enumerator.GetProperty("Current")

            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) = 
                        if ls1 = null && ls2 = null then
                            true
                        elif ls1 = null || ls2 = null then
                            false
                        else
                            let e1 = mGetEnumerator.Invoke(ls1,[||])
                            let e2 = mGetEnumerator.Invoke(ls2,[||])
                            let rec loopNext i =
                                let hasNext1 = mMoveNext.Invoke(e1,[||]) |> unbox<bool>
                                let hasNext2 = mMoveNext.Invoke(e2,[||]) |> unbox<bool>

                                if hasNext1 <> hasNext2 then
                                    false
                                elif hasNext1 then
                                    let c1 = pCurrent.GetValue(e1)
                                    let c2 = pCurrent.GetValue(e2)
                                    if loopElement.Equals(c1, c2) && i < 999 then
                                        loopNext(i+1)
                                    else false
                                else true
                            loopNext 0
                    member this.GetHashCode(ls) = hash ls
            }

