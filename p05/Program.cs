var lines = File.ReadAllLines("TextFile1.txt");

SolveA(lines);
SolveB(lines);

void SolveB(string[] lines)
{
    var ranges = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(x => Range.Parse(x)).ToList();

    start:
    for (var i = 0; i < ranges.Count; i++)
    {
        for (var j = i + 1; j < ranges.Count; j++)
        {
            var union = ranges[i].Union(ranges[j]);
            if (union.Length == 1)
            {
                ranges.RemoveAt(j);
                ranges.RemoveAt(i);
                ranges.Add(union.First());
                goto start;
            }
        }
    }

    Console.WriteLine(ranges.Sum(r => r.Length()));
}

static void SolveA(string[] lines)
{
    var ranges = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(x => Range.Parse(x)).ToList();
    var points = lines.Skip(ranges.Count + 1).Select(long.Parse).ToList();

    var res = points.Count(p => ranges.Any(r => r.Contains(p)));
    Console.WriteLine(res);
}

internal class Range
{
    public long Start;
    public long End;

    public bool Contains(long l) => Start <= l && l <= End;

    public Range[] Union(Range other) => other.End < Start - 1 || other.Start > End + 1
        ? [this, other]
        : [new Range
        {
            Start = Math.Min(Start, other.Start),
            End = Math.Max(End, other.End)
        }];

    public static Range Parse(string s)
    {
        var splits = s.Split('-', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToArray();
        return new Range
        {
            Start = splits[0],
            End = splits[1]
        };
    }

    public long Length() => End - Start + 1;
}