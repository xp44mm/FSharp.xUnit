namespace FSharp.xUnit

open System.Collections

//    [<ClassData(typeof<MyArrays1>)>]
type ClassDataBase(sq: seq<obj[]>) = 
    interface seq<obj[]> with
        member _.GetEnumerator() = sq.GetEnumerator()
        member _.GetEnumerator() = sq.GetEnumerator() :> IEnumerator

