using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;


internal class TitleScene : Scene
{

    public event GameAction StartRequested;
    Random card = new Random();
    public override void Load()
    { }

    public override void Unload()
    {
    
    }

    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            StartRequested?.Invoke();
        }

        if (Input.IsKeyDown(ConsoleKey.Escape))
        {
            Environment.Exit(0);
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteTextCentered(3, "  ____    _    ____   ____ ", ConsoleColor.Yellow);
        buffer.WriteTextCentered(4, " / ___|  / \\  |  _ \\ |  _ \\", ConsoleColor.Yellow);
        buffer.WriteTextCentered(5, "| |     / _ \\ | |_) || | | |", ConsoleColor.Yellow);
        buffer.WriteTextCentered(6, "| |___ / ___ \\|  _ < | |_| |", ConsoleColor.Yellow);
        buffer.WriteTextCentered(7, " \\____/_/   \\_\\_| \\_\\|____/ ", ConsoleColor.Yellow);
        buffer.WriteTextCentered(9, "   ____    _    __  __ _____ ", ConsoleColor.Yellow);
        buffer.WriteTextCentered(10, "  / ___|  / \\  |  \\/  | ____|", ConsoleColor.Yellow);
        buffer.WriteTextCentered(11, " | | __  / _ \\ | |\\/| |  _|  ", ConsoleColor.Yellow);
        buffer.WriteTextCentered(12, " | |_| |/ ___ \\| |  | | |___ ", ConsoleColor.Yellow);
        buffer.WriteTextCentered(13, "  \\____/_/   \\_\\_|  |_|_____|", ConsoleColor.Yellow);

        buffer.WriteText(18, 16, $"곰돌이와 소녀의 카드 대결!", ConsoleColor.Yellow);

        buffer.WriteTextCentered(17, "Press ENTER to Start", ConsoleColor.Green);


        buffer.WriteTextCentered(18, "ESC: Quit", ConsoleColor.DarkGray);


    }


}
