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
      var lines = File.ReadAllLines("Input.txt").Select(int.Parse).ToList();
      var differences = CountDifferences(lines);
      var value = differences[1] * differences[3];
      Console.Write($"Part 1 - {value}");
    }

    public static Dictionary<int, int> CountDifferences(List<int> lines)
    {
      // Look mum, no loops. Also probably no teeth if we did this in production code, but this isn't production code \o/
      var differences = new int[3];
      lines.Add(0); // Charging Outlet
      lines = lines.OrderBy(i => i).ToList();
      lines.Add(lines.Last() + 3); // Built-in Adapter
      var deltas = lines.Zip(lines.Skip(1), (current, next) => next - current);
      return deltas.GroupBy(d => d).OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Count());
    }
  }
}
