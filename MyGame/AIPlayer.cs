using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class AIPlayer : PlayerOB
{
    private float thinkDelay = 1.0f;
    public AIPlayer(Scene scene, GameBoardOB board, int x, int y, string name) : base(scene, board, x, y, name)
    {
        Name = name;
        IsAI = true;
    }

    public override void Update(float deltaTime)
    {
        if (!IsMyTurn || Hand.Count == 0)
            return;

        thinkDelay -= deltaTime;

        if (thinkDelay <= 0)
        {
            PlayAI();
            thinkDelay = 1.0f;
        }
    }

    private void PlayAI()
    {
        // 낼 수 있는 카드 찾기
        for (int i = 0; i < Hand.Count; i++)
        {
            var card = Hand[i];

            if (CanPlay(card))
            {
                Hand.RemoveAt(i);
                board.AddCard(this, card);
                IsMyTurn = false;

                if (!board.IsTrickEnding)
                    OnCardPlayed?.Invoke();

                return;
            }
        }
    }

    private bool CanPlay(Card card)
    {
        if (board.CurrentLeadSuit == null) return true;

        bool hasLead = Hand.Any(c => c.Suit == board.CurrentLeadSuit);

        if (hasLead)
            return card.Suit == board.CurrentLeadSuit;

        return true;
    }
}
