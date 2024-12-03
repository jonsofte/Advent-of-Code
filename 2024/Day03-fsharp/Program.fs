// https://adventofcode.com/2024/day/3

open System.IO
open System.Text.RegularExpressions;

let input = "input.txt" |> File.ReadAllText
let multiply (m: Match) = int m.Groups[1].Value * int m.Groups[2].Value

let mulMatches = Regex.Matches(input,"mul\((\d+),(\d+)\)")
let part1 = mulMatches |> Seq.map multiply |> Seq.sum

type result = {Enabled: bool; Sum: int}

let calculate current (value: Match) = 
    match (current.Enabled,value.Value) with
    | _,"don't()" -> {Enabled = false; Sum = current.Sum}
    | _,"do()" -> {Enabled = true; Sum = current.Sum}
    | true,_ -> {Enabled = true; Sum = current.Sum + multiply value}
    | false,_ -> {Enabled = false; Sum = current.Sum}

let allMatches = Regex.Matches(input,"mul\((\d+),(\d+)\)|do[(][)]|don't[(][)]")
let part2 = (allMatches |> Seq.fold calculate {Enabled = true; Sum=0}).Sum

printfn "%d" part1
printfn "%d" part2