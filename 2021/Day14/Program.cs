// https://adventofcode.com/2021/day/14 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt").Split("\r\n\r\n");

var polymerTemplate = input[0].ToCharArray().ToList();
var rules = input[1].Split("\r\n").Select(l => l.Split("->",StringSplitOptions.TrimEntries)).ToDictionary(l => l[0], l=> char.Parse(l[1]));

Console.WriteLine(GetCounts(10));
Console.WriteLine(GetCounts(40));

long GetCounts(int steps)
{
    var createdPairs = new Dictionary<string, long>();

    foreach (int pos in Enumerable.Range(0, polymerTemplate.Count - 1))
    {
        string elementPair = $"{polymerTemplate[pos]}{polymerTemplate[pos + 1]}";
        createdPairs[elementPair] = createdPairs.GetValueOrDefault(elementPair) + 1;
    }

    foreach (int i in Enumerable.Range(0,steps))
    {
        var newPairs = new Dictionary<string, long>();
        foreach (var (pair, count) in createdPairs)
        {
            newPairs[$"{pair[0]}{rules[pair]}"] = newPairs.GetValueOrDefault($"{pair[0]}{rules[pair]}") + count;
            newPairs[$"{rules[pair]}{pair[1]}"] = newPairs.GetValueOrDefault($"{rules[pair]}{pair[1]}") + count;
        }
        createdPairs = newPairs;
    }

    var pairCounts = new Dictionary<char, long>();
    foreach (var (pair, count) in createdPairs)
    {
        var first = pair[0];
        pairCounts[first] = pairCounts.GetValueOrDefault(first) + count;
    }
    pairCounts[polymerTemplate.Last()]++;

    return pairCounts.Values.Max() - pairCounts.Values.Min();
}