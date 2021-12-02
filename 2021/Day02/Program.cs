// https://adventofcode.com/2021/day/2

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(x => x.Split(' '))
    .Select(x => (Direction: x[0], Value: int.Parse(x[1].ToString())));

var position = CalculatePosition(input.Select(GetDirections));
var positionWithAim = CalculatePositionWithAim(input.Select(GetDirectionsWithAim));

Console.WriteLine(position.SumHorizontal * position.SumDepth);
Console.WriteLine(positionWithAim.SumHorizontal * positionWithAim.SumDepth);

static (int SumHorizontal, int SumDepth) CalculatePosition(IEnumerable<(int horizontal, int depth)> items) =>
    items.Aggregate((h: 0, d: 0), (x, y) => (x.h + y.horizontal, x.d + y.depth));

static (int SumHorizontal, int SumDepth, int sumAim) CalculatePositionWithAim(IEnumerable<(int aim, int forward)> items) =>
    items.Aggregate((h: 0, d: 0, a: 0), (x, y) => (x.h + y.forward, x.d + (y.forward * x.a), x.a + y.aim));

static (int horizontal, int depth) GetDirections((string Direction, int Value) i) => i.Direction switch
{
    "forward" => (i.Value, 0),
    "up" => (0, -i.Value),
    "down" => (0, +i.Value),
    _ => (0, 0)
};

static (int aim, int forward) GetDirectionsWithAim((string Direction, int Value) i) => i.Direction switch
{
    "forward" => (0, i.Value),
    "up" => (-i.Value, 0),
    "down" => (+i.Value, 0),
    _ => (0, 0)
};