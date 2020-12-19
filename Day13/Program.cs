using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();
      var timestamp = Convert.ToInt64(lines[0]);
      var busInfos = ReadBusInfos(lines[1]);
      var earliestBusNumber = -1;
      var earliestTimestamp = -1;
      var magicValue = -1L;
      foreach (var busInfo in busInfos)
      {
        var busNumber = busInfo.Number;
        var nextBusTimestamp = busNumber * (int) Math.Ceiling((float) timestamp / busNumber);
        if (earliestBusNumber == -1 || nextBusTimestamp < earliestTimestamp)
        {
          earliestBusNumber = busNumber;
          earliestTimestamp = nextBusTimestamp;
          magicValue = busNumber * (earliestTimestamp - timestamp);
        }
      }

      Console.WriteLine($"Part 1 - Arrived at {timestamp}, earliest bus is {earliestBusNumber} at {earliestTimestamp}, value: {magicValue}");

      timestamp = 0;
      var minBusNumber = busInfos.First().Number;
      var remainingBuses = busInfos.Skip(1).ToList();
      while (true)
      {
        var found = true;
        foreach (var bus in remainingBuses)
        {
          if ((timestamp + bus.Offset) % bus.Number != 0)
          {
            found = false;
            break;
          }
        }

        if (found)
          break;

        timestamp += minBusNumber;
      }

      Console.WriteLine($"Part 2 - Earliest timestamp: {timestamp}");
    }

    public static List<BusInfo> ReadBusInfos(string line)
    {
      var buses = new List<BusInfo>();
      var offset = 0;
      foreach (var value in line.Split(","))
      {
        if (int.TryParse(value, out var busNumber))
        {
          buses.Add(new BusInfo
          {
            Number = busNumber,
            Offset =  offset
          });
        }

        offset++;
      }

      return buses;
    }

    public class BusInfo
    {
      public int Number { get; set; }
      public int Offset { get; set; }
    }
  }
}
