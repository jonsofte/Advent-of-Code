// Advent of Code Day 04
// https://adventofcode.com/2019/day/4

using System;
using System.Linq;
using System.Collections.Generic;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "172851-675869";
            int min = Int32.Parse(input.Substring(0, input.IndexOf('-')));
            int max = Int32.Parse(input.Substring(input.IndexOf('-')+1));

            // Part 1
            var NumberOfValidPasswords = Enumerable.Range(min, max - min).Where( p => ValidPassword(p, tripleCheck:false)).Count();
            Console.WriteLine(NumberOfValidPasswords);

            // Part 2
            NumberOfValidPasswords = Enumerable.Range(min, max - min).Where(p => ValidPassword(p, tripleCheck: true)).Count();
            Console.WriteLine(NumberOfValidPasswords);

            bool ValidPassword(int password, bool tripleCheck)
            {
                List<int> doubles = new List<int>();
                List<int> triples = new List<int>();
                bool isAscending = true;

                int rest = password;
                int previousDigit = 10;
                int secondPreviousDigit = 11;

                while (rest != 0)
                {
                    int digit = rest % 10;
                    if (previousDigit == digit)
                    {
                        if (!doubles.Contains(digit)) doubles.Add(digit);
                    }
                    if ((secondPreviousDigit == previousDigit) && (previousDigit == digit))
                    {
                        if (!triples.Contains(digit)) triples.Add(digit);
                    }
                    if (previousDigit < digit) isAscending = false;

                    secondPreviousDigit = previousDigit;
                    previousDigit = digit;
                    rest /= 10;
                }

                if (!tripleCheck) return (isAscending && (doubles.Count > 0));
                return (isAscending && (doubles.Count > triples.Count));
            }
        }
    }       
}
