using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;

internal class PlayScene : Scene
{

    private GameManager gameManager;
    private PlayerOB player1;
    private PlayerOB player2;
    private PlayerOB player3;
    private Deck deck;

    private GameBoardOB table;
    bool isRoundEnded = false;
    private PlayerOB finalWinner;
    public event Action ExitToTitleRequested;

    int currentPlayerIndex = 0;
    List<PlayerOB> players = new List<PlayerOB>();

   public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        if (!isRoundEnded)
        {
            Score(buffer);
        }

        if (isRoundEnded)
        {
            buffer.WriteText(10, 3, "=== 라운드 종료 ===", ConsoleColor.White);

            for (int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                buffer.WriteText(10, 5 + i, $"{p.Name}: {p.Score}", ConsoleColor.White);
            }

            buffer.WriteText(10, 9, $"승자: {finalWinner.Name}", ConsoleColor.Yellow);
            buffer.WriteText(10, 11, "Press ENTER to Title", ConsoleColor.Green);
        }
    }


    public void NextTurn()
    {
        currentPlayerIndex++;

        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].IsMyTurn = (i == currentPlayerIndex);
        }

    }
    private void ShowResult()
    {
        var sorted = players
            .OrderByDescending(p => p.Score)
            .ToList();

        finalWinner = sorted[1];
    }

    private void SetNextStartPlayer(PlayerOB winner)
    {
        currentPlayerIndex = players.IndexOf(winner);

    
        foreach (var p in players)
            p.IsMyTurn = false;


        players[currentPlayerIndex].IsMyTurn = true;

    }

    private void Score(ScreenBuffer buffer)
    {
        for (int i = 0; i < players.Count; i++)
        {
            var p = players[i];
            buffer.WriteText(2, 1 + i, $"{p.Name}: {p.Score}", ConsoleColor.White);
        }
    }

    public override void Load()
    {
        gameManager = new GameManager();
        deck = new Deck();

        table = new GameBoardOB(this);
        AddGameObject(table);

        player1 = new PlayerOB(this, table, 5, 17,"플레이어");
        player1.TablePosX = 40;
        player1.TablePosY = 15;
        player2 = new AIPlayer(this, table, 5, 8,"사나운 곰돌이");
        player1.TablePosX = 10;
        player1.TablePosY = 5;
        player3 = new AIPlayer(this, table, 5, 5,"불법 소녀");
        player1.TablePosX = 70;
        player1.TablePosY = 5;

        AddGameObject(player1);
        AddGameObject(player2);
        AddGameObject(player3);
        players.Add(player1);
        players.Add(player2);
        players.Add(player3);
        player1.OnCardPlayed = NextTurn;
        player2.OnCardPlayed = NextTurn;
        player3.OnCardPlayed = NextTurn;
        deck.AllDraw(players);
   

        currentPlayerIndex = 0;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].IsMyTurn = (i == currentPlayerIndex);
        }
        table.OnTrickEnd = (winner, card) =>
        {
            gameManager.OnTrickEnd(winner, card);
            SetNextStartPlayer(winner);        
        };

    }

    public override void Unload()
    {
    }


    public override void Update(float deltaTime)
    {
        if (isRoundEnded)
        {
            if (Input.IsKeyDown(ConsoleKey.Enter))
            {
                ExitToTitleRequested?.Invoke();
            }
            return;
        }

        if (!players.Any(p => p.IsMyTurn))
        {
            players[currentPlayerIndex].IsMyTurn = true;
        }

        foreach (var player in players)
        {
            player.Update(deltaTime);
        }

        table.Update(deltaTime);

        if (!isRoundEnded && players.All(p => p.Hand.Count == 0))
        {
            isRoundEnded = true;
            ClearGameObjects();
            ShowResult();
        }
    }
}
