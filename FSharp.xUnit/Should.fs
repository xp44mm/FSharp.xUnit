module FSharp.xUnit.Should

let config = EqualConfig()

let equal<'a when 'a:equality> (expected:'a) actual =
    config.equal expected actual

let notEqual<'a when 'a:equality> (expected:'a) actual = 
    config.notEqual expected actual

