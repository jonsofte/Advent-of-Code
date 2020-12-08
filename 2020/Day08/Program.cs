using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt").Select(l => l.Split(' '));
var instructions = lines.Select(x => (inst: x[0], value: int.Parse(x[1][1..]) * (x[1][0] == '-' ? -1 : 1))).ToList();

Console.WriteLine(ProcessInstructions(instructions).accumulator);
Console.WriteLine(FlipInstructions(instructions));

(int accumulator, int pointer) ProcessInstructions(List<(string inst, int value)> i)
{
   var instructionsExecuted = new List<int>();
   (int accumulator, int pointer) reg = new(0, 0);
   (int accumulator, int pointer) Process((string inst, int value) i) => i.inst switch
   {
       "acc" => (reg.accumulator + i.value, reg.pointer + 1),
       "jmp" => (reg.accumulator, reg.pointer + i.value),
       "nop" => (reg.accumulator, reg.pointer + 1)
   }; 

   while (reg.pointer < instructions.Count && !instructionsExecuted.Contains(reg.pointer))
   {
      instructionsExecuted.Add(reg.pointer);
      reg = Process(instructions[reg.pointer]);
   }
   return reg;
}

int FlipInstructions(List<(string inst, int value)> instructions)
{
   string flip(string value) => value switch { "jmp" => "nop", "nop" => "jmp", _ => value};
   for (int i = 0; i < instructions.Count; i++)
   {
      instructions[i] = (flip(instructions[i].inst), instructions[i].value);
      var comp = ProcessInstructions(instructions);
      if (comp.pointer == instructions.Count) return comp.accumulator;
      instructions[i] = (flip(instructions[i].inst), instructions[i].value);
   }
   return 0;
}