using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

internal class Player : GameObject
{

    public List<Card> Hand { get; set; }= new List<Card>();
    private int x;
    private int y;

    public void AddCard(Card card)
    {
        Hand.Add(card);
    }
    public Player(Scene scene,int x, int y) : base(scene)
    {
        Name = "Player";
        this.x = x;
        this.y = y;

    }

    public override void Draw(ScreenBuffer buffer)
    {
        int offset = 0;

        foreach (Card card in Hand)
        {
            buffer.DrawBox(this.x + offset, this.y, 4, 3,ConsoleColor.Blue);
            buffer.WriteText(this.x + offset+1, this.y+1, card.ToString());
            offset += 3;
        }
    }

    public override void Update(float deltaTime)
    {
    }
}
