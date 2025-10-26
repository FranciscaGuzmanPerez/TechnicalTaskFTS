open System

// -----------------------------------------------------------
// Function: Add
// Purpose:  Sums ANY number of integers in a string.
// Throws an exception for negatives and ignores numbers >1000.
//
// Supported delimiters:
//   - Default: ","  "\n"  "\r\n"  literal "\n"
//   - Custom:  //[delimiter]\n[numbers...] or //[***]\n[numbers...]
// Example:
//   2,1001 = 2
//   //;\n1;2;1002 = 3
//   1,-2,-3 = Exception: "negatives not allowed: -2, -3"
// -----------------------------------------------------------
let Add (numbers: string) =
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Default delimiters
        let baseDelims : string[] = [| ","; "\n"; "\r\n"; "\\n" |]

        // Extract custom delimiter if present
        let parseCustom (s:string) =
            if s.StartsWith("//") then
                let rest = s.Substring(2)
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

        // Build final delimiters and payload
        let (delims, payload) =
            let (maybeDelim, remaining) = parseCustom numbers
            match maybeDelim with
            | Some d when d <> "" -> (Array.append baseDelims [| d |], remaining)
            | _ -> (baseDelims, numbers)

        // Split and process
        let parts = payload.Split(delims, StringSplitOptions.RemoveEmptyEntries)
        let mutable total = 0
        let mutable negatives = []

        for part in parts do
            let clean = part.Trim()
            let mutable value = 0
            if Int32.TryParse(clean, &value) then
                if value < 0 then
                    negatives <- clean :: negatives
                elif value <= 1000 then
                    total <- total + value
                // else ignore numbers >1000

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
        | null -> keepRunning <- false
        | _ ->
            try
                let result = Add input
                printfn "%d" result
            with
            | :? ArgumentException as ex ->
                printfn "Exception: \"%s\"" ex.Message
    0
