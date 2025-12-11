using System.Numerics;

var devices = File.ReadAllLines("TextFile1.txt").Select(Device.Parse).ToList();
var allTos = devices.SelectMany(x => x.Devices).ToList();
foreach(var to in allTos)
    if (!devices.Any(x => x.Name == to))
        devices.Add(new Device { Name = to });

Console.WriteLine(SolveA("you", "out"));
Console.WriteLine(SolveA("svr", "fft")); // 68994
Console.WriteLine(SolveA("fft", "dac")); // 271728822
Console.WriteLine(SolveA("dac", "fft")); // 0
Console.WriteLine(SolveA("dac", "out")); // 15131
Console.WriteLine(SolveA("out", "fft")); // 0
Console.WriteLine(SolveA("out", "dac")); // 0
//Console.WriteLine(SolveA("svr", "out"));
//Console.WriteLine(long.MaxValue);

Console.WriteLine(SolveA("fft", "svr")); // 0
Console.WriteLine(SolveA("dac", "svr")); // 0

var b = SolveA("svr", "fft") * SolveA("fft", "dac") * SolveA("dac", "out");
// 283670818419223908 too high
Console.WriteLine(b);

BigInteger SolveA(string from, string to)
{
    var wave = new[] { devices.First(x => x.Name == from) }.ToList();
    wave.First().Count = 1;

    while (wave.Any())
    {
        var newWave = new List<Device>();

        foreach (var d in wave)
        {
            foreach (var n in d.Devices)
            {
                var existing = newWave.FirstOrDefault(x => x.Name == n);
                if (existing == null)
                {
                    existing = devices.First(x => x.Name == n);
                    newWave.Add(existing);
                }
                if (existing.Count + d.Count < 0)
                    throw new ArithmeticException();
                //if (existing.Count + d.Count > long.MaxValue)
                //    Console.WriteLine();
                existing.Count += d.Count;
            }
        }
        wave = newWave;
    }

    var result = devices.First(x => x.Name == to).Count;
    foreach (var d in devices)
        d.Count = 0;
    return result;
}

BigInteger SolveADFS(string from, string to)
{
    var start = devices.First(x => x.Name == from);
    start.Count = 1;

    R(start, to);

    var result = devices.First(x => x.Name == to).Count;
    foreach (var d in devices)
        d.Count = 0;
    return result;
}

void R(Device start, string to)
{
    if (start.Name == to)
        return;

    foreach (var next in start.Devices)
    {
        var n = devices.First(x => x.Name == next);
        n.Count += start.Count;
        R(n, to);
    }
}

class Device
{
    public string Name;
    public List<string> Devices = new List<string>();

    public BigInteger Count = 0;

    public static Device Parse(string s)
    {
        var splits = s.Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
        return new Device
        {
            Name = splits[0],
            Devices = splits.Skip(1).ToList()
        };
    }
}