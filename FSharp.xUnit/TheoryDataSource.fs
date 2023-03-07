namespace FSharp.xUnit

type TheoryDataSource<'k,'v when 'k:comparison>(source:list<'k*'v>) =
    let mp = Map.ofList source

    /// 
    member _.keys = 
        source
        |> Seq.map (fst>>box>>Array.singleton)

    ///
    member _.Item with get(key) = mp.[key]
