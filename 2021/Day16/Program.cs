// https://adventofcode.com/2021/day/16
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var package = ParsePackage(new Queue<char>(HexToBinary(File.ReadAllText("input.txt").Trim()).ToCharArray()));

Console.WriteLine(GetSumOfVersions(package));
Console.WriteLine(CalculateValue(package));

static Package ParsePackage(Queue<char> q)
{
    int version = (int)BinaryToDecimal(DequeueSelection(q, 3));
    long type = BinaryToDecimal(DequeueSelection(q, 3));
    long value = 0;
    var packages = new List<Package>();

    if (type == 4) value = BinaryToDecimal(string.Concat(ParseLiteralData(q)));
    else packages.AddRange((BinaryToDecimal(DequeueSelection(q, 1)) == 0) ? ParseTotalLength(q) : ParseNumberOfPackages(q));
    
    return new Package(version, type, packages, value);
}

static IEnumerable<string> ParseLiteralData(Queue<char> q)
{
    while (q.Dequeue() == '1') yield return DequeueSelection(q, 4);
    yield return DequeueSelection(q, 4);
}

static IEnumerable<Package> ParseTotalLength(Queue<char> q)
{
    long totalLength = BinaryToDecimal(DequeueSelection(q, 15));
    int currentTotal = q.Count;
    while (q.Count > currentTotal - totalLength) yield return ParsePackage(q);
}

static IEnumerable<Package> ParseNumberOfPackages(Queue<char> q) =>
    Enumerable.Range(0, (int)BinaryToDecimal(DequeueSelection(q, 11))).Select(p => ParsePackage(q));

static long CalculateValue(Package p) => p.Type switch
{
    4 => p.Value,
    0 => p.SubPackages.Sum(s => CalculateValue(s)),
    1 => p.SubPackages.Aggregate((long)1, (v, s) => v *= CalculateValue(s)),
    2 => p.SubPackages.Min(s => CalculateValue(s)),
    3 => p.SubPackages.Max(s => CalculateValue(s)),
    5 => (CalculateValue(p.SubPackages[0]) > CalculateValue(p.SubPackages[1])) ? 1 : 0,
    6 => (CalculateValue(p.SubPackages[0]) < CalculateValue(p.SubPackages[1])) ? 1 : 0,
    7 => (CalculateValue(p.SubPackages[0]) == CalculateValue(p.SubPackages[1])) ? 1 : 0,
    _ => 0,
};

static int GetSumOfVersions(Package p) => p.SubPackages.Sum(x => GetSumOfVersions(x)) + p.Version;
static string HexToBinary(string input) => string.Concat(input.ToCharArray().Select(c => HexCharToNibble(c)));
static string HexCharToNibble(char c) => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0');
static long BinaryToDecimal(string input) => Convert.ToInt64(input, 2);
static string DequeueSelection(Queue<char> q, int length) => String.Concat(Enumerable.Range(0, length).Select(i => q!.Dequeue()));
record Package(int Version, long Type, List<Package> SubPackages, long Value);