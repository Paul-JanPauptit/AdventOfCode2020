using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("Input.txt").ToList();
      var value = CountValidPasswords(lines);

      Console.WriteLine(value);
    }

    private static int CountValidPasswords(List<string> lines)
    {
      var regEx = new Regex(@"(\d*)-(\d*) (.): (.*)");
      var numValid = 0;

      foreach (var line in lines)
      {
        var match = regEx.Match(line);
        if (IsValidPassword(match.Groups[4].Value, Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value), match.Groups[3].Value[0]))
          numValid++;
      }

      return numValid;
    }

    private static bool IsValidPassword(string password, int minOccurences, int maxOccurences, char character)
    {
      var occurences = 0;
      foreach (var c in password)
        if (c == character)
          occurences++;

      return (minOccurences <= occurences) && (maxOccurences >= occurences);
    }
  }
}
