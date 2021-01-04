using System;
using System.IO;
using System.Linq;

namespace Day18
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();

      var result = lines.Sum(Evaluate1);
      Console.WriteLine($"Part 1 - {result}");

      result = lines.Sum(Evaluate2);
      Console.WriteLine($"Part 2 - {result}");
    }

    private static long Evaluate1(string expression)
    {
      var index = 0;
      return Evaluate1(expression, ref index);
    }

    private static long Evaluate1(string expression, ref int index)
    {
      var result = 0L;
      var op = Operator.Start;
      while (index < expression.Length)
      {
        var character = expression[index++];

        if (character == '*')
          op = Operator.Multiplication;
        else if (character == '+')
          op = Operator.Addition;
        else if (int.TryParse(character.ToString(), out int value))
          ApplyOperation(value);
        else if (character == '(')
          ApplyOperation(Evaluate1(expression, ref index));
        else if (character == ')')
          return result;
      }

      return result;

      void ApplyOperation(long value)
      {
        switch (op)
        {
          case Operator.Addition:
            result += value;
            break;
          case Operator.Multiplication:
            result *= value;
            break;
          default:
            result = value;
            break;
        }
      }
    }

    private static long Evaluate2(string expression)
    {
      var index = 0;
      // We don't do proper tokenizing so remove any spaces :)
      expression = expression.Replace(" ", "");

      return ParseExpression();

      long ParseExpression()
      {
        var value = ParseHigherPresedence();
        if (index < expression.Length)
        {
          var token = expression[index];
          if (token == '*')
          {
            index++;
            value *= ParseExpression();
          }
          else if (token == ')') // Hacky unbalanced parser, oh well.
            index++;

        }
        return value;
      }

      long ParseHigherPresedence()
      {
        var value = ParseTerm();
        if (index < expression.Length)
        {
          var token = expression[index];
          if (token == '+')
          {
            index++;
            value += ParseHigherPresedence();
          }
        }
        return value;
      }

      long ParseTerm()
      {
        if (index < expression.Length)
        {
          var token = expression[index];
          if (int.TryParse(token.ToString(), out int number))
          {
            index++;
            return number;
          }

          if (token == '(')
          {
            index++;
            return ParseExpression();
          }
        }
        return 0;
      }

    }

    private enum Operator { Start, Addition, Multiplication }
  }
}
