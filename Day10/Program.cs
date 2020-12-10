using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("Input.txt").Select(int.Parse).OrderBy(i => i).ToList();
      lines.Insert(0, 0); // Charging Outlet
      lines.Add(lines.Last() + 3); // Built-in Adapter
      var differences = CountDifferences(lines);
      var value = differences[1] * differences[3];
      Console.WriteLine($"Part 1 - {value}");

      var value2 = CountCombinations(lines);
      Console.WriteLine($"Part 2 - {value2}");

    }

    public static Dictionary<int, int> CountDifferences(List<int> lines)
    {
      // Look mum, no loops. Also probably no teeth if we did this in production code, but this isn't production code \o/
      var differences = new int[3];
      var deltas = lines.Zip(lines.Skip(1), (current, next) => next - current);
      return deltas.GroupBy(d => d).OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Count());
    }

    public static long CountCombinations (List<int> lines)
    {
      var solution = new Dictionary<long, long> {[0] = 1};
      foreach (var line in lines.Skip(1))
      {
        solution[line] = 0;
        if (solution.TryGetValue(line - 1, out var value))
          solution[line] += value;
        if (solution.TryGetValue(line - 2, out value))
          solution[line] += value;
        if (solution.TryGetValue(line - 3, out value))
          solution[line] += value;
      }

      return solution[lines.Last()];
    }
  }

}
