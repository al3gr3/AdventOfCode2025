using Microsoft.Z3;

var lines = File.ReadAllLines("TextFile1.txt");

(int number, List<int[]>) NewMethod(List<int[]> switches, List<int> joltage)
{
    //var number = joltage.Where(x => x > 0).Min();
    //var index = joltage.IndexOf(number);

    var index = Enumerable.Range(0, joltage.Count)
        .Where(i => switches.Count(x => x.Contains(i)) > 0)
        .OrderBy(i => switches.Count(x => x.Contains(i))).First();

    var minSwitches = switches.Where(x => x.Contains(index)).ToList();
    return (index, minSwitches);
}

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
    var result = R3(switches, joltage);
    Console.WriteLine(result);
    return result;
}

long R3(List<int[]> switches, int[] joltage)
{
    using var ctx = new Context();
    using var opt = ctx.MkOptimize();
    var presses = Enumerable.Range(0, switches.Count)
        .Select(i => ctx.MkIntConst($"p{i}"))
        .ToArray();

    foreach (var press in presses)
        opt.Add(ctx.MkGe(press, ctx.MkInt(0)));

    for (int i = 0; i < joltage.Length; i++)
    {
        var affecting = presses.Where((_, j) => switches[j].Contains(i)).ToArray();
        var sum = ctx.MkAdd(affecting);
        opt.Add(ctx.MkEq(sum, ctx.MkInt(joltage[i])));
    }

    opt.MkMinimize(ctx.MkAdd(presses));

    opt.Check();
    var model = opt.Model;

    return presses.Sum(p => ((IntNum)model.Evaluate(p, true)).Int64);
}

long R2(List<int[]> switches, List<int> joltage, Dictionary<string, long> cache)
{
    var key = string.Join("|", joltage.Select(x => x.ToString()));
    if (cache.ContainsKey(key))
        return cache[key];

    if (joltage.All(x => x == 0))
        return 0;
    if (!switches.Any())
        return 1000000L;

    var (index, minSwitches) = NewMethod(switches, joltage);
    var number = joltage[index];

    var combs = AllCombs(number, minSwitches.Count);

    var newJoltages = combs.Select(comb => ApplyAll(joltage, minSwitches, comb))
        .Where(newJoltage => !newJoltage.Any(x => x < 0))
        .ToList();
    var result = 1000000L;
    foreach (var newJoltage in newJoltages
        //.OrderBy(x => x.Sum())
        )
    {
        if (result < newJoltage.Max() + number)
            continue;
        var next = R2(switches.Except(minSwitches).ToList(), newJoltage, cache);
        result = Math.Min(result, next + number);
    }
    return cache[key] = result;
}

List<int> ApplyAll(List<int> joltage, List<int[]> minSwitches, List<int> comb)
{
    var newJoltage = joltage.ToList();
    comb.Zip(minSwitches).ToList().ForEach(z => newJoltage = Apply(z.Second, newJoltage, z.First * -1));
    return newJoltage;
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
        var next = AllCombs(number - i, count - 1).Select(x => x.ToList()).ToList(); // hardcopy
        foreach (var n in next)
            n.Add(i);
        result.AddRange(next);
    }
    return allcombsCache[key] = result;
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
