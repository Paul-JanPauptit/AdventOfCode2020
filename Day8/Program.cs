using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();
      var program = ReadProgram(lines);

      ExecuteProgram(program);
      
      Console.Write($"Part 1: {program.State.Accumulator}");
    }

    private static void ExecuteProgram(BootProgram program)
    {
      var ip = 0;
      var instructions = program.Instructions;
      var state = program.State;
      while (ip < instructions.Count)
      {
        var instruction = instructions[ip];
        if (instruction.IsExecuted)
          return; // We are done
        instruction.IsExecuted = true;
        switch (instruction.Operation)
        {
          case "nop":
          {
            ip++;
            break;
          }

          case "acc":
          {
            state.Accumulator += instruction.Argument;
            ip++;
            break;
          }

          case "jmp":
          {
            ip += instruction.Argument;
            break;
          }

          default:
            throw new Exception($"Unpexpected instruction: {instruction.Operation}");
        }
      }
    }

    private static BootProgram ReadProgram(List<string> lines)
    {
      var program = new BootProgram();
      foreach (var line in lines)
      {
        var instruction = ReadInstruction(line);
        program.Instructions.Add(instruction);
      }
      return program;
    }

    private static Instruction ReadInstruction(string line)
    {
      var instruction = new Instruction();

      var index = line.IndexOf(" ", StringComparison.Ordinal);
      instruction.Operation = line.Substring(0, index);
      instruction.Argument = Convert.ToInt32(line.Substring(index + 1));

      return instruction;
    }


    private class BootProgram
    {
      public List<Instruction> Instructions { get; } = new List<Instruction>();
      public ProgramState State { get; } = new ProgramState();
    }

    private class ProgramState
    {
      public int Accumulator { get; set; }
    }

    private class Instruction
    {
      public string Operation { get; set; }
      public int Argument { get; set; }
      public bool IsExecuted { get; set; }
    }
  }
}
