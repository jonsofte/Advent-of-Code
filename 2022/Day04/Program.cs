// https://adventofcode.com/2022/day/4

var input = File.ReadAllLines("input.txt")
    .Select(x => x.Split(',').Select(y => y.Split('-').Select(int.Parse).ToArray()).ToArray());

var part1 = input.Select(x => 
    (x[0][0] <= x[1][0] && x[0][1] >= x[1][1]) ||  
    (x[1][0] <= x[0][0] && x[1][1] >= x[0][1]) );

var part2 = input.Select(x => (x[0][1] >= x[1][0]) && (x[1][1] >= x[0][0]));

Console.WriteLine(part1.Count(x => x == true));
Console.WriteLine(part2.Count(x => x == true));
