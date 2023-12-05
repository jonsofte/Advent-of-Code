// https://adventofcode.com/2023/day/4

var input = File.ReadAllLines("input.txt");
var cardsWon = new Dictionary<int, int>();
int points = 0;

foreach (var card in input)
{
    var lineContent = card.Split([':', '|']);
    var cardID = int.Parse(lineContent[0].Split(" ").Last());
    var winningNumbers = lineContent[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
    var handNumbers = lineContent[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
    var matchingNumbers = winningNumbers.Intersect(handNumbers);

    points += (int)Math.Pow(2, matchingNumbers.Count() - 1);
    cardsWon[cardID] = cardsWon.GetValueOrDefault(cardID, 0) + 1;

    Enumerable.Range(cardID + 1, matchingNumbers.Count()).ToList().ForEach(id =>
        cardsWon[id] = cardsWon.GetValueOrDefault(id, 0) + cardsWon[cardID]);   
}

Console.WriteLine(points);
Console.WriteLine(cardsWon.Values.Sum());