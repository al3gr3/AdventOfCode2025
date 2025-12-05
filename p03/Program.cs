var lines = File.ReadAllLines("TextFile1.txt").Select(line => line.Select(c => c - '0').ToArray()).ToList();

Console.WriteLine(lines.Sum(x => Joltage(x, 2)));
Console.WriteLine(lines.Sum(x => Joltage(x, 12)));

long Joltage(int[] numbers, int count)
{
    if (count == 0)
        return 0;
    var first = numbers[.. (numbers.Length - count + 1)].Max();
    var rest = numbers.SkipWhile(n => n != first).Skip(1).ToArray();

    var res = first * Power(10, count - 1) + Joltage(rest, count - 1);
    return res;
}

long Power(long v1, int power) => Enumerable.Range(1, power).Aggregate(1L, (current, _) => current * v1);