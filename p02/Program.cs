var ranges = File.ReadAllText("TextFile1.txt").Split(',').Select(x => Range.Parse(x)).ToArray();

SolveA(ranges);
SolveB(ranges);

void SolveB(Range[] ranges)
{
    var max = ranges.Max(x => x.End);

    var halfLen = Math.Max(max.ToString().Length / 2, 1);
    var halfmax = long.Parse(max.ToString()[0..halfLen]);

    var found = new HashSet<long>();
    for (long i = 1; i <= halfmax; i++)
    {
        var check = $"{i}{i}"; // 2 repeats
        while (true)
        {
            var parseCheck = long.Parse(check);
            if (parseCheck > max)
                break;
            
            var add = ranges.Count(p => p.Contains(parseCheck)) * parseCheck;
            if (add > 0 && !found.Contains(parseCheck))
                found.Add(parseCheck);

            check += i; // 3 repeats, etc
        }
    }

    Console.WriteLine(found.Sum());
}

static void SolveA(Range[] ranges)
{
    var res = 0L;

    foreach (var range in ranges)
    {
        var halfLen = Math.Max(range.Start.ToString().Length / 2, 1);
        var cur = long.Parse(range.Start.ToString()[0..halfLen]);
        do
        {
            var check = long.Parse($"{cur}{cur}");

            if (range.Contains(check))
                res += check;
            else if (check > range.End)
                break;

            cur++;
        } while (true);
    }
    Console.WriteLine(res);
}

internal class Range
{
    public long Start;
    public long End;

    public static Range Parse(string s)
    {
        var splits = s.Split('-').Select(long.Parse).ToArray();
        return new Range
        {
            Start = splits[0],
            End = splits[1]
        };
    }

    public bool Contains(long l) => Start <= l && l <= End;
}