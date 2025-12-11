using System.Numerics;

var devices = File.ReadAllLines("TextFile1.txt").Select(Device.Parse).ToList();
devices.Add(new Device { Name = "out" });

Console.WriteLine(SolveA("you", "out"));
Console.WriteLine(SolveA("svr", "fft")); // 68994
Console.WriteLine(SolveA("fft", "dac")); // 271728822
Console.WriteLine(SolveA("dac", "fft")); // 0
Console.WriteLine(SolveA("dac", "out")); // 15131
Console.WriteLine(SolveA("out", "fft")); // 0
Console.WriteLine(SolveA("out", "dac")); // 0
Console.WriteLine(SolveA("svr", "out"));
Console.WriteLine(long.MaxValue);

var b = SolveA("svr", "fft") * SolveA("fft", "dac") * SolveA("dac", "out");
// 283670818419223908 too high
Console.WriteLine(b);

void SolveB()
{
    var wave = new[] { devices.First(x => x.Name == "svr") }.ToList();
    wave.First().Count = 1;
    wave.First().From.Add(new List<string>());

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
                existing.Count += d.Count;
                foreach(var from in d.From)
                {
                    var copy = from.ToList();
                    copy.Add(d.Name);
                    existing.From.Add(copy);
                }
            }
        }
        wave = newWave;
    }

    Console.WriteLine(devices.First(x => x.Name == "out").From.Count(x => x.Contains("fft") && x.Contains("dac")));
}

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

class Device
{
    public string Name;
    public List<string> Devices = new List<string>();
    public List<List<string>> From = new List<List<string>>();

    public BigInteger Count;

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