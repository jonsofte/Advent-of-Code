// Advent of Code Day 03
// https://adventofcode.com/2019/day/3

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<(int x, int y), (List<int> Wires, int Length)> grid = new Dictionary<(int x, int y), (List<int> Wires,int Length)>();
            (int x, int y) currentPos = (0, 0);
            int length = 0;

            var inputPaths = GetInputPaths("input.txt");
            inputPaths[0].ForEach(x => MoveWire(x, WireID: 1));
            currentPos = (0, 0);
            length = 0;
            inputPaths[1].ForEach(x => MoveWire(x, WireID: 2));

            // Part 1
            int closestIntersection = grid.Where(x => x.Value.Wires.Count == 2).Select(p => Math.Abs(p.Key.x) + Math.Abs(p.Key.y)).Min();
            Console.WriteLine(closestIntersection);

            // Part 2
            var shortestDistance = grid.Where(x => x.Value.Wires.Count == 2).Select(x => x.Value.Length).Min();
            Console.WriteLine(shortestDistance);

            List<List<(char, int)>> GetInputPaths(string filename) => File.ReadAllText(filename)
                .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => (Direction: char.Parse(x.Substring(0, 1)), Distance: Int32.Parse(x.Substring(1, x.Length - 1)))).ToList()
                    ).ToList();

            void MoveWire((char Direction, int Distance) point, int WireID)
            {
                List<(int, int)> positions = new List<(int, int)>();
                if (point.Direction == 'R') positions = Enumerable.Range(1, point.Distance).Select(x => (currentPos.x + x, currentPos.y)).ToList();
                if (point.Direction == 'L') positions = Enumerable.Range(1, point.Distance).Select(x => (currentPos.x - x, currentPos.y)).ToList();
                if (point.Direction == 'U') positions = Enumerable.Range(1, point.Distance).Select(y => (currentPos.x, currentPos.y + y)).ToList();
                if (point.Direction == 'D') positions = Enumerable.Range(1, point.Distance).Select(y => (currentPos.x, currentPos.y - y)).ToList();

                foreach ((int x, int y) p in positions)
                {
                    length++;
                    if (!grid.ContainsKey(p)) grid.Add(p, (new List<int>(), 0));
                    if (!grid[p].Wires.Contains(WireID)) grid[p].Wires.Add(WireID);
                    grid[p] = (grid[p].Wires, grid[p].Length + length);
                }
                currentPos = positions.Last();
            }

        }
    }
}
