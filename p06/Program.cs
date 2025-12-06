var lines = File.ReadAllLines("TextFile1.txt");

SolveA(lines);
SolveB(lines);

void SolveB(string[] lines)
{
    var temp = lines[..(lines.Length - 1)].Select(line => line.ToList()).ToList();
    temp = Transform(temp);
    var allNumbers = temp.Select(x => (new string(x.ToArray())).Trim()).ToList();

    var actions = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

    var result = 0L;
    for (int i = 0; i < actions.Count; i++)
    {
        var numbers = allNumbers.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(long.Parse).ToList();
        allNumbers = allNumbers.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1).ToList();
        if (actions[i] == "+")
            result += numbers.Sum();
        else
            result += numbers.Aggregate(1L, (a, b) => a * b);
    }
    Console.WriteLine(result);
}

void SolveA(string[] lines)
{
    var numbers = lines[..(lines.Length - 1)].Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList()).ToList();
    numbers = Transform(numbers);
    
    var actions = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
    

    var result = 0L;
    for (int i = 0; i < actions.Count; i++)
    {
        if (actions[i] == "+")
            result += numbers[i].Sum();
        else
            result += numbers[i].Aggregate(1L, (a, b) => a * b);
    }
    Console.WriteLine(result);
}


List<List<T>> Transform<T>(List<List<T>> numbers) 
{
    var result = new List<List<T>>();

    for (int j = 0; j < numbers.First().Count; j++)
    {
        var newRow = new List<T>();
        for (int i = 0; i < numbers.Count; i++)
        {
            newRow.Add(numbers[i][j]);
        }
        result.Add(newRow);
    }
    return result;
}