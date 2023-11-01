module FSharp.xUnit.EqualityComparers.EqualityComparerCases

open System
open System.Collections
open System.Collections.Generic

open FSharp.Idioms
open FSharp.Reflection

let NullableEqualityComparerCase (ty: Type) =
    {
        finder = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>>
        get = fun (loop:Type -> IEqualityComparer) -> 
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
            }
    }
    
let ArrayEqualityComparerCase (ty: Type) =
    {
        finder = ty.IsArray && ty.GetArrayRank() = 1
        get = fun (loop:Type -> IEqualityComparer) -> 
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
            }
    }

let TupleEqualityComparerCase (ty: Type) =
    {
        finder = FSharpType.IsTuple ty
        get = fun (loop:Type -> IEqualityComparer) -> 
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
            }
    }

let RecordEqualityComparerCase (ty: Type) =
    {
        finder = FSharpType.IsRecord ty
        get = fun (loop:Type -> IEqualityComparer) -> 
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
    }
    
let ListEqualityComparerCase (ty: Type) =
    {
        finder = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<list<_>>
        get = fun (loop:Type -> IEqualityComparer) -> 
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
            }
    }
    
let SetEqualityComparerCase (ty: Type) =
    {
        finder = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>>
        get = fun (loop:Type -> IEqualityComparer) -> 
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
            }
    }
    
let MapEqualityComparerCase (ty: Type) =
    {
        finder = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>>
        get = fun (loop:Type -> IEqualityComparer) -> 
            let reader = MapType.readMap ty
            let elementType = FSharpType.MakeTupleType(ty.GenericTypeArguments)
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
            }
    }
    
let UnionEqualityComparerCase (ty: Type) =
    {
        finder = FSharpType.IsUnion ty
        get = fun (loop:Type -> IEqualityComparer) -> 
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
    }
    
let SeqEqualityComparerCase (ty: Type) =
    {
        finder = ty.IsGenericType && ty.GenericTypeArguments.Length = 1 && typedefof<seq<_>>.MakeGenericType(ty.GenericTypeArguments).IsAssignableFrom ty
        get = fun (loop:Type -> IEqualityComparer) -> 
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
    }



