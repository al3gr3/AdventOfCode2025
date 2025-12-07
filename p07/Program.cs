var lines = File.ReadAllLines("TextFile1.txt");

var beams = new[]
{
    new Beam
    {
        X = lines.First().IndexOf('S'),
        Y = 0,
        Count = 1
    }
}.ToList();

var resultA = 0;
while(beams.First().Y < lines.Length - 1)
{
    var newBeams = new List<Beam>();
    foreach (var beam in beams)
    {
        if (lines[beam.Y + 1][beam.X] == '^')
        {
            resultA++;
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

Console.WriteLine(resultA);
Console.WriteLine(beams.Sum(x => x.Count));

void AddWithCount(List<Beam> newBeams, Beam point)
{
    var existing = newBeams.FirstOrDefault(x => x.X == point.X && x.Y == point.Y);
    if (existing == null)
        newBeams.Add(point);
    else
        existing.Count += point.Count;
}

class Beam
{
    public int X;
    public int Y;
    public long Count;
}