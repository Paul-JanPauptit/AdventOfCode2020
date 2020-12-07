using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadLines("input.txt").ToList();
      var bags = ParseBags(lines);
      var numHoldingBags = CountBagsHolding(bags, "shiny gold");

      Console.WriteLine($"Part 1 - {numHoldingBags}");

      // -1 because we don't want to count the outer Shiny Gold bag itself.
      var numBagsInside = CountBagsWithContents(bags, "shiny gold") - 1;
      Console.WriteLine($"Part 2 - {numBagsInside}");

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
        var bagContents = new BagContents();
        bag.Contents.Add(bagContents);

        var nextIndex = index;
        while (char.IsDigit(line[nextIndex]))
          nextIndex++;
        bagContents.Count = Convert.ToInt32(line.Substring(index, nextIndex - index));
        index = nextIndex;
        index++;
        nextIndex = line.IndexOf( separatorMarker, index, StringComparison.Ordinal);
        bagContents.Color = line.Substring(index, nextIndex - index);
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

    private static bool ContentsContainColor(List<BagContents> contents, string color, List<Bag> bags)
    {
      if (contents.Any(c => c.Color == color))
        return true;

      foreach (var bagContents in contents)
      {
        // Look inside nested bags
        var bag = bags.Single(b => b.Color == bagContents.Color);
        if (ContentsContainColor(bag.Contents, color, bags))
          return true;
      }

      return false;
    }

    private static int CountBagsWithContents(List<Bag> bags, string color)
    {
      var bag = bags.Single(b => b.Color == color);

      var count = 1;
      foreach (var bagContents in bag.Contents)
      {
        count += bagContents.Count * CountBagsWithContents(bags, bagContents.Color);
      }

      return count;
    }


    private class Bag
    {
      public string Color { get; set; }
      public List<BagContents> Contents { get; } = new List<BagContents>();
    }

    private class BagContents
    {
      public int Count { get; set; }
      public string Color { get; set; }
    }
  }
}
