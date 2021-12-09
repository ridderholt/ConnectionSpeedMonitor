module ArgumentsParser
    type Arguments = {
        ShowHelp: bool;
        Input: string;
        Output: string;
    }

    let parseArguments (args: string array) =
        match args |> Array.toList with
        | _::input::output::_ -> Some({ ShowHelp = false; Input = input; Output = output })
        | _::showHelp::_ when showHelp = "--help" -> Some({ ShowHelp = true; Input = ""; Output = "" })
        | _ -> None
