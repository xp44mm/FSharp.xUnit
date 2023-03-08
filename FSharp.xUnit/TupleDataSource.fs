namespace FSharp.xUnit

open FSharp.Reflection
open System.Collections.Generic

type TupleDataSource<'k,'v when 'k:comparison>(source:list<'k*'v>) =
    let mp = Map.ofList source

    /// 
    member _.dataSource =
        let ty = typeof<'k>
        if FSharpType.IsTuple ty then
            source
            |> Seq.map(fun(k,_) -> FSharpValue.GetTupleFields k)
        else failwith $"expect tuple but: {ty}"

    ///
    member _.Item with get(key) = mp.[key]

        
