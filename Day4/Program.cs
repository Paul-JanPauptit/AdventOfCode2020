using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
  class Program
  {
    private static readonly List<PassportRequirement> _passportRequirements = new List<PassportRequirement>
    {
      new PassportRequirement { Name = "byr", RegEx = @"\d{4}", MinValue = 1920, MaxValue = 2002 },
      new PassportRequirement { Name = "iyr", RegEx = @"\d{4}", MinValue = 2010, MaxValue = 2020 },
      new PassportRequirement { Name = "eyr", RegEx = @"\d{4}", MinValue = 2020, MaxValue = 2030 },
      new PassportRequirement { Name = "hgt", RegEx = @"(\d+)(cm|in)", ValidateMatchDelegate = ValidateHGT },
      new PassportRequirement { Name = "hcl", RegEx = @"#[0-9|a-f]{6}" },
      new PassportRequirement { Name = "ecl", RegEx = "amb|blu|brn|gry|grn|hzl|oth" },
      new PassportRequirement { Name = "pid", RegEx = @"^\d{9}$" },
    };

    private static bool ValidateHGT(Match match)
    {
      if (match.Groups.Count != 3)
        return false;
      var value = Convert.ToInt32(match.Groups[1].Value);
      var unit = match.Groups[2].Value;

      if (unit == "cm")
        return value >= 150 && value <= 193;
      if (unit == "in")
        return value >= 59 && value <= 76;
      throw new Exception($"Unexpected unit {unit}");
    }

    private static int CountValidPassports(string[] passportTexts, IsValidPassportElementDelegate isValidPassportElementDelegate)
    {
      var numValid = 0;
      foreach (var passportText in passportTexts)
      {
        var validElements = _passportRequirements.ToDictionary(p => p.Name, p => false, StringComparer.OrdinalIgnoreCase);

        var regex = new Regex(@"(\S*):(\S*)");
        foreach (Match match in regex.Matches(passportText))
        {
          var element = match.Groups[1].Value;
          var value = match.Groups[2].Value;
          if (validElements.ContainsKey(element))
          {
            validElements[element] = isValidPassportElementDelegate(element, value);

            // Debugging elements 
            //if (element == "pid")
            //{
            //    Console.WriteLine($"{(validElements[element] ? "+" : "-")} {element}:{value}");
            //}
          }
        }

        if (validElements.All(e => e.Value))
          numValid++;
      }

      return numValid;
    }

    private static bool IsValidPassportElement(string name, string value)
    {
      var requirement = _passportRequirements.Single(pr => pr.Name == name);
      var match = Regex.Match(value, requirement.RegEx);
      if (requirement.ValidateMatchDelegate != null)
        return requirement.ValidateMatchDelegate(match);
      if (!match.Success)
        return false;
      if (requirement.MinValue != null && requirement.MinValue > Convert.ToInt32(value))
        return false;
      if (requirement.MaxValue != null && requirement.MaxValue < Convert.ToInt32(value))
        return false;
      return true;
    }

    static void Main()
    {
      var text = File.ReadAllText("input.txt");
      var passportTexts = Regex.Split(text, "\r\n\r\n");
      
      var numValid = CountValidPassports(passportTexts, (name, value) => true);
      Console.WriteLine($"Part 1: {numValid} valid passports");

      numValid = CountValidPassports(passportTexts, IsValidPassportElement);
      Console.WriteLine($"Part 2: {numValid} valid passports");
    }
  }

  public delegate bool ValidateMatchDelegate(Match m);
  public delegate bool IsValidPassportElementDelegate(string name, string value);

  public class PassportRequirement
  {
    public string Name { get; set; }
    public string RegEx { get; set; }
    public int? MinValue { get; set; }
    public int? MaxValue { get; set; }
    public ValidateMatchDelegate ValidateMatchDelegate { get; set; }
  }
}
