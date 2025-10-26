open System

let Add (numbers: string) =
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        let parts = numbers.Split(',')
        let firstTwo = parts |> Array.truncate 2
        let mutable total = 0
        for part in firstTwo do
            let cleanPart = part.Trim()
            let mutable value = 0

            if Int32.TryParse(cleanPart, &value) then
                total <- total + value

        total

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
