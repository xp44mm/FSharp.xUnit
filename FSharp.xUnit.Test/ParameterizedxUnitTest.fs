namespace FSharp.xUnit

open Xunit
//open Xunit.Abstractions

type ParameterizedxUnitTest(output:ITestOutputHelper) =
    // InlineData has a limitation though: 
    // It only accepts basic data types (string, int, bool, etc). 
    // It can’t deal with collections or custom types.
    [<Theory>]
    [<InlineData(1, 42, 43)>]
    [<InlineData(1,  2,  3)>]
    member _.``inlinedata hello world``(a: int, b: int, expected: int) =
        let actual = a + b
        Assert.Equal(expected, actual)

