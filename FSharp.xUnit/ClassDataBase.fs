﻿namespace FSharp.xUnit

open System.Collections

//    [<ClassData(typeof<MyArrays1>)>]
type ClassDataBase(sq: seq<obj[]>) = 
    interface seq<obj[]> with
        member this.GetEnumerator() = sq.GetEnumerator()
        member this.GetEnumerator() = sq.GetEnumerator() :> IEnumerator

