// Advent of Code Day 05
// https://adventofcode.com/2019/day/5

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part 1
            int diagnosticCode = Process(GetInstructions("input.txt"), 1);
            Console.WriteLine(diagnosticCode);

            // Part 2
            diagnosticCode = Process(GetInstructions("input.txt"), 5);
            Console.WriteLine(diagnosticCode);

            List<int> GetInstructions(string filename) => File.ReadAllText(filename).Split(',').Select(x => Int32.Parse(x)).ToList();

            int Process(List<int> memory, int input)
            {
                int Pointer = 0;
                int output = 99;
                bool halt = false;
                List<int> inst = memory;

                while (!halt)
                {
                    inst = memory.GetRange(Pointer, (memory.Count - Pointer > 4) ? 4 : memory.Count - Pointer);
                    string instruction = inst[0].ToString().PadLeft(5, '0');
                    int opCode = Int32.Parse(instruction.Substring(3, 2));
                    bool[] imidiateMode = Enumerable.Range(0, 3).Select(x => instruction[2 - x] == '1' ? true : false).ToArray();
                    int parameter(int i) => (imidiateMode[i - 1]) ? inst[i] : memory[inst[i]];

                    switch (opCode)
                    {
                        case 1:
                            memory[inst[3]] = parameter(1) + parameter(2);
                            Pointer += 4;
                            break;
                        case 2:
                            memory[inst[3]] = parameter(1) * parameter(2);
                            Pointer += 4;
                            break;
                        case 3:
                            memory[inst[1]] = input;
                            Pointer += 2;
                            break;
                        case 4:
                            output = memory[inst[1]];
                            Pointer += 2;
                            break;
                        case 5:
                            Pointer = (parameter(1) != 0) ? parameter(2) : (Pointer + 3);
                            break;
                        case 6:
                            Pointer = (parameter(1) == 0) ? parameter(2) : (Pointer + 3);
                            break;
                        case 7:
                            memory[inst[3]] = parameter(1) < parameter(2) ? 1 : 0;
                            Pointer += 4;
                            break;
                        case 8:
                            memory[inst[3]] = parameter(1) == parameter(2) ? 1 : 0;
                            Pointer += 4;
                            break;
                        case 99:
                            if (output != 0) halt = true;
                            Pointer += 1;
                            break;
                        default:
                            throw new ApplicationException("Unknown opcode: " + opCode);
                    }
                }
                return output;
            }
        }
    } 
}