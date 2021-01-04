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
      var result = lines.Sum(Evaluate);
      
      Console.WriteLine($"Part 1 - {result}");
    }

    private static long Evaluate(string expression)
    {
      var index = 0;
      return Evaluate(expression, ref index);
    }

    private static long Evaluate(string expression, ref int index)
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
          ApplyOperation(Evaluate(expression, ref index));
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

    private enum Operator { Start, Addition, Multiplication }
  }
}
