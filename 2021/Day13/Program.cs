// https://adventofcode.com/2021/day/13

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt").Split("\r\n\r\n");
var coordinates = input[0].Split("\r\n").Select(l => l.Split(',')).Select(l => new Point(X: int.Parse(l[0]), Y: int.Parse(l[1])));
var folds = input[1].Split("\r\n").Select(l => l.Split('=')).Select(l => (axis: l[0].ToCharArray().Last(), value: int.Parse(l[1])));

folds.ToList().ForEach(f => coordinates = Fold(f, coordinates));

foreach (int y in Enumerable.Range(0, coordinates.Max(c => c.Y) + 1))
{
    foreach (int x in Enumerable.Range(0, coordinates.Max(c => c.X) + 1))
    {
        if (coordinates.Contains(new Point(x, y))) Console.Write('#');
        else Console.Write(' ');
    }
    Console.WriteLine();
}

static IEnumerable<Point> Fold((char axis, int value) fold, IEnumerable<Point> coordinates)
{
    var folded = new List<Point>();

    foreach (var p in coordinates)
    {
        if (fold.axis == 'x' && p.X > fold.value)
            folded.Add(new Point(X: fold.value - (p.X - (fold.value)), Y: p.Y));
        else if (fold.axis == 'y' && p.Y > fold.value)
            folded.Add(new Point(X: p.X, Y: fold.value - (p.Y - (fold.value))));
        else
            folded.Add(p);
    }
    return folded;
}

record Point(int X, int Y);