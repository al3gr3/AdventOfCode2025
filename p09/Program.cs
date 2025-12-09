var reds = File.ReadAllLines("TextFile1.txt").Select(Point.Parse).ToList();

var rectangles = GetAllRectangles().OrderByDescending(x => x.Area()).ToList();

Console.WriteLine(rectangles.First().Area());

var greens = new List<Point>();
var previous = reds.Last();
foreach(var red in reds)
{
    if (previous.X == red.X)
    {
        for (var y = Math.Min(previous.Y, red.Y) + 1; y < Math.Max(previous.Y, red.Y); y++)
            greens.Add(new Point
            {
                X = red.X,
                Y = y,
                IsGreen = true,
            });
    }
    else
    {
        for (var x = Math.Min(previous.X, red.X) + 1; x < Math.Max(previous.X, red.X); x++)
            greens.Add(new Point
            {
                X = x,
                Y = red.Y,
                IsGreen = true,
            });
    }
    previous = red;
}

Console.WriteLine(rectangles.First(rect => !greens.Any(green => rect.Contains(green))).Area());

IEnumerable<Rectange> GetAllRectangles()
{
    for (int i = 0; i < reds.Count; i++)
        for (int j = i + 1; j < reds.Count; j++)
            yield return new Rectange
            {
                A = reds[i],
                B = reds[j],
            };
}

public class Rectange
{
    public Point A, B;

    public long Area() => (Math.Abs(A.X - B.X) + 1) * (Math.Abs(A.Y - B.Y) + 1);

    public bool Contains(Point p) => Math.Min(A.X, B.X) < p.X && p.X < Math.Max(A.X, B.X)
        && Math.Min(A.Y, B.Y) < p.Y && p.Y < Math.Max(A.Y, B.Y);
}

public class Point
{
    public long X, Y;
    public bool IsGreen;

    internal static Point Parse(string arg1)
    {
        var splits = arg1.Split(',');
        return new Point
        {
            X = int.Parse(splits[0]),
            Y = int.Parse(splits[1]),
        };
    }
}