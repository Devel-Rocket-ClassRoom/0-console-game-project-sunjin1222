using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

internal class PlayScene : Scene
{

    private Player player2;
    private Player player1;
    private Player player3;
    public Deck deck;

    public override void Draw(ScreenBuffer buffer)
    {
    
        DrawGameObjects(buffer);

    }

    public override void Load()
    {
        deck = new Deck();

        player1 = new Player(this, 5, 17);
        player2 = new Player(this, 5, 8);
        player3 = new Player(this, 5, 5);
        AddGameObject(player1);
        AddGameObject(player2);
        AddGameObject(player3);
        List<Player> players = new List<Player>()
         {
              player1,
              player2,
              player3
         };

        deck.AllDraw(players);

    }

    public override void Unload()
    {
    }

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);
    }
}