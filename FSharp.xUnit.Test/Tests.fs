namespace FSharp.xUnit

open System
open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open System.Collections.Generic

type Test(output:ITestOutputHelper) =
    let should = EqualConfig()
    [<Fact>]
    member _.``My test`` () =
        Should.equal 1 1

    [<Fact>]
    member _.``Nullable test`` () =
        should.equal (Nullable()) (Nullable())
        should.equal (Nullable 1) (Nullable 1)
        should.notEqual (Nullable 1) (Nullable 2)

    [<Fact>]
    member _.``array test`` () =
        should.equal [||] [||]
        should.equal [|1|] [|1|]
        should.notEqual [|1|] [|2|]

    [<Fact>]
    member _.``tuple test`` () =
        should.equal (1,2) (1,2)
        should.notEqual (1,2) (1,3)

    [<Fact>]
    member _.``record test`` () =
        should.equal {|x=1;y=2|} {|x=1;y=2|}

    [<Fact>]
    member _.``list test`` () =
        should.equal [1] [1]

    [<Fact>]
    member _.``set test`` () =
        should.equal (set[1]) (set[1])

    [<Fact>]
    member _.``map test`` () =
        should.equal (Map[1,"1"]) (Map[1,"1"])

    [<Fact>]
    member _.``uion test`` () =
        should.equal (Some 1) (Some 1)

    [<Fact>]
    member _.``seq test`` () =
        let x = seq { 1..3}
        let y = seq { 1; 2; 3}
        should.equal x y

    [<Fact>]
    member _.``HashSet test`` () =
        let x = HashSet([ 1..3])
        let y = HashSet([ 1..3])
        should.equal x y
