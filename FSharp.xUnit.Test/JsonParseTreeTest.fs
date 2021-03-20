namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals

open FSharpCompiler.Parsing

open FSharp.xUnit

type JsonParseTreeTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
    let should = Register()

    [<Fact>]
    member this.``empty object``() =
        let x = Interior("value",[Interior("object",[Terminal ""])])
        let y = Interior("value",[Interior("object",[Terminal ""])])
        //show y
        should.equal x y

    //[<Fact>]
    //member this.``empty array``() =
    //    let x = "[]"
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Interior("array",[Terminal LEFT_BRACK;Terminal RIGHT_BRACK])])

    //[<Fact>]
    //member this.``null``() =
    //    let x = "null"
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Terminal NULL])

    //[<Fact>]
    //member this.``false``() =
    //    let x = "false"
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Terminal FALSE])

    //[<Fact>]
    //member this.``true``() =
    //    let x = "true"
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Terminal TRUE])

    //[<Fact>]
    //member this.``empty string``() =
    //    let x = String.replicate 2 "\""
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Terminal(STRING "")])

    //[<Fact>]
    //member this.``number``() =
    //    let x = "0"
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Terminal(NUMBER 0.0)])

    //[<Fact>]
    //member this.``single field object``() =
    //    let x = """{"a":0}"""
    //    let y = parse x
    //    //show y
    //    //show y.[0].[1].[0] //field
    //    should.equal y 
    //    <| Interior("value",[Interior("object",[Terminal LEFT_BRACE;
    //        Interior("fields",[Interior("field",[Terminal(STRING "a");Terminal COLON;Interior("value",[Terminal(NUMBER 0.0)])])]);
    //        Terminal RIGHT_BRACE])])

    //[<Fact>]
    //member this.``many field object``() =
    //    let x = """{"a":0,"b":null}"""
    //    let y = parse x
    //    //show y
    //    should.equal y 
    //    <| Interior("value",[Interior("object",[Terminal LEFT_BRACE;
    //        Interior("fields",[Interior("fields",[Interior("field",[Terminal(STRING "a");Terminal COLON;Interior("value",[Terminal(NUMBER 0.0)])])]);Terminal COMMA;Interior("field",[Terminal(STRING "b");Terminal COLON;Interior("value",[Terminal NULL])])]);
    //        Terminal RIGHT_BRACE])])

    //[<Fact>]
    //member this.``singleton array``() =
    //    let x = "[0]"
    //    let y = parse x
    //    show y
    //    should.equal y 
    //    <| Interior("value",[Interior("array",[Terminal LEFT_BRACK;Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])]);Terminal RIGHT_BRACK])])

    //[<Fact>]
    //member this.``many elements array``() =
    //    let x = "[0,1]"
    //    let y = parse x
    //    show y
    //    should.equal y 
    //    <| Interior("value",[Interior("array",[Terminal LEFT_BRACK;
    //        Interior("values",[Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])]);Terminal COMMA;Interior("value",[Terminal(NUMBER 1.0)])]);
    //        Terminal RIGHT_BRACK])])

