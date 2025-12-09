var reds = File.ReadAllLines("TextFile1.txt").Select(Point.Parse).ToList();

var rectangles = GetAllRectangles().OrderByDescending(x => x.Area()).ToList();

Console.WriteLine(rectangles.First().Area());

reds.Add(reds.First());
var sides = reds.Zip(reds.Skip(1)).Select(z => new Rectangle { A = z.First, B = z.Second }).ToList();

/*todo: also have to raycast and make sure the inside of the rect is green,
otherwise I give incorrect Problem B result 32 for this input:
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

IEnumerable<Rectangle> GetAllRectangles()
{
    for (int i = 0; i < reds.Count; i++)
        for (int j = i + 1; j < reds.Count; j++)
            yield return new Rectangle
            {
                A = reds[i],
                B = reds[j],
            };
}

public class Rectangle
{
    public Point A, B;

    public long Left => Math.Min(A.X, B.X);
    public long Right => Math.Max(A.X, B.X);
    public long Top => Math.Min(A.Y, B.Y);
    public long Bottom => Math.Max(A.Y, B.Y);

    public long Area() => (Math.Abs(A.X - B.X) + 1) * (Math.Abs(A.Y - B.Y) + 1);

    public bool Contains(Point p) => 
        Left < p.X && p.X < Right &&
        Top < p.Y && p.Y < Bottom;

    internal bool Intersects(Rectangle side)
    {
        if (this.Contains(side.A) || this.Contains(side.B))
            return true;

        if (side.A.X == side.B.X)
        {
            // side is vertical
            return side.Top <= this.Top && this.Bottom <= side.Bottom
                && this.Left < side.A.X && side.A.X < this.Right;
        }
        else if (side.A.Y == side.B.Y)
        {
            // side is horizontal
            return side.Left <= this.Left && this.Right <= side.Right // actually "<=" is needed only in "this.Right <= side.Right", see todo above
                && this.Top < side.A.Y && side.A.Y < this.Bottom;
        }
        else
            throw new Exception("incorrect side");
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