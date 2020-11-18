using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i))
                .ToArray();

            var node = GetNode(ref input);
            var result = node.Meta().Sum();

            // Part 1
            Console.WriteLine(result);

            // Part 2

            Console.WriteLine(node.GetNodeValue());
            Console.Read();
        }

        static Node GetNode(ref int[] values)
        {
            var node = new Node();
            int childNodes = values[0];
            int metadata = values[1];
            values = values.Skip(2).ToArray();

            foreach (int i in Enumerable.Range(0,childNodes))
            {
                node.ChildNodes.Add(GetNode(ref values));
            }

            node.MetaData.AddRange(values.Take(metadata));
            values = values.Skip(metadata).ToArray();

            return node;
        }
    }

    public class Node
    {
        public List<Node> ChildNodes { get; set; }
        public List<int> MetaData { get; set; }

        public Node()
        {
            ChildNodes = new List<Node>();
            MetaData = new List<int>();
        }

        public List<int> Meta()
        {
            List<int> r = new List<int>();
            foreach (Node n in ChildNodes)
            {
                r.AddRange(n.Meta());
            }
            r.AddRange(MetaData);
            return r;
        }

        public int GetNodeValue()
        {
            if (ChildNodes.Count == 0)
            {
                return MetaData.Sum();
            }
            else
            {
                int r = 0;

                var nodeArray = ChildNodes.ToArray();
                foreach (int n in MetaData)
                {
                    if (n <= nodeArray.Length)
                    {
                        r += ChildNodes[n-1].GetNodeValue();
                    }
                }

                return r;
            }
        }
    }
}
