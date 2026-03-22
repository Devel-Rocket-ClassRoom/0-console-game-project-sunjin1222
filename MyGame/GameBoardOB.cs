using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

class GameBoardOB : GameObject
{
    private List<(PlayerOB player, Card card)> currentTrick = new List<(PlayerOB, Card)>();
    public string CurrentLeadSuit { get; private set; }
    public List<PlayerOB> players = new List<PlayerOB>();
    List<Card> playedCards = new List<Card>();

    public const int Left = 10;
    public const int Top = 5;
    public const int Right = 47;
    public const int Bottom = 12;

    public int TablePosX { get; set; } 
    public int TablePosY { get; set; }
    public Action<PlayerOB, Card> OnTrickEnd;
    public bool IsTrickEnding { get; private set; }

    public GameBoardOB(Scene scene) : base(scene)
    {
        Name = "GameBoard";
    }

    public bool IsTrickFull()
    {
        return currentTrick.Count >= 3;
    }

    public void ClearTrick()
    {
        currentTrick.Clear();
    }

    public void AddCard(PlayerOB player, Card card)
    {
        IsTrickEnding = false;

        if (currentTrick.Count == 0)
        {
            CurrentLeadSuit = card.Suit;
            playedCards.Clear();
        }
        currentTrick.Add((player, card));
        playedCards.Add(card);
        if (currentTrick.Count == 3)
        {
            IsTrickEnding = true; 

            var winnerData = DetermineWinner();
            CurrentLeadSuit = null;

            OnTrickEnd?.Invoke(winnerData.player, winnerData.card);
        }
    }
    private (PlayerOB player, Card card) DetermineWinner()
    {
        var sorted = currentTrick.Select((t, index) => new
            {
                t.player,
                t.card,
                score = t.card.Number + index * 0.1
            })
            .OrderByDescending(x => x.score)
            .ToList();

        return (sorted[1].player, sorted[1].card);
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.DrawBox(Left - 1, Top - 1, Right - Left + 3, Bottom - Top + 3, ConsoleColor.DarkGreen);
        buffer.WriteText(Left +16, Top + 1,"---→", ConsoleColor.DarkYellow);
        buffer.WriteText(Left + 27, Top + 4, "↙", ConsoleColor.DarkYellow);
        buffer.WriteText(Left + 9, Top + 4, "↖", ConsoleColor.DarkYellow);
        buffer.WriteText(0, 16, "press S: SuitArray", ConsoleColor.DarkGray);
        buffer.WriteText(0, 17 , "press D: NumberArray", ConsoleColor.DarkGray);

        int cardWidth = 5;
        int cardHeight = 3;
        int handCount = playedCards.Count;

        foreach (var (player, card) in currentTrick)
        {
            int drawX = player.TablePosX;
            int drawY = player.TablePosY;

            buffer.DrawBox(drawX, drawY, cardWidth, cardHeight, card.Color);
            string cardText = $"{card.Suit[0]}{card.Number}";
            buffer.WriteText(drawX + 1, drawY + 1, cardText, card.Color);
        }
    }
    public override void Update(float deltaTime)
    {
    }
}

