namespace FSharp.xUnit.EqualityComparers

open System
open System.Collections

type EqualityComparerCase = {
    finder: bool
    get: (Type -> IEqualityComparer) -> IEqualityComparer
}

