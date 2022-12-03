// https://adventofcode.com/2022/day/3

var input = File.ReadAllLines("input.txt");

var part1 = input
    .Select(SplitBackpack)
    .SelectMany(x => x.first.ToCharArray().Intersect(x.second.ToCharArray()))
    .Select(CharValue)
    .Sum();

var part2 = input
    .Where((x, i) => i % 3 == 0)
    .SelectMany((x, i) => input[i * 3].ToCharArray().Intersect(input[(i*3)+1].ToCharArray().Intersect(input[(i * 3) + 2].ToCharArray())))
    .Select(CharValue)
    .Sum();

Console.WriteLine(part1);
Console.WriteLine(part2);

static (string first,string second) SplitBackpack(string x) => (x[..(x.Length / 2)], x[(x.Length / 2)..]);
static int CharValue(char c) => ((int)c % 32) + (Char.IsUpper(c) ? 26 : 0);