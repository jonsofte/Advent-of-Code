using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            Part1(input);
            Part2(input);

            Console.Read();
        }

        private static void Part1(string polymer)
        {
            Console.WriteLine(GetLength(polymer));
        }

        private static void Part2(string downloadString)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Dictionary<char, int> reducedResult = new Dictionary<char, int>();

            foreach (char a in alphabet)
            {
                string parseString  = downloadString.Replace(char.ToLower(a).ToString(), "").Replace(char.ToUpper(a).ToString(), "");
                reducedResult.Add(a, GetLength(parseString));

            }

            Console.WriteLine((reducedResult.OrderBy(x => x.Value).First().Value));
        }

        private static int GetLength(string downloadString)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char c in downloadString)
            {
                if (stack.Count == 0) stack.Push(c);
                else
                    if (char.ToUpper(c) == char.ToUpper(stack.Peek()))
                        if (char.IsUpper(c))
                            if (!char.IsUpper(stack.Peek()))
                                stack.Pop();
                            else
                                stack.Push(c);
                        else
                            if (char.IsUpper(stack.Peek()))
                                stack.Pop();
                            else
                                stack.Push(c);
                    else
                        stack.Push(c);
            }
            return stack.Count();
        }
    }
}
