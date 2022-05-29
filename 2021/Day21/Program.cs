using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var matches = Regex.Matches(File.ReadAllText("input.txt"), "[0-9]+");
var (player1, player2) = (new Player(int.Parse(matches[1].Value), 0), new Player(int.Parse(matches[3].Value), 0));

Console.WriteLine(PracticeRound(player1, player2));
Console.WriteLine(GetMaxUniverses(player1, player2));

static long PracticeRound(Player currentPlayer, Player nextPlayer)
{
    (int diceValue, int NumberOfRolls) = (1, 0);

    while (currentPlayer.Score < 1000 && nextPlayer.Score < 1000)
    {
        NumberOfRolls += 3;
        (int moves, diceValue) = RollDiceThreeTimes(diceValue);
        int Position = GetPosition(currentPlayer.Position + moves);
        currentPlayer = new Player(Position, currentPlayer.Score + Position);
        (nextPlayer, currentPlayer) = (currentPlayer, nextPlayer);
    }

    return Math.Min(currentPlayer.Score, nextPlayer.Score) * NumberOfRolls;
}

static long GetMaxUniverses(Player player1, Player player2)
{
    var combinations = new Dictionary<(Player, Player), (long, long)>();
    var (universes1, universes2) = NumberOfSplitUniverses(player1, player2);
    return Math.Max(universes1, universes2);

    (long universes1, long universes2) NumberOfSplitUniverses(Player currentPlayer, Player nextPlayer)
    {
        if (currentPlayer.Score >= 21) return (1, 0);
        if (nextPlayer.Score >= 21) return (0, 1);
        if (combinations.ContainsKey((currentPlayer, nextPlayer))) return combinations[(currentPlayer, nextPlayer)];

        (long combinationsCurrent, long combinationsNext) = (0, 0);
        foreach (var (d1, d2, d3) in QuantumRolls())
        {
            int position = GetPosition(currentPlayer.Position + d1 + d2 + d3);
            var (existingNext, existingCurrent) = NumberOfSplitUniverses(nextPlayer, new Player(position, currentPlayer.Score + position));
            (combinationsCurrent, combinationsNext) = (combinationsCurrent + existingCurrent, combinationsNext + existingNext);
        }
        combinations.Add((currentPlayer, nextPlayer), (combinationsCurrent, combinationsNext));
        return (combinationsCurrent, combinationsNext);
    }
}

static int GetPosition(int value) => ((value - 1) % 10) + 1;
static (int moves, int diceValue) RollDiceThreeTimes(int value) => (DeterministicDice(value) + DeterministicDice(value + 1) + DeterministicDice(value + 2), DeterministicDice(value + 3));
static int DeterministicDice(int value) => (value - 1) % 100 + 1;
static IEnumerable<(int d1, int d2, int d3)> QuantumRolls() => Enumerable.Range(1, 3).SelectMany(d1 => Enumerable.Range(1, 3).SelectMany(d2 => Enumerable.Range(1, 3).Select(d3 => (d1, d2, d3))));
record Player(int Position, long Score);