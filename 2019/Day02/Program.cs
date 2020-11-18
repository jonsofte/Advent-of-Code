// Advent of Code Day 02
// https://adventofcode.com/2019/day/2

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            const int _checksum = 19690720;

            //Part 1
            Console.WriteLine(Process(GetInstructions("input.txt", 12, 2)));

            //Part 2
            foreach (int v in Enumerable.Range(0, 99))
            {
                foreach (int n in Enumerable.Range(0, 99))
                {
                    if (Process(GetInstructions("input.txt", v, n)) == _checksum)
                    {
                        Console.WriteLine((v * 100) + n);
                    }
                }
            }

            List<int> GetInstructions(string filename, int verb, int noun)
            {
                List<int> data = File.ReadAllText(filename).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();
                data[1] = verb;
                data[2] = noun;
                return data;
            }

            int Process(List<int> instructions)
            {
                List<int> memory = instructions;
                while (instructions[0] != 99)
                {
                    if (instructions[0] == 1) memory[instructions[3]] = memory[instructions[1]] + memory[instructions[2]];
                    if (instructions[0] == 2) memory[instructions[3]] = memory[instructions[1]] * memory[instructions[2]];
                    instructions = instructions.Skip(4).ToList();
                }
                return memory[0];
            }
        }
    }
}
