using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines("input.txt").ToList();

      var pockedDimension = PocketDimension.FromLines(lines);
      for (var i = 0; i < 6; i++)
        pockedDimension = pockedDimension.CalculateNextCycle();
      Console.WriteLine($"Part 1 - {pockedDimension.NumActive}");
    }

    public class PocketDimension
    {
      public CubeType[,,] Cubes { get; set; }
      
      public int SizeX { get; }
      public int SizeY { get; }
      public int SizeZ { get; }

      public int NumActive => Cubes.Cast<CubeType>().Count(c => c == CubeType.Active);

      public PocketDimension(int sizeX, int sizeY, int sizeZ)
      {
        Cubes = new CubeType[sizeX, sizeY, sizeZ];
        SizeX = sizeX;
        SizeY = sizeY;
        SizeZ = sizeZ;
      }

      public static PocketDimension FromLines(List<string> lines)
      {
        var dimension = new PocketDimension(lines.First().Length, lines.Count, 1);
        for (var y = 0; y < lines.Count; y++)
        {
          var line = lines[y];
          for (var x = 0; x < line.Length; x++)
          {
            var cube = line[x] == '#' ? CubeType.Active : CubeType.Inactive;
            dimension.Cubes[x, y, 0] = cube;
          }
        }

        return dimension;
      }

      public PocketDimension CalculateNextCycle()
      {
        var dimension = new PocketDimension(SizeX + 2, SizeY + 2, SizeZ + 2);
        for (var z = 0; z < dimension.SizeZ; z++)
        {
          for (var y = 0; y < dimension.SizeY; y++)
          {
            for (var x = 0; x < dimension.SizeX; x++)
            {
              var (numInactive, numActive) = GetNeighboursState(x - 1, y - 1, z - 1);
              var cube = GetSafeValue(x - 1, y - 1, z - 1);

              if (cube == CubeType.Active)
                cube = numActive == 2 || numActive == 3 ? CubeType.Active : CubeType.Inactive;
              else
                cube = numActive == 3 ? CubeType.Active : CubeType.Inactive;
              dimension.Cubes[x, y, z] = cube;
            }
          }
        }

        return dimension;
      }

      private CubeType GetSafeValue(int x, int y, int z)
      {
        if (x < 0 || y < 0 || z < 0 ||
            x >= SizeX || y >= SizeY || z >= SizeZ)
          return CubeType.Inactive;
        return Cubes[x, y, z];
      }

      private (int, int) GetNeighboursState(int x, int y, int z)
      {
        var numInactive = 0;
        var numActive = 0;
        for (var xIndex = x - 1; xIndex <= x + 1; xIndex++)
        {
          for (var yIndex = y - 1; yIndex <= y + 1; yIndex++)
          {
            for (var zIndex = z - 1; zIndex <= z + 1; zIndex++)
            {
              if (xIndex == x && yIndex == y && zIndex == z)
                continue;
              var value = GetSafeValue(xIndex, yIndex, zIndex);
              if (value == CubeType.Active)
                numActive++;
              else
                numInactive++;
            }
          }
        }

        return (numInactive, numActive);
      }
    }

    public enum CubeType
    {
      Inactive,
      Active
    }
  }
}
