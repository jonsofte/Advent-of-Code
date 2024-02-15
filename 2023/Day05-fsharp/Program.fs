open System.IO

// Part 1
type SeedMap = { Destination: int64; Source: int64 ; Range: int64 }

let input = "input.txt" |> File.ReadAllText |> fun t -> t.Split "\r\n\r\n"

let MapNewPosition map position = map.Destination + position - map.Source
let SeedIsInMap position map = position >= map.Source && position < map.Source + map.Range;

let CreateSeedMap (input: string array) = 
    { Destination = int64 input[0]; Source = int64 input[1]; Range = int64 input[2] }

let GetMapChunk (chunk: string) = 
    chunk.Split("\r\n") |> Seq.skip 1 |> Seq.map (fun t -> t.Split " ") |> Seq.map CreateSeedMap

let GetNewSeedPosition seedPosition maps =
    match maps |> Seq.tryFind(fun map -> map |> SeedIsInMap seedPosition) with
        | Some map -> MapNewPosition map seedPosition
        | None -> seedPosition

let MapSeed seed maps = (seed, maps) ||> Seq.fold (fun x -> GetNewSeedPosition(x))
let seeds = input[0].Split " " |> Seq.skip 1 |> Seq.map int64
let maps = input[1..] |> Seq.map GetMapChunk
let part1 = seeds |> Seq.map(fun seed -> maps |> MapSeed seed) |> Seq.min

printfn "%d" part1

// Part 2
type Range = {Start: int64; End: int64}

let MapSeedRange rangeStart rangeEnd map = 
    { Start = MapNewPosition map rangeStart; End = MapNewPosition map rangeEnd}

let FindPreRange map currentSeedRange = 
    let Start, End = currentSeedRange.Start, min currentSeedRange.End map.Source
    if Start < End then Some { Start = Start; End = End } else None

let FindPostRange map currentSeedRange = 
    let Start, End = max (map.Source + map.Range) currentSeedRange.Start, currentSeedRange.End
    if Start < End then Some { Start = Start; End = End } else None

let FindMappedRange map currentSeedRange = 
    let Start, End = max currentSeedRange.Start (map.Source), min currentSeedRange.End (map.Source + map.Range)
    if Start < End then Some (MapSeedRange Start End map) else None

let PartitionRanges unmapped mapped map seedRange =
    let prePartition = FindPreRange map seedRange;
    let postPartiton = FindPostRange map seedRange;
    let mappedPartiton = FindMappedRange map seedRange;
    let unmapped  = if prePartition.IsSome then Seq.append unmapped [prePartition.Value] else unmapped 
    let unmapped = if postPartiton.IsSome then Seq.append unmapped [postPartiton.Value] else unmapped
    let mapped =  if mappedPartiton.IsSome then Seq.append mapped [mappedPartiton.Value] else mapped
    (unmapped,mapped)

let ApplyMapToRange seedRanges mappedRanges map =
    ((Seq.empty,mappedRanges), seedRanges) ||> Seq.fold (fun (unmapped, mapped) range -> 
        PartitionRanges unmapped mapped map range)

let MapSegment mapCollection unmappedRanges =
    let (unmappedRanges, foundRanges) = 
        ((unmappedRanges, Seq.empty), mapCollection) ||> Seq.fold 
            (fun (unmappedRanges, foundRanges) seedMap -> ApplyMapToRange unmappedRanges foundRanges seedMap)
    Seq.append unmappedRanges foundRanges

let MapSeedRanges mapSegments seedRanges =
    mapSegments |> Seq.fold (fun range map -> MapSegment map range ) seedRanges

let seedRanges = seeds |> Seq.chunkBySize 2 |> Seq.map (fun r -> {Start = int64 r[0]; End = (int64 r[0] + r[1] - 1L)})
let part2 = MapSeedRanges maps seedRanges |> Seq.map(fun range -> range.Start) |> Seq.min

printfn "%d" part2