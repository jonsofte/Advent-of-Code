using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var depthMap = File.ReadAllLines("input.txt").Select(x => x.Trim().ToCharArray().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

Console.WriteLine(SumOfRiskLevels(depthMap));
Console.WriteLine(BasinSum(depthMap));

static int SumOfRiskLevels(int[][] depthMap) => FindLowPoints(depthMap).Sum(p => depthMap[p.r][p.c] + 1);

static List<(int r, int c)> FindLowPoints(int[][] depthMap)
{
    List<(int, int)> lowPoints = new();
    foreach (int r in Enumerable.Range(0, depthMap.Length))
    {
        foreach (int c in Enumerable.Range(0, depthMap[r].Length))
        {
            if (HasNoLowerNeighbours(r, c, depthMap)) lowPoints.Add((r, c));
        }
    }
    return lowPoints;
}

static bool HasNoLowerNeighbours(int r, int c, int[][] depthMap)
{
    var deltas = new List<(int r, int c)> { (-1, 0), (1, 0), (0, -1), (0, 1) };
    return deltas.All(d => !(ValidPosition(r + d.r, c + d.c, depthMap) && depthMap[r + d.r][c + d.c] <= depthMap[r][c]));
}

static bool ValidPosition(int rr, int cc, int[][] depthMap) =>
    0 <= cc && cc < depthMap[0].Length && 0 <= rr && rr < depthMap.Length;

static int BasinSum(int[][] depthMap)
{
    var basinSizes = new List<int>();
    var visitedPoints = new HashSet<(int, int)>();
    FindLowPoints(depthMap).ForEach(p => basinSizes.Add(GetBasinSize(p, depthMap, visitedPoints)));
    return basinSizes.OrderByDescending(b => b).Take(3).Aggregate(1, (sum, x) => sum * x);
}

static int GetBasinSize((int r, int c) point, int[][] depthMap, HashSet<(int, int)> visitedPoints)
{
    visitedPoints.Add(point);
    int sum = 0;

    foreach ((int dr, int dc) in new [] { (-1, 0), (1, 0), (0, -1), (0, 1) })
    {
        if (ValidPosition(point.r + dr, point.c + dc, depthMap) &&
            !visitedPoints.Contains((point.r + dr, point.c + dc)) &&
            depthMap[point.r + dr][point.c + dc] < 9)
        {
            sum += GetBasinSize((point.r + dr, point.c + dc), depthMap, visitedPoints);
        }
    }
    return 1 + sum;
}