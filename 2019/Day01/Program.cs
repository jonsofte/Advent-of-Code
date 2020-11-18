// Advent of Code Day 01
// https://adventofcode.com/2019/day/1

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var FileLines = File.ReadAllText("input.txt").Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Part1(FileLines);
            Part2(FileLines);
        }

        static int GetFuel(int i) => (i / 3) - 2;

        private static void Part1(List<string> lines)
        {
            int TotalFuel = lines.Select(x => GetFuel(Int32.Parse(x))).Sum();
            Console.WriteLine(TotalFuel);
        }

        private static void Part2(List<string> lines)
        {
            long total = 0;
            foreach (string s in lines.ToArray())
            {
                int FuelSum = GetFuel(Int32.Parse(s));
                int FuelRest = FuelSum;
                while (FuelRest > 0)
                {
                    FuelRest = GetFuel(FuelRest);
                    if (FuelRest > 0) FuelSum += FuelRest;
                }
                total += FuelSum;
            }
            Console.WriteLine(total);
        }
    }
}
