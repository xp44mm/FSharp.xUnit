namespace FSharp.xUnit
open FSharp.Reflection

type MemberDataSource<'v>(source:list<obj[]*'v>) =

    let mp = System.Collections.Generic.Dictionary(HashIdentity.Structural)
    do for (k,v) in source do
        mp.Add(k,v)

    /// 
    member _.keys = source |> Seq.map fst

    ///
    member _.Item with get(key) = mp.[key]

    static member fromPrimitive(source:list<'k*'v>) =
        let ty = typeof<'k>
        if ty.IsPrimitive then
            source
            |> List.map(fun(k,v)->[|box k|],v)
        else failwith $"{ty}"
        |> MemberDataSource

    static member fromTuple(source:list<'k*'v>) =
        let ty = typeof<'k>
        if FSharpType.IsTuple ty then
            source
            |> List.map(fun(k,v) -> FSharpValue.GetTupleFields(box k),v)
        else failwith $"{ty}"
        |> MemberDataSource
