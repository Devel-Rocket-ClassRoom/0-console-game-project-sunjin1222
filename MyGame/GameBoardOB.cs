using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

class GameBoardOB : GameObject
{
    private List<(PlayerOB player, Card card)> currentTrick = new List<(PlayerOB, Card)>();
    public string CurrentLeadSuit { get; private set; }


    List<Card> playedCards = new List<Card>();

    public const int Left = 1;
    public const int Top = 6;
    public const int Right = 38;
    public const int Bottom = 11;

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
            currentTrick.Clear();
            CurrentLeadSuit = null;

            OnTrickEnd?.Invoke(winnerData.player, winnerData.card);
        }
    }

    private (PlayerOB player, Card card) DetermineWinner()
    {
        var sorted = currentTrick
            .Select((t, index) => new
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


        int cardWidth = 4;
        int cardHeight = 3;
        int spacing = 1;
        int offset = 0;
        int i = 0;
        int handCount = playedCards.Count;

        if (handCount == 0)
        {
            return;
        }
        int totalWidth = handCount * cardWidth + (handCount - 1) * spacing;

        int boardWidth = Right - Left;
        int startX = Left + (boardWidth - totalWidth) / 2;
        int centerY = (Top + Bottom) / 2;

        foreach (Card card in playedCards)
        {
            int drawX = startX + offset;
            int drawY = centerY;

            buffer.DrawBox(drawX, drawY, cardWidth, cardHeight, card.Color);

            string text = card.ToString();
            if (text.Length > 2)
            {
                text = text.Substring(0, 2);
            }
            buffer.WriteText(drawX + 1, drawY + 1, text, card.Color);
            offset += cardWidth + spacing;
            i++;
        }
    }

    public override void Update(float deltaTime)
    {
    }
}

