using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllText("input.txt")
                .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(' '));

            Dictionary<char, List<char>> values = new Dictionary<char, List<char>>();

            foreach (var l in lines)
            {
                if (!values.ContainsKey(l[1][0]))
                {
                    values.Add(l[1][0], new List<char>());
                }
                values[l[1][0]].Add(l[7][0]);
            }

            //Part1(values);
            Part2(values);

            Console.ReadKey();
        }

        private static void Part1(Dictionary<char, List<char>> values)
        {
            StringBuilder result = new StringBuilder();
            while (values.MustBeFinished().Count > 1)
            {
                char current = values.CanStart().First();
                values.RemoveBefore(current);
                values.RemoveKey(current);
                result.Append(current);
            }

            result.Append(values.MustBeFinished().First());
            result.Append(values.CanNotBegin().First());
            Console.WriteLine(result);
        }

        private static void Part2(Dictionary<char, List<char>> values)
        {
            List<(int w, char c)> workers = new List<(int w, char c)>()
            {
                (0,' '),(0,' '),(0,' '),(0,' '),(0,' ')
            };

            int seconds = 0;

            while ((values.MustBeFinished().Count > 0) || !(workers.Where(x => x.w == 0).Count() == 5))
            {
                for (int i = 0; i < workers.Count; i++)
                {
                    if (workers[i].w > 0)
                    {
                        workers[i] = (workers[i].w - 1, workers[i].c);
                        if (workers[i].w == 0)
                        {
                            //Console.WriteLine($"{seconds} worker: {i + 1} Finnished {workers[i].c}");
                            if (values.CanNotBegin().Count == 1)
                            {
                                //Console.WriteLine($"Last worker time {values.CanNotBegin().First().WorkTime()}");
                                seconds += values.CanNotBegin().First().WorkTime();
                            }
                            values.RemoveKey(workers[i].c);
                            values.RemoveBefore(workers[i].c);
                        }
                    }
                }
                for (int i = 0; i < workers.Count; i++)
                {
                    if (workers[i].w == 0 && values.CanStart().Except(workers.Select(x => x.c)).Count() > 0)
                    {
                        char current = values.CanStart().Except(workers.Select(x => x.c)).First();
                        workers[i] = (current.WorkTime(), current);
                        //Console.WriteLine($"{seconds} worker: {i + 1} start {current} worktime {current.WorkTime()}");
                    }
                }
                seconds++;
            }
            seconds--;
            Console.WriteLine(seconds);
        }
    }
    
    public static class F
    {
        public static List<char> CanNotBegin(this Dictionary<char, List<char>> values) => values.Values.SelectMany(x => x).Distinct().ToList();
        public static List<char> MustBeFinished(this Dictionary<char, List<char>> values) => values.Keys.OrderBy(x => x).ToList();
        public static List<char> CanStart(this Dictionary<char, List<char>> values) => MustBeFinished(values).Except(CanNotBegin(values)).OrderBy(x => x).ToList();
        public static void RemoveBefore(this Dictionary<char, List<char>> values, char c) => values.Values.ToList().ForEach(x => x.Remove(c));
        public static void RemoveKey(this Dictionary<char, List<char>> values, char c) => values.Remove(c);
        public static int WorkTime(this char c) => (char.ToUpper(c) - 64) + 60;
    }   
}
