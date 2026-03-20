using System;
using System.Collections.Generic;
using System.Text;
using Framework.Engine;


internal class Main : GameApp
{
    private readonly SceneManager<Scene> _scene = new SceneManager<Scene>();

    public Main() : base(60, 30)
    {
    }
    public Main(int width, int height) : base(50, 50)
    {
    }

    protected override void Initialize()
    {
        ChangeToTitle();
    }

    protected override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Escape))
        {
            Quit();
            return;
        }

        _scene.CurrentScene?.Update(deltaTime);
    }


    protected override void Draw()
    {
        _scene.CurrentScene?.Draw(Buffer);
    }

    public void ChangeToTitle()
    {
        var title = new TitleScene();
        title.StartRequested += SelectGame;
        _scene.ChangeScene(title);
    }


    public void ChangeToPlayEasy()
    {
        var play = new PlayScene(true);
        play.ExitToTitleRequested += ChangeToTitle;
        _scene.ChangeScene(play);
    }

    public void ChangeToPlayHard()
    {
        var play = new PlayScene(false);
        play.ExitToTitleRequested += ChangeToTitle;
        _scene.ChangeScene(play);
    }


    public void SelectGame()
    {
        var select = new SelectGame();
        select.SelectGameRequested += OnGameSelected;
        _scene.ChangeScene(select);
    }
    private void OnGameSelected(bool isEasy)
    {
        if (isEasy)
        {
            ChangeToPlayEasy();
        }
        else
        {
            ChangeToPlayHard();

        }
    }
}

