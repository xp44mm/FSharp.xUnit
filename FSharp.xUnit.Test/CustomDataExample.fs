namespace FSharp.xUnit
open System
open FSharp.Idioms.EqualityComparers
open System.Collections


type ProductionCrew(production:list<string>,leftside:string,body:list<string>) =
    member _.production = production
    member _.leftside = leftside
    member _.body = body

module CustomDataExample =
    let ProductionCrewEqualityChecker (ty:Type) =
        if ty = typeof<ProductionCrew> then
            fun (comparer:IEqualityComparer) (x:obj,y:obj) ->
                let x = unbox<ProductionCrew> x
                let y = unbox<ProductionCrew> y
                comparer.Equals( x.production, y.production)  &&
                comparer.Equals( x.leftside, y.leftside)      &&
                comparer.Equals( x.body, y.body)
            |> Some
        else None
    let eqs = ProductionCrewEqualityChecker :: EqualityComparerUtils.equalities
    let equals<'t> (x:'t, y:'t) = 
        EqualityComparerUtils.equalFn eqs ( x, y)
    
    let should<'t> = EqualConfig<'t>(equals<'t>)
