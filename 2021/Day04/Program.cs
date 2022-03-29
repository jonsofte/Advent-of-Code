// https://adventofcode.com/2021/day/4
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllText("input.txt");

var drawnNumbers = lines.Split("\r\n").Take(1).First().Split(',').Select(x => int.Parse(x)).ToList();

var boards = lines.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries).Skip(1)
    .Select(x => x.Split("\r\n").Select(y => y.Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(x => (Value: int.Parse(x), Marked: false)).ToList()).ToList()).ToList();

Console.WriteLine(FirstBingo(drawnNumbers, boards));
Console.WriteLine(LastBingo(drawnNumbers, boards));

static int FirstBingo(List<int> drawnNumbers, IEnumerable<List<List<(int Value, bool Marked)>>> boards)
{
    foreach (int number in drawnNumbers)
    {
        foreach (var board in boards)
        {
            MarkNumber(board, number);
            if (IsBingo(board)) return GetBoardValue(board) * number;
        }
    }
    return 0;
}

static int LastBingo(List<int> drawnNumbers, IEnumerable<List<List<(int Value, bool Marked)>>> boards)
{
    foreach (int number in drawnNumbers)
    {
        foreach (var board in boards)
        {
            if (!IsBingo(board) && (boards.Count(x => IsBingo(x)) == boards.Count() - 1))
            {
                MarkNumber(board, number);
                if (IsBingo(board)) return GetBoardValue(board) * number;
            }
            else
                MarkNumber(board, number);
        }
    }
    return 0;
}

static bool HasHorizontalBingo(List<List<(int Value, bool Marked)>> board) => board.Any(r => r.All(c => c.Marked));

static bool HasVerticalBingo(List<List<(int Value, bool Marked)>> board) =>
    Enumerable.Range(0, 5).Any(c => board.Select(r => r[c]).All(c => c.Marked));

static bool IsBingo(List<List<(int Value, bool Marked)>> board) => HasHorizontalBingo(board) || HasVerticalBingo(board);

static void MarkNumber(List<List<(int Value, bool Marked)>> board, int number) =>
    Enumerable.Range(0, 5).SelectMany(s => Enumerable.Range(0, 5), (r, c) => (r, c)).ToList()
        .ForEach(i => { if (board[i.r][i.c].Value == number) board[i.r][i.c] = (number, true); });

static int GetBoardValue(List<List<(int Value, bool Marked)>> board) =>
    board.Sum(l => l.Where(v => !v.Marked).Sum(v => v.Value));