namespace FSharp.xUnit

type ProductionCrew(production:list<string>,leftside:string,body:list<string>) =
    member _.production = production
    member _.leftside = leftside
    member _.body = body

open System
open FSharp.Idioms.EqualityCheckers

module CustomDataExample =
    let ProductionCrewEqualityChecker (ty:Type) =
        {
        check = ty = typeof<ProductionCrew>
        equal = fun (loop:Type->obj->obj->bool) x y ->
            let x = unbox<ProductionCrew> x
            let y = unbox<ProductionCrew> y
            [
                loop typeof<list<string>> x.production y.production
                loop typeof<string> x.leftside y.leftside
                loop typeof<list<string>> x.body y.body
            ] |> List.reduce ( && )
        }
    let checkers = ProductionCrewEqualityChecker :: EqualityCheckerUtils.checkers
    let equals<'t> (x:'t) (y:'t) = 
        EqualityCheckerUtils.equalsFn checkers typeof<'t> x y
    
    let should<'t> = EqualConfig<'t>(equals<'t>)
