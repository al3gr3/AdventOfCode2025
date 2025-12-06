var lines = File.ReadAllLines("TextFile1.txt");
var actions = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

SolveA();
SolveB();

void SolveB()
{
    var temp = lines[..(lines.Length - 1)].Select(line => line.ToList()).ToList();
    temp = Transform(temp);
    var allNumbers = temp.Select(x => new string(x.ToArray()).Trim()).ToList();

    var result = 0L;
    for (int i = 0; i < actions.Count; i++)
    {
        var numbers = allNumbers.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse).ToList();
        allNumbers = allNumbers.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1).ToList();
        result += AddOrMultiply(actions[i], numbers);
    }
    Console.WriteLine(result);
}

long AddOrMultiply(string v, List<long> numbers) => v == "+"
    ? numbers.Sum()
    : numbers.Aggregate(1L, (a, b) => a * b);

void SolveA()
{
    var numbers = lines[..(lines.Length - 1)].Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList()).ToList();
    numbers = Transform(numbers);

    var result = actions.Zip(numbers).Sum(z => AddOrMultiply(z.First, z.Second));
    Console.WriteLine(result);
}

static List<List<T>> Transform<T>(List<List<T>> numbers) 
{
    var result = new List<List<T>>();

    for (int j = 0; j < numbers.First().Count; j++)
    {
        var newRow = new List<T>();
        for (int i = 0; i < numbers.Count; i++)
            newRow.Add(numbers[i][j]);
        result.Add(newRow);
    }
    return result;
}