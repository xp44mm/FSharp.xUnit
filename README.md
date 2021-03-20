The class library extends `xunit` to assert equal. 

- It output data in F# style. 

- It's easier to customize the `IEqualityComparer`.

## install

```pm
Install-Package FSharp.xUnit
```

## GetStarted 

ʹ��ϵͳĬ�ϵ���ȱȽ�����

```F#
open FSharp.xUnit

[<Fact>]
let ``My test`` () =
    Should.equal 1 1
```

�෴�Ĳ���`Should.notEqual`

ʹ���Զ������ȱȽ����������ñȽ�����newtonsoft.net��.net���е�json�⣬����jpropertyȱ�ٽṹ���Ƚϵķ������������ʾ����ʾ��ζ�����������

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

�����`should`�����ǾͿ��Խ����Զ���Ĳ����ˣ����Եķ���������Ĳ�ࣺ

```F#
[<Fact>]
member this.``translateFields``() =
    let y = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
    let z = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
        
    should.equal y z

```

�෴�Ĳ���`should.notEqual`

