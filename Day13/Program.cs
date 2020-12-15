using System;
using System.IO;
using System.Linq;

namespace Day13
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();
      var timestamp = Convert.ToInt32(lines[0]);
      var busNumbers = lines[1].Split(",").Where(x => x != "x").Select(x => Convert.ToInt32(x));
      var earliestBusNumber = -1;
      var earliestTimestamp = -1;
      var magicValue = -1;
      foreach (var busNumber in busNumbers)
      {
        var nextBusTimestamp = busNumber * (int) Math.Ceiling((float) timestamp / busNumber);
        if (earliestBusNumber == -1 || nextBusTimestamp < earliestTimestamp)
        {
          earliestBusNumber = busNumber;
          earliestTimestamp = nextBusTimestamp;
          magicValue = busNumber * (earliestTimestamp - timestamp);
        }
      }

      Console.Write($"Part 0 - Arrived at {timestamp}, earliest bus is {earliestBusNumber} at {earliestTimestamp}, value: {magicValue}");
    }
  }
}
