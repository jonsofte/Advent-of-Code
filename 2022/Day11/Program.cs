// https://adventofcode.com/2022/day/11

Console.WriteLine(GetMonkeyBusiness(part1: true));
Console.WriteLine(GetMonkeyBusiness(part1: false));

static long GetMonkeyBusiness(bool part1)
{
    var monkeys = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(GetMonkey).ToDictionary(x => x.monkey.ID);
    long leastCommonDivisor = monkeys.Values.Select(x => x.monkey.DivisibleBy).Aggregate((long)1, (sum, val) => sum * val);
    var inspections = monkeys.Values.Select(x => x.monkey.ID).ToDictionary(k => k, v => (long)0);

    foreach (int i in Enumerable.Range(1, part1 ? 20 : 10000))
    {
        foreach ((Monkey monkey, List<long> items) in monkeys.Values)
        {
            foreach (long item in items)
            {
                long worry = monkey.Operation(item) / (part1 ? 3 : 1);
                monkeys[ThrowToMonkey(monkey, worry)].items.Add(worry % leastCommonDivisor);
                inspections[monkey.ID]++;
            }
            items.Clear();
        }
    }
    var numberOfInspections = inspections.Values.OrderDescending().ToArray();
    return numberOfInspections[0] * numberOfInspections[1];
}

static int ThrowToMonkey(Monkey monkey, long worry) =>
    worry % monkey.DivisibleBy == 0 ? monkey.TrueThrow : monkey.FalseThrow;

static (Monkey monkey, List<long> items) GetMonkey(string x)
{
    var lines = x.Split('\n');
    return (new Monkey(
                int.Parse(lines[0].Split(' ').Last().ToCharArray().AsSpan()[..1]),
                getOperation(lines[2].Trim()),
                int.Parse(lines[3].Split(' ').Last()),
                int.Parse(lines[4].Split(' ').Last()),
                int.Parse(lines[5].Split(' ').Last())
            ),
            lines[1].Trim().Split(' ').Skip(2).Select(x => x.Replace(',', ' ')).Select(x => long.Parse(x)).ToList()
        );
}

static Func<long, long> getOperation(string v)
{
    var values = v.Split(' ')[^2..];
    return values[0] switch
    {
        "*" => x => x * (long.TryParse(values[1], out long value) ? value : x),
        "+" => x => x + (long.TryParse(values[1], out long value) ? value : x),
        _ => x => x
    };
}

record Monkey(int ID, Func<long, long> Operation, long DivisibleBy, int TrueThrow, int FalseThrow);