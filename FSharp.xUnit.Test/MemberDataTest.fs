namespace FSharp.xUnit

open Xunit
open Xunit.Abstractions
open FSharp.Reflection

type TestData() =
  static member MyTestData =
    [ "smallest prime?", 2, true
      "how many roads must a man walk down?", 41, false
    ] |> Seq.map FSharpValue.GetTupleFields

type MemberDataTest(output:ITestOutputHelper) =
    // in another type
    [<Theory; MemberData("MyTestData", MemberType=typeof<TestData>)>]
    member _.myTest(q, a, expected) =
        let isAnswer (q:string) a =
            q.Split(" ").Length = a
        Assert.Equal(isAnswer q a, expected)
    // in same type
    static member samples =
        [
            [|"Homer";""|],"Homer"
            [|"Marge";""|],"Marge"
        ]
        |> Seq.map FSharpValue.GetTupleFields

    [<Theory>]
    [<MemberData(nameof(MemberDataTest.samples))>]
    member _.``array different types``(a:string[], b) =
        // 基元数组能够分行
        let c = a.[0]
        Assert.Equal(c, b)


