// https://adventofcode.com/2022/day/2

var input = File.ReadAllLines("input.txt").Select(x => (f: x[0], s: x[2]));
var firstGame = input.Select(x => GamePoints(LetterToHand1(x.f), LetterToHand2(x.s)) + TypePoints(LetterToHand2(x.s)));
var secondGame = input
    .Select(x => (f: LetterToHand1(x.f), n: GetGameHand(LetterToHand1(x.f), x.s)))
    .Select(x => GamePoints(x.f, x.n) + TypePoints(x.n));

Console.WriteLine(firstGame.Sum());
Console.WriteLine(secondGame.Sum());

static int GamePoints(char first, char second) => (first, second) switch
{
    ('R', char s) => s switch { 'R' => 3, 'P' => 6, 'S' => 0, _ => 0 },
    ('P', char s) => s switch { 'R' => 0, 'P' => 3, 'S' => 6, _ => 0 },
    ('S', char s) => s switch { 'R' => 6, 'P' => 0, 'S' => 3, _ => 0 },
    _ => 0
};

static char GetGameHand(char first, char second) => (first, second) switch
{
    (char f, 'X') => f switch { 'R' => 'S', 'P' => 'R', 'S' => 'P', _ => f },
    (char f, 'Y') => f switch { 'R' => 'R', 'P' => 'P', 'S' => 'S', _ => f },
    (char f, 'Z') => f switch { 'R' => 'P', 'P' => 'S', 'S' => 'R', _ => f },
    _ => first
};

static int TypePoints(char second) => second switch { ('R') => 1, ('P') => 2, ('S') => 3, _ => 0 };
static char LetterToHand1(char l) => l switch { 'A' => 'R', 'B' => 'P', 'C' => 'S', _ => l };
static char LetterToHand2(char l) => l switch { 'X' => 'R', 'Y' => 'P', 'Z' => 'S', _ => l };