var lines = File.ReadAllLines("TextFile1.txt").SkipWhile(x => !x.Contains('x'))
    .Select(line => line.Split([' ', 'x', ':'], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
Console.WriteLine(lines.Count(ns => (ns[0] / 3) * (ns[1] / 3) >= ns.Skip(2).Sum()));