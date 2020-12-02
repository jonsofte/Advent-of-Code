using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Console.WriteLine(GetLines().Where(x => ValidPassword(x)).Count());
Console.WriteLine(GetLines().Where(x => ValidPassword2(x)).Count());

IEnumerable<Line> GetLines() => 
   File.ReadAllLines("input.txt")
   .Select(x => x.Split(new char[] { ' ', '-' }))
   .Select(x => new Line(int.Parse(x[0]), int.Parse(x[1]), x[2][0], x[3]));

bool ValidPassword(Line line) => 
   line.Password.Count(x => x == line.Letter) >= line.Min &&
   line.Password.Count(x => x == line.Letter) <= line.Max;

bool ValidPassword2(Line line) =>
   line.Password[line.Min - 1] == line.Letter ^
   line.Password[line.Max - 1] == line.Letter;

record Line(int Min, int Max, char Letter, string Password);