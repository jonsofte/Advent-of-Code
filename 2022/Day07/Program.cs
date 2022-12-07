// https://adventofcode.com/2022/day/7

IEnumerable<string> input = File.ReadAllLines("input.txt").Skip(1);
var sizes = new List<DirectorySize>();
GetFolderSizes(ParseInput("\\"));

Console.WriteLine(sizes.Where(x => x.Size <= 100000).Sum(x => x.Size));

int unusedSpace = 70000000 - sizes.Single(x => x.Name == "\\").Size;
Console.WriteLine(sizes.Where(x => x.Size + unusedSpace > 30000000).Min(x => x.Size));

Directory ParseInput(string path)
{
    var directory = new Directory(path, new Dictionary<string, Node>());
    while (input.Any() && input.First() != "$ cd ..")
    {
        string line = input.First();
        if (line.Substring(2, 2) == "ls")
        {
            directory = directory with { Nodes = Content(input.Skip(1).TakeWhile(x => !x.StartsWith('$')), directory.Nodes) };
            input = input.Skip(1).SkipWhile(x => !x.StartsWith('$'));
        }
        else if (line.Substring(2, 2) == "cd")
        {
            string directoryPath = line[5..];
            input = input.Skip(1);
            directory.Nodes[directoryPath] = ParseInput(directoryPath);
        }
    }
    input = input.Skip(1);
    return directory;
}

static Dictionary<string, Node> Content(IEnumerable<string> lines, Dictionary<string, Node> nodes)
{
    foreach (string entry in lines)
    {
        if (entry[..3].Contains("dir"))
        {
            nodes.Add(entry[4..], new Directory(entry[4..], new Dictionary<string, Node>()));
        }
        else
        {
            var file = entry.Split(' ');
            nodes.Add(file[1], new EFile(file[1], int.Parse(file[0])));
        }
    }
    return nodes;
}

int GetFolderSizes(Node currentNode)
{
    int sum = 0;
    if (currentNode is Directory directory)
    {
        directory.Nodes.ToList().ForEach(x => sum += GetFolderSizes(x.Value));
        sizes.Add(new DirectorySize(currentNode.Name, sum));
    }
    else
    {
        sum += ((EFile)currentNode).Size;
    }
    return sum;
}

internal record DirectorySize(string Name, int Size);
internal record Node(string Name);
internal record EFile(string Name, int Size) : Node(Name);
internal record Directory(string Name, Dictionary<string, Node> Nodes) : Node(Name);