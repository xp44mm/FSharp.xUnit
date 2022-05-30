namespace FSharp.xUnit

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Reflection

type MyArrays1() = 
    inherit ClassDataBase([ 
        [| 3; 4 |]; 
        [| 32; 42 |] 
    ])

type MyArrays2() = 
    inherit ClassDataBase([ 
        ("smallest prime?", 2, true)
        ("how many roads must a man walk down?", 41, false)
    ] |> Seq.map FSharpValue.GetTupleFields
)

type ClassDataBaseTest(output:ITestOutputHelper) =

    [<Theory>]
    [<ClassData(typeof<MyArrays1>)>]
    member _. v1 (a : int, b : int) = 
        Assert.NotEqual(a, b)

    [<Theory; ClassData(typeof<MyArrays2>)>]
    member _.myTest(q, a, expected) =
        let isAnswer (q:string) a =
            q.Split(" ").Length = a            
        Assert.Equal(isAnswer q a, expected)