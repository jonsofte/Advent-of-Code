using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(t => int.Parse(t)).ToList();
input.Add(0);
input.Add(input.Max() + 3);
var adapters = input.OrderBy(x=>x).ToArray();

Console.WriteLine(Part1());
Console.WriteLine(Part2());

int Part1()
{
   int ones = 0;
   int threes = 0;

   foreach (int i in Enumerable.Range(1, adapters.Length - 1))
   {
      if (adapters[i] - adapters[i - 1] == 1) ones++;
      if (adapters[i] - adapters[i - 1] == 3) threes++;
   }
   return ones * threes;
}

long Part2()
{
   var foundCombinations = new Dictionary<int, long>();

   long Combinations(int i)
   {
      if (i == adapters.Length - 1) return 1;
      if (foundCombinations.ContainsKey(i)) return foundCombinations[i];
      long sum = 0;
      foreach (var j in Enumerable.Range(i + 1, adapters.Length - (i + 1)))
      {
         if ((adapters[j] - adapters[i]) <= 3) sum += Combinations(j);
      }
      foundCombinations.Add(i, sum);
      return sum;
   }

   return Combinations(0);
}