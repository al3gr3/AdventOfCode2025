var reds = File.ReadAllLines("TextFile1.txt").Select(Point.Parse).ToList();

var rectangles = GetAllRectangles().OrderByDescending(x => x.Area()).ToList();

Console.WriteLine(rectangles.First().Area());

reds.Add(reds.First());
var sides = reds.Zip(reds.Skip(1)).Select(z => new Rectange { A = z.First, B = z.Second }).ToList();

// todo: also have to raycast and make sure the inside of the rect is green,
// otherwise I give incorrect result 32 for this input:
/*
7,1
11,1
11,8
9,8
9,5
2,5
2,3
7,3
*/
Console.WriteLine(rectangles.First(rect => !sides.Any(side => rect.Intersects(side))).Area());

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

    public bool Contains(Point p) => 
        Math.Min(A.X, B.X) < p.X && p.X < Math.Max(A.X, B.X) &&
        Math.Min(A.Y, B.Y) < p.Y && p.Y < Math.Max(A.Y, B.Y);

    internal bool Intersects(Rectange side)
    {
        if (this.Contains(side.A) || this.Contains(side.B))
            return true;

        if (side.A.X == side.B.X)
        {
            // side is vertical
            return Math.Min(side.A.Y, side.B.Y) <= this.A.Y && this.A.Y <= Math.Max(side.A.Y, side.B.Y)
                && Math.Min(this.A.X, this.B.X) < side.A.X && side.A.X < Math.Max(this.A.X, this.B.X);         
        }
        else
        {
            // side is horizontal
            return Math.Min(side.A.X, side.B.X) <= this.A.X && this.A.X <= Math.Max(side.A.X, side.B.X)
                && Math.Min(this.A.Y, this.B.Y) < side.A.Y && side.A.Y < Math.Max(this.A.Y, this.B.Y);
        }
    }
}

public class Point
{
    public long X, Y;

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