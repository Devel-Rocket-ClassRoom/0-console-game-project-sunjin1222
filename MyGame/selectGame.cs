using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;


internal class SelectGame : Scene
{
    public event Action<bool> SelectGameRequested;
    public bool enterPressed { get; set; }


    int selectedIndex = 0;
    string[] options = { "Easy Mode", "Hard Mode" };




    public override void Load()
    {
        enterPressed = false;
    }

    public override void Unload() { }

    public override void Update(float deltaTime)
    {


        if (Input.IsKeyDown(ConsoleKey.UpArrow))
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = options.Length - 1;
        }

        if (Input.IsKeyDown(ConsoleKey.DownArrow))
        {
            selectedIndex++;
            if (selectedIndex >= options.Length) selectedIndex = 0;
        }
        if (Input.IsKeyDown(ConsoleKey.Enter) && !enterPressed)
        {
            enterPressed = true;

            bool isEasy = (selectedIndex == 0);
            SelectGameRequested?.Invoke(isEasy);
        }

        if (!Input.IsKeyDown(ConsoleKey.Enter))
        {
            enterPressed = false;
        }
    }
        public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(19, 7, "난이도를 선택해 주세요", ConsoleColor.Yellow);

        for (int i = 0; i < options.Length; i++)
        {
            string prefix = (i == selectedIndex) ? "▶ " :"";
            buffer.WriteTextCentered(11 + i, prefix + options[i], ConsoleColor.White);
        }

        buffer.WriteTextCentered(14, "ESC: Quit");
        buffer.WriteTextCentered(15, "Press ENTER to Start", ConsoleColor.Green);
    }
}
