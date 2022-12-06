// https://adventofcode.com/2022/day/6

var input = File.ReadAllText("input.txt").ToCharArray();

Console.WriteLine(StartOfPacketPosition(length: 4, input));
Console.WriteLine(StartOfPacketPosition(length: 14, input));

static int StartOfPacketPosition(int length, char[] input) =>
    Enumerable.Range(length - 1, input.Length - length - 1)
    .Select(x => (r: input[(x - (length - 1))..(x + 1)], i: x))
    .Where(x => x.r.Distinct().Count() == length).First().i + 1;