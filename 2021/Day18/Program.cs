// https://adventofcode.com/2021/day/18
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");
Console.WriteLine(Magnitude(AddAllSnailFishNumbers(input)));
Console.WriteLine(GetMaximumMagnitude(input));

Element AddAllSnailFishNumbers(string[] input)
{
    Element element = ParseTree(input[0]);
    input.Skip(1).ToList().ForEach(line => element = Reduce(new Pair(element, ParseTree(line))));
    return element;
}

int GetMaximumMagnitude(string[] input) =>
    GetLinesCombination(input.Length).Max(x => Magnitude(Reduce(new Pair(ParseTree(input[x.first]), ParseTree(input[x.second])))));

static int Magnitude(Element element) => element switch
{
    Pair => (Magnitude(((Pair)element).Left) * 3) + (Magnitude(((Pair)element).Right) * 2),
    _ => ((RegularNumber)element).Value,
};

static IEnumerable<(int first, int second)> GetLinesCombination(int max) =>
    Enumerable.Range(0, max).SelectMany(s => Enumerable.Range(0, max), (x, y) => (x, y)).Where(p => p.x != p.y);

static Element Reduce(Element element)
{
    (bool isExploded, bool isSplitted) = (true, true);
    while (isExploded || isSplitted)
    {
        (_, _, element, isExploded) = CheckExplode(element, depth: 1);
        if (!isExploded) (element, isSplitted) = CheckSplit(element);
    }
    return element;
}

static (Element element, bool isSplitted) CheckSplit(Element element)
{
    if (element is RegularNumber number) return (number.Value > 9) ? (Split(number), true) : (element, false);
    var result = CheckSplit(((Pair)element).Left);
    if (result.isSplitted) return (new Pair(result.element, ((Pair)element).Right), true);
    result = CheckSplit(((Pair)element).Right);
    if (result.isSplitted) return (new Pair(((Pair)element).Left, result.element), true);
    return (element, false);
};

static (int left, int right, Element updated, bool wasUpdated) CheckExplode(Element element, int depth) => (element, depth) switch
{
    (Pair, < 5) => ExplodeTree((Pair)element, depth),
    (Pair, >= 5) => ExplodePair((Pair)element),
    (_, _) => (0, 0, element, false),
};

static (int left, int right, Element updated, bool wasUpdated) ExplodeTree(Pair pair, int depth)
{
    var (leftRest, rightRest, elementLeft, isUpdatedLeft) = CheckExplode(pair.Left, depth + 1);
    if (isUpdatedLeft) return (leftRest, 0, new Pair(elementLeft, AddLeft(pair.Right, rightRest)), isUpdatedLeft);
    (leftRest, rightRest, var elementRight, var isUpdatedRight) = CheckExplode(pair.Right, depth + 1);
    if (isUpdatedRight) return (0, rightRest, new Pair(AddRight(pair.Left, leftRest), elementRight), isUpdatedRight);
    else return (0, 0, new Pair(elementLeft, elementRight), (isUpdatedLeft || isUpdatedRight));
}

static (int leftRest, int rightRest, RegularNumber, bool) ExplodePair(Pair element) =>
    (((RegularNumber)element.Left).Value, ((RegularNumber)element.Right).Value, new RegularNumber(0), true);

static Pair Split(RegularNumber element) =>
    new(new RegularNumber(element.Value / 2), new RegularNumber((int)Math.Ceiling((decimal)element.Value / 2)));

static Element AddRight(Element element, int value) => (element is Pair pair) ?
    new Pair(pair.Left, AddRight(pair.Right, value)) : new RegularNumber(((RegularNumber)element).Value + value);

static Element AddLeft(Element element, int value) => (element is Pair pair) ?
    new Pair(AddLeft(pair.Left, value), pair.Right) : new RegularNumber(((RegularNumber)element).Value + value);

static Element ParseTree(string input)
{
    var queue = new Queue<char>(input.ToCharArray());
    var elements = new Stack<Element>();

    while (queue.Count > 0)
    {
        char c = queue.Dequeue();
        if (int.TryParse(c.ToString(), out int value)) elements.Push(new RegularNumber(value));
        else if (c == ']')
        {
            Element second = elements.Pop();
            elements.Push(new Pair(elements.Pop(), second));
        }
    }
    return elements.Pop();
}

abstract record Element;
record RegularNumber(int Value) : Element;
record Pair(Element Left, Element Right) : Element;