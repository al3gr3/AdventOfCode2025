var lines = File.ReadAllLines("TextFile1.txt");
var actions = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

Print(SolveA());
Print(SolveB());

void Print(List<List<long>> list) => Console.WriteLine(actions.Zip(list).Sum(z => AddOrMultiply(z.First, z.Second)));

List<List<long>> SolveB() => lines[..(lines.Length - 1)]
    .Select(line => line.ToList())
    .ToList().Transpose()
    .Select(x => new string(x.ToArray()).Trim())
    .GroupWithDelimiter(x => string.IsNullOrWhiteSpace(x))
    .Select(grp => grp.Select(long.Parse).ToList()).ToList();

long AddOrMultiply(string v, List<long> numbers) => v == "+"
    ? numbers.Sum()
    : numbers.Aggregate(1L, (a, b) => a * b);

List<List<long>> SolveA() => lines[..(lines.Length - 1)]
    .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList())
    .ToList().Transpose();

public static class Extensions
{
    public static List<List<T>> Transpose<T>(this List<List<T>> numbers) =>
        Enumerable.Range(0, numbers.First().Count).Select(j =>
            Enumerable.Range(0, numbers.Count).Select(i => numbers[i][j]).ToList()
        ).ToList();

    public static List<List<T>> GroupWithDelimiter<T>(this IEnumerable<T> numbers, Func<T, bool> isDelimiter)
    {
        var result = new List<List<T>>();

        while (numbers.Any())
        {
            var grp = numbers.TakeWhile(x => !isDelimiter(x)).ToList();
            result.Add(grp);
            numbers = numbers.Skip(grp.Count + 1).ToList();
        }

        return result;
    }
}