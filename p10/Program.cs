var lines = File.ReadAllLines("TextFile1.txt");

var allcombsCache = new Dictionary<string, List<List<int>>>();
Console.WriteLine(lines.Sum(SolveB));

(string, List<int[]>, int[]) Parse(string line)
{
    var splits = line.Split(['[', ']', ' ', '(', ')', '{', '}'], StringSplitOptions.RemoveEmptyEntries);
    var all = splits.Skip(1).Select(sw => sw.Split(',').Select(int.Parse).ToArray()).ToArray();
    var switches = all[0..(all.Length - 1)].ToList();
    var joltage = all.Last();

    return (splits[0], switches, joltage);
}

long SolveB(string line)
{
    var (_, switches, joltage) = Parse(line);
    var result = R2(switches
        //.OrderByDescending(x => x.Length).ToList()
        , joltage.ToList(), new Dictionary<string, long>());
    Console.WriteLine(result);
    return result;
}

long R2(List<int[]> switches, List<int> joltage, Dictionary<string, long> cache)
{
    var key = string.Join("|", joltage.Select(x => x.ToString()));
    if (cache.ContainsKey(key))
        return cache[key];

    if (joltage.All(x => x == 0))
        return 0;

    var min = joltage.Where(x => x > 0).Min();
    var index = joltage.IndexOf(min);
    var result = 1000000L;
    var minSwitches = switches.Where(x => x.Contains(index)).ToList();

    var combs = AllCombs(min, minSwitches.Count());
    foreach (var comb in combs)
    {
        var newJoltage = joltage.ToList();

        comb.Zip(minSwitches).ToList().ForEach(z => newJoltage = Apply(z.Second, newJoltage, z.First * -1));
        if (newJoltage.Any(x => x < 0))
            continue;
        var next = R2(switches.Except(minSwitches).ToList(), newJoltage, cache);
        result = Math.Min(result, next + min);
    }
    return cache[key] = result;
}

List<List<int>> AllCombs(int number, int count)
{
    var key = $"{number}|{count}";

    if (allcombsCache.ContainsKey(key))
        return allcombsCache[key];
    if (count == 0)
        return new List<List<int>>();
    if (count == 1)
        return new[] { new[] { number }.ToList() }.ToList();

    var result = new List<List<int>>();
    for (var i = 0; i <= number; i++)
    {
        var next = AllCombs(number - i, count - 1);
        foreach (var n in next)
            n.Add(i);
        result.AddRange(next);
    }
    //allcombsCache[key] = result;
    return result;
}

List<int> Apply(int[] sw, List<int> joltage, int diff)
{
    var result = joltage.ToList(); // hardcopy?
    foreach (var i in sw)
        result[i] += diff;
    return result;
}

long SolveA(string line)
{
    var (state, switches, _) = Parse(line);

    var startState = new string(state.Select(i => '.').ToArray());
    var result =  R(startState, switches, 0, state);
    return result;
}

long R(string startState, List<int[]> switches, int v, string state)
{
    if (startState == state)
        return 0;
    if (v >= switches.Count)
        return long.MaxValue - 1;

    return Math.Min(
        R(Xor(startState, switches[v]), switches, v + 1, state) + 1,
        R(startState, switches, v + 1, state));
}

static string Xor(string state, int[] indices)
{
    var result = state.ToArray();
    foreach (var i in indices)
    {
        if (result[i] == '#')
            result[i] = '.';
        else
            result[i] = '#';
    }
    return new string(result);
}
