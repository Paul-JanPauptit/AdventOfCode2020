using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day6
{
  class Program
  {
    static void Main()
    {
      var text = File.ReadAllText("input.txt");
      var groupsLines = Regex.Split(text, "\r\n\r\n").ToList();

      var numYesses = CountYesses(groupsLines, (s, c) =>
      {
        if (s == null)
          s = new HashSet<char>();
        s.UnionWith(c);
        return s;
      });

      Console.WriteLine($"Part 1: {numYesses}");

      numYesses = CountYesses(groupsLines, (s, c) =>
      {
        if (s == null)
          s = new HashSet<char>(c);
        else
          s.IntersectWith(c);
        return s;
      });
      Console.WriteLine($"Part 2: {numYesses}");

    }

    private static int CountYesses(List<string> groupsLines, Func<HashSet<char>, IEnumerable<char>, HashSet<char>> combineFunc)
    {
      var yesses = 0;
      foreach (var groupLines in groupsLines)
      {
        HashSet<char> groupYesses = null;
        var lines = groupLines.Split("\r\n");
        foreach (var line in lines)
          groupYesses = combineFunc(groupYesses, line);
        yesses += groupYesses?.Count ?? 0;
      }

      return yesses;
    }
  }
}
