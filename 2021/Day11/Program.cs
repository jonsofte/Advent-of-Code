// https://adventofcode.com/2021/day/11

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var octopusEnergy = File.ReadAllLines("input.txt").Select(x => x.Trim().ToCharArray().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();

Console.WriteLine(TotalFlashesInHundredSteps(octopusEnergy));
Console.WriteLine(NumberOfStepsWhenAllFlashing(octopusEnergy));

static int TotalFlashesInHundredSteps(int[][] octopusEnergy) => Enumerable.Range(0, 100).Sum(x => step(octopusEnergy));

static int NumberOfStepsWhenAllFlashing(int[][] octopusEnergy)
{
    int steps = 100;
    int currentFlashes = 0;
    while (octopusEnergy.Length * octopusEnergy[0].Length != currentFlashes)
    {
        currentFlashes = step(octopusEnergy);
        steps++;
    }
    return steps;
}

static int step(int[][] matrix)
{
    var deltas = new List<(int r, int c)> { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
    int numberOfFlashes = 0;
    var Flash = new Queue<(int r, int c)>();
    var hasFlashed = new HashSet<(int r, int c)>();

    foreach (int r in Enumerable.Range(0, matrix.Length))
    {
        foreach (int c in Enumerable.Range(0, matrix[r].Length))
        {
            matrix[r][c]++;
            if (matrix[r][c] > 9)
            {
                Flash.Enqueue((r, c));
            }
        }
    }

    while (Flash.Count > 0)
    {
        (int r, int c) = Flash.Dequeue();
        hasFlashed.Add((r, c));
        numberOfFlashes++;
        matrix[r][c] = 0;

        foreach (var (dr, dc) in deltas)
        {
            if (ValidPosition(r + dr, c + dc, matrix) && !hasFlashed.Contains((r + dr, c + dc)) && !Flash.Contains((r + dr, c + dc)))
            {
                matrix[r + dr][c + dc]++;
                if (matrix[r + dr][c + dc] > 9)
                {
                    Flash.Enqueue((r + dr, c + dc));
                }
            }
        }
    }
    return numberOfFlashes;
}

static bool ValidPosition(int rr, int cc, int[][] matrix) => 0 <= cc && cc < matrix[0].Length && 0 <= rr && rr < matrix.Length;