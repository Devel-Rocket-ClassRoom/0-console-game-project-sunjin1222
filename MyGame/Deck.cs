using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;

internal class Deck
{
    private List<Card> cards;
    SelectGame selectGame;

    public Deck()
    {
        cards = new List<Card>();
        NewDeck();
    }
    private void Make()
    {
        string[] suits = { "◆", "●", "▲" };

        foreach (var suit in suits)
        {
            ConsoleColor color = ConsoleColor.White;

            if (suit == "◆")
            { 
                color = ConsoleColor.DarkRed;
            }
            if (suit == "●")
            {
                color = ConsoleColor.Cyan;
            }
            if (suit == "▲")
            {
                color = ConsoleColor.Yellow;
            }
            for (int i = 1; i <=12; i++)
            {
                cards.Add(new Card(suit, i,color));
            }
        }
    }
    private void NewDeck()
    {
        cards.Clear();
        Make();

        for (int i = cards.Count - 1; i > 1; i--)
        {
            Random random = new Random();
            int x = random.Next(0, i + 1);

            var temp = cards[i];
            cards[i] = cards[x];
            cards[x] = temp;
        }
    }
    public Card Draw()
    {
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
    public void DrawCards(List<PlayerOB> players, int cardsPerPlayer)
    {
        for (int i = 0; i < cardsPerPlayer; i++)
        {
            for (int j = 0; j < players.Count; j++)
            {
                if (cards.Count == 0) return;

                players[j].AddCard(Draw());
            }
        }
    }

    public void AllDraw(List<PlayerOB> players)
    {
        int playerIndex = 0;

        while (cards.Count > 0)
        {
            players[playerIndex].AddCard(Draw());
            playerIndex = (playerIndex + 1) % players.Count;
        }
    }
}

