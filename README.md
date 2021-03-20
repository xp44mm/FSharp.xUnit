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

使用自定义的相等比较器，先配置比较器，newtonsoft.net是.net流行的json库，但是jproperty缺少结构化比较的方法，这个代码示例演示如何定义适配器：

```F#
type JPropertyEqualityComparerAdapter() =
    static member Singleton = JPropertyEqualityComparerAdapter() :> EqualityComparerAdapter
    interface EqualityComparerAdapter with
        member this.filter ty = ty = typeof<JProperty>
        member this.getEqualityComparer(loop,ty) =
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
            }
    

type JTokenEqualityComparerAdapter() =
    static member Singleton = JTokenEqualityComparerAdapter() :> EqualityComparerAdapter
    interface EqualityComparerAdapter with
        member this.filter ty = typeof<JToken>.IsAssignableFrom ty
        member this.getEqualityComparer(loop,ty) =
            let genericComp = JTokenEqualityComparer()
            {
                new IEqualityComparer with
                    member this.Equals(p1,p2) = genericComp.Equals(unbox<JToken> p1,unbox<JToken> p2)
                    member this.GetHashCode(p) = 
                        genericComp.GetHashCode(unbox<JToken> p)
            }

let should = Register.``override``[ JTokenEqualityComparerAdapter.Singleton; JPropertyEqualityComparerAdapter.Singleton ]
```

获得了`should`，我们就可以进行自定义的测试了，测试的方法和上面的差不多：

```F#
[<Fact>]
member this.``translateFields``() =
    let y = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
    let z = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
        
    should.equal y z

```

相反的操作`should.notEqual`

