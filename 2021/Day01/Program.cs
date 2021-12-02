// https://adventofcode.com/2021/day/1
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(x => int.Parse(x)).ToList();

Console.WriteLine(NumberOfIncreasingDepths(input));
Console.WriteLine(NumberOfIncreasingDepths(SlidingWindow(input)));

static int NumberOfIncreasingDepths(List<int> depths) => depths.Skip(1)
    .Zip(depths, (x, y) => x - y)
    .Count(x => x > 0);

static List<int> SlidingWindow(List<int> readings) => Enumerable.Range(2, readings.Count - 2)
    .Select(x => readings[x - 2] + readings[x - 1] + readings[x])
    .ToList();