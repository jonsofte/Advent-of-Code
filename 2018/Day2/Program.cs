using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Day2
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

        private static void Part1(string[] ids)
        {
            var twos = ids.Count(id => id.GroupBy(c => c).Any(g => g.Count() == 2));
            var threes = ids.Count(id => id.GroupBy(c => c).Any(g => g.Count() == 3));
            Console.WriteLine(twos * threes);
        }

        private static (bool twos, bool threes) GetChecksum(string input)
        {
            var groupByCount = input.ToCharArray().GroupBy(c => c).Select(g => new { key = g, count = g.Count() });
            bool threeExists = groupByCount.Where(x => x.count == 3).Count() > 0;
            bool twoExists = groupByCount.Where(x => x.count == 2).Count() > 0;
            return (threeExists, twoExists);
        }

        private static void Part2(string[] ids)
        {
            for (int i = 0; i < ids[0].Length; i++)
            {
                var pair = ids.Select(id => id.Remove(i, 1)).GroupBy(id => id).FirstOrDefault(g => g.Count() > 1);
                if (pair != null)
                {
                    var common = pair.First();
                    Console.WriteLine(common);
                }
            }
        }

        private static int CompareString(string first, string second)
        {
            int numberOfDifferences = 0;
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i]) numberOfDifferences++;
            }
            return numberOfDifferences;
        }

        private static string GetCommonString(string first, string second)
        {
            StringBuilder CommonString = new StringBuilder();
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] == second[i]) CommonString.Append(first[i]);
            }
            return CommonString.ToString();
        }
    }
}