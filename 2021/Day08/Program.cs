// https://adventofcode.com/2021/day/8

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt").Select(x => x.Split('|').ToList()
    .Select(x=> x.Split(' ',StringSplitOptions.RemoveEmptyEntries)).ToList()).ToList();

Console.WriteLine(FourDigitsSum(lines));
Console.WriteLine(DecodedDigitsSum(lines));

static int FourDigitsSum(List<List<string[]>> lines) =>  lines.Select(x => x[1].Where(v => (new[] { 2, 3, 4, 7 }).Contains(v.Length)).Count()).Sum();

static int DecodedDigitsSum(List<List<string[]>> lines)
{
    int sum = 0;
    foreach (var l in lines)
    {
        var digits = DecodeDigits(l[0].ToList());
        sum += l[1].Aggregate(0, (v, d) => (v * 10) + digits[String.Concat(d.OrderBy(c => c))]);
    }
    return sum;
}

static int IntersectingCharacters(string one, string two) => String.Concat(one.Intersect(two)).Length;

static Dictionary<string,int> DecodeDigits(List<string> digitStrings)
{
    var digits = new Dictionary<string, int>();

    foreach (string line in digitStrings)
    {
        switch (line.Length)
        {
            case 2:
                digits[String.Concat(line.OrderBy(c => c))] = 1;
                break;
            case 3:
                digits[String.Concat(line.OrderBy(c => c))] = 7;
                break;
            case 4:
                digits[String.Concat(line.OrderBy(c => c))] = 4;
                break;
            case 7:
                digits[String.Concat(line.OrderBy(c => c))] = 8;
                break;
            case 5:
                if (IntersectingCharacters(line, digitStrings.Single(x => x.Length == 2)) == 2)
                {
                    digits[String.Concat(line.OrderBy(c => c))] = 3;
                }
                else if (IntersectingCharacters(line, digitStrings.Single(x => x.Length == 4)) == 2)
                {
                    digits[String.Concat(line.OrderBy(c => c))] = 2;
                }
                else
                {
                    digits[String.Concat(line.OrderBy(c => c))] = 5;
                }
                break;
            case 6:
                if (IntersectingCharacters(line, digitStrings.Single(x => x.Length == 4)) == 4)
                {
                    digits[String.Concat(line.OrderBy(c => c))] = 9;
                }
                else if (IntersectingCharacters(line, digitStrings.Single(x => x.Length == 2)) == 2)
                {
                    digits[String.Concat(line.OrderBy(c => c))] = 0;
                }
                else
                {
                    digits[String.Concat(line.OrderBy(c => c))] = 6;
                }
                break;
        }
    }
    return digits;
}