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

      var parts = FindContiguousRangeForSum(numbers, firstInvalidNumber);
      var weakness = parts.Min() + parts.Max();
      Console.WriteLine($"Part 2: {weakness}"); // 137394018

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

    private static List<long> FindContiguousRangeForSum(List<long> numbers, long total)
    {
      for (var startIndex = 0; startIndex < numbers.Count - 1; startIndex++)
      {
        // Sum of at least 2 parts.
        var sum = numbers[startIndex] + numbers[startIndex + 1];
        var endIndex = startIndex + 2;
        while (endIndex < numbers.Count && sum < total)
          sum += numbers[endIndex++];
        if (sum == total)
          return numbers.GetRange(startIndex, endIndex - startIndex);
      }

      throw new Exception($"No contiguous range found for sum {total}");
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
