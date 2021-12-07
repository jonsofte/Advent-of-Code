//var input = File.ReadAllLines("input.txt")
//    .Select(x => x.Split(' '))
    //.Select(x => (Direction: x[0], Value: int.Parse(x[1].ToString())));
    //.Select(x => (int.Parse(x[0]), int.Parse(x[1]), x[2][0], x[3]));
    //.Select(s => s.Split(':')).Select(x => (key: x[0], value: x[1]))).ToList();


// import

//var input = File.ReadAllText("input.txt")
//    .Split("\r\n")
//    .Select(x => x.Split(' '))
//    .Select(x => (Direction: x[0], Value: int.Parse(x[1].ToString())));

//var GetLines =
//   File.ReadAllLines("input.txt")
//   .Select(
//        l => l.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
//            .Select(x => x.Split(new char[] { ' ' }))
//            .Select(x => (int.Parse(x[0]), int.Parse(x[1]), x[2][0], x[3]))
//            );

//var passports = File.ReadAllText("input.txt").Split("\r\n\r\n")
//      .Select(l => l.Split(new string[] { " ", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
//         .Select(s => s.Split(':')).Select(x => (key: x[0], value: x[1]))).ToList();

//var input2 = File.ReadAllLines("input.txt").Select(x => (Action: x[0], Value: int.Parse(x[1..]))).ToList();

//// Aggregate Example

//int ToInt(string input, char c) => input.Aggregate(0, (t, n) => n == c ? ((t << 1) + 1) : t << 1);
//int numEven = input.Select(x=> x.Value).Aggregate(0, (total, next) => next % 2 == 0 ? total + 1 : total);


