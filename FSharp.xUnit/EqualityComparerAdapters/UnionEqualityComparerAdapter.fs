namespace FSharp.xUnit.EqualityComparerAdapters

open System.Collections
open FSharp.xUnit
open System
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type UnionEqualityComparerAdapter() =
    static member Singleton = UnionEqualityComparerAdapter() :> EqualityComparerAdapter

    interface EqualityComparerAdapter with
        member this.filter ty = FSharpType.IsUnion ty
        member this.getEqualityComparer(loop,ty) =
            let unionCases = FSharpType.GetUnionCases ty
            let tagReader = FSharpValue.PreComputeUnionTagReader ty

            let unionCases =
                unionCases
                |> Array.map(fun uc ->
                    let fieldTypes =
                        uc.GetFields() 
                        |> Array.map(fun pi -> pi.PropertyType)
                    {|
                        name =  uc.Name
                        fieldTypes = fieldTypes
                        reader = FSharpValue.PreComputeUnionReader uc
                    |})

            {
                new IEqualityComparer with
                    member this.Equals(ls1,ls2) = 
                        let n1 = tagReader ls1
                        let n2 = tagReader ls2
                        if n1 = n2 then
                            let uc = unionCases.[n1]
                            let objs1 = uc.reader ls1
                            let objs2 = uc.reader ls2
                            Array.zip3 uc.fieldTypes objs1 objs2
                            |> Array.forall(fun(fty,obj1,obj2)-> (loop fty).Equals(obj1,obj2))
                        else false

                    member this.GetHashCode(x) = 
                        let tag = tagReader x
                        let uc = unionCases.[tag]
                        let objs = uc.reader x
                        let types = [|yield typeof<int>;yield! uc.fieldTypes|]
                        let values = [|yield box tag; yield! objs|]

                        Array.zip types values
                        |> Array.map(fun(t,v) -> (loop t).GetHashCode(v))
                        |> hash
            }

