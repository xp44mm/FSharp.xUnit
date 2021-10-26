module FSharp.xUnit.EqualityComparer

open FSharp.Literals
open System
open System.Collections
open System.Collections.Generic

let getters = [
   NullableEqualityComparer.tryGet
   ArrayEqualityComparer.tryGet
   TupleEqualityComparer.tryGet
   RecordEqualityComparer.tryGet
   ListEqualityComparer.tryGet
   SetEqualityComparer.tryGet
   MapEqualityComparer.tryGet
   UnionEqualityComparer.tryGet
   SeqEqualityComparer.tryGet
]

let rec mainTryGetDynamic (getters: seq<Type -> ((Type -> IEqualityComparer) -> IEqualityComparer) option>) (ty:Type) =
    //Console.WriteLine(Render.stringify ty)
    let pickedTryGet =
        getters
        |> Seq.tryPick(fun tryGet -> tryGet ty)
        |> Option.defaultValue(fun _ -> {
            new IEqualityComparer with
                member this.Equals(ls1,ls2) = ls1 = ls2
                member this.GetHashCode(ls) = hash ls
        })
    pickedTryGet (mainTryGetDynamic getters)

let toGenericIEqualityComparer<'T> (getters: seq<Type -> ((Type -> IEqualityComparer) -> IEqualityComparer) option>) = 
    let ty = typeof<'T>
    let comp = mainTryGetDynamic getters ty
    {
        new IEqualityComparer<'T> with
            member this.Equals(x,y) = comp.Equals(x,y)
            member this.GetHashCode(x) = comp.GetHashCode x
    }
