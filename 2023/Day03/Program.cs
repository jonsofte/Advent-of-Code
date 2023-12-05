// https://adventofcode.com/2023/day/3

using System.Text.RegularExpressions;

var (SumOfValidNumbers, SumOfGears) = Calculate(File.ReadAllLines("input.txt"));
Console.WriteLine(SumOfValidNumbers);
Console.WriteLine(SumOfGears);

static (int SumOfValidNumbers, int SumOfGears) Calculate(string[] input)
{
    var validNumbers = new List<int>();
    var symbols = new Dictionary<Symbol, List<int>>();

    foreach (var line in input.Select((value, i) => (value, i)))
    {
        foreach (var number in Regex.Matches(line.value, "\\d+").Select(x => (value: x.Value, pos: x.Index)))
        {
            var adjacentSymbol = Enumerable.Range(number.pos, number.value.Length)
                .SelectMany(pos => GetAdjacentSymbols(input, line.i, pos)).Distinct().SingleOrDefault();
            if (adjacentSymbol is null) continue;

            if (!symbols.TryGetValue(adjacentSymbol, out List<int>? adjacentValues))
            {
                symbols.Add(adjacentSymbol, adjacentValues = ([]));
            }

            validNumbers.Add(int.Parse(number.value));
            adjacentValues.Add(int.Parse(number.value));
        }
    }
    var SumOfGears = symbols.Where(x => x.Key.Char == '*' && x.Value.Count == 2).Sum(x => x.Value[0] * x.Value[1]);
    return (validNumbers.Sum(), SumOfGears);
}

static List<Symbol> GetAdjacentSymbols(string[] input, int line, int position) => NeighbourDelta()
    .Select(x => (l: line + x.l, p: position + x.p))
    .Where(p => p.l >= 0 && p.l < input.Length && p.p >= 0 && p.p < input[0].Length)
    .Where(x => input[x.l][x.p] != '.' && !char.IsDigit(input[x.l][x.p]))
    .Select(x => new Symbol(input[x.l][x.p], x.l, x.p)).ToList();

static IEnumerable<(int l, int p)> NeighbourDelta() =>
    new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

record Symbol(char Char, int Line, int Position);