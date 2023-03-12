namespace FSharp.xUnit

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type SingleDataSourceTest(output:ITestOutputHelper) =

    static let dataSource = SingleDataSource([
        0,[]
        1,[()]
        2,[();()]
    ])

    static member keys = dataSource.keys

    [<Theory>]
    [<MemberData(nameof SingleDataSourceTest.keys)>]
    member _.``unit list test`` (x) =
        let y = List.replicate x ()
        let e = dataSource.[x]
        Should.equal e y

