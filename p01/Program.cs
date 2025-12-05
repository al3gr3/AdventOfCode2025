var lines = File.ReadAllLines("TextFile1.txt");

SolveA();
SolveB();

void SolveA()
{
    var cur = 50;
    var res = 0;
    foreach (var line in lines)
    {
        cur += (line.StartsWith('L') ? -1 : 1) * int.Parse(line[1..]);

        if (cur % 100 == 0)
            res++;
    }
    Console.WriteLine(res);
}

void SolveB()
{
    var cur = 50;
    var res = 0;
    foreach (var line in lines)
    {
        var dir = line.StartsWith('L') ? -1 : 1;
        Enumerable.Range(1, int.Parse(line[1..])).ToList().ForEach(_ =>
        {
            cur += dir;
            if (cur == 100)
                cur = 0;

            if (cur < 0)
                cur = 99;

            if (cur == 0)
                res++;
        });
    }
    Console.WriteLine(res);
}