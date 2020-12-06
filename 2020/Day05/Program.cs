using System;
using System.IO;
using System.Linq;

Console.WriteLine(File.ReadAllLines("input.txt").Select(t => SeatValue(t)).Max());
var seats = File.ReadAllLines("input.txt").Select(t => SeatValue(t)).OrderBy(v => v).ToList();
Console.WriteLine(seats.Skip(1).Where((s, i) => s > seats[i]+1).First()-1);

int SeatValue(string input) => ToInt(input[..7], 'B') * 8 + ToInt(input[^3..], 'R');
int ToInt(string input, char c) => input.Aggregate(0, (t, n) => n == c  ? ((t << 1) + 1) : t << 1);