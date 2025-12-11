var devices = File.ReadAllLines("TextFile1.txt").Select(Device.Parse).ToList();
devices.Add(new Device { Name = "out" });

var wave = new[] { devices.First(x => x.Name == "you") }.ToList();
wave.First().Count = 1;

while (wave.Any())
{
    var newWave = new List<Device>();

    foreach(var d in wave)
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
        }
    }
    wave = newWave;
}

Console.WriteLine(devices.First(x => x.Name == "out").Count);






class Device
{
    public string Name;
    public List<string> Devices = new List<string>();

    public int Count;

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