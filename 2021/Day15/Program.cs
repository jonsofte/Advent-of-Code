// https://adventofcode.com/2021/day/15

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(l => l.ToCharArray()).ToArray();

Console.WriteLine(CalculatePath(createMap(input, multiplier: 1)));
Console.WriteLine(CalculatePath(createMap(input, multiplier: 5)));

static Dictionary<(int r, int c), int> createMap(char[][] input, int multiplier)
{
    var createdMap = new Dictionary<(int r, int c), int>();

    foreach ((int nr, int nc) in GetMultiplierPairs(multiplier))
    {
        foreach ((int r, int c) in GetMapCoordinates(input))
        {
            createdMap[(r + (input.Length * nr), c + (input[0].Length * nc))] =
            CapValue((int)char.GetNumericValue(input[r][c]) + ((nr + nc) * 1));
        };
    }
    return createdMap;
}

static int CapValue(int input) => input <= 9 ? input : (input % 10) + 1;

static IEnumerable<(int nr, int nc)> GetMultiplierPairs(int multiplier) =>
    Enumerable.Range(0, multiplier).SelectMany(s => Enumerable.Range(0, multiplier), (nr, nc) => (nr, nc));

static IEnumerable<(int r, int c)> GetMapCoordinates(char[][] input) =>
    Enumerable.Range(0, input.Length).SelectMany(s => Enumerable.Range(0, input[0].Length), (r, c) => (r, c));

static int CalculatePath(Dictionary<(int r, int c), int> map)
{
    var queue = new PriorityQueue<(int r, int c), int>();
    var acumulatedRisk = new Dictionary<(int r, int c), int>();
    var deltas = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

    queue.Enqueue((0, 0), 0);
    acumulatedRisk.Add((0, 0), 0);

    (int r, int c) max = (map.Keys.Max(x => x.r), map.Keys.Max(x => x.c));

    while (queue.Count > 0)
    {
        var (r, c) = queue.Dequeue();
        if (r == max.r && c == max.c) break;

        foreach ((int dr, int dc) in deltas)
        {
            if (map.ContainsKey((r + dr, c + dc)))
            {
                int totalRisk = acumulatedRisk[(r, c)] + map[(r + dr, c + dc)];
                if (totalRisk < acumulatedRisk.GetValueOrDefault((r + dr, c + dc), int.MaxValue))
                {
                    acumulatedRisk[(r + dr, c + dc)] = totalRisk;
                    queue.Enqueue((r + dr, c + dc), totalRisk);
                }
            }
        }
    }
    return acumulatedRisk[(map.Keys.Max(x => x.r), map.Keys.Max(x => x.c))];
}