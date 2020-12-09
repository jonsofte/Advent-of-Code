using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var passports = File.ReadAllText("input.txt").Split("\r\n\r\n")
      .Select(l => l.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
         .Select(s => s.Split(':')).Select(x => (key: x[0], value: x[1]))).ToList();

var ValidPassports = passports.Where(x => x.Count(y => y.key != "cid") == 7);
Console.WriteLine(ValidPassports.Count());
Console.WriteLine(ValidPassports.Where(x => x.All(y => isValidValue(y.key, y.value))).Count());

bool isValidValue(string key, string value) => key switch
{
   "byr" => Int32.Parse(value) is >= 1920 and <= 2002,
   "iyr" => Int32.Parse(value) is >= 2010 and <= 2020,
   "eyr" => Int32.Parse(value) is >= 2020 and <= 2030,
   "hgt" => Regex.IsMatch(value, "(1([5-8][0-9]|9[0-3])cm|(59|6[0-9]|7[0-6])in)$"),
   "hcl" => Regex.IsMatch(value, "#([0-9]|[a-f]){6}"),
   "ecl" => new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(value),
   "pid" => value.Length == 9,
   "cid" => true,
   _ => false
};