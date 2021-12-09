module ChartPlotter
    open XPlot.Plotly
    open System.IO
    open LogParser


    let plotResults (outputPath:string) (measurements:ProcessedMeasurement list) =
        let getDataFromList (list:ProcessedMeasurement list) (mapFn: ProcessedMeasurement -> 'a) = list |> List.map(mapFn)
        let getData mapFn = getDataFromList measurements mapFn
        let dates = getData (fun m -> m.Date)

        let downloadData = 
            Scatter (
                x = dates,
                y = (getData (fun m -> m.Down)),
                name = "Download (Mbit)"
            )

        let uploadData = 
            Scatter (
                x = dates,
                y = (getData (fun m -> m.Up)),
                name = "Upload (Mbit)"
            )

        let pingData =
            Scatter (
                x = dates,
                y = (getData (fun m -> m.Ping)),
                name = "Latency (ms)"
            )

        let layout = Layout(title = "ISP Speedtext")
        let chart = [downloadData;uploadData;pingData]
                        |> Chart.Plot
                        |> Chart.WithLayout layout
                        |> Chart.WithHeight 1000
                        |> Chart.WithWidth 1500
                        |> Chart.WithLegend true
                        |> Chart.WithXTitle "Dates"
                        |> Chart.WithYTitle "Data points"
        
        File.WriteAllText(outputPath, chart.GetHtml())


