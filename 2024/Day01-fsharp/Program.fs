open System
open System.IO

let input = "input.txt" |> File.ReadAllLines |> Seq.map (fun t -> t.Split(' ',StringSplitOptions.RemoveEmptyEntries))

let firstColumn = input |> Seq.map (fun x -> int x[0]) |> Seq.sort
let secondColumn = input |> Seq.map (fun x -> int x[1]) |> Seq.sort

let part1 = (firstColumn,secondColumn) ||> Seq.zip  |> Seq.map (fun (f,s) -> abs (f-s)) |> Seq.sum
let part2 = firstColumn |> Seq.map (fun f -> (secondColumn |> Seq.filter (fun s -> f=s) |> Seq.length) * f) |> Seq.sum 

printfn "%d" part1
printfn "%d" part2 