using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day11
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("Input.txt").ToList();
      var seatMap = ReadSeatMap(lines);

      var round = 0;
      var hasChanged = true;
      while (hasChanged)
      {
        seatMap = ProcessRound1(seatMap, out hasChanged);
        round++;
      }

      Console.WriteLine($"Part 1: {seatMap.NumOccupied}");

      seatMap = ReadSeatMap(lines);
      round = 0;
      hasChanged = true;
      while (hasChanged)
      {
        Console.WriteLine(($"Round {round}:\r\n----------------"));
        seatMap.Dump();
        Console.WriteLine();
        seatMap = ProcessRound2(seatMap, out hasChanged);
        round++;
      }

      Console.WriteLine($"Part 2: {seatMap.NumOccupied}");
    }

    private static SeatMap ReadSeatMap(List<string> lines)
    {
      var seatMap = new SeatMap(lines.First().Length, lines.Count);
      for (var row = 0; row < lines.Count; row++)
      {
        var line = lines[row];
        for (var col = 0; col < line.Length; col++)
        {
          MapSlot slot;
          var character = line[col];
          if (character == '#')
            slot = MapSlot.Occupied;
          else if (character == 'L')
            slot = MapSlot.Empty;
          else
            slot = MapSlot.Floor;
          seatMap.Slots[row][col]  = slot;
        }
      }

      return seatMap;
    }

    private static SeatMap ProcessRound1(SeatMap seatMap, out bool hasChanged)
    {
      var result = new SeatMap(seatMap.NumCols, seatMap.NumRows);
      hasChanged = false;

      for (var row = 0; row < seatMap.NumRows; row++)
      {
        for (var col = 0; col < seatMap.NumCols; col++)
        {
          var slot = seatMap.Slots[row][col];
          var adjacentSlots = seatMap.GetAdjacentSlots(row, col);
          if (slot == MapSlot.Empty && adjacentSlots.All(s => s != MapSlot.Occupied))
          {
            slot = MapSlot.Occupied;
            hasChanged = true;
          }
          else if (slot == MapSlot.Occupied && adjacentSlots.Count(s => s == MapSlot.Occupied) >= 4)
          {
            slot = MapSlot.Empty;
            hasChanged = true;
          }

          result.Slots[row][col] = slot;
        }
      }

      return result;
    }

    private static SeatMap ProcessRound2(SeatMap seatMap, out bool hasChanged)
    {
      var result = new SeatMap(seatMap.NumCols, seatMap.NumRows);
      hasChanged = false;

      for (var row = 0; row < seatMap.NumRows; row++)
      {
        for (var col = 0; col < seatMap.NumCols; col++)
        {
          var slot = seatMap.Slots[row][col];
          var visibleSeats = seatMap.GetVisibleSeats(row, col);
          if (slot == MapSlot.Empty && visibleSeats.All(s => s != MapSlot.Occupied))
          {
            slot = MapSlot.Occupied;
            hasChanged = true;
          }
          else if (slot == MapSlot.Occupied && visibleSeats.Count(s => s == MapSlot.Occupied) >= 5)
          {
            slot = MapSlot.Empty;
            hasChanged = true;
          }

          result.Slots[row][col] = slot;
        }
      }

      return result;
    }


    class SeatMap
    {
      public SeatMap(int cols, int rows)
      {
        NumCols = cols;
        NumRows = rows;
        Slots = new MapSlot[rows][];
        for (var i = 0; i < rows; i++)
          Slots[i] = new MapSlot[cols];
      }

      public MapSlot[][] Slots { get; }
      public int NumCols { get; }
      public int NumRows { get; }

      public int NumOccupied => Slots.Sum(r => r.Count(s => s == MapSlot.Occupied));

      public List<MapSlot> GetAdjacentSlots(int row, int col)
      {
        var slots = new List<MapSlot>();
        for (var y = Math.Max(row - 1, 0); y <= Math.Min(row + 1, NumRows - 1); y++)
        {
          for (var x = Math.Max(col - 1, 0); x <= Math.Min(col + 1, NumCols - 1); x++)
          {
            if (y == row && x == col)
              continue;
            slots.Add(Slots[y][x]);
          }
        }

        return slots;
      }

      public List<MapSlot> GetVisibleSeats(int row, int col)
      {
        var seats = new List<MapSlot>();
        var directions = new List<Direction>
        {
          new Direction { X = -1, Y = -1 },
          new Direction { X =  0, Y = -1 },
          new Direction { X = +1, Y = -1 },
          new Direction { X = -1, Y =  0 },
          new Direction { X = +1, Y =  0 },
          new Direction { X = -1, Y = +1 },
          new Direction { X =  0, Y = +1 },
          new Direction { X = +1, Y = +1 },
        };
        foreach (var direction in directions)
        {
          var y = row;
          var x = col;
          MapSlot seat;
          do
          {
            x += direction.X;
            y += direction.Y;
            if (y < 0 || y >= NumRows || x < 0 || x >= NumCols)
              seat = MapSlot.OutOfBounds;
            else
              seat = Slots[y][x];
          } while (seat == MapSlot.Floor);

          if (seat == MapSlot.Empty || seat == MapSlot.Occupied)
            seats.Add(seat);
        }
        
        return seats;
      }

      public void Dump()
      {
        foreach (var row in Slots)
        {
          var sb = new StringBuilder();
          foreach (var slot in row)
          {
            if (slot == MapSlot.Occupied)
              sb.Append("#");
            else if (slot == MapSlot.Empty)
              sb.Append("L");
            else
              sb.Append(".");
          }
          Console.WriteLine(sb.ToString());
        }
      }
    }

    enum MapSlot
    {
      Floor,
      Empty,
      Occupied,
      OutOfBounds
    }

    public class Direction
    {
      public int X;
      public int Y;

    }
  }

}
