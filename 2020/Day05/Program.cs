using System;
using System.IO;
using System.Linq;

var tickets = File.ReadAllLines("input.txt");
Console.WriteLine(tickets.Select(t => SeatValue(t)).Max());

var seats = tickets.Select(t => SeatValue(t)).OrderBy(v => v).ToList();
Console.WriteLine(seats.Take(seats.Count() - 1).Where((s, i) => seats[i + 1] > s + 1).First()+1);

int SeatValue(string input) => Column(input) * 8 + Row(input);
int Column(string input) => ToInt(input[..7], 'B');
int Row(string input) => ToInt(input[^3..], 'R');
int ToInt(string input, char c) => input.Aggregate(0, (t, n) => n == c ? ((t << 1) + 1) : t << 1);