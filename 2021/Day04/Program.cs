// https://adventofcode.com/2021/day/4

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllText("input.txt");
var drawnNumbers = lines.Split("\r\n").Take(1).First().Split(',').Select(x => int.Parse(x)).ToList();
var boards = lines.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)
    .Select(x => x.Split("\r\n").Select( y => y.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(x => (Value: int.Parse(x), Marked: false )).ToList()).ToList()).ToList();

Console.WriteLine(FirstBingo());
Console.WriteLine(LastBingo());

// Functions

static bool HasHorizontalBingo(List<List<(int Value, bool Marked)>> board) => board.Any(r => r.All(c => c.Marked));
static bool HasVerticalBingo(List<List<(int Value, bool Marked)>> board) => Enumerable.Range(0, 5).Select(c => board[c]).Any(r => r.All(x => x.Marked));
static bool IsBingo(List<List<(int Value, bool Marked)>> board) => HasHorizontalBingo(board) || HasVerticalBingo(board);
static int GetBoardValue(List<List<(int Value, bool Marked)>> board) => board.Sum(l => l.Where(v => !v.Marked).Sum(v => v.Value));

static void MarkNumber(List<List<(int Value, bool Marked)>> board, int number) => Enumerable.Range(0, 5)
        .SelectMany(s => Enumerable.Range(0, 5), (x, y) => (x, y)).ToList()
        .ForEach(i => { if (board[i.x][i.y].Value == number) board[i.x][i.y] = (number, true); });

int FirstBingo()
{
    foreach (int number in drawnNumbers)
    {
        foreach (var board in boards.ToList())
        {
            MarkNumber(board, number);
            if (IsBingo(board)) return GetBoardValue(board) * number;
        }
    }
    return 0;
}

int LastBingo()
{
    foreach (int number in drawnNumbers)
    {
        foreach (var board in boards.ToList())
        {
            if (!IsBingo(board) && (boards.Count(x => IsBingo(x)) == boards.Count - 1))
            {
                MarkNumber(board, number);
                var result = IsBingo(board);
                if (result) return GetBoardValue(board) * number;
            }
            else
            {
                MarkNumber(board, number);
            }
        }
    }
    return 0;
}