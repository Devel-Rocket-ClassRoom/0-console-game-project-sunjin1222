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
            for (int i = 1; i <= 8; i++)
            {
                cards.Add(new Card(suit, i));
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

    public void AllDraw(List<Player> players)
    {
        int Playercount = 0;
        while (cards.Count > 0)
        {
            players[Playercount].AddCard(Draw());
            Playercount= (Playercount + 1)%players.Count;
        }

    }




}

