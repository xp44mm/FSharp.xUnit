namespace FSharp.xUnit

open System

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Literals.Literal

type ListDS()=
    static let ds = ArrayDataSource [
        //[],[] 
        [1],[()]
        [1;2],[();()]
    ]

    static member keys = ds.keys
    static member get key = ds.[key]

type ArrayDS()=
    static let ds = ArrayDataSource [
        //[],[] 
        [|1|],[()]
        [|1;2|],[();()]
    ]

    static member keys = ds.keys
    static member get key = ds.[key]


type ArrayDataSourceTest(output:ITestOutputHelper) =

    [<Theory>]
    [<MemberData(nameof ListDS.keys,MemberType=typeof<ListDS>)>]
    member _.``ListDS test`` ([<ParamArray>]xs:int[]) =
        output.WriteLine(stringify xs)
        let y = List.replicate xs.Length ()
        let e = ListDS.get xs
        Should.equal e y

    [<Theory>]
    [<MemberData(nameof ArrayDS.keys,MemberType=typeof<ArrayDS>)>]
    member _.``ArrayDS test`` ([<ParamArray>]xs:int[]) =
        output.WriteLine(stringify xs)
        let y = List.replicate xs.Length ()
        let e = ListDS.get xs
        Should.equal e y
