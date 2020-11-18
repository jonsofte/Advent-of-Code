using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string importString = File.ReadAllText("input.txt");
            string[] lines = importString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, Claim> claims = new Dictionary<int, Claim>();

            foreach (string line in lines)
            {
                List<string> LineContent = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                string pos = LineContent[2].Remove(LineContent[2].Length - 1);
                string size = LineContent[3];

                Claim claim = new Claim()
                {
                    ID = Int32.Parse(LineContent[0].Substring(1)),
                    Left = Int32.Parse(pos.Substring(0, pos.IndexOf(','))),
                    Top = Int32.Parse(pos.Substring(pos.IndexOf(',') + 1)),
                    Width = Int32.Parse(size.Substring(0, size.IndexOf('x'))),
                    Tall = Int32.Parse(size.Substring(size.IndexOf('x') + 1))
                };

                claims.Add(claim.ID, claim);
            }

            int arraySize = 2000;
            int[,] fabric = new int[arraySize, arraySize];

            foreach (KeyValuePair<int, Claim> kvp in claims)
            {
                for (int i = 0; i < kvp.Value.Width; i++)
                {
                    for (int j = 0; j < kvp.Value.Tall; j++)
                    {
                        fabric[kvp.Value.Left + i, kvp.Value.Top + j]++;
                    }
                }
            }

            Part1(arraySize, fabric);
            Part2(claims, fabric);
        }

        private static void Part1(int arraySize, int[,] fabric)
        {
            int fabricExists = 0;

            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    if (fabric[i, j] > 1) fabricExists++;
                }
            }

            Console.WriteLine(fabricExists);
        }

        private static void Part2(Dictionary<int, Claim> claims, int[,] fabric)
        {
            foreach (KeyValuePair<int, Claim> kvp in claims)
            {
                bool onlyOnes = true;
                for (int i = 0; i < kvp.Value.Width; i++)
                {
                    for (int j = 0; j < kvp.Value.Tall; j++)
                    {
                        if (fabric[kvp.Value.Left + i, kvp.Value.Top + j] > 1) onlyOnes = false;
                    }
                }
                if (onlyOnes) Console.WriteLine(kvp.Key);
            }
            Console.Read();
        }
    }

    class Claim
    {
        public int ID { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Tall { get; set; }
    }
}
