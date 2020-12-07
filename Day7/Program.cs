using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadLines("input.txt").ToList();
      var bags = ParseBags(lines);
      var numHoldingBags = CountBagsHolding(bags, "shiny gold");

      Console.WriteLine($"Part 1 - {numHoldingBags}");

    }

    private static List<Bag> ParseBags(List<string> lines)
    {
      var bags = new List<Bag>();
      foreach (var line in lines)
        bags.Add(ParseBag(line));
      return bags;
    } 

    private static Bag ParseBag(string line)
    {
      const string containMarker = " bags contain ";
      const string separatorMarker = " bag";

      var bag = new Bag();

      var index = line.IndexOf(containMarker, StringComparison.Ordinal);
      bag.Color = line.Substring(0, index);

      index += containMarker.Length;
      var done = line.Substring(index) == "no other bags.";

      while (!done)
      {
        while (char.IsDigit(line[index]))
          index++;
        index++;
        var nextIndex = line.IndexOf( separatorMarker, index, StringComparison.Ordinal);
        bag.Contents.Add(line.Substring(index, nextIndex - index));
        index = nextIndex + separatorMarker.Length;
        if (line[index] == 's')
          index++;
        done = line[index] == '.';
        index += 2;
      }

      return bag;
    }

    private static int CountBagsHolding(List<Bag> bags, string color)
    {
      var count = 0;
      foreach (var bag in bags)
      {
        if (ContentsContainColor(bag.Contents, color, bags))
          count++;
      }
      return count;
    }

    private static bool ContentsContainColor(List<string> contents, string color, List<Bag> bags)
    {
      if (contents.Contains(color))
        return true;

      foreach (var contentColor in contents)
      {
        // Look inside nested bags
        var bag = bags.Single(b => b.Color == contentColor);
        if (ContentsContainColor(bag.Contents, color, bags))
          return true;
      }

      return false;
    }

    private class Bag
    {
      public string Color { get; set; }
      public List<string> Contents { get; set; } = new List<string>();
    }
  }
}
