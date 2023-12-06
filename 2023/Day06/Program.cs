// https://adventofcode.com/2023/day/6

var lines = File.ReadAllLines("input.txt").Select(x => x.Split(":").Last());
var separatedValues = GetPairs(lines.Select(GetLineValues).ToArray());
var mergedValues = GetPairs(lines.Select(x => x.Replace(" ", "")).Select(GetLineValues).ToArray());

Console.WriteLine(AggregateRecords(separatedValues));
Console.WriteLine(AggregateRecords(mergedValues));

static IEnumerable<Measurement> GetPairs(long[][] line) => Enumerable.Range(0, line[0].Length).Select(x => new Measurement((int)line[0][x], line[1][x]));
static long[] GetLineValues(string s) => s.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
static long GetNewRecords(Measurement value) => Enumerable.Range(1, value.Time).Select(x => (long)(value.Time - x) * x).Where(x => x > value.Record).Count();
static long AggregateRecords(IEnumerable<Measurement> values) => values.Select(GetNewRecords).Aggregate((long)1, (x, y) => x * y);
record Measurement(int Time, long Record);