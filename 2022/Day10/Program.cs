var instructions = File.ReadAllLines("input.txt").Select(x => x.Split(' ')).Select(GetInstruction);

Console.WriteLine(ExecuteInstructions(instructions));
ExecuteInstructions(instructions, printToScreen: true);

static int ExecuteInstructions(IEnumerable<Instruction> instructions, bool printToScreen = false)
{
    int x = 1;
    int signalStrength = 0;
    int clockCycle = 0;
    int nextExecution = 0;

    var queue = new Queue<Instruction>(instructions);
    Instruction currentInstruction = new("noop", 0);

    while (queue.Any())
    {
        if (clockCycle == nextExecution)
        {
            x += currentInstruction.Value;
            currentInstruction = queue.Dequeue();
            nextExecution = GetExecutingCycles(currentInstruction.Command) + clockCycle;
        }

        if (GetSignalStrength(clockCycle))
        {
            signalStrength += x * clockCycle;
        }

        if (printToScreen)
        {
            if (clockCycle % 40 == 0) Console.Write("\n");
            Console.Write((DrawPixel(x, clockCycle) ? "#" : " "));
        }

        clockCycle++;
    }
    return signalStrength;
}

static bool GetSignalStrength(int clockCycle) => clockCycle > 0 && (clockCycle == 20 || (clockCycle + 20) % 40 == 0);
static bool DrawPixel(int x, int clockCycle) => clockCycle % 40 >= x - 1 && clockCycle % 40 <= x + 1;
static int GetExecutingCycles(string command) => command switch { "noop" => 1, "addx" => 2, _ => 0 };
static Instruction GetInstruction(string[] i) => new(i[0], (i.Length > 1) ? int.Parse(i[1]) : 0);
record Instruction(string Command, int Value);