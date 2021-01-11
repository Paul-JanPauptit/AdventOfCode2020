using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
  class Program
  {
    static void Main(string[] args)
    {
      var lines = File.ReadAllLines("input.txt").ToList();
      var players = ParsePlayers(lines);
      PlayGame(players);

      var winningScore = players.Max(p => p.Score);
      Console.WriteLine($"Part 1: {winningScore}");
    }

    private static List<Player> ParsePlayers(List<string> lines)
    {
      var players = new List<Player>();

      var player = new Player();
      players.Add(player);

      foreach (var line in lines)
      {
        if (string.IsNullOrEmpty(line))
          player = null;
        else if (int.TryParse(line, out var value))
        {
          if (player == null)
          {
            player = new Player();
            players.Add(player);
          }
          player.Cards.Add(value);
        }
      }

      return players;
    }

    private static void PlayGame(List<Player> players)
    {
      var roundIndex = 1;
      while (players.All(p => p.Cards.Any()))
      {
        var orderedPlayers = players.OrderByDescending(p => p.Cards.First()).ToList();
        var winner = orderedPlayers.First();

        Console.WriteLine($"\n-- Round {roundIndex++} --");
        var playerIndex = 1;
        foreach (var player in players)
        {
          Console.WriteLine($"Player {playerIndex++} deck: {string.Join(",",player.Cards)}");
        }
        Console.WriteLine($"Winner is player {(winner == players[0] ? "1" : "2")}");


        foreach (var player in orderedPlayers)
        {
          winner.Cards.Add(player.Cards.First());
          player.Cards = player.Cards.Skip(1).ToList();
        }



      }
    }

    private class Player
    {
      public List<int> Cards { get; set; } = new List<int>();

      public int Score => Cards.Select((value, index) => (Cards.Count - index) * value).Sum();
    }
  }
}
