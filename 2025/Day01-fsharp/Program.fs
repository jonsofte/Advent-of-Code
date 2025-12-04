// https://adventofcode.com/2025/day/1

open System.IO

type Command = Left of int | Right of int

let createCommand (input: string) =
    match (input[0], input[1..]) with
        | 'L', distance -> Left (int distance)
        | 'R', distance -> Right (int distance) 
        | _,_ -> raise (System.ApplicationException("Invalid input"))

let commands = 
    "input.txt" 
    |> File.ReadAllLines 
    |> Seq.map (createCommand)

let rotate current command =
    match command with
        | Left distance -> (current - distance) % 100
        | Right distance -> (current + distance) % 100

let part1 = 
    commands
    |> Seq.scan rotate 50  
    |> Seq.filter ((=) 0) 
    |> Seq.length

let getSteps command = 
    match command with
        | Left distance -> Seq.replicate distance -1
        | Right distance -> Seq.replicate distance 1

let rotateStep currentValue zeros step =
    let newValue = (100 + currentValue + step) % 100;
    (newValue, zeros + if newValue = 0 then 1 else 0)

let part2 = 
    commands 
    |> Seq.collect getSteps
    |> Seq.fold (fun (current,zeros) step -> rotateStep current zeros step) (50,0) 
    |> snd

printfn "%d" part1
printfn "%d" part2