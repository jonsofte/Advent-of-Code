// https://adventofcode.com/2021/day/12

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(l => l.Split('-')).Select(x => (from: x[0], to: x[1]));
var graph = CreateGraphFromEdges(input);

var visitedCaves = new HashSet<string>() { "start" };

Console.WriteLine(MoveToCave(currentCave: "start", visitedCaves, smallCaveIsVisited: true));
Console.WriteLine(MoveToCave(currentCave: "start", visitedCaves, smallCaveIsVisited: false));

int MoveToCave(string currentCave, HashSet<string> visitedCaves, bool smallCaveIsVisited)
{
    if (currentCave == "end") return 1;
    int paths = 0;

    foreach (string nextCave in graph[currentCave])
    {
        bool isBigCave = nextCave.ToUpper() == nextCave;
        bool isStartOrEndNode = (nextCave == "start" || nextCave == "end");

        if (isBigCave || !visitedCaves.Contains(nextCave))
        {
            visitedCaves.Add(nextCave);
            paths += MoveToCave(nextCave, visitedCaves, smallCaveIsVisited);
            visitedCaves.Remove(nextCave);
        }
        else if (!smallCaveIsVisited && !isBigCave && !isStartOrEndNode)
        {
            paths += MoveToCave(nextCave, visitedCaves, smallCaveIsVisited: true);
        }
    }
    return paths;
}

static Dictionary<string, List<string>> CreateGraphFromEdges(IEnumerable<(string from, string to)> input)
{
    var graph = new Dictionary<string, List<string>>();
    foreach (var edge in input)
    {
        if (!graph.ContainsKey(edge.from)) graph.Add(edge.from, new List<string>());
        if (!graph.ContainsKey(edge.to)) graph.Add(edge.to, new List<string>());
        graph[edge.from].Add(edge.to);
        graph[edge.to].Add(edge.from);
    }
    return graph;
}