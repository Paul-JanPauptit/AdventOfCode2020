using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
  class Program
  {
    static void Main()
    {
      Console.WriteLine("Hello World!");

      var field = File.ReadAllLines("input.txt").ToList();

      long result = CountTreesEncountered(3, 1, field);
      Console.WriteLine($"Part 1: {result}");

      result = CountTreesEncountered(1, 1, field);
      result *= CountTreesEncountered(3, 1, field);
      result *= CountTreesEncountered(5, 1, field);
      result *= CountTreesEncountered(7, 1, field);
      result *= CountTreesEncountered(1, 2, field);
      Console.WriteLine($"Part 2: {result}");
    }

    public static int CountTreesEncountered(int slopeX, int slopeY, List<string> field)
    {
      var startX = 0;
      var startY = 0;
      var numTrees = 0;

      Token token;
      do
      {
        startX += slopeX;
        startY += slopeY;

        token = GetTokenAtPosition(startX, startY, field);
        if (token == Token.Tree)
          numTrees++;
      } while (token != Token.OutOfBounds);

      return numTrees;
    }

    private static Token GetTokenAtPosition(int x, int y, List<string> field)
    {
      if (y < 0 || y >= field.Count)
        return Token.OutOfBounds;

      var row = field[y];
      var character = row[x % row.Length];
      if (character == '#')
        return Token.Tree;
      return Token.Open;
    }


    private enum Token
    {
      Open,
      Tree,
      OutOfBounds
    };
  }
}
