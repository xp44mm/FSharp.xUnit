namespace FSharp.xUnit

type ClassDataBase(generator: seq<obj[]>) = 
    interface seq<obj[]> with
        member this.GetEnumerator() = 
            generator.GetEnumerator()
        member this.GetEnumerator() = 
            generator.GetEnumerator() 
            :> System.Collections.IEnumerator
