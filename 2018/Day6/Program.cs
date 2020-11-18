using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputString = File.ReadAllText("input.txt");
            var coordinates = inputString.Replace(" ", "")
                .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(',').Select(i => int.Parse(i)).ToArray())
                .Select(v => (x: v[0], y: v[1])).ToArray();

            Part1(coordinates);
            Part2(coordinates);

            Console.Read();
        }

        private static void Part2((int x, int y)[] coordinates)
        {
            int MaxX = coordinates.Max(i => i.x);
            int MaxY = coordinates.Max(i => i.y);
            int MinX = coordinates.Min(i => i.x);
            int MinY = coordinates.Min(i => i.y);

            Dictionary<(int, int), int> matrix = new Dictionary<(int, int), int>();

            for (int x1 = MinX; x1 <= MaxX; x1++)
            {
                for (int y1 = MinY; y1 <= MaxY; y1++)
                {
                    if (!matrix.ContainsKey((x1, y1))) matrix.Add((x1, y1), 0);
                    foreach (var (x2, y2) in coordinates)
                    {
                        int d = GetDistance(x1, y1, x2, y2);
                        matrix[(x1, y1)] += GetDistance(x1, y1, x2, y2);
                    }
                }
            }

            Console.WriteLine(matrix.Where(x => x.Value < 10000).Count());
        }

        private static void Part1((int x, int y)[] coordinates)
        {
            int MaxX = coordinates.Max(i => i.x);
            int MaxY = coordinates.Max(i => i.y);
            int MinX = coordinates.Min(i => i.x);
            int MinY = coordinates.Min(i => i.y);

            var matrix = new Dictionary<(int x, int y), (int c, int distance)>();
            List<int> invalid = new List<int>();

            for (int x1 = MinX; x1 <= MaxX; x1++)
            {
                for (int y1 = MinY; y1 <= MaxY; y1++)
                {
                    if (!matrix.ContainsKey((x1, y1))) matrix.Add((x1, y1), (-1, Int32.MaxValue));
                    foreach (var (x2, y2) in coordinates)
                    {
                        int d = GetDistance(x1, y1, x2, y2);
                        (int c, int distance) = matrix[(x1, y1)];
                        if (d < distance) matrix[(x1, y1)] = (Array.IndexOf(coordinates, (x2, y2)), d);
                        if (d == distance) matrix[(x1, y1)] = (Int32.MaxValue, d);
                    }
                    (int cID, int _) = matrix[(x1, y1)];
                    if ((x1 == MinX || x1 == MaxX) && (!invalid.Contains(cID))) invalid.Add(cID);
                    if ((y1 == MinY || y1 == MaxY) && (!invalid.Contains(cID))) invalid.Add(cID);
                }
            }

            Dictionary<int, int> numberOfPoints = new Dictionary<int, int>();
            foreach (KeyValuePair<(int, int), (int, int)> kvp in matrix)
            {
                (int c, int _) = kvp.Value;
                if (!numberOfPoints.ContainsKey(c)) numberOfPoints.Add(c, 0);
                numberOfPoints[c]++;
            }

            Console.WriteLine(numberOfPoints.Where(x => !invalid.Contains(x.Key)).Select(x => x.Value).OrderByDescending(x => x).First());
        }

        static private int GetDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

    }
    public class Point
    {
        public int ID { get; set; }
        public int Distance { get; set; }
    }
}
