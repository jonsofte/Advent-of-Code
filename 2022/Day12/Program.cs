// https://adventofcode.com/2022/day/12

var map = File.ReadAllText("input.txt").Split("\r\n")
    .SelectMany((l, r) => l.ToCharArray()
        .Select((height, c) => (pos: new Position(r, c), height)));

Console.WriteLine(GetDistance(map, queue: new(new[] { (map.Single(x => x.height == 'S').pos, 'a') })));
Console.WriteLine(GetDistance(map, queue: new(map.Where(x => x.height == 'a'))));

static int GetDistance(IEnumerable<(Position pos, char height)> map, Queue<(Position pos, char height)> queue)
{
    var visited = new Dictionary<Position, int>();
    queue.ToList().ForEach(x => visited[x.pos] = 0);

    while (queue.Any())
    {
        var current = queue.Dequeue();
        var neighbours = GetNeighbourPos()
            .Select(x => new Position(current.pos.Row + x.r, current.pos.Column + x.c));

        foreach (var neighbour in map.Where(x => neighbours.Contains(x.pos)))
        {
            if ((neighbour.height == 'E' ? 'z' : neighbour.height) - current.height < 2 &&
                visited.GetValueOrDefault(neighbour.pos, int.MaxValue) > visited[current.pos] + 1)
            {
                if (neighbour.height == 'E') return visited[current.pos] + 1;
                visited[neighbour.pos] = visited[current.pos] + 1;
                queue.Enqueue(neighbour);
            }
        }
    }
    return int.MaxValue;
}

static IEnumerable<(int r, int c)> GetNeighbourPos() => new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
record Position(int Row, int Column);