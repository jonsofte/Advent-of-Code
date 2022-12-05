// https://adventofcode.com/2022/day/5

using System.Text.RegularExpressions;
using Stacks = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Stack<char>>;

var input = File.ReadAllText("input.txt").Split("\r\n\r\n");

var (stacks, moves) = GetStacksAndMoves(input);
Console.WriteLine(Part1(stacks, moves));

(stacks, moves) = GetStacksAndMoves(input);
Console.WriteLine(Part2(stacks, moves));

static (Stacks stacks, IEnumerable<Move> moves) GetStacksAndMoves(string[] input)
{
    var stacksInput = input[0].Split('\n').Select(x => x.Chunk(4).Select(y => y[1]));
    var stacks = new Stacks();

    Enumerable.Range(1, stacksInput.First().Count()).ToList().ForEach(x => stacks.Add(x, new Stack<char>()));
    stacksInput.Reverse().Skip(1).ToList()
        .ForEach(l => l.Select((c, i) => (i: i + 1, c)).Where(y => y.c != ' ').ToList()
            .ForEach(s => stacks[s.i].Push(s.c)));

    return (stacks, input[1].Split("\n").Select(GetInts));
}

static string Part1(Stacks stacks, IEnumerable<Move> moves)
{
    foreach ((int count, int from, int to) in moves)
    {
        Enumerable.Range(0, count).ToList().ForEach(x => stacks[to].Push(stacks[from].Pop()));
    }
    return new string(GetTopOfStacks(stacks).ToArray());
}

static string Part2(Stacks stacks, IEnumerable<Move> moves)
{
    foreach ((int count, int from, int to) in moves)
    {
        var reversedStack = new Stack<char>();
        Enumerable.Range(0, count).ToList().ForEach(x => reversedStack.Push(stacks[from].Pop()));
        Enumerable.Range(0, count).ToList().ForEach(x => stacks[to].Push(reversedStack.Pop()));
    }
    return new string(GetTopOfStacks(stacks).ToArray());
}

static Move GetInts(string x) => GetValues(Regex.Matches(x, @"\d+").Select(x => int.Parse(x.Value)).ToArray());
static Move GetValues(int[] values) => new(values[0], values[1], values[2]);
static IEnumerable<char> GetTopOfStacks(Stacks stacks) => stacks.Select(x => x.Value.Pop());
internal record struct Move(int Count, int From, int To);