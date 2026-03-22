using Framework.Engine;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

class GameManager
{

    int orderCounter = 0;

        public void OnTrickEnd(PlayerOB winner, Card card)
    {
        winner.Score += card.Number;

        if (winner.ranking == 0)
        {
            winner.ranking = ++orderCounter;
        }
    }
}

