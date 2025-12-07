var lines = File.ReadAllLines("TextFile1.txt");

SolveA();
SolveB();

void SolveB()
{
    var beams = new[]
    {
        new Beam
        {
            X = Array.FindIndex(lines.First().ToArray(), x => x =='S'),
            Y = 0,
            Count = 1
        }
    }.ToList();

    var res = 0;
    for (var i = 0; i < lines.Length - 1; i++)
    {
        var newBeams = new List<Beam>();
        foreach (var beam in beams)
        {
            var currentChar = lines[beam.Y + 1][beam.X];
            if (currentChar == '^')
            {
                res++;
                AddWithCount(newBeams, new Beam
                {
                    X = beam.X - 1,
                    Y = beam.Y + 1,
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
                    X = beam.X,
                    Y = beam.Y + 1,
                    Count = beam.Count
                });
        }
        beams = newBeams;
    }

    Console.WriteLine(beams.Sum(x => x.Count));
    Console.WriteLine(res);
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
            X = Array.FindIndex(lines.First().ToArray(), x => x =='S'),
            Y = 0,
        }
    }.ToList();

    var res = 0;
    for (var i = 0; i < lines.Length - 1; i++)
    {
        var newBeams = new List<Beam>();
        foreach (var beam in beams)
        {
            var currentChar = lines[beam.Y + 1][beam.X];
            if (currentChar == '^')
            {
                res++;
                Add(newBeams, new Beam
                {
                    X = beam.X + 1,
                    Y = beam.Y + 1
                });
                Add(newBeams, new Beam
                {
                    X = beam.X - 1,
                    Y = beam.Y + 1
                });
            }
            else
                Add(newBeams, new Beam
                {
                    X = beam.X,
                    Y = beam.Y + 1,
                });
        }
        beams = newBeams;
    }

    Console.WriteLine(res);
}

class Beam
{
    public int X;
    public int Y;
    public long Count;
}