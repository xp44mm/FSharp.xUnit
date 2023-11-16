module FSharp.xUnit.Program

open System
open Xunit
open Xunit.Abstractions
open FSharp.Idioms.Literal

open System.Collections.Generic

let [<EntryPoint>] main _ = 
    let x = HashSet [1;2;3]
    let y = HashSet [1;2;3]
    Console.WriteLine($"{x=y}")
    Console.WriteLine($"{x.SetEquals y}")

    0
