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
    let mutable keepRunning = true
    while keepRunning do
        let input = Console.ReadLine()

        match input with
        | null ->
            keepRunning <- false

        | _ ->
            let result = Add input
            printfn "%d" result

    // retornar 0 para indicar que termina el programa
    0
