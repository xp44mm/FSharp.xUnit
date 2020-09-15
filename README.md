仅一个函数`Should.equal`的库，利用F#语言的类型推导，断言时不用给出类型参数。

```F#
open FSharp.xUnit

[<Fact>]
let ``My test`` () =
    Should.equal 1 1
```