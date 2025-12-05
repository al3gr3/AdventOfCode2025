var lines = File.ReadAllText("TextFile1.txt").Split(',');

SolveA(lines);
SolveB(lines);

void SolveB(string[] lines)
{
    var pairs = lines.Select(x => x.Split('-', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse))
        .Select(a => new Pair
        {
            Start = a.First(),
            End = a.Last()
        }).ToArray();

    var max = pairs.Max(x => x.End);

    var halfLen = Math.Max(max.ToString().Length / 2, 1);
    var halfmax = long.Parse(max.ToString()[0..halfLen]);

    var res = 0L;
    var found = new HashSet<long>();
    for (long i = 1; i <= halfmax; i++)
    {
        var check = i.ToString() + i;
        while (long.Parse(check) <= max)
        {
            var parseCheck = long.Parse(check); 
            var add = pairs.Count(p => p.Contains(parseCheck)) * parseCheck;
            if (add > 0 && !found.Contains(parseCheck))
            {
                found.Add(parseCheck);
                res += add;
            }
            check += i;
        }
    }

    Console.WriteLine(res);
}

static void SolveA(string[] lines)
{
    var res = 0L;

    foreach (var line in lines)
    {
        var splits = line.Split('-', StringSplitOptions.RemoveEmptyEntries);

        var halfLen = Math.Max(splits[0].Length / 2, 1);
        var cur = long.Parse(splits.First()[0..halfLen]);
        do
        {
            var check = long.Parse(cur.ToString() + cur.ToString());

            if (check >= long.Parse(splits[0]) && check <= long.Parse(splits[1]))
                res += check;
            else if (check > long.Parse(splits[1]))
                break;

            cur++;
        } while (true);
    }
    Console.WriteLine(res);
}

internal class Pair
{
    public long Start;
    public long End;

    public bool Contains(long l)
    {
        return Start <= l && l <= End;
    }
}