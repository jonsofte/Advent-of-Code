// https://adventofcode.com/2021/day/19
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var scanners = getScanners("input.txt").ToArray();
var (allProbes, mappedScanners) = GetAllProbesAndMappedScanners(scanners);

Console.WriteLine(allProbes.Count);
Console.WriteLine(GetDistances(mappedScanners.Select(x => x.position).Distinct().ToArray()).Select(x => x.Value).Max());

static (HashSet<Position> allProbes, List<(int, Position position)> mappedScanners) GetAllProbesAndMappedScanners(Scan[] scanners)
{
    var maps = new List<Map>();
    var allProbes = new HashSet<Position>();
    var queue = new Queue<int>(new[] { 0 });
    var mappedScanners = new List<(int, Position)>();

    while (queue.Count > 0)
    {
        int mapTo = queue.Dequeue();

        foreach (var mapFrom in Enumerable.Range(0, scanners.Length))
        {
            if (mapTo != mapFrom && !maps.Any(x => x.From == mapFrom))
            {
                var (isFound, position, transformation) = FindPositionAndTransformation(scanners[mapTo], scanners[mapFrom]);
                if (isFound)
                {
                    maps.Add(new Map(mapFrom, mapTo, transformation, position));
                    if (!queue.Contains(mapFrom)) queue.Enqueue(mapFrom);
                    (Position scanner, IEnumerable<Position> probes) = Transform(maps, scanners[mapFrom].Probes, new Position(0, 0, 0), mapFrom, mapTo);
                    mappedScanners.Add((mapFrom, scanner));
                    probes.ToList().ForEach(x => allProbes.Add(x));
                }
            }
        }
    }
    return (allProbes, mappedScanners);
}

static (Position, IEnumerable<Position>) Transform(IEnumerable<Map> maps, Position[] probes, Position scanner, int from, int to)
{
    var PosList = new List<Position>();
    var map = maps.Where(x => x.From == from && x.To == to).Single();

    foreach (var p in probes)
    {
        var transformed = map.Transformation(p);
        PosList.Add(new Position(transformed.X + map.Relative.X, transformed.Y + map.Relative.Y, transformed.Z + map.Relative.Z));
    }

    var pos = map.Transformation(scanner);
    var updatedScanner = new Position(pos.X + map.Relative.X, pos.Y + map.Relative.Y, pos.Z + map.Relative.Z);
    if (to == 0) return (updatedScanner, PosList);
    var nextMap = maps.Where(x => x.From == to).First();
    return Transform(maps, PosList.ToArray(), updatedScanner, nextMap.From, nextMap.To);
}

static (bool, Position, Func<Position, Position>) FindPositionAndTransformation(Scan scan1, Scan scan2)
{
    foreach (var transform in GetTransformations())
    {
        var totalHits = new Dictionary<Position, int>();
        var distance1 = scan1.Distance.ToList().OrderBy(x => x.Value).ToArray();
        var distance2 = scan2.Distance.ToList().OrderBy(x => x.Value).ToArray();
        (int i1, int i2) = (0, 0);
        while (i1 < distance1.Length && i2 < distance2.Length)
        {
            if (distance1[i1].Value > distance2[i2].Value) i2++;
            else if (distance1[i1].Value < distance2[i2].Value) i1++;
            else if (distance1[i1].Value == distance2[i2].Value)
            {
                (Position p1From, Position p1To) = (scan1.Probes[distance1[i1].From], scan1.Probes[distance1[i1].To]);
                (Position p2From, Position p2To) = (transform(scan2.Probes[distance2[i2].From]), transform(scan2.Probes[distance2[i2].To]));

                if (RelativePosition(p1From, p1To) == RelativePosition(p2From, p2To))
                {
                    var difference = new Position(p1From.X - p2From.X, p1From.Y - p2From.Y, p1From.Z - p2From.Z);

                    if (!totalHits.ContainsKey(difference)) totalHits.Add(difference, 0);
                    totalHits[difference]++;
                    if (totalHits[difference] >= 12) return (true, difference, transform);
                }
                (i1, i2) = (i1 + 1, i2 + 1);
            }
        }
    }
    return (false, new Position(0, 0, 0), (Position x) => x);
}

static List<Scan> getScanners(string file)
{
    var inputScanners = new List<Scan>();

    foreach (var l in File.ReadAllText(file).Split("\r\n\r\n"))
    {
        var probes = new List<Position>();
        foreach (var line in l.Split("\r\n").Skip(1))
        {
            var values = line.Split(',').Select(v => int.Parse(v)).ToArray();
            probes.Add(new Position(X: values[0], Y: values[1], Z: values[2]));
        }
        var distances = GetDistances(probes.ToArray());
        inputScanners.Add(new Scan(probes.ToArray(), distances.ToArray()));
    }
    return inputScanners;
}

static Position RelativePosition(Position from, Position to) => new(from.X - to.X, from.Y - to.Y, from.Z - to.Z);

static IEnumerable<Func<Position, Position>> GetTransformations() =>
    CubeSides().SelectMany(x => RotateXAxis().Select<Func<Position, Position>, Func<Position, Position>>(y => (Position i) => x(y(i))));

static IEnumerable<Func<Position, Position>> CubeSides()
{
    yield return (Position i) => new Position(i.X, i.Y, i.Z);
    yield return (Position i) => new Position(-i.Z, i.Y, i.X);
    yield return (Position i) => new Position(-i.X, i.Y, -i.Z);
    yield return (Position i) => new Position(i.Z, i.Y, -i.X);
    yield return (Position i) => new Position(i.Y, -i.X, i.Z);
    yield return (Position i) => new Position(-i.Y, i.X, i.Z);
};

static IEnumerable<Func<Position, Position>> RotateXAxis()
{
    yield return (Position i) => new Position(i.X, i.Y, i.Z);
    yield return (Position i) => new Position(i.X, -i.Z, i.Y);
    yield return (Position i) => new Position(i.X, -i.Y, -i.Z);
    yield return (Position i) => new Position(i.X, i.Z, -i.Y);
};

static IEnumerable<Distance> GetDistances(Position[] probes) =>
    GetCombination(probes.Length).Select(x => new Distance(x.first, x.second, GetManhattanDistance(probes[x.first], probes[x.second])));

static IEnumerable<(int first, int second)> GetCombination(int max) =>
    Enumerable.Range(0, max).SelectMany(s => Enumerable.Range(0, max), (x, y) => (x, y)).Where(p => p.x < p.y);

static int GetManhattanDistance(Position first, Position second) =>
    Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y) + Math.Abs(first.Z - second.Z);

internal record struct Scan(Position[] Probes, Distance[] Distance);
internal record struct Position(int X, int Y, int Z);
internal record struct Distance(int From, int To, int Value);
internal record struct Map(int From, int To, Func<Position, Position> Transformation, Position Relative);