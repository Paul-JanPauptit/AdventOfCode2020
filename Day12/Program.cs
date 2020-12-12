using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
  class Program
  {
    static void Main()
    {
      var lines = File.ReadAllLines("input.txt").ToList();
      var ship = new Ship();
      ship.ExecuteInstructions1(lines);
      Console.WriteLine($"Part 1: {ship.Distance}");

      ship = new Ship();
      ship.ExecuteInstructions2(lines);
      Console.WriteLine($"Part 2: {ship.Distance}");
    }


    public class Ship
    {
      public Direction Direction = Direction.East;
      public int X { get; set; }
      public int Y { get; set; }

      public int WaypointDeltaX { get; set; } = 10;
      public int WaypointDeltaY { get; set; } = -1;

      public int Distance => Math.Abs(X) + Math.Abs(Y);

      private static readonly List<DirectionInfo> DirectionInfos = new List<DirectionInfo>
      {
        new DirectionInfo(Direction.North, "N",  0, -1),
        new DirectionInfo(Direction.East, "E",  1,  0),
        new DirectionInfo(Direction.South, "S",  0, +1),
        new DirectionInfo(Direction.West, "W", -1,  0)
      };


      public void ExecuteInstructions1(List<string> lines)
      {
        foreach (var line in lines)
        {
          var action = line.Substring(0, 1);
          var amount = Convert.ToInt32(line.Substring(1));
          var direction = DirectionInfos.FirstOrDefault(d => d.Action == action);
          if (direction != null)
          {
            X += amount * direction.DeltaX;
            Y += amount * direction.DeltaY;
          }
          else
          {
            switch (action)
            {
              case "L":
              {
                for (var i = 0; i < amount / 90; i++)
                  Direction = Direction.Prev();
                break;
              }

              case "R":
              {
                for (var i = 0; i < amount / 90; i++)
                  Direction = Direction.Next();
                break;
              }

              case "F":
              {
                var directionInfo = DirectionInfos.First(d => d.Direction == Direction);
                X += amount * directionInfo.DeltaX;
                Y += amount * directionInfo.DeltaY;
                break;
              }

              default: 
                throw new Exception($"Unexpected action {action}");
            }
          }
        }
      }

      public void ExecuteInstructions2(List<string> lines)
      {
        foreach (var line in lines)
        {
          var action = line.Substring(0, 1);
          var amount = Convert.ToInt32(line.Substring(1));
          var direction = DirectionInfos.FirstOrDefault(d => d.Action == action);
          if (direction != null)
          {
            WaypointDeltaX += amount * direction.DeltaX;
            WaypointDeltaY += amount * direction.DeltaY;
          }
          else
          {
            switch (action)
            {
              case "L":
              {
                for (var i = 0; i < amount / 90; i++)
                {
                  var x = 1 * WaypointDeltaY;
                  var y = -1 * WaypointDeltaX;
                  WaypointDeltaX = x;
                  WaypointDeltaY = y;
                }
                break;
              }

              case "R":
              {
                for (var i = 0; i < amount / 90; i++)
                {
                  var x = -1 * WaypointDeltaY;
                  var y = 1 * WaypointDeltaX;
                  WaypointDeltaX = x;
                  WaypointDeltaY = y;
                }
                break;
              }

              case "F":
              {
                X += amount * WaypointDeltaX;
                Y += amount * WaypointDeltaY;
                break;
              }

              default:
                throw new Exception($"Unexpected action {action}");
            }
          }
        }
      }

    }

    public enum Direction
    {
      North,
      East,
      South,
      West
    };

    public class DirectionInfo
    {
      public DirectionInfo(Direction direction, string action, int deltaX, int deltaY)
      {
        Direction = direction;
        Action = action;
        DeltaX = deltaX;
        DeltaY = deltaY;
      }

      public Direction Direction { get; set; }
      public string Action { get; set; }
      public int DeltaX { get; set; }
      public int DeltaY { get; set; }
    }

  }

  public static class EnumExtensions
  {
    public static T Prev<T>(this T src) where T : struct
    {
      if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

      var values = (T[])Enum.GetValues(src.GetType());
      var index = Array.IndexOf(values, src) - 1;
      return (index == -1) ? values[^1] : values[index];
    }

    public static T Next<T>(this T src) where T : struct
    {
      if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

      var values = (T[])Enum.GetValues(src.GetType());
      var index = Array.IndexOf(values, src) + 1;
      return (index == values.Length) ? values[0] : values[index];
    }
  }
}
