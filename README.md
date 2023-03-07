The class library extends `xunit` to assert equal. 

- It output data in F# style. 

- It's easier to customize the `IEqualityComparer`.

## install

```pm
Install-Package FSharp.xUnit
```

## GetStarted 

使用系统默认的相等比较器：

```F#
open FSharp.xUnit

[<Fact>]
let ``My test`` () =
    Should.equal 1 1
```

相反的操作`Should.notEqual`

使用自定义的相等比较器，先配置比较器，newtonsoft.net是.net流行的json库，但是`JProperty`缺少结构化比较的方法，这个代码示例演示如何定义适配器：

```F#
let tryGetJProperty (ty: Type) =
    if ty = typeof<JProperty> then
        Some(fun (loop:Type -> IEqualityComparer) -> 
            let stringComparer = loop typeof<string>
            let jTokenEqualityComparer = JTokenEqualityComparer()
            {
                new IEqualityComparer with
                    member this.Equals(p1,p2) =
                        let p1 = unbox<JProperty> p1
                        let p2 = unbox<JProperty> p2
                        stringComparer.Equals(p1.Name, p2.Name) && jTokenEqualityComparer.Equals(p1.Value, p2.Value)
                    member this.GetHashCode(p) = 
                        let p = unbox<JProperty> p
                        hash [|stringComparer.GetHashCode p.Name; jTokenEqualityComparer.GetHashCode p.Value|]
            })
    else None
    
let tryGetJToken  (ty: Type) =
    if typeof<JToken>.IsAssignableFrom ty then
        Some(fun (loop:Type -> IEqualityComparer) -> 
            let genericComp = JTokenEqualityComparer()
            {
                new IEqualityComparer with
                    member this.Equals(p1,p2) = genericComp.Equals(unbox<JToken> p1,unbox<JToken> p2)
                    member this.GetHashCode(p) = 
                        genericComp.GetHashCode(unbox<JToken> p)
            })
    else None

let should = EqualConfig.``override``[ 
    tryGetJProperty
    tryGetJToken
    ]
```

注意派生类要放到基类的上面，否则派生类数据就会进到基类适配器，派生类适配器被旁路掉了。

获得了`should`，我们就可以进行自定义的测试了，测试的方法和上面的差不多：

```F#
[<Fact>]
member this.``translateFields``() =
    let y = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
    let z = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
    should.equal y z
```

相反的操作`should.notEqual`

### ClassDataBase

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

### TheoryDataSource

TheoryDataSource 向构造函数提供一个键值对列表，成员keys提供给MemberData，索引属性提供额外数据字段。

```FSharp
namespace FSharp.xUnit

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type TheoryDataSourceTest(output:ITestOutputHelper) =
    //* ctor
    static let dataSource = TheoryDataSource([
        0,[]
        1,[()]
        2,[();()]
    ])

    //* keys
    static member keys = dataSource.keys

    [<Theory>]
    [<MemberData(nameof TheoryDataSourceTest.keys)>]
    member _.``unit list test`` (x) =
        let y = List.replicate x ()
        let e = dataSource.[x] //*
        Should.equal e y
```