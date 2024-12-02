// https://adventofcode.com/2024/day/2

open System.IO

let input = "input.txt" |> File.ReadAllLines |> Seq.map (fun t -> t.Split(' ') |> Seq.map int)

let isSorted report = 
    (report,Seq.sort report) ||> Seq.compareWith Operators.compare = 0 || 
    (report,Seq.sortDescending report) ||> Seq.compareWith Operators.compare = 0

let invalidLevel report = 
    (report |> Seq.skip 1, report) ||> Seq.zip |> Seq.map (fun (f,s) -> abs (f-s)) 
    |> Seq.exists (fun x -> x>3 || x=0)

let isSafe report = isSorted report && not (invalidLevel report)

let isSafeWithSingleBadLevel report =
    [0..report |> Seq.length] 
    |> Seq.map (fun postion -> report |> Seq.indexed |> Seq.filter (fun (i,_) -> i <> postion) |> Seq.map (fun (_,v) -> v))
    |> Seq.exists (fun r -> isSafe r)

let getNumberOfReports filter input = input |>  Seq.map filter |> Seq.filter (fun x -> x) |> Seq.length 

printfn "%d" (input |> getNumberOfReports isSafe)
printfn "%d" (input |> getNumberOfReports isSafeWithSingleBadLevel)