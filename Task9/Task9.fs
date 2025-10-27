open System

// -----------------------------------------------------------
// Function: Add
// Sum ANY number of integers in the input string.
//
// - Default delimiters: ","  "\n"  "\r\n"  literal "\n" (written as "\\n")
// - Custom delimiter header (no regex used):
//      //;<nl>numbers...
//      //[***]<nl>numbers...
//      //[*][%]<nl>numbers...    (multiple delimiters, any length)
// - Throws exception for negatives and ignores numbers > 1000.
//
// Examples:
//   1,2,3 = 6
//   //;\n1;2 = 3
//   //[***]\n1***2***3 = 6
//   2,1001 = 2
//   1,-2,-3 = Exception: "negatives not allowed: -2, -3"
// -----------------------------------------------------------
let Add (numbers: string) =
    // Empty / null / only spaces => 0
    if String.IsNullOrWhiteSpace numbers then
        0
    else
        // Always-supported delimiters
        let baseDelims : string[] = [| ","; "\n"; "\r\n"; "\\n" |]

        // Parse optional custom delimiter header and return (delimiters, payload)
        // Split header manually looking for [ ... ] blocks.
        let parseHeader (s:string) : string[] * string =
            if s.StartsWith("//") then
                let rest = s.Substring(2)

                // Find the header terminator: support \r\n, \n, and literal \n ("\\n")
                let idxRN   = rest.IndexOf("\r\n", StringComparison.Ordinal)
                let idxN    = if idxRN < 0 then rest.IndexOf('\n') else -1
                let idxLitN = if idxRN < 0 && idxN < 0 then rest.IndexOf("\\n", StringComparison.Ordinal) else -1

                let (pos, nlLen) =
                    if idxRN >= 0 then idxRN, 2
                    elif idxN >= 0 then idxN, 1
                    elif idxLitN >= 0 then idxLitN, 2
                    else -1, -1

                if pos >= 0 then
                    let header  = rest.Substring(0, pos).Trim()
                    let payload = rest.Substring(pos + nlLen)

                    // Collect custom delimiters:
                    // If the header contains [ and ], extract ALL [ ... ] blocks.
                    // Otherwise, treat the whole header as a single delimiter.
                    let delimiters =
                        if header.Contains("[") && header.Contains("]") then
                            // Manual scan: find every [ ... ] 
                            let collected = System.Collections.Generic.List<string>()
                            let mutable i = 0
                            while i < header.Length do
                                if header.[i] = '[' then
                                    let closeIdx = header.IndexOf(']', i + 1)
                                    if closeIdx > i + 1 then
                                        let d = header.Substring(i + 1, closeIdx - i - 1)
                                        if not (String.IsNullOrEmpty d) then
                                            collected.Add(d)
                                        i <- closeIdx + 1
                                    else
                                        // malformed "["
                                        i <- i + 1
                                else
                                    i <- i + 1
                            collected |> Seq.toArray
                        else
                            // Single-char (or simple) delimiter form
                            if String.IsNullOrEmpty header then [||] else [| header |]

                    if delimiters.Length = 0 then
                        // No valid custom delimiters found = use defaults
                        baseDelims, s
                    else
                        // Merge defaults + custom(s)
                        Array.append baseDelims delimiters, payload
                else
                    // Malformed header = fall back to defaults
                    baseDelims, s
            else
                // No custom header = defaults only
                baseDelims, s

        let (delims, payload) = parseHeader numbers

        // Split by all delimiters, ignoring empty pieces
        let parts = payload.Split(delims, StringSplitOptions.RemoveEmptyEntries)

        // Convert, validate negatives, and sum ignoring > 1000
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
                // else: ignore > 1000

        // Throw if any negatives were found
        if negatives <> [] then
            let msg = "negatives not allowed: " + String.Join(", ", List.rev negatives)
            raise (ArgumentException msg)

        total

// -----------------------------------------------------------
// Main Program Entry Point
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
