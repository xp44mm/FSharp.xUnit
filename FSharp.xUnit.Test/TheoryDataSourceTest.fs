namespace FSharp.xUnit

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type TheoryDataSourceTest(output:ITestOutputHelper) =
    static let dataSource = TheoryDataSource([
        0,[]
        1,[()]
        2,[();()]
    ])

    static member keys = dataSource.keys

    [<Theory>]
    [<MemberData(nameof TheoryDataSourceTest.keys)>]
    member _.``unit list test`` (x) =
        let y = List.replicate x ()
        let e = dataSource.[x]
        Should.equal e y

