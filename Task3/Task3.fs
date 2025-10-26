open System

// -----------------------------------------------------------
// Function: Add
// Purpose:  Sums ANY number of integers separated by commas, real newlines,
//           or the literal "\n" sequence within the same line.
// Examples:
//   1,2,3,4  = 10
//        = 0
//   -3,5,8  = 10
//   1\n2,3 =  6   (where \n is literally backslash + n)
// Notes:
//   - Accepts negative numbers.
//   - Returns 0 for null/empty/whitespace.
//   - Ignores invalid parts (like "1,hello,2" = 3).
// -----------------------------------------------------------
let Add (numbers: string) =
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Support comma, real newlines, and the literal "\n"
        let delimiters : string[] = [| ","; "\n"; "\r\n"; "\\n" |] 
        let parts = numbers.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)

        let mutable total = 0
        for part in parts do
            let cleanPart = part.Trim()
            let mutable value = 0
            if Int32.TryParse(cleanPart, &value) then
                total <- total + value
        total

// -----------------------------------------------------------
// Main Program Entry Point
// Reads multiple lines and prints the sum for each.
// Press Enter for 0, EOF (Ctrl+Z / Ctrl+D) to exit.
// -----------------------------------------------------------
[<EntryPoint>]
let main argv =
    let mutable keepRunning = true
    while keepRunning do
        let input = Console.ReadLine()
        match input with
        | null ->
            keepRunning <- false // EOF -> exit
        | _ ->
            let result = Add input
            printfn "%d" result
    0

