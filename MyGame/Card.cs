using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Card
{
    public string Suit { get; set; }
    public int Number { get; set; }


    public Card (string suit,int number)
    {
        Number = number;
        Suit = suit;
    }

    public override string ToString()
    {
        return $"{Suit}{Number}";
    }
}
