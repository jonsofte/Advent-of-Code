// https://adventofcode.com/2025/day/2

open System.IO

let getRanges file =
    File.ReadAllText(file).Split(',')
    |> Seq.map(fun s -> s.Split('-')) 
    |> Seq.map(fun p -> (int64 p[0], int64 p[1]))

let splitString n (value: string) =
    if (value.Length < n) || value.Length % n <> 0 then
        Seq.empty
    else 
        value
        |> Seq.chunkBySize (value.Length / n)
        |> Seq.map (fun s -> new string(s))

let isInvalid split value =
    string value
    |> splitString split  
    |> Seq.distinct 
    |> Seq.length = 1

let getInvalidValues isInvalid range =
    [(fst range)..(snd range)]
    |> Seq.filter isInvalid 

let part1 =
    getRanges "input.txt"
    |> Seq.collect (getInvalidValues (isInvalid 2))
    |> Seq.sum 

let isInvalidAllSplits value = 
    [2..(string value).Length] 
    |> Seq.exists (fun i -> isInvalid i value)

let part2 =
    getRanges "input.txt"
    |> Seq.collect (getInvalidValues isInvalidAllSplits)
    |> Seq.sum 

printfn "%d" part1
printfn "%d" part2
