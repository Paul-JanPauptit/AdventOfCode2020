using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();
      var result = ExecuteProgram(lines);

      Console.WriteLine($"Part 0 - {result}");
    }

    private static long ExecuteProgram(List<string> lines)
    {
      var memory = new SortedDictionary<long, long>();
      var mask = new DualMask();
      foreach (var line in lines)
      {
        if (line.StartsWith("mask = "))
          mask = DualMask.FromString(line.Substring(7));
        else
        {
          var regEx = new Regex(@"mem\[(\d*)\] = (\d*)");
          var match = regEx.Match(line);
          var address = Convert.ToInt64(match.Groups[1].Value);
          var value = Convert.ToInt64(match.Groups[2].Value);
          memory[address] = mask.ApplyTo(value);
        }
      }
      return memory.Values.Sum();
    }

    private class DualMask
    {
      private long AndMask { get; set; }
      private long OrMask { get; set; }

      public static DualMask FromString(string mask)
      {
        return new DualMask
        {
          AndMask = Convert.ToInt64(mask.Replace("X", "1"), 2),
          OrMask = Convert.ToInt64(mask.Replace("X", "0"), 2)
        };
      }

      public long ApplyTo(long value)
      {
        return (value & AndMask) | OrMask;
      }
    }
  }
}
