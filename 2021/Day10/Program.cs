// https://adventofcode.com/2021/day/10

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToList();
var result = lines.Select(GetChuncks);

Console.WriteLine(result.Sum(x => x.points));

var remaining = result.Where(x => x.points == 0).Select(x => x.remaining);
var totals = remaining.Select(x => x.Aggregate((long)0, (score, v) => (score * 5) + PointsPart2(v)));
var middleScore = totals.OrderBy(x => x).ToArray()[totals.Count() / 2];

Console.WriteLine(middleScore);

static int PointsPart1(char c) => c switch { ')' => 3, ']' => 57, '}' => 1197, '>' => 25137, _ => 0 };
static int PointsPart2(char c) => c switch { '(' => 1, '[' => 2, '{' => 3, '<' => 4, _ => 0 };

static (int points, char[] remaining) GetChuncks(char[] lines)
{
    var stack = new Stack<char>();
    int position = 0;

    foreach (char c in lines)
    {
        position++;
        if ((new[] { '(', '[', '{', '<' }).Contains(c))
        {
            stack.Push(c);
        }
        else
        {
            char found = stack.Pop();
            if (found == '(' && c != ')') return (PointsPart1(c), Array.Empty<char>());
            if (found == '[' && c != ']') return (PointsPart1(c), Array.Empty<char>());
            if (found == '{' && c != '}') return (PointsPart1(c), Array.Empty<char>());
            if (found == '<' && c != '>') return (PointsPart1(c), Array.Empty<char>());
        }
    }
    return (0, stack.ToArray());
}