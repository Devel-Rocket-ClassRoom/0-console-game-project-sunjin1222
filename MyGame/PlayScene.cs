using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Xml.Linq;

internal class PlayScene : Scene
{

    private GameManager gameManager;
    private PlayerOB player1;
    private PlayerOB player2;
    private PlayerOB player3;
    private Deck deck;

    private PlayerOB lastWinner;
    private Card lastWinningCard;

    private GameBoardOB table;
    bool isRoundEnded = false;
    private PlayerOB finalWinner;
    public event Action ExitToTitleRequested;
    public event Action TurnEnd;
    int currentPlayerIndex = 0;
    List<PlayerOB> players = new List<PlayerOB>();
    bool WaitingTurn = false;
    PlayerOB pendingStartPlayer;
    private bool isEasyMode;
    private bool AllTie = false;

    public PlayScene(bool isEasyMode)
    {
        this.isEasyMode = isEasyMode;
    }

    public int RandoumPlayerIndex()
    { 
        Random turn = new Random();
        int First=turn.Next(0,3);

        return First;
    }



    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        if (!isRoundEnded)
        {
            Score(buffer);
        }

        if (WaitingTurn)
        {
            if (!isRoundEnded)
            {
                TurnWiner(buffer);
            }
        }

        if (isRoundEnded)
        {
            buffer.WriteText(17, 3, "=== 라운드 종료 ===", ConsoleColor.White);

            for (int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                buffer.WriteText(17, 5 + i, $"{p.Name}: {p.Score}", ConsoleColor.White);
            }

            if (AllTie)
            {
                buffer.WriteText(17, 9, "모두가 동점입니다 ", ConsoleColor.Green);
                buffer.WriteText(17, 11, "특별한 업적 추가", ConsoleColor.Green);
            }
            else
            {
                switch(finalWinner.Name)
                {   
                 case "플레이어":
                        buffer.WriteText(17, 10, $"(곰돌이와 소녀 모두가 축하해주고 있습니다)", ConsoleColor.Yellow);
                        break;

                    case "비행 소녀":
                    case "친절한 소녀":
                        buffer.WriteText(17, 10, $"(소녀가 춤을 추면서 기뻐합니다)", ConsoleColor.Yellow);
                        break;


                    case "착한 곰돌이":
                    case "사나운 곰돌이":
                        buffer.WriteText(17, 10, $"(곰이 재주를 부리며 기뻐하고 있습니다)", ConsoleColor.Yellow);
                        break;

                }
                buffer.WriteText(17, 9, $"승자: {finalWinner.Name}", ConsoleColor.Yellow);
                buffer.WriteText(17, 11, "Press ENTER to Title", ConsoleColor.Green);
            }
        }
    }


    public void NextTurn()
    {
       currentPlayerIndex++;
       
        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }         
        for (int i = 0; i < players.Count; i++)
            {
                  players[i].IsMyTurn = (i == currentPlayerIndex);
            }

    }



    private void ShowResult()
    {
        if (players.All(p => p.Score == players[0].Score))
        {
            AllTie = true;
            finalWinner = null;
            return;
        }

        var groups = players
            .GroupBy(p => p.Score)
            .OrderByDescending(g => g.Key)
            .ToList();

        var tieGroup = groups.FirstOrDefault(g => g.Count() > 1);
        if (tieGroup != null)
        {
            var other = players.FirstOrDefault(p => p.Score != tieGroup.Key);

            if (other != null)
            {
                finalWinner = other;
                return;
            }
        }
        var sorted = players
            .OrderByDescending(p => p.Score)
            .ThenByDescending(p => p.ranking)
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

    public void TurnWiner(ScreenBuffer buffer)
    {
        if (lastWinner == null || lastWinningCard == null)
        {
            return;
        }
        buffer.WriteText(GameBoardOB.Left + 6, 14, $"턴 승자: {lastWinner.Name} (+{lastWinningCard.Number}점)", ConsoleColor.Yellow);
    }



    private void Score(ScreenBuffer buffer)
    {
        var p1 = players[0];
        buffer.WriteText(24, 15, $"{p1.Name}", ConsoleColor.White);
        buffer.WriteText(24, 16, $"점수:{p1.Score}", ConsoleColor.White);
        var p2 = players[1];
        buffer.WriteText(9, 2, $"{p2.Name}", ConsoleColor.White);
        buffer.WriteText(12, 3, $"점수:{p2.Score}", ConsoleColor.White);
        var p3 = players[2];
        buffer.WriteText(30, 2, $"{p3.Name}", ConsoleColor.White);
        buffer.WriteText(36, 3, $"점수:{p3.Score}", ConsoleColor.White);
    }

    public override void Load()
    {
        gameManager = new GameManager();
        deck = new Deck();

        table = new GameBoardOB(this);
        AddGameObject(table);

        player1 = new PlayerOB(this, table, 5, 20,"플레이어",10,26);
  
        player2 = new AIPlayer(this, table, 0, 0, "Ai1", 5,15, isEasyMode);

        player3 = new AIPlayer(this, table, 0, 0, "AI2",5,38, isEasyMode);

        AddGameObject(player1);
        AddGameObject(player2);
        AddGameObject(player3);
        players.Add(player1);
        players.Add(player2);
        players.Add(player3);
        player1.OnCardPlayed = NextTurn;
        player2.OnCardPlayed = NextTurn;
        player3.OnCardPlayed = NextTurn;

        if (isEasyMode)
        {
            deck.DrawCards(players, 8); 
        }
        else
        {
            deck.DrawCards(players, 12); 
        }

        currentPlayerIndex = RandoumPlayerIndex();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].IsMyTurn = (i == currentPlayerIndex);
        }
        table.OnTrickEnd = (winner, card) =>
        {
            gameManager.OnTrickEnd(winner, card);
            lastWinner = winner; 
            lastWinningCard = card;
            pendingStartPlayer = winner;
            WaitingTurn = true;
        };
    }

    public override void Unload()
    {
        AIPlayer.ResetNames();
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
        if (WaitingTurn)
        {
            if (Input.IsKeyDown(ConsoleKey.Enter))
            {
                table.ClearTrick();
                WaitingTurn = false;
                SetNextStartPlayer(pendingStartPlayer);
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
