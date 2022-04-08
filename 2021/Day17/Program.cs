// https://adventofcode.com/2021/day/17
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = Regex.Matches(File.ReadAllText("input.txt"), "-?[0-9]+").Select(x => int.Parse(x.Value)).ToList();
Target target = new(input[0], input[1], input[2], input[3]);

var maxHeight = PossibleShootingDirections(target.Xmax, target.Ymax).Max(s => Shoot(s.x, s.y, target).maxHeight);
var numberOfHits = PossibleShootingDirections(target.Xmax, target.Ymax).Count(s => Shoot(s.x, s.y, target).targetIsHit);

Console.WriteLine(maxHeight);
Console.WriteLine(numberOfHits);

static (bool targetIsHit, int maxHeight) Shoot(int dx, int dy, Target target)
{
    (int x, int y) = (0, 0);
    int maxY = y;

    while (x < target.Xmax && y > target.Ymin)
    {
        (x, y) = (x + dx, y + dy);
        (dx, dy) = ((dx > 0) ? dx - 1 : dx, dy - 1);
        maxY = (y > maxY) ? y : maxY;
        if (TargetHit(x, y, target)) return (true, maxY);
    }
    return (false, 0);
}

static IEnumerable<(int x, int y)> PossibleShootingDirections(int maxX, int maxY) =>
    Enumerable.Range(1, maxX).SelectMany(s => Enumerable.Range(-Math.Abs(maxY * 2), Math.Abs(maxY * 4)), (x, y) => (x, y));

static bool TargetHit(int x, int y, Target t) => (t.Xmin <= x && x <= t.Xmax && t.Ymin <= y && y <= t.Ymax);

record Target(int Xmin, int Xmax, int Ymin, int Ymax);