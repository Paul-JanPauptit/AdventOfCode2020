using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
  class Program
  {
    static void Main()
    {
      var text = File.ReadAllText("input.txt");
      var passportTexts = Regex.Split(text, "\r\n\r\n");
      var numValid = 0;
      foreach (var passportText in passportTexts)
      {
        var foundElements = new Dictionary<string, bool>
        {
          {"byr", false},
          {"iyr", false},
          {"eyr", false},
          {"hgt", false},
          {"hcl", false},
          {"ecl", false},
          {"pid", false}
          //"cid" // Don't care about no Country ID \o/ (Country ID)
        };

        // Lazy regex, because this is not code we'll have to maintain in the future :)
        var regex = new Regex(@"(\S*):");
        foreach (Match match in regex.Matches(passportText))
        {
          var element = match.Groups[1].Value;
          foundElements[element] = true;
        }

        if (foundElements.All(e => e.Value))
          numValid++;
      }

      Console.Write($"Part 1: {numValid} valid passorts");
    }
  }
}
