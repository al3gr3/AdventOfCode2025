var boxes = File.ReadAllLines("TextFile1.txt").Select(Point.Parse).ToList();

var connections = GetAllConnections().OrderBy(c => c.Length()).ToList();

var circuits = boxes.Select(x => new[] { x }.ToList()).ToList();
var count = 0;
foreach (var nextC in connections)
{
    var circuitFrom = circuits.First(c => c.Any(p => p.IsEqual(nextC.From)));
    var circuitTo = circuits.First(c => c.Any(p => p.IsEqual(nextC.To)));
    if (circuitFrom != circuitTo)
    {
        circuitFrom.AddRange(circuitTo);
        circuits.Remove(circuitTo);
    }

    if (++count == 1000)
    {
        var res = circuits.Select(circuit => circuit.Count).OrderByDescending(x => x).Take(3).Aggregate(1L, (a, b) => a * b);
        Console.WriteLine(res);
    }

    if (circuits.Count == 1)
    {
        Console.WriteLine(nextC.From.X * nextC.To.X);
        break;
    }
}

IEnumerable<Connection> GetAllConnections()
{
    for (int i = 0; i < boxes.Count; i++)
        for (int j = i + 1; j < boxes.Count; j++)
            yield return new Connection
            {
                From = boxes[i],
                To = boxes[j],
            };
}

public class Connection
{
    public Point From;
    public Point To;

    public bool IsEqual(Connection other) =>
        (From.IsEqual(other.From) && To.IsEqual(other.To)) ||
        (From.IsEqual(other.To) && To.IsEqual(other.From));

    public long Length() => From.DistanceTo(To);
}

public class Point
{
    public long X;
    public long Y;
    public long Z;

    public static Point Parse(string s)
    {
        var parts = s.Split(',');
        return new Point
        {
            X = long.Parse(parts[0]),
            Y = long.Parse(parts[1]),
            Z = long.Parse(parts[2]),
        };
    }

    public long DistanceTo(Point other) =>
        (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y) + (Z - other.Z) * (Z - other.Z);

    public bool IsEqual(Point other) =>
        X == other.X && Y == other.Y && Z == other.Z;
}