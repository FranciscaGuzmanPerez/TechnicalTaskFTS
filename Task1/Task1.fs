// -----------------------------------------------------------
// Function: Add
// Purpose:  Sums up to two comma-separated numbers in a string.
// Behavior:
//   - Returns 0 if the input is null, empty, or only whitespace.
//   - Trims spaces around each number.
//   - Ignores non-numeric parts (they are skipped).
// Example:
//      =  0
//   1,2   = 3
//   1,abc  = 1
//   1,4,100 = 5
//   -1,100 = 99
// -----------------------------------------------------------
let Add (numbers: string) =
    // If the string is empty, null, or contains only spaces, return 0
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Split the input string into parts using comma as a separator
        let parts = numbers.Split(',')

        // Take only the first two elements (ignore the rest)
        let firstTwo = parts |> Array.truncate 2

        // Create a mutable variable to store the running total
        let mutable total = 0

        // Loop through each element and try to convert it to an integer
        for part in firstTwo do
            // Remove any spaces around the number
            let cleanPart = part.Trim()

            // Temporary variable to hold the parsed number
            let mutable value = 0

            // Try to parse the string into an integer
            // If successful, add it to the total
            if Int32.TryParse(cleanPart, &value) then
                total <- total + value

        // Return the final sum
        total


// -----------------------------------------------------------
// Program Entry Point
// Continuously reads input lines from the console and
// prints the result of Add() for each line.
// Press Enter on an empty line to print 0.
// -----------------------------------------------------------
[<EntryPoint>]
let main argv =
    // Boolean flag to control the loop
    let mutable keepRunning = true

    // Main read-eval-print loop
    while keepRunning do
        // Read one line from the console
        let input = Console.ReadLine()

        match input with
        // If null (EOF), exit the loop
        | null ->
            keepRunning <- false

        // Otherwise, calculate and print the sum
        | _ ->
            let result = Add input
            printfn "%d" result
 
    // Return 0 to indicate successful program termination
    0