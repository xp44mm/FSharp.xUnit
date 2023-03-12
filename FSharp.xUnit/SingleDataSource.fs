namespace FSharp.xUnit

type SingleDataSource<'k,'v when 'k:comparison>(source:list<'k*'v>) =
    let mp = Map.ofList source

    /// 
    member _.keys = 
        let ty = typeof<'k>
        if ty.IsPrimitive || ty = typeof<System.String> then
            source
            |> Seq.map (fst>>box>>Array.singleton)
        else failwith $"expect primitive or string but: {ty}"


    ///
    member _.Item with get(key) = mp.[key]
