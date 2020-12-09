using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
  class Program
  {
    static void Main()
    {
      var numbers = File.ReadAllLines("input.txt").Select(l => Convert.ToInt64(l)).ToList();

      var firstInvalidNumber = FindFirstInvalidNumber(numbers, 25);
      Console.WriteLine($"Part 1: {firstInvalidNumber}"); // 1038347917
    }

    private static long FindFirstInvalidNumber(List<long> numbers, int preambleSize)
    {
      for (var index = preambleSize; index < numbers.Count; index++)
      {
        var number = numbers[index];
        var preamble = numbers.GetRange(index - preambleSize, preambleSize);
        if (!IsSumOf(number, preamble))
        {
          return number;
        }
      }

      return -1;
    }

    private static bool IsSumOf(long total, List<long> numbers)
    {
      for (var i = 0; i < numbers.Count; i++)
      {
        for (var j = 0; j < numbers.Count; j++)
        {
          if (i == j)
            continue;
          if (numbers[i] + numbers[j] == total)
            return true;
        }
      }

      return false;
    }
  }
}
