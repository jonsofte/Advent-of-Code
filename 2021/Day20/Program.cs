// https://adventofcode.com/2021/day/19
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt").Split("\r\n\r\n");
var (rules, image) = (input[0].ToCharArray(), GetImage(input[1]));

Console.WriteLine(Enhance(image, rules, 2).Count(x => x.Value == '#'));
Console.WriteLine(Enhance(image, rules, 50).Count(x => x.Value == '#'));

static Dictionary<Pos, char> Enhance(Dictionary<Pos, char> image, char[] rules, int ne)
{
    var (maxRow, minRow) = (image.Keys.Max(p => p.R) + (ne * 3), image.Keys.Min(p => p.R) - (ne * 3));
    var (maxColumn, minColumn) = (image.Keys.Max(p => p.C) + (ne * 3), image.Keys.Min(p => p.C) - (ne * 3));
    Enumerable.Range(0, ne).ToList().ForEach(x => image = EnhanceImage(image, rules, minRow, maxRow, minColumn, maxColumn).ToDictionary(x => x.pos, x => x.c));
    return CropImage(image, ne).ToDictionary(x => x.Key, x => x.Value);
}

static IEnumerable<KeyValuePair<Pos, char>> CropImage(Dictionary<Pos, char> image, int ne)
{
    var (maxRow, minRow) = (image.Keys.Max(p => p.R) - ne, image.Keys.Min(p => p.R) + ne);
    var (maxColumn, minColumn) = (image.Keys.Max(p => p.C) - ne, image.Keys.Min(p => p.C) + ne);
    return image.Where(x => x.Key.R > minRow && x.Key.R < maxRow && x.Key.C > minColumn && x.Key.C < maxColumn);
};

static IEnumerable<(Pos pos, char c)> EnhanceImage(Dictionary<Pos, char> inputImage, char[] rules, int minRow, int maxRow, int minColumn, int maxColumn)
{
    var rowRange = Enumerable.Range(minRow, Math.Abs(minRow) + maxRow + 1);
    var columnRange = Enumerable.Range(minColumn, Math.Abs(minColumn) + maxColumn + 1);
    return rowRange.SelectMany(r => columnRange.Select(c => (new Pos(r, c), EnhancePosition(rules, inputImage, new Pos(r, c)))));
}

static char EnhancePosition(char[] rules, Dictionary<Pos, char> image, Pos pos) => rules[BinaryToDecimal(GetBinaryString(getChars(getPositions(pos), image)))];
static long BinaryToDecimal(string input) => Convert.ToInt64(input, 2);
static string GetBinaryString(char[] input) => new String(input.Select(i => (i == '#') ? '1' : '0').ToArray());
static IEnumerable<(int r, int c)> ps() => new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 0), (0, 1), (1, -1), (1, 0), (1, 1) };
static IEnumerable<Pos> getPositions(Pos pos) => ps().Select(p => new Pos(pos.R + p.r, pos.C + p.c));
static char[] getChars(IEnumerable<Pos> pos, Dictionary<Pos, char> image) => pos.Select(p => image.GetValueOrDefault(p, '.')).ToArray();
Dictionary<Pos, char> GetImage(string v) => v.Split("\r\n").SelectMany((l, i) => l.Select((c, j) => (pos: new Pos(i, j), c))).ToDictionary(x => x.pos, x => x.c);

record struct Pos(int R, int C);