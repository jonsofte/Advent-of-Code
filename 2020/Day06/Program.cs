using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var questions = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(l => l.Split("\r\n"));
IEnumerable<char> AllAnswers(string[] group) => group.SelectMany(y => y).Distinct();
IEnumerable<char> EveryoneAnswers(string[] group) => AllAnswers(group).Where(y => group.All(c => c.Contains(y)));

Console.WriteLine(questions.Select(x => AllAnswers(x)).Select(x => x.Count()).Sum());
Console.WriteLine(questions.Select(x => EveryoneAnswers(x)).Select(x => x.Count()).Sum());