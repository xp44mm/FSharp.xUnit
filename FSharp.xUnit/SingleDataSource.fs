namespace FSharp.xUnit

open System

type SingleDataSource<'k,'v when 'k:comparison>(source:list<'k*'v>) =
    let mp = Map.ofList source

    /// 
    member _.keys = 
        let ty = typeof<'k>
        let allowedType (ty:Type) = ty.IsPrimitive || ty = typeof<String>

        if  allowedType ty || ty.IsArray && allowedType(ty.GetElementType()) then
            source
            |> Seq.map (fst>>box>>Array.singleton)
        else failwith $"expect primitive or string but: {ty}"


    ///
    member _.Item with get(key) = mp.[key]
