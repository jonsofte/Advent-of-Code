// Advent of Code Day 06
// https://adventofcode.com/2019/day/6

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputPlanets = GetOrbits("input.txt");
            var orbits = new Dictionary<string, string>();
            inputPlanets.ForEach(p => orbits.Add(p.orbit, p.planet));

            // Part 1
            Console.WriteLine(GetNumberOfTotalOrbits());

            List<(string planet, string orbit)> GetOrbits(string filename) => File.ReadAllText(filename)
                .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(')'))
                .Select(x => (planet: x[0], orbit: x[1])).ToList();

            int GetNumberOfOrbits(string planet) => (orbits.ContainsKey(planet)) ? GetNumberOfOrbits(orbits[planet]) + 1 : 0;
            int GetNumberOfTotalOrbits() => orbits.Select(x => x.Key).Select(p => GetNumberOfOrbits(p)).Sum();

            // Part 2
            var PathYou = getPath("YOU");
            var PathSanta = getPath("SAN");

            var intersection = PathYou.OrderByDescending(x => x.Value).Where(k => PathSanta.ContainsKey(k.Key)).Select(k => k.Key).First();
            var distance = PathYou["YOU"] + PathSanta["SAN"] - (PathYou[intersection] * 2) - 2;

            Console.WriteLine(distance);

            Dictionary<string, int> getPath(string name)
            {
                var path = new Dictionary<string, int>();
                bool done = false;

                while (!done)
                {
                    path.Add(name, GetNumberOfOrbits(name));
                    if (path[name] == 0) done = true;
                    else name = orbits[name];
                }
                return path;
            }
        }
    }
}
