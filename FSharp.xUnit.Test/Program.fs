module Program

open System
open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Literals
open FSharpCompiler.Parsing


let [<EntryPoint>] main _ = 
    let comparer = EqualityComparer.Automata<ParseTree<string>> EqualityComparer.adapters
    let x = Interior("value",[Interior("object",[Terminal ""])])
    let y = Interior("value",[Interior("object",[Terminal ""])])
    Console.WriteLine(Render.stringify <| comparer.GetType())
    let res = comparer.Equals(x,y)
    Console.WriteLine(Render.stringify res)

    0
