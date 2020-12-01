using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(x => Int32.Parse(x));

int result1 = input.SelectMany(s => input, (x, y) => (x, y))
   .Where(i => i.x + i.y == 2020)
   .Select(i => i.x * i.y).First();

var result2 = input.SelectMany(s => input, (x, y) => (x, y)).SelectMany(s => input, (i, z) => (i.x, i.y, z))
   .Where(i => i.x + i.y +i.z == 2020)
   .Select(i => i.x * i.y * i.z).First();

Console.WriteLine(result1);
Console.WriteLine(result2);