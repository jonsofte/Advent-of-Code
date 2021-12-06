// https://adventofcode.com/2021/day/5

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");
var values = input.Select(l => l.Split("->").Select(d => d.Split(',').Select(i => int.Parse(i)).ToList()).ToList()).ToList();

var diagram = CreateDiagram(values);
Console.WriteLine(GetNumberOfOverlaps(includeDiagonal: false, values, diagram));

diagram = CreateDiagram(values);
Console.WriteLine(GetNumberOfOverlaps(includeDiagonal: true, values, diagram));

static List<List<int>> CreateDiagram(List<List<List<int>>> values)
{
    List<List<int>> diagram = new();

    var rangexmax1 = values.Max(x => x[0][0]);
    var rangexmax2 = values.Max(x => x[1][0]);
    var rangeymax1 = values.Max(x => x[0][0]);
    var rangeymax2 = values.Max(x => x[0][1]);

    int x = rangexmax1 > rangexmax2 ? rangexmax1 : rangexmax2;
    int y = rangeymax1 > rangeymax2 ? rangeymax1 : rangeymax2;

    var xRange = Enumerable.Range(0, x + 1);
    var yRange = Enumerable.Range(0, y + 1);

    foreach (int x1 in xRange)
    {
        var line = new List<int>();
        foreach (int y1 in yRange)
        {
            line.Add(0);
        }
        diagram.Add(line);
    }
    return diagram;
}

static int GetNumberOfOverlaps(bool includeDiagonal, List<List<List<int>>> values, List<List<int>> diagram)
{
    foreach (var line in values)
    {
        if (line[0][0] == line[1][0])
        {
            int xp = line[0][0];
            if (line[1][1] > line[0][1])
            {
                Enumerable.Range(line[0][1], (line[1][1] - line[0][1]) + 1).ToList().ForEach(p => diagram[p][xp]++);
            }
            else
            {
                Enumerable.Range(line[1][1], (line[0][1] - line[1][1]) + 1).ToList().ForEach(p => diagram[p][xp]++);
            }
        }
        else if (line[1][1] == line[0][1])
        {
            int yp = line[0][1];
            if (line[0][0] > line[1][0])
            {
                Enumerable.Range(line[1][0], (line[0][0] - line[1][0]) + 1).ToList().ForEach(p => diagram[yp][p]++);
            }
            else
            {
                Enumerable.Range(line[0][0], (line[1][0] - line[0][0]) + 1).ToList().ForEach(p => diagram[yp][p]++);
            }
        }
        else
        {
            if (includeDiagonal)
            {
                int xd = (line[1][0] - line[0][0]);
                int yd = (line[1][1] - line[0][1]);

                if (xd < 0 && yd < 0) Enumerable.Range(0, Math.Abs(xd) + 1).ToList().ForEach(p => diagram[line[0][1] - p][line[0][0] - p]++);
                if (xd < 0 && yd > 0) Enumerable.Range(0, Math.Abs(xd) + 1).ToList().ForEach(p => diagram[line[0][1] + p][line[0][0] - p]++);
                if (xd > 0 && yd < 0) Enumerable.Range(0, Math.Abs(xd) + 1).ToList().ForEach(p => diagram[line[0][1] - p][line[0][0] + p]++);
                if (xd > 0 && yd > 0) Enumerable.Range(0, Math.Abs(xd) + 1).ToList().ForEach(p => diagram[line[0][1] + p][line[0][0] + p]++);
            }
        }
    }
    return (diagram.Sum(xs1 => xs1.Count(ys1 => ys1 > 1)));
}
