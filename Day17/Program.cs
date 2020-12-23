using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();

      var pockedDimension = PocketDimension3.FromLines(lines);
      for (var i = 0; i < 6; i++)
        pockedDimension = pockedDimension.CalculateNextCycle();
      Console.WriteLine($"Part 1 - {pockedDimension.NumActive}");

      pockedDimension = PocketDimension4.FromLines(lines);
      for (var i = 0; i < 6; i++)
        pockedDimension = pockedDimension.CalculateNextCycle();
      Console.WriteLine($"Part 2 - {pockedDimension.NumActive}");
    }

    public interface IPocketDimension
    {
      IPocketDimension CalculateNextCycle();
      int NumActive { get; }
    }

    public class PocketDimension3: IPocketDimension
    {
      public CubeType[,,] Cubes { get; set; }
      
      public int SizeX { get; }
      public int SizeY { get; }
      public int SizeZ { get; }

      public int NumActive => Cubes.Cast<CubeType>().Count(c => c == CubeType.Active);

      private PocketDimension3(int sizeX, int sizeY, int sizeZ)
      {
        Cubes = new CubeType[sizeX, sizeY, sizeZ];
        SizeX = sizeX;
        SizeY = sizeY;
        SizeZ = sizeZ;
      }

      public static IPocketDimension FromLines(List<string> lines)
      {
        var dimension = new PocketDimension3(lines.First().Length, lines.Count, 1);
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

      public IPocketDimension CalculateNextCycle()
      {
        var dimension = new PocketDimension3(SizeX + 2, SizeY + 2, SizeZ + 2);
        for (var z = 0; z < dimension.SizeZ; z++)
        {
          for (var y = 0; y < dimension.SizeY; y++)
          {
            for (var x = 0; x < dimension.SizeX; x++)
            {
              var numActive = GetNumActiveNeightbours(x - 1, y - 1, z - 1);
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

      private int GetNumActiveNeightbours(int x, int y, int z)
      {
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
            }
          }
        }

        return numActive;
      }
    }

    public class PocketDimension4 : IPocketDimension
    {
      public CubeType[,,,] Cubes { get; set; }

      public int SizeX { get; }
      public int SizeY { get; }
      public int SizeZ { get; }
      public int SizeW { get; }

      public int NumActive => Cubes.Cast<CubeType>().Count(c => c == CubeType.Active);

      private PocketDimension4(int sizeX, int sizeY, int sizeZ, int sizeW)
      {
        Cubes = new CubeType[sizeX, sizeY, sizeZ, sizeW];
        SizeX = sizeX;
        SizeY = sizeY;
        SizeZ = sizeZ;
        SizeW = sizeW;
      }

      public static IPocketDimension FromLines(List<string> lines)
      {
        var dimension = new PocketDimension4(lines.First().Length, lines.Count, 1, 1);
        for (var y = 0; y < lines.Count; y++)
        {
          var line = lines[y];
          for (var x = 0; x < line.Length; x++)
          {
            var cube = line[x] == '#' ? CubeType.Active : CubeType.Inactive;
            dimension.Cubes[x, y, 0, 0] = cube;
          }
        }

        return dimension;
      }

      public IPocketDimension CalculateNextCycle()
      {
        var dimension = new PocketDimension4(SizeX + 2, SizeY + 2, SizeZ + 2, SizeW + 2);
        for (var w = 0; w < dimension.SizeW; w++)
        {
          for (var z = 0; z < dimension.SizeZ; z++)
          {
            for (var y = 0; y < dimension.SizeY; y++)
            {
              for (var x = 0; x < dimension.SizeX; x++)
              {
                var numActive = GetNumActiveNeightbours(x - 1, y - 1, z - 1, w - 1);
                var cube = GetSafeValue(x - 1, y - 1, z - 1, w - 1);

                if (cube == CubeType.Active)
                  cube = numActive == 2 || numActive == 3 ? CubeType.Active : CubeType.Inactive;
                else
                  cube = numActive == 3 ? CubeType.Active : CubeType.Inactive;
                dimension.Cubes[x, y, z, w] = cube;
              }
            }
          }
        }

        return dimension;
      }

      private CubeType GetSafeValue(int x, int y, int z, int w)
      {
        if (x < 0 || y < 0 || z < 0 || w < 0 ||
            x >= SizeX || y >= SizeY || z >= SizeZ || w >= SizeW)
          return CubeType.Inactive;
        return Cubes[x, y, z, w];
      }

      private int GetNumActiveNeightbours(int x, int y, int z, int w)
      {
        var numActive = 0;
        for (var xIndex = x - 1; xIndex <= x + 1; xIndex++)
        {
          for (var yIndex = y - 1; yIndex <= y + 1; yIndex++)
          {
            for (var zIndex = z - 1; zIndex <= z + 1; zIndex++)
            {
              for (var wIndex = w - 1; wIndex <= w + 1; wIndex++)
              {
                if (xIndex == x && yIndex == y && zIndex == z && wIndex == w)
                  continue;
                var value = GetSafeValue(xIndex, yIndex, zIndex, wIndex);
                if (value == CubeType.Active)
                  numActive++;
                else
                {
                }
              }
            }
          }
        }

        return numActive;
      }
    }

    public enum CubeType
    {
      Inactive,
      Active
    }
  }
}
