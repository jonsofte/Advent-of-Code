using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {

            string downloadString = File.ReadAllText("input.txt");
            string[] lines = downloadString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var SleepTimesPerGuard = new Dictionary<int, int>();
            var SleepMinutesPerGuard = new Dictionary<int, int[]>();

            GetSleepData(lines, SleepTimesPerGuard, SleepMinutesPerGuard);

            Part1(SleepTimesPerGuard, SleepMinutesPerGuard);
            Part2(SleepMinutesPerGuard);

            Console.Read();

        }

        private static void GetSleepData(string[] linesInString, Dictionary<int, int> SleepTimesPerGuard, Dictionary<int, int[]> SleepMinutesPerGuard)
        {
            int currentGuard = -1;
            int SleepStart = 0;

            foreach (string line in linesInString.OrderBy(x => x))
            {
                string partTime = line.Substring(1, 16);
                string[] partText = line.Substring(18).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                if (partText[0] == "Guard")
                {
                    currentGuard = Int32.Parse(partText[1].Substring(1));
                    if (!SleepTimesPerGuard.ContainsKey(currentGuard)) SleepTimesPerGuard.Add(currentGuard, 0);
                    if (!SleepMinutesPerGuard.ContainsKey(currentGuard)) SleepMinutesPerGuard.Add(currentGuard, new int[60]);
                }
                if (partText[0] == "falls")
                {
                    DateTime time = DateTime.Parse(partTime);
                    SleepStart = time.Minute;
                }
                if (partText[0] == "wakes")
                {
                    DateTime time = DateTime.Parse(partTime);
                    int wake = time.Minute;

                    for (int i = SleepStart; i < wake; i++)
                    {
                        SleepTimesPerGuard[currentGuard]++;
                        SleepMinutesPerGuard[currentGuard][i]++;
                    }
                }
            }
        }

        private static void Part1(Dictionary<int, int> SleepTimesPerGuard, Dictionary<int, int[]> SleepMinutesPerGuard)
        {
            int guard = SleepTimesPerGuard.OrderByDescending(x => x.Value).Select(x => x.Key).First();
            int maxSleepTime = SleepMinutesPerGuard[guard].Max();
            int minuteAtSleep = SleepMinutesPerGuard.Where(g => g.Key == guard).Select(x => x.Value.ToList().FindIndex(m => m == maxSleepTime)).First();

            Console.WriteLine(minuteAtSleep * guard);
        }

        private static void Part2(Dictionary<int, int[]> SleepMinutesPerGuard)
        {
            var max = SleepMinutesPerGuard.SelectMany(x => x.Value).Max();

            foreach (var guard in SleepMinutesPerGuard.Keys)
            {
                for (int minute = 0; minute < 60; minute++)
                    if (SleepMinutesPerGuard[guard][minute] == max)
                        Console.WriteLine(minute * guard);
            }
        }
    }
}
