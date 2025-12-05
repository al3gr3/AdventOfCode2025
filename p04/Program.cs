var directions = new[]
{
    new Point { Y = -1, X = 0 },
    new Point { Y = 0, X = 1 },
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },

    new Point { Y = 1, X = 1 },
    new Point { Y = 1, X = -1 },
    new Point { Y = -1, X = 1 },
    new Point { Y = -1, X = -1 },
};

(bool, string[], int) Once(string[] lines)
{
    var isChanged = false;
    var result = 0;
    var newLines = new List<string>();
    for (int y = 0; y < lines.Length; y++)
    {
        var newLine = "";
        for (int x = 0; x < lines[0].Length; x++)
            if (lines[y][x] == '@')
            {
                var count = 0;
                foreach (var dir in directions)
                {
                    var pos = new Point { X = x, Y = y };
                    var test = pos.AddClone(dir);
                    if (test.X >= 0 && test.X < lines[0].Length && test.Y >= 0 && test.Y < lines.Length)
                        if (lines[test.Y][test.X] == '@')
                            count++;
                }
                if (count < 4)
                {
                    result++;
                    newLine += '.';
                    isChanged = true;
                }
                else
                    newLine += '@';
            }
            else
                newLine += lines[y][x];
        newLines.Add(newLine);
    }
    return (isChanged, newLines.ToArray(), result);
}

var lines = File.ReadAllLines("TextFile1.txt");

SolveA(lines);
SolveB(lines);

void SolveB(string[] lines)
{
    var isChanged = true;
    var result = 0;
    while (isChanged)
    {
        int add;
        (isChanged, lines, add) = Once(lines);
        result += add;
    }

    Console.WriteLine(result);
}

void SolveA(string[] lines)
{
    var (_, _, result) = Once(lines);
    Console.WriteLine(result);
}

class Point
{
    internal int X;
    internal int Y;

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this;
    }

    internal Point AddClone(Point point) => this.Clone().Add(point);

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal Point Multiply(int i)
    {
        this.X *= i;
        this.Y *= i;
        return this;
    }

    internal Point MultiplyClone(int i) => this.Clone().Multiply(i);
}
