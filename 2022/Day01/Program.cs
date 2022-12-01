// https://adventofcode.com/2022/day/1

var elves = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(int.Parse));
Console.WriteLine(elves.Select(x => x.Sum()).Max());
Console.WriteLine(elves.Select(x => x.Sum()).Order().Reverse().Take(3).Sum());