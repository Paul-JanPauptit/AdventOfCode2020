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
      Console.WriteLine($"Part 1: {program.State.Accumulator}");

      var finished = false;
      var patchIndex = 0;
      while (!finished)
        finished = PatchProgram(program, patchIndex++);
      Console.WriteLine($"Part 2: {program.State.Accumulator}");
    }

    private static bool ExecuteProgram(BootProgram program)
    {
      // Reset execution state
      foreach (var instruction in program.Instructions)
        instruction.IsExecuted = false;
      program.State.Accumulator = 0;

      var ip = 0;
      var instructions = program.Instructions;
      var state = program.State;
      while (ip < instructions.Count)
      {
        var instruction = instructions[ip];
        if (instruction.IsExecuted)
          return false; // we stop but didn't finish
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

      return true;
    }

    private static bool PatchProgram(BootProgram program, int patchIndex)
    {
      // Only works after executing once, we find the latest executed jmp instruction and patch to nop
      var lastInstruction = program.Instructions.AsEnumerable()?.Skip(patchIndex).FirstOrDefault(i => i.Operation == "jmp" || i.Operation == "nop");
      if (lastInstruction == null)
        throw new Exception($"Found no more instructions to patch (patched {patchIndex + 1}");
      
      // Patch and check if we finish now
      PatchOperation(lastInstruction);
      var result = ExecuteProgram(program);
      PatchOperation(lastInstruction); // Patch it back, so the next run gets a clean app

      return result;
    }

    private static void PatchOperation(Instruction instruction)
    {
      instruction.Operation = instruction.Operation == "jmp" ? "nop" : "jmp";
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
