namespace FSharp.xUnit

open System
open System.Collections

type EqualityComparerAdapter =
    abstract filter: Type -> bool
    abstract getEqualityComparer: loop:(Type -> IEqualityComparer) * Type -> IEqualityComparer

