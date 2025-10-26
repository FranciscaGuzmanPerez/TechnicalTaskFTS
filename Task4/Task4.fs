open System

// -----------------------------------------------------------
// Sums ANY number of integers in a string.
//
// Delimiters supported (in order):
//   • Default: ","  "\n"  "\r\n"  literal "\n" (backslash + n)
//   • Custom single/multi-character delimiter:
//       Format: "//<delim>\n<numbers...>"   
//       Also accepts bracketed form: "//[<delim>]\n<numbers...>"
//       Works if the newline is real or the literal sequence "\n".
//
//
//   - Returns 0 for null/empty/whitespace.
//   - Accepts negative numbers.
//   - Ignores non-numeric parts safely (uses TryParse).
// -----------------------------------------------------------
let Add (numbers: string) =
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Base delimiters (always supported)
        let baseDelims : string[] = [| ","; "\n"; "\r\n"; "\\n" |]

        // Try to extract a custom delimiter header if the text starts with "//"
        let hasCustom = numbers.StartsWith("//")

        // Helper to parse custom delimiter header:
        // Returns: (maybeCustomDelimiter, remainingNumbersText)
        let parseCustom (s:string) =
            // Remove leading "//"
            let rest = s.Substring(2)

            // Find the first newline that ends the delimiter header.
            // We support: "\r\n", '\n', or the literal "\\n".
            let posRN = rest.IndexOf("\r\n", StringComparison.Ordinal)
            let posN  = if posRN < 0 then rest.IndexOf('\n') else -1
            let posLitN = if posRN < 0 && posN < 0 then rest.IndexOf("\\n", StringComparison.Ordinal) else -1

            // Decide which newline was found and its length
            let (pos, sepLen) =
                if posRN >= 0 then posRN, 2
                elif posN >= 0 then posN, 1
                elif posLitN >= 0 then posLitN, 2
                else -1, -1

            if pos >= 0 then
                // Delimiter text is everything before the newline
                let rawDelim = rest.Substring(0, pos).Trim()
                // Remaining numbers start right after the newline
                let remaining = rest.Substring(pos + sepLen)

                // Accept bracketed form: //[***]\n
                let customDelim =
                    if rawDelim.StartsWith("[") && rawDelim.EndsWith("]") && rawDelim.Length >= 2 then
                        rawDelim.Substring(1, rawDelim.Length - 2)
                    else
                        rawDelim

                (Some customDelim, remaining)
            else
                // No newline found; treat as no custom header
                (None, s)

        // Build the delimiter list and the payload (numbers) to split
        let (delimiters: string[], payload: string) =
            if hasCustom then
                let (maybeDelim, restText) = parseCustom numbers
                match maybeDelim with
                | Some d when d <> "" ->
                    (Array.append baseDelims [| d |], restText)
                | _ ->
                    (baseDelims, numbers) // malformed header = fall back to base behavior
            else
                (baseDelims, numbers)

        // Split and sum safely
        let parts = payload.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)

        let mutable total = 0
        for part in parts do
            let clean = part.Trim()
            let mutable value = 0
            if Int32.TryParse(clean, &value) then
                total <- total + value
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
        | null -> keepRunning <- false
        | _ ->
            let result = Add input
            printfn "%d" result
    0

