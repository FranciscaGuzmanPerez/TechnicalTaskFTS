open System

// -----------------------------------------------------------
// Function: Add
// Purpose:  Sums ANY number of comma-separated integers in a string.
// Examples:
//   1,2,3,4 = 10
//           =  0
//   -3,5,8   10
// Notes:
//   - Accepts negative numbers.
//   - Returns 0 for null/empty/whitespace.
//   - Ignores invalid parts (like 1,hello,2 = 3).
// -----------------------------------------------------------
let Add (numbers: string) =
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Split by comma into many parts
        let parts = numbers.Split(',')

        // Accumulate the sum over all parts (no truncation now)
        let mutable total = 0
        for part in parts do
            let cleanPart = part.Trim()
            let mutable value = 0
            if Int32.TryParse(cleanPart, &value) then
                total <- total + value
            // If parsing fails, just skip this part
        total


// -----------------------------------------------------------
// Main Program Entry Point
// Reads multiple lines and prints the sum for each.
// -----------------------------------------------------------
[<EntryPoint>] 
let main argv =

    let mutable keepRunning = true
    while keepRunning do
        let input = Console.ReadLine()
        match input with
        | null ->
            keepRunning <- false // EOF = exit
        | _ ->
            let result = Add input
            printfn "%d" result
    0
