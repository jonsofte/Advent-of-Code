using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var values = File.ReadAllLines("input.txt").Select(t => long.Parse(t)).ToArray();

long result = Part1();
Console.WriteLine(result);

long part2 = Part2(result);
Console.WriteLine(part2);

long Part2(long result)
{
   for (int x = 0; x < values.Length+1; x++)
   {
      for (int y = x+1; y < values.Length; y++)
      {
         if (values[(x)..(y)].Sum()== result) return (values[(x)..(y)].Min() + values[(x)..(y)].Max());
      }
   }
   return 0;
}

long Part1() 
{ 
   for (int x=25;x<values.Length;x++)
   {
      if (!isSum(values[x], values[(x - 25)..(x)])) return values[x];
   }
   return 0;
}

bool isSum(long check, long[] preamble)
{
   for (int i = 0; i < preamble.Length; i++)
   {
      for (int j = 0; j < preamble.Length; j++)
      {
         if (i != j) if (preamble[i] + preamble[j] == check) return true;
      }
   }
   return false;
}