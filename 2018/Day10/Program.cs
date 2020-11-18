using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var coordinates = File.ReadAllText("input.txt")
                .Replace("position=", "").Replace("velocity=", "").Replace(" ", "").Replace("<", ",").Replace(">", "")
                .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToArray())
                .Select(v => (x: v[0], y: v[1], dx: v[2], dy: v[3])).ToArray();

            foreach (int i in Enumerable.Range(0, 11000))
            {
                Display(coordinates, i);
            }
            Console.Read();
        }

        static (int x, int y) Transform(int t, int x, int y, int dx, int dy) => (x + dx * t, y + dy * t);


        private static void Display((int x, int y, int dx, int dy)[] c, int t)
        {
            var values = new List<(int x, int y)>();

            foreach (var (x, y, dx, dy) in c)
            {
                values.Add(Transform(t, x, y, dx, dy));
            }

            int maxX = values.Max(i => i.x)+2;
            int maxY = values.Max(i => i.y)+2;
            int minX = values.Min(i => i.x)-2;
            int minY = values.Min(i => i.y)-2;

            if ((maxY-minY) < 22) { 
                foreach (int y in Enumerable.Range(minY, maxY - minY))
                {
                    foreach (int x in Enumerable.Range(minX, maxX - minX))
                    {
                        if (values.Contains((x, y))) Console.Write('*');
                        else Console.Write(' ');
                    }
                    Console.Write("\r\n");
                }
                Console.WriteLine(t);
            }
        }
    }
}
