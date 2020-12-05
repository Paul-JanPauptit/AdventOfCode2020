using System;
using System.IO;
using System.Linq;

namespace Day5
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines("input.txt");
      var seatIDs = lines.Select(GetSeatID).ToList();
      
      // 835
      Console.WriteLine($"Part 1) Max. seat ID: {seatIDs.Max()}");

      seatIDs.Sort();
      var prevID = seatIDs.First();
      foreach (var id in seatIDs.Skip(1))
      {
        if (id - 1 > prevID)
        {
          Console.WriteLine($"Part 2) Found empty seat: {id - 1}");
          break;
        }
        prevID = id;
      }

    }

    static int GetSeatID(string code)
    {
      return Convert.ToInt32(code.
        Replace('B', '1').
        Replace('F', '0').
        Replace('R', '1').
        Replace('L', '0'), 2);
    }
  }
}
