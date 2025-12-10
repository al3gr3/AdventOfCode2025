var lines = File.ReadAllLines("TextFile1.txt");

Console.WriteLine(lines.Sum(SolveA));
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
    return 0;
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
