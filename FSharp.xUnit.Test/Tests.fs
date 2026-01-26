namespace FSharp.xUnit

open System
open System.Collections.Generic

open Xunit
//open Xunit.Abstractions

open FSharp.Idioms.Literal


type Test(output:ITestOutputHelper) =
    [<Theory>]
    [<InlineData()>] // **note**
    [<InlineData(1)>]
    [<InlineData(1,2)>]
    member _.``variable arguments`` ([<System.ParamArray>]sq:int[]) =
        for i in sq do
            output.WriteLine($"{i}")

    [<Fact>]
    member _.``My test`` () =
        Should.equal 1 1

    [<Fact>]
    member _.``Nullable test`` () =
        Should.equal (Nullable()) (Nullable())
        Should.equal (Nullable 1) (Nullable 1)
        Should.notEqual (Nullable 1) (Nullable 2)

    [<Fact>]
    member _.``array test`` () =
        Should.equal [||] [||]
        Should.equal [|1|] [|1|]
        Should.notEqual [|1|] [|2|]

    [<Fact>]
    member _.``tuple test`` () =
        Should.equal (1,2) (1,2)
        Should.notEqual (1,2) (1,3)

    [<Fact>]
    member _.``record test`` () =
        Should.equal {|x=1;y=2|} {|x=1;y=2|}

    [<Fact>]
    member _.``list test`` () =
        Should.equal [1] [1]

    [<Fact>]
    member _.``set test`` () =
        Should.equal (set [1]) (set [1])

    [<Fact>]
    member _.``map test`` () =
        Should.equal (Map[1,"1"]) (Map[1,"1"])

    [<Fact>]
    member _.``uion test`` () =
        Should.equal (Some 1) (Some 1)

    //[<Fact>]
    //member _.``seq test`` () =
    //    let x = seq { 1..3 }
    //    let y = seq { 1; 2; 3 }
    //    let ex = Assert.Throws<NotImplementedException>(
    //        fun () ->
    //        Should.equal x y
    //    )
    //    output.WriteLine(ex.Message)

    [<Fact>]
    member _.``HashSet test`` () =
        let x = HashSet([ 1..3])
        let y = HashSet([ 1..3])
        Should.equal x y

    [<Fact>]
    member _.``ProductionCrewEqualityChecker test`` () =
        let x = ProductionCrew([""],"",[])
        let y = ProductionCrew([""],"",[])
        let z = ProductionCrew(["w"],"w",[])

        CustomDataExample.should.equal x y
        CustomDataExample.should.notEqual x z


