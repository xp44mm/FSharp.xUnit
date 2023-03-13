namespace FSharp.xUnit

open System

type ArrayDataSource<'k,'v when 'k:comparison>(source:list<seq<'k>*'v>) =
    let mp = 
        source
        |> Seq.map(fun(x,y)-> Seq.toArray x,y)
        |> Map.ofSeq

    /// 
    member _.keys = 
        let ty = typeof<'k>
        if ty.IsPrimitive || ty = typeof<String> then
            source
            |> Seq.map(fst>>Seq.map box>>Seq.toArray)
        else failwith $"expect primitive or string but: {ty}"

    ///
    member _.Item with get(key) = mp.[key]
