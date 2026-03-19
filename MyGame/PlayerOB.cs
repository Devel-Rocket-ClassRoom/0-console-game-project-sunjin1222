using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

internal class PlayerOB : GameObject
{
   
    protected GameBoardOB board;
    public List<Card> Hand { get; set; }= new List<Card>();
    private int x;
    private int y;
    int select = 0;
    public bool IsMyTurn;
    private float inputDelay = 0f;
    public int Score { get; set; } = 0;

    public Action OnCardPlayed;
    public bool IsAI=false;
    public int TablePosX { get; set; } 
    public int TablePosY { get; set; } 

    public Card PlayedCard { get; set; }


    public PlayerOB(Scene scene, GameBoardOB board, int x, int y, string name) : base(scene)
    {
        Name = name;
        this.board = board;
        this.x = x;
        this.y = y;
    }



public void AddCard(Card card)
    {
        Hand.Add(card);
    }
    protected bool CanPlayCard(Card card)
    {
        if (board.CurrentLeadSuit == null) return true;

        bool hasLeadSuit = Hand.Any(c => c.Suit == board.CurrentLeadSuit);

        if (hasLeadSuit)
            return card.Suit == board.CurrentLeadSuit;

        return true;

    }
    private void PlaySelectedCard()
    {
        if (select < 0 || select >= Hand.Count) return;

        Card selectedCard = Hand[select];

        if (!CanPlayCard(selectedCard))
            return;

        Hand.RemoveAt(select);
        board.AddCard(this, selectedCard);

        if (select >= Hand.Count && Hand.Count > 0)
            select = Hand.Count - 1;

        IsMyTurn = false;


        if (board.IsTrickEnding)
        {
            return;
        }
        OnCardPlayed?.Invoke();
    }





    public void SelectCard()
    {
        if (inputDelay <= 0)
        {
            if (Input.IsKeyDown(ConsoleKey.LeftArrow))
            {
                select--;
                inputDelay = 0.15f;
            }
            else if (Input.IsKeyDown(ConsoleKey.RightArrow))
            {
                select++;
                inputDelay = 0.15f;
            }
        }

        if (select < 0)
            select = Hand.Count - 1;

        if (select >= Hand.Count)
            select = 0;
    }



    public Card PlayCard()
    {
        if (Hand.Count == 0)
        {
            return null;
        }

        Card selectedCard = Hand[select];
        Hand.RemoveAt(select);

        if (select >= Hand.Count)
        {
            select = Hand.Count - 1;
        }

        return selectedCard;
    }



    public override void Draw(ScreenBuffer buffer)
    {
        if (IsAI)
        {
            return;
        }
        if (Hand.Count == 0)
        {
            return;
        }

        int cardWidth = 4;
        int cardHeight = 3;
        int spacing = 1;
        int totalWidth = Hand.Count * (cardWidth + spacing) - spacing;
        int startX = (buffer.Width - totalWidth) / 2;

        for (int i = 0; i < Hand.Count; i++)
        {
            int drawX = startX + i * (cardWidth + spacing);
            int drawY = (i == select && IsMyTurn) ? y - 1 : y; 

            buffer.DrawBox(drawX, drawY, cardWidth, cardHeight, Hand[i].Color);
            string text = Hand[i].ToString();
            buffer.WriteText(drawX + 1, drawY + 1, text.Length > 2 ? text.Substring(0, 2) : text, Hand[i].Color);
        }
    }

    public override void Update(float deltaTime)
    {
        if (!IsMyTurn || Hand.Count == 0)
            return;

        inputDelay -= deltaTime;

        SelectCard(); 

        if (inputDelay <= 0 && Input.IsKeyDown(ConsoleKey.Enter))
        {
            PlaySelectedCard();
            inputDelay = 0.2f;
        }
    }
}
