The class library extends `xunit` to assert equal. 

- It output data in F# style. 

- It's easier to customize the `equals`.

## GetStarted 

使用系统默认的相等比较器：

```F#
open FSharp.xUnit

[<Fact>]
let ``My test`` () =
    Should.equal 1 1
```

相反的操作`Should.notEqual`

使用自定义的相等比较器：

.\FSharp.xUnit.Test\CustomDataExample.fs

获得了`should`，我们就可以进行自定义的测试了，测试的方法和上面的差不多：

.\FSharp.xUnit.Test\Test.fs

相反的操作`should.notEqual`

## NaturalAttribute

Natural number sequence Theory is the simplest form of testing our theory with data.

```F#
type NaturalAttributeTest(output: ITestOutputHelper) =
    [<Theory>]
    [<Natural(4)>]
    member _.``Natural Number Sequence``(i:int) =
        let expect = [0;1;2;3]
        let actual = [0;1;2;3]
        Should.equal expect.[i] actual.[i]
```

`[<Natural(4)>]` is same as 

```F#
    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
```

### SingleDataSource

单参数数据源

`SingleDataSource`向构造函数提供一个键值对列表，成员`dataSource`提供给`MemberData`特性，索引属性提供不在参数表的额外数据。

使用方法：.\FSharp.xUnit.Test\SingleDataSourceTest.fs

### TupleDataSource

多参数数据源

使用方法：.\FSharp.xUnit.Test\TupleDataSourceTest.fs

### ArrayDataSource

变长参数数据源

使用方法：.\FSharp.xUnit.Test\ArrayDataSourceTest.fs


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

