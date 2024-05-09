namespace FSharp.xUnit

open Xunit
open Xunit.Sdk

open System.Reflection


[<Sealed>]
type NaturalAttribute(start,count)=
    inherit DataAttribute()
    let values = new TheoryData<int>(seq { start .. count-1 })

    new(count) = new NaturalAttribute(0,count)
    override _.GetData(testMethod:MethodInfo) = values :> seq< obj[] >
