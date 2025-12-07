var lines = File.ReadAllLines("TextFile1.txt")
    .Select(line => line.ToList()).ToList()
    .Transpose()
    .Select(x => new string(x.ToArray())).ToArray();

SolveA();
SolveB();

void SolveB()
{
    var beams = new[]
    {
        new Beam
        {
            Y = Array.FindIndex(lines, x => x.StartsWith("S")),
            X = 0,
            Count = 1
        }
    }.ToList();

    var res = 0;
    for (var i = 0; i < lines.First().Length - 1; i++)
    {
        var newBeams = new List<Beam>();
        foreach (var beam in beams)
        {
            var currentChar = lines[beam.Y][beam.X + 1];
            if (currentChar == '^')
            {
                res++;
                AddWithCount(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y - 1,
                    Count = beam.Count,
                });
                AddWithCount(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y + 1,
                    Count = beam.Count,
                });
            }
            else
                AddWithCount(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y,
                    Count = beam.Count
                });
        }
        beams = newBeams;
    }

    Console.WriteLine(beams.Sum(x => x.Count));
}

void Add(List<Beam> newBeams, Beam point)
{
    var existing = newBeams.FirstOrDefault(x => x.X == point.X && x.Y == point.Y);
    if (existing == null)
        newBeams.Add(point);
}

void AddWithCount(List<Beam> newBeams, Beam point)
{
    var existing = newBeams.FirstOrDefault(x => x.X == point.X && x.Y == point.Y);
    if (existing == null)
        newBeams.Add(point);
    else
        existing.Count += point.Count;
}

void SolveA()
{
    var beams = new[]
    {
        new Beam
        {
            Y = Array.FindIndex(lines, x => x.StartsWith("S")),
            X = 0
        }
    }.ToList();

    var res = 0;
    for (var i = 0; i < lines.First().Length - 1; i++)
    {
        var newBeams = new List<Beam>();
        foreach (var beam in beams)
        {
            var currentChar = lines[beam.Y][beam.X + 1];
            if (currentChar == '^')
            {
                res++;
                Add(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y - 1
                });
                Add(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y + 1
                });
            }
            else
                Add(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y
                });
        }
        beams = newBeams;
    }

    Console.WriteLine(res);
}

public static class Extensions
{
    public static List<List<T>> Transpose<T>(this List<List<T>> numbers) =>
        Enumerable.Range(0, numbers.First().Count).Select(j =>
            Enumerable.Range(0, numbers.Count).Select(i => numbers[i][j]).ToList()
        ).ToList();

    public static List<List<T>> GroupWithDelimiter<T>(this IEnumerable<T> numbers, Func<T, bool> isDelimiter)
    {
        var result = new List<List<T>>();

        while (numbers.Any())
        {
            var grp = numbers.TakeWhile(x => !isDelimiter(x)).ToList();
            result.Add(grp);
            numbers = numbers.Skip(grp.Count + 1).ToList();
        }

        return result;
    }
}

class Beam
{
    public int X;
    public int Y;
    public long Count;
}