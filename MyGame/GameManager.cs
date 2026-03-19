using Framework.Engine;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

class GameManager
{
    public void OnTrickEnd(PlayerOB winner, Card card)
    {
        winner.Score += card.Number;
    }

}

