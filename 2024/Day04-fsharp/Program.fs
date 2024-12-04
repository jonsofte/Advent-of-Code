// https://adventofcode.com/2024/day/4

open System.IO

type Position = {l: int; r: int}

let content = 
    "input.txt" 
    |> File.ReadAllLines |> Seq.indexed 
    |> Seq.collect (fun (li,l) -> l.ToCharArray() |> Seq.indexed |> Seq.map (fun (ri,c) -> ({l = li ; r = ri}, c)))
    |> Map

let getCharacter pos (dl,dr) = content |> Map.tryFind {l = pos.l + dl; r = pos.r + dr} 
let getDeltaString pos deltas = new string (deltas |> Seq.map (fun delta -> getCharacter pos delta) |> Seq.choose id |> Array.ofSeq)
let containsXMAS pos delta =  match (getDeltaString pos delta) with | "XMAS" -> true | "SAMX" -> true | _ -> false

let xmasDeltas = 
    [| 
    [|(0,0);(0,1);(0,2);(0,3)|]; 
    [|(0,0);(1,0);(2,0);(3,0)|];
    [|(0,0);(1,1);(2,2);(3,3)|];
    [|(0,0);(-1,1);(-2,2);(-3,3)|] 
    |]

let part1 = 
    content.Keys 
    |> Seq.collect (fun pos -> xmasDeltas |> Seq.map (fun deltas -> containsXMAS pos deltas)) 
    |> Seq.filter id 
    |> Seq.length 

let crossDeltas = 
    [| 
    [|(1,1);(0,0);(-1,-1)|]; 
    [|(-1,1);(0,0);(1,-1)|];
    |]

let containsMAS pos delta = match (getDeltaString pos delta) with | "MAS" -> true | "SAM" -> true | _ -> false

let part2 = 
    content.Keys 
    |> Seq.map (fun pos -> crossDeltas |> Seq.forall (fun deltas -> containsMAS pos deltas)) 
    |> Seq.filter id
    |> Seq.length 

printfn "%d" part1
printfn "%d" part2
