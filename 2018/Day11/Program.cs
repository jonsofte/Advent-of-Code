using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            int serial = 7857;
            int gridSize = 300;
            
            int[,] grid = new int[gridSize + 1, gridSize + 1];
            foreach (int x in Enumerable.Range(1, gridSize))
            {
                foreach (int y in Enumerable.Range(1, gridSize))
                {
                    grid[x, y] = Power(x, y, serial);
                }
            }

            Part1(gridSize, grid);
            // TODO Part2
            Console.Read();

        }

        private static void Part1(int gridSize, int[,] grid)
        {
            (int x, int y) pos = (0, 0);
            int Max = 0;

            foreach (int x in Enumerable.Range(1, gridSize - 3))
            {
                foreach (int y in Enumerable.Range(1, gridSize - 3))
                {
                int sum = grid[x, y] + grid[x + 1, y] + grid[x + 2, y] +
                    grid[x, y + 1] + grid[x + 1, y + 1] + grid[x + 2, y + 1] +
                    grid[x, y + 2] + grid[x + 1, y + 2] + grid[x + 2, y + 2];
                if (sum > Max)
                    {
                        Max = sum;
                        pos = (x,y);
                    }
                }
            }

            Console.WriteLine($"{pos.x},{pos.y}");
        }

        private static int SumArea(int x, int y, int size, int[,] grid)
        {
            int sum = 0;
            foreach (int x1 in Enumerable.Range(x, size-1))
            {
                foreach (int y1 in Enumerable.Range(y, size-1))
                {
                    sum += grid[x1, y1];
                }
            }
            return sum;
        }

        private static int Power(int x, int y, int serial)
        {
            int RackID = (x + 10);
            int Power = ((RackID * y) + serial)*RackID;
            char i = Power.ToString().Reverse().ToArray()[2];
            int r = Int32.Parse(i.ToString())-5;
            return r;
        }
    }
}
