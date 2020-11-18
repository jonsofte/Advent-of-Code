using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            String downloadString = File.ReadAllText("input.txt");
            string[] linesInString = downloadString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Part1(linesInString);
            Part2(linesInString);
            Console.Read();
        }

        private static void Part1(string[] linesInString)
        {
            long sum = 0;
            foreach (string s in linesInString)
            {
                if (s[0] == '+') sum += Int32.Parse(s.Substring(1));
                if (s[0] == '-') sum -= Int32.Parse(s.Substring(1));
            }
            Console.WriteLine(sum);
        }

        private static void Part2(string[] linesInString)
        {
            int sum = 0;
            List<int> values = new List<int>();
            bool show = true;

            while (show)
            {
                foreach (string s in linesInString)
                {
                    if (s[0] == '+') sum += Int32.Parse(s.Substring(1));
                    if (s[0] == '-') sum -= Int32.Parse(s.Substring(1));

                    if (values.Contains(sum))
                    {
                        if (show)
                        {
                            Console.WriteLine(sum);
                            show = false;
                        }
                    }
                    values.Add(sum);
                }
            }
        }
    }
}
