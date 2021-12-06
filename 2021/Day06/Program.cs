// https://adventofcode.com/2021/day/6

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt").Split(',').Select(x => int.Parse(x)).ToList();

Console.WriteLine(GetNunberOfFishByDay(input, 80));
Console.WriteLine(GetNunberOfFishByDay(input, 256));

static long GetNunberOfFishByDay(List<int> input, int numberOfDays)
{
    var fishPerDay = input.GroupBy(x => x).ToDictionary(g => g.Key, g => (long)g.Count());
    Enumerable.Range(0, numberOfDays).ToList().ForEach(x => fishPerDay = NextDay(fishPerDay));
    return fishPerDay.Values.Sum();
};

static Dictionary<int, long> NextDay(Dictionary<int, long> fishPerDay) => new()
{
    [0] = fishPerDay.GetValueOrDefault(1),
    [1] = fishPerDay.GetValueOrDefault(2),
    [2] = fishPerDay.GetValueOrDefault(3),
    [3] = fishPerDay.GetValueOrDefault(4),
    [4] = fishPerDay.GetValueOrDefault(5),
    [5] = fishPerDay.GetValueOrDefault(6),
    [6] = fishPerDay.GetValueOrDefault(7) + fishPerDay.GetValueOrDefault(0),
    [7] = fishPerDay.GetValueOrDefault(8),
    [8] = fishPerDay.GetValueOrDefault(0),
};