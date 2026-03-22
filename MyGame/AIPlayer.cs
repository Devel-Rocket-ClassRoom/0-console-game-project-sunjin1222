using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
internal class AIPlayer : PlayerOB
{
    private float thinkDelay = 1.0f;

    private static List<string> EasyNames = new List<string>
    {
        "착한 곰돌이",
        "친절한 소녀",
    };
    private static List<string> HardNames = new List<string>
    {
        "사나운 곰돌이",
        "비행 소녀",
    };


    public AIPlayer(Scene scene, GameBoardOB board, int x, int y, string name, int tablePosY, int tablePosX, bool isEasy)
       : base(scene, board, x, y, GetRandomName(isEasy), tablePosY, tablePosX)
    {
        IsAI = true;
    }

    private static Random rand = new Random();

    public static string GetRandomName(bool isEasy)
    {
        var list = isEasy ? EasyNames : HardNames;
        int index = rand.Next(list.Count);
        string selected = list[index];

        list.RemoveAt(index); 

        return selected;
    }

    public static void ResetNames()
{
    EasyNames = new List<string>
    {
        "착한 곰돌이",
        "친절한 소녀",
    };

    HardNames = new List<string>
    {
        "사나운 곰돌이",
        "비행 소녀",
    };
}

    public override void Update(float deltaTime)
    {
        if (!IsMyTurn || Hand.Count == 0)
        {
            return;
        }
        thinkDelay -= deltaTime;

        if (thinkDelay <= 0)
        {
            PlayEasyAI();
            thinkDelay = 1.0f;
        }
    }

    private void PlayEasyAI()
    {
        
        for (int i = 0; i < Hand.Count; i++)
        {
            var card = Hand[i];

            if (CanPlay(card))
            {
                Hand.RemoveAt(i);
                board.AddCard(this, card);
                IsMyTurn = false;

                if (!board.IsTrickEnding)
                {
                    OnCardPlayed?.Invoke();
                }
                return;
            }
        }
    }



    private bool CanPlay(Card card)
    {
        if (board.CurrentLeadSuit == null)
        {
            return true;
        }
        bool hasLead = Hand.Any(c => c.Suit == board.CurrentLeadSuit);

        if (hasLead)
        {
            return card.Suit == board.CurrentLeadSuit;
        }
        return true;
    }
}
