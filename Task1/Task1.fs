open System

let Add (numbers: string) =
    let parts = numbers.Split(',')
    if parts.Length = 2 then
        let a = int parts[0]
        let b = int parts[1]
        a + b
    else
        0

[<EntryPoint>]
let main argv =
    printfn "Result of '1,2' -> %d" (Add "1,2")
    0