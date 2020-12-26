using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var values = File.ReadAllLines("input.txt").Select(t => t
   .Split(new string[] { "contain", ",", "." }, StringSplitOptions.RemoveEmptyEntries)
   .Select(x => x.Trim().Replace("bags", "bag")).ToList())
   .ToDictionary( 
      k => k[0], 
      v => v[1].Trim() != "no other bag" ? v.Skip(1).ToDictionary(
         v => v[2..], 
         v => int.Parse(v.Substring(0, 1))) : new Dictionary<string, int>()
         );

Console.WriteLine(values.Keys.Count(x => Contains(x)));
Console.WriteLine(BagsContaining("shiny gold bag"));

bool Contains(string bag) => values[bag].ContainsKey("shiny gold bag") || values[bag].Keys.Any(Contains);
int BagsContaining(string bag) => values[bag].Sum(kvp => kvp.Value * (BagsContaining(kvp.Key) + 1));
