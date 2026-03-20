using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;


internal class TitleScene : Scene
{

    public event GameAction StartRequested;
    public override void Load()
    {
    }

    public override void Unload()
    {
    }

    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            StartRequested?.Invoke();
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteTextCentered(6, "Goldilocks", ConsoleColor.Yellow);
        buffer.WriteTextCentered(7, "-Card Game-", ConsoleColor.Yellow);
        buffer.WriteTextCentered(14, "ESC: Quit");
        buffer.WriteTextCentered(15, "Press ENTER to Start", ConsoleColor.Green);
    }


}

