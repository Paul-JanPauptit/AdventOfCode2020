using System;
using System.IO;

namespace Day5
{
  class Program
  {
    static void Main(string[] args)
    {
      var maxID = 0;
      var lines = File.ReadAllLines("input.txt");
      foreach (var code in lines)
      {
        maxID = Math.Max(maxID, GetSeatID(code));
      }
      
      Console.Write($"Part 1) Max. seat ID: {maxID}");
    }

    static int GetSeatID(string code)
    {
      var row = GetPartition(code.Substring(0, 7), 127);
      var seat = GetPartition(code.Substring(7), 7);

      return row * 8 + seat;
    }

    static int GetPartition(string code, int range)
    {
      var minValue = 0;
      var maxValue = range;

      foreach (var character in code)
      {
        var midPoint = (minValue + maxValue) / 2;
        if (character == 'B' || character == 'R')
          minValue = midPoint + 1;
        else
          maxValue = midPoint;
      }

      if (minValue < maxValue)
        throw new Exception("Partition failed");
      return minValue;
    }
  }
}
