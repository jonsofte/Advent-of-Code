using System;
using System.IO;
using System.Linq;

var grid = File.ReadAllLines("input.txt").ToList();

int calc(int x, int y)
{
   int count = 0;
   (int x, int y) pos = (0, 0);
   while (grid.Count > pos.y && grid[0].Length > (pos.x % grid[0].Length))
   {
      if (grid[pos.y][pos.x % grid[0].Length] == '#') count++;
      pos = (pos.x += x, pos.y += y);
   }
   return count;
}

Console.WriteLine(calc(3, 1));
Console.WriteLine(calc(1, 1) * calc(3, 1) * calc(5, 1) * calc(7, 1) * calc(1, 2));