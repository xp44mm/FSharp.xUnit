namespace FSharp.xUnit

open Xunit
//open Xunit.Abstractions
open FSharp.Idioms.Literal

type IntDS() =
    static let ds = SingleDataSource([
        0,[]
        1,[()]
        2,[();()]
    ])
    static member keys = ds.keys
    static member get key = ds.[key]

type SingleDataSourceTest(output:ITestOutputHelper) =
    static let arrs = SingleDataSource [
        [||],[]
        [|1|],[()]
        [|1;2|],[();()]
    ]

    static member keys = arrs.keys

    [<Theory>]
    [<MemberData(nameof IntDS.keys,MemberType=typeof<IntDS>)>]
    member _.``unit list test`` (x:int) =
        let y = List.replicate x ()
        let e = IntDS.get x
        Should.equal e y

    [<Theory>]
    [<MemberData(nameof SingleDataSourceTest.keys)>]
    member _.``ArrayDataSource test`` (xs:int[]) =
        output.WriteLine(stringify xs)
        let y = List.replicate xs.Length ()
        let e = arrs.[xs]
        Should.equal e y
