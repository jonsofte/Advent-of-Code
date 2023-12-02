// https://adventofcode.com/2023/day/2

var games = File.ReadAllLines("input.txt");

Console.WriteLine(games.Select(GetGame).Where(x => x.validGame).Sum(x => x.gameID));
Console.WriteLine(games.Select(GetGame).Sum(x => x.power));

static (int gameID, bool validGame, int power) GetGame(string game)
{
    var gameCubes = new Dictionary<string, int>();
    var content = game.Split(':');

    foreach (var round in content[1].Split(";"))
    {
        var cubes = round.Split(",").Select(x => x.Trim().Split(" ")).Select(c => (quantity: int.Parse(c[0]), color:c[1]));
        foreach (var (quantity, color) in cubes)
        {
            if (!gameCubes.TryGetValue(color, out int value) || value < quantity)
            {
                gameCubes[color] = quantity;
            }
        }
    }

    int gameID = int.Parse(content[0].Split(' ').Last());
    bool validGame = gameCubes["green"] <= 13 && gameCubes["red"] <= 12 && gameCubes["blue"] <= 14;
    int power = gameCubes["green"] * gameCubes["red"] * gameCubes["blue"];
    return (gameID, validGame, power);
}