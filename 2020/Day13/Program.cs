using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");
int timestamp = int.Parse(input[0]);
var departures = input[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();

Console.WriteLine(closestDeparture(departures));

int closestDeparture(List<int> departures) => departures
   .Select(x => (time: x - (timestamp % x), x))
   .OrderBy(t => t.time).Select(t => t.time * t.x).First();

