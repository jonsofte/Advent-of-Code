// https://adventofcode.com/2021/day/7

using System;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt").Split(',').Select(x => int.Parse(x)).ToList();

Console.WriteLine(Cost(FuelCostPart1));
Console.WriteLine(Cost(FuelCostPart2));

int Cost(Func<int,int,int> fuelCostCalculation) => Enumerable.Range(0, input.Max(x => x)).Select(i => input.Sum(d => fuelCostCalculation(d, i))).Min();
static int FuelCostPart1(int position, int crab) => Math.Abs(position - crab);
static int FuelCostPart2(int position, int crab) => (Math.Abs(position - crab) * (Math.Abs(position - crab)+1)) / 2;