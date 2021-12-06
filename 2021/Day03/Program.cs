// https://adventofcode.com/2021/day/3

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(x => x.ToList()).ToList();
static int NumberOfBits(List<List<char>> input, char i, int pos) => input.Select(x => x[pos]).Count(x => x == i);

// Part 1
var bitSummary = Enumerable.Range(0, input.First().Count).Select(x => (NumberOfBits(input, '1', x) > NumberOfBits(input, '0', x)) ? '1' : '0');
int gamma = Convert.ToInt32(new string(bitSummary.ToArray()), 2);
int epsilon = Convert.ToInt32(new string(bitSummary.Select(x => x == '1' ? '0' : '1').ToArray()), 2);
Console.WriteLine(gamma * epsilon);

// Part 2
var oxygen = input.ToList();
var co2 = input.ToList();

foreach (int pos in Enumerable.Range(0, oxygen.First().Count))
{
    int NumberOfOnes = NumberOfBits(oxygen, '1', pos);
    oxygen.RemoveAll(x => x[pos] == ((NumberOfOnes >= oxygen.Count - NumberOfOnes) ? '0' : '1'));
    if (oxygen.Count == 1) break;
}

foreach (int pos in Enumerable.Range(0, co2.First().Count))
{
    int NumberOfZeros = NumberOfBits(co2, '0', pos);
    co2.RemoveAll(x => x[pos] == ((NumberOfZeros > co2.Count - NumberOfZeros) ? '0' : '1'));
    if (co2.Count == 1) break;
}

int oxygenGeneratorRating = Convert.ToInt32(new string(oxygen.First().ToArray()), 2);
int co2ScrubberRating = Convert.ToInt32(new string(co2.First().ToArray()), 2);
Console.WriteLine(oxygenGeneratorRating * co2ScrubberRating);