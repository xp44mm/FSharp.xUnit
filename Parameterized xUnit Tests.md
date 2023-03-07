# Parameterized xUnit Tests with F#

## InlineData

```Fsharp
open Xunit
open Xunit.Abstractions

type ParameterizedxUnitTest(output:ITestOutputHelper) =
    [<Theory>]
    [<InlineData(1, 42, 43)>]
    [<InlineData(1,  2,  3)>]
    member _.``add 1 2 equals 3``(a: int, b: int, expected: int) =
        let actual = a + b
        Assert.Equal(expected, actual)
```

`InlineData`的限制是只能使用字面量常数。因为属性的参数必须是字面量。

> The minute you go further, you'll run into the same restrictions on what the CLI will actually enable - the bottom line is that at the IL level, using attribute constructors implies that everything needs to be boiled down to constants at compile time.

而`ClassData`和`MemberData`不要求使用字面量，但是如果在运行器能显示每行，每个参数必须使用基元类型或基元数组类型。

## ClassData

用法是定义一个类型，其继承自`ClassDataBase`，其定义在`FSharp.xUnit`中。

```Fsharp
open FSharp.xUnit

type MyArrays1() = 
    inherit ClassDataBase([ 
        [| 3; 4 |]; 
        [| 32; 42 |] 
    ])

type ClassDataBaseTest(output:ITestOutputHelper) =
    [<Theory>]
    [<ClassData(typeof<MyArrays1>)>]
    member _.v1 (a : int, b : int) = 
        Assert.NotEqual(a, b)
```

## MemberData

However, most idiomatic with xUnit for me is to use straight `MemberData`. 因为参数表每个参数可以有不同的类型，you have to use tuples to allow different arguments types. You can use the `FSharp.Reflection` namespace to good effect here. 

引用相同类型，请提供成员的名称：

```Fsharp
type MemberDataTest(output:ITestOutputHelper) =
    static member samples =
        [
            [|"Homer";""|],"Homer"
            [|"Marge";""|],"Marge"
        ]
        |> Seq.map FSharpValue.GetTupleFields

    [<Theory>]
    [<MemberData(nameof(MemberDataTest.samples))>]
    member _.``array different types``(a:string[], b) =
        let c = a.[0]
        Assert.Equal(c, b)
```

引用不同类型中的数据，请提供参数`MemberType`：

```Fsharp
open FSharp.Reflection

type TestData() =
  static member MyTestData = 
      [
          "smallest prime?", 2, true
          "how many roads must a man walk down?", 41, false
      ]
      |> Seq.map FSharpValue.GetTupleFields

type MemberDataTest(output:ITestOutputHelper) =
    [<Theory; MemberData("MyTestData", MemberType=typeof<TestData>)>]
    member _.myTest(q, a, expected) =
        let isAnswer (q:string) a =
            q.Split(" ").Length = a
        Assert.Equal(isAnswer q a, expected)
```

上面两个示例，The key thing is the line

```Fsharp
|> Seq.map FSharpValue.GetTupleFields
```

It takes the list of tuples and transforms it to the `seq<obj[]>` that XUnit expects.

## 用例

此用例，使用`Map`绕过xUnit不支持的数据类型，在测试资源管理器显示`MemberData`多行。

```Fsharp
type UnifyVoidElementTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    static let source = [
            "<br/>",[{index= 0;length= 5;value= TagSelfClosing("br",[])}]
            "<p><br></br></p>",[{index= 0;length= 3;value= TagStart("p",[])};{index= 3;length= 4;value= TagSelfClosing("br",[])};{index= 12;length= 4;value= TagEnd "p"}]
        ]

    static let mp = Map.ofList source

    static member keys = 
        source
        |> Seq.map (fst>>Array.singleton)
        
    [<Theory;MemberData(nameof UnifyVoidElementTest.keys)>]
    member _.``self closing``(x:string) =
        let y = 
            x
            |> SeniorTokenizer.tokenize
            |> Seq.choose (HtmlTokenSeniorUtils.unifyVoidElement)
            |> Seq.toList

        let a = mp.[x]
        show y
        Should.equal y a
```

此例中，所有的数据都收集在`source`中，构成一个行列表。每行是一个记录元组，第一项是输入数据，字符串类型。第二项输出数据，复杂类型。且第一项就是元组的键。`Theory`所需要的`keys`仅取第一项，因为每个键是xunit支持的类型，可以Theory分行显示，而剩余的数据部分，在测试方法中通过`mp.[x]`查询Map来获得。
