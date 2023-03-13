namespace FSharp.xUnit

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type TupleDataSourceTest (output:ITestOutputHelper) =

    static let tuples = TupleDataSource([
        (0,"a"),[]
        (1,"b"),[()]
        (2,"c"),[();()]
    ])

    static member tupleDataSource = tuples.keys

    [<Theory>]
    [<MemberData(nameof TupleDataSourceTest.tupleDataSource)>]
    member _.``tuple test`` (a:int,b:string) =
        let y = List.replicate a ()
        let e = tuples.[a,b]
        Should.equal e y
