var lines = File.ReadAllLines("TextFile1.txt").SkipWhile(x => !x.Contains('x'))
    .Select(line => line.Split([' ', 'x', ':'], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
Console.WriteLine(lines.Count(splits => splits[0] * splits[1] >= splits.Skip(2).Sum(x => 9 * x)));