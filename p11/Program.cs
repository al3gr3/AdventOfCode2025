var ds = File.ReadAllLines("TextFile1.txt").Select(Device.Parse).ToDictionary(x => x.Name, x => x);
var allTos = ds.Values.SelectMany(x => x.Devices).ToList();
foreach (var to in allTos)
    if (!ds.ContainsKey(to))
        ds[to] = new Device { Name = to };

var cache = new Dictionary<string, long>();
Console.WriteLine(SolveA("you", "out"));
Console.WriteLine(SolveA("svr", "fft"));
Console.WriteLine(SolveA("fft", "dac"));
Console.WriteLine(SolveA("dac", "out"));

Console.WriteLine(SolveA("dac", "fft"));
Console.WriteLine(SolveA("out", "fft"));
Console.WriteLine(SolveA("out", "dac"));
Console.WriteLine(SolveA("fft", "svr"));
Console.WriteLine(SolveA("dac", "svr"));

Console.WriteLine(SolveA("svr", "out"));

Console.WriteLine(SolveA("svr", "fft") * SolveA("fft", "dac") * SolveA("dac", "out"));

long SolveA(string from, string to)
{
    cache = new Dictionary<string, long>();

    return R(from, to);
}

long R(string from, string to) => from == to
    ? 1L
    : cache[from] = ds[from].Devices.Sum(next => cache.ContainsKey(next) ? cache[next] : R(next, to));

class Device
{
    public string Name;
    public List<string> Devices = new List<string>();

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