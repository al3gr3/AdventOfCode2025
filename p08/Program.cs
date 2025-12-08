var boxes = File.ReadAllLines("TextFile1.txt").Select(Point.Parse).ToList();

var connections = GetAllConnections().OrderBy(c => c.Length()).ToList();

var circuits = boxes.Select(x => new[] { x }.ToList()).ToList();
var count = 0;
foreach (var nextC in connections)
{
    var circuitFrom = circuits.First(c => c.Contains(nextC.From));
    var circuitTo = circuits.First(c => c.Contains(nextC.To));
    if (circuitFrom != circuitTo)
    {
        circuitFrom.AddRange(circuitTo);
        circuits.Remove(circuitTo);
    }

    if (++count == 1000)
        Console.WriteLine(circuits.Select(circuit => circuit.Count).OrderByDescending(x => x).Take(3).Aggregate(1L, (a, b) => a * b));

    if (circuits.Count == 1)
    {
        Console.WriteLine(nextC.From.Coords[0] * nextC.To.Coords[0]);
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
    public Point From, To;

    public long Length() => From.Coords.Zip(To.Coords).Sum(z => (z.First - z.Second) * (z.First - z.Second));
}

public class Point
{
    public long[] Coords;

    public static Point Parse(string s) => new Point
    {
        Coords = s.Split(',').Select(long.Parse).ToArray()
    };
}