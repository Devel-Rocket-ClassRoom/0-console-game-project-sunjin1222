using System;
using System.Collections.Generic;
using System.Text;
using Framework.Engine;


internal class Main : GameApp
{
    private readonly SceneManager<Scene> _scene = new SceneManager<Scene>();

    public Main() : base(40, 20)
    {
    }
    public Main(int width, int height) : base(40, 20)
    {
    }

    protected override void Draw()
    {
        _scene.CurrentScene?.Draw(Buffer);
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
    public void ChangeToTitle()
    {
        var title = new TitleScene();
        _scene.ChangeScene(title);
    }
}

