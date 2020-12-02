using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
  class Program
  {
    private delegate bool IsValidatePasswordDelegate(string password, int value1, int value2, char character);


    static void Main()
    {
      var lines = File.ReadAllLines("Input.txt").ToList();
      var value1 = CountValidPasswords(lines, IsValidPassword1);
      Console.WriteLine(value1);

      var value2 = CountValidPasswords(lines, IsValidPassword2);
      Console.WriteLine(value2);
    }

    private static int CountValidPasswords(List<string> lines, IsValidatePasswordDelegate isValidatePasswordDelegate)
    {
      var regEx = new Regex(@"(\d*)-(\d*) (.): (.*)");
      var numValid = 0;

      foreach (var line in lines)
      {
        var match = regEx.Match(line);
        if (isValidatePasswordDelegate(match.Groups[4].Value, Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value), match.Groups[3].Value[0]))
          numValid++;
      }

      return numValid;
    }

    private static bool IsValidPassword1(string password, int minOccurences, int maxOccurences, char character)
    {
      var occurences = 0;
      foreach (var c in password)
        if (c == character)
          occurences++;

      return (minOccurences <= occurences) && (maxOccurences >= occurences);
    }


    private static bool IsValidPassword2(string password, int index1, int index2, char character)
    {
      return (password[index1 - 1] == character) ^ (password[index2 - 1] == character);
    }

  }
}
