module LogParser
    open System
    open System.IO
    open System.Text
    open System.Globalization

    type ProcessedMeasurement = {
        Date: DateTime
        Up: double
        Down: double
        Ping: double
    }

    type Measurement = 
        | FinishedMeasurement of ProcessedMeasurement
        | UnfinishedMeasurement of Option<DateTime> * Option<double> * Option<double> * Option<double>
        

    let getAllLines path = File.ReadAllLines path |> Array.toList

    let dataPointRegex = new RegularExpressions.Regex(@"^[a-zA-Z]*\s*[a-zA-Z]*:")
    let getDateRegex = new RegularExpressions.Regex(@"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}")
    let getDataRegex = new RegularExpressions.Regex(@"\d{1,3},\d{1,3}")

    let parseDate str = DateTime.Parse(str, new CultureInfo("sv-SE"))

    let parseDouble (str:string) = 
        match Double.TryParse str with
        | true, x -> x
        | _ -> 0.0

    let processLine line measurement =
        match dataPointRegex.Match(line).Value with
        | "Start:" -> 
            let (_, up, down, ping) = measurement
            let parsedDate = getDateRegex.Match(line).Value |> parseDate
            (Some(parsedDate), up, down, ping)
        | "Download:" ->
            let (date, up, _, ping) = measurement
            let parsedDown = getDataRegex.Match(line).Value |> parseDouble
            (date, up, Some(parsedDown), ping)
        | "Upload:" ->
            let (date, _, down, ping) = measurement
            let parsedUp = getDataRegex.Match(line).Value |> parseDouble
            (date, Some(parsedUp), down, ping)
        | "Latency:" ->
            let (date, up, down, _) = measurement
            let parsedPing = getDataRegex.Match(line).Value |> parseDouble
            (date, up, down, Some(parsedPing))       
        | _ -> measurement
        

    let rec processLines lines measurements currentMeasurement =
       match currentMeasurement with
       | FinishedMeasurement(fm) -> 
            processLines lines (measurements @ [fm]) (UnfinishedMeasurement(None, None, None, None))
       | UnfinishedMeasurement(date, up, down, ping) -> 
           match lines with
           | head::tail -> 
                let measurement = processLine head (date, up, down, ping)
                match measurement with
                | (Some date, Some up, Some down, Some ping) -> processLines tail measurements (FinishedMeasurement({ Date = date; Up = up; Down = down; Ping = ping }))
                | (date, up, down, ping) -> processLines tail measurements (UnfinishedMeasurement(date, up, down, ping))
           | [] -> measurements

    let getMeasurements fromFile = processLines (getAllLines fromFile) [] (UnfinishedMeasurement(None, None, None, None))


