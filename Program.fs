
open System
open ArgumentsParser
open LogParser
open ChartPlotter

let args = Environment.GetCommandLineArgs() |> parseArguments

match args with
| Some(arg) ->
    if arg.ShowHelp then
        printf $"First argument: path to input{Environment.NewLine}Second argument: path to output"
    else
        getMeasurements arg.Input |> plotResults arg.Output
| _ -> printf "Invalid arguments"        

