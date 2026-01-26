namespace FSharp.xUnit

open Xunit
open Xunit.v3

open System.Reflection
open System.Collections.Generic
open System.Threading.Tasks

[<Sealed>]
type NaturalAttribute(start: int, count: int) =
    inherit DataAttribute()

    let values = [|
        for i in start .. (start + count - 1) -> [| box i |]: obj[]
    |]

    /// 从 0 开始的构造函数
    new(count: int) = new NaturalAttribute(0, count)

    override _.GetData(testMethod: MethodInfo, disposalTracker: Sdk.DisposalTracker) =
        let rows = ResizeArray<ITheoryDataRow>()

        for i in start .. (start + count - 1) do
            rows.Add(TheoryDataRow([| box i |]))

        ValueTask<IReadOnlyCollection<ITheoryDataRow>>(rows)

    override _.SupportsDiscoveryEnumeration() = true
