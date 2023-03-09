// https://adventofcode.com/2022/day/9

var input = File.ReadAllLines("input.txt").Select(GetDirections);

Console.WriteLine(GetNumberOfTailVisits(2, input));
Console.WriteLine(GetNumberOfTailVisits(10, input));

static int GetNumberOfTailVisits(int numberOfKnots, IEnumerable<(char Direction, int Length)> input)
{
    var tailVisits = new HashSet<Position>();
    var knots = Enumerable.Repeat(new Position(0, 0), numberOfKnots).ToArray();
    input.ToList().ForEach(x => Move(x.Direction, x.Length));
    return tailVisits.Count;

    void Move(char Direction, int length)
    {
        var (x, y) = GetOrientation(Direction);

        foreach (int i in Enumerable.Range(1, length))
        {
            knots[0] = new Position(knots[0].X + x, knots[0].Y + y);
            Enumerable.Range(1, numberOfKnots - 1).ToList().ForEach(t => knots[t] = MoveKnot(knots[t - 1], knots[t]));
            tailVisits.Add(knots[numberOfKnots - 1]);
        }
    }
}

static Position MoveKnot(Position p1, Position p2) => (Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y)) switch
{
    ( >= 2, >= 2) => new Position(p1.X + ((p2.X - p1.X) / 2), p1.Y + ((p2.Y - p1.Y) / 2)),
    ( >= 2, _) => new Position(p1.X + ((p2.X - p1.X) / 2), p1.Y),
    (_, >= 2) => new Position(p1.X, p1.Y + ((p2.Y - p1.Y) / 2)),
    (_, _) => p2
};

static (char Direction, int Length) GetDirections(string input) => (input[0], int.Parse(input[2..]));
static (int x, int y) GetOrientation(char Direction) =>
    Direction switch { 'U' => (0, 1), 'D' => (0, -1), 'R' => (1, 0), 'L' => (-1, 0), _ => (0, 0) };
record Position(int X, int Y);