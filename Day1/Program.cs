using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("Input.txt").Select(int.Parse).ToList();
      var value = FindProductOf2020Parts(lines);

      Console.WriteLine(value);
    }

    private static int FindProductOf2020Parts(IReadOnlyList<int> lines)
    {
      for (var index1 = 0; index1 < lines.Count; index1++)
      {
        var value1 = lines[index1];
        for (var index2 = 0; index2 < lines.Count; index2++)
        {
          if (index2 == index1)
            continue;

          var value2 = lines[index2];
          if (value1 + value2 == 2020)
            return value1 * value2;
        }
      }

      return -1;
    }
  }
}
