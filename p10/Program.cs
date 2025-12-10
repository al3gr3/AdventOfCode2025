

var lines = File.ReadAllLines("TextFile1.txt");

Console.WriteLine(lines.Sum(SolveA));

long SolveA(string line)
{
    var splits = line.Split(['[', ']', ' ', '(', ')'], StringSplitOptions.RemoveEmptyEntries);
    var state = splits[0];

    var switches = splits[1..(splits.Length - 1)].Select(sw => sw.Split(',').Select(int.Parse).ToArray()).ToList();

    var startState = new string(Enumerable.Range(0, state.Length).Select(i => '.').ToArray());
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
