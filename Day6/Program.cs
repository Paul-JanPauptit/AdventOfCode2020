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

      var numYesses = CountYesses(groupsLines);

      Console.WriteLine($"Part 1: {numYesses}");
    }

    private static int CountYesses(List<string> groupsLines)
    {
      var yesses = 0;
      foreach (var groupLines in groupsLines)
      {
        var groupYesses = new HashSet<char>();
        var lines = groupLines.Split("\r\n");
        foreach (var line in lines)
          groupYesses.UnionWith(line);
        yesses += groupYesses.Count;
      }

      return yesses;
    }
  }
}
