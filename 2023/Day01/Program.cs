// https://adventofcode.com/2023/day/1

var map = new Dictionary<string, char>
{
    { "one", '1' },{ "two", '2' },{ "three", '3' },{ "four", '4' },{ "five", '5' },
    { "six", '6' },{ "seven", '7' },{ "eight", '8' },{ "nine", '9' }
};

var lines = File.ReadAllLines("input.txt");

Console.WriteLine(GetCalibrationValue(lines, map, withLetterDigits: false));
Console.WriteLine(GetCalibrationValue(lines, map, withLetterDigits: true));

static int GetCalibrationValue(string[] lines, Dictionary<string, char> map, bool withLetterDigits) => 
    withLetterDigits ?
    lines.Sum(l => GetValue(l.Select((c, i) => GetLineLetterNumber(l[i..], map)).ToList())) :
    lines.Sum(l => GetValue([.. l]));

static char GetLineLetterNumber(string line, Dictionary<string, char> map) =>
    char.IsDigit(line[0]) ? line[0] : map.GetValueOrDefault(map.Keys.FirstOrDefault(line.StartsWith, ""), ' ');

static int GetValue(List<char> n) => int.Parse(new char[] { n.First(char.IsDigit), n.Last(char.IsDigit) });