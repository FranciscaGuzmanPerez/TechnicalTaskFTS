open System

// -----------------------------------------------------------
// Function: Add
// Purpose:  Sums ANY number of integers in a string.
// Throws an exception if there are negative numbers.
//
// Supported delimiters:
//   - Default: ","  "\n"  "\r\n"  literal "\n"
//   - Custom:  //[delimiter]\n[numbers...] or //[***]\n[numbers...]
// Example:
//   1,2,3 = 6
//   //;\n1;2 = 3
//   1,-2,-3 =Exception: "negatives not allowed: -2, -3"
// -----------------------------------------------------------
let Add (numbers: string) =
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Default delimiters
        let baseDelims : string[] = [| ","; "\n"; "\r\n"; "\\n" |]

        // Helper to extract a custom delimiter (same as before)
        let parseCustom (s:string) =
            if s.StartsWith("//") then
                let rest = s.Substring(2)
                // find the first newline marker
                let idxRN = rest.IndexOf("\r\n")
                let idxN  = if idxRN < 0 then rest.IndexOf('\n') else -1
                let idxLitN = if idxRN < 0 && idxN < 0 then rest.IndexOf("\\n") else -1
                let (pos, sepLen) =
                    if idxRN >= 0 then idxRN, 2
                    elif idxN >= 0 then idxN, 1
                    elif idxLitN >= 0 then idxLitN, 2
                    else -1, -1
                if pos >= 0 then
                    let rawDelim = rest.Substring(0, pos).Trim()
                    let remaining = rest.Substring(pos + sepLen)
                    let delim =
                        if rawDelim.StartsWith("[") && rawDelim.EndsWith("]") then
                            rawDelim.Substring(1, rawDelim.Length - 2)
                        else rawDelim
                    (Some delim, remaining)
                else
                    (None, s)
            else
                (None, s)

        // Build final delimiters list
        let (custom, payload) =
            let (maybeDelim, remaining) = parseCustom numbers
            match maybeDelim with
            | Some d when d <> "" -> (Array.append baseDelims [| d |], remaining)
            | _ -> (baseDelims, numbers)

        // Split text
        let parts = payload.Split(custom, StringSplitOptions.RemoveEmptyEntries)

        // Convert and check for negatives
        let mutable total = 0
        let mutable negatives = []

        for part in parts do
            let clean = part.Trim()
            let mutable value = 0
            if Int32.TryParse(clean, &value) then
                if value < 0 then
                    negatives <- clean :: negatives
                else
                    total <- total + value

        // If any negative numbers were found → throw an exception
        if negatives <> [] then
            let msg = "negatives not allowed: " + String.Join(", ", List.rev negatives)
            raise (ArgumentException msg)

        total


// -----------------------------------------------------------
// Main Program Entry Point
// Reads multiple lines and prints either the sum or the exception message.
// -----------------------------------------------------------
[<EntryPoint>]
let main argv =
    let mutable keepRunning = true
    while keepRunning do
        let input = Console.ReadLine()
        match input with
        | null ->
            keepRunning <- false
        | _ ->
            try
                let result = Add input
                printfn "%d" result
            with
            | :? ArgumentException as ex ->
                // Show the exception message for negatives
                printfn "Exception: \"%s\"" ex.Message
