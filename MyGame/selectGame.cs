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
        buffer.WriteText(19, 10, "난이도를 선택해 주세요", ConsoleColor.Yellow);

        for (int i = 0; i < options.Length; i++)
        {
            string prefix = (i == selectedIndex) ? "▶ " :"";
            buffer.WriteTextCentered(11 + i, prefix + options[i], ConsoleColor.White);
        }
        switch (selectedIndex)
        {
            case 0:
                buffer.WriteText(10,4, "게임을 처음 시작하는 사람에게",ConsoleColor.DarkCyan);
                buffer.WriteText(10,5, "친절하게 게임을 알려주는 상대가 준비중 입니다", ConsoleColor.DarkCyan);
                break;

            case 1:
                buffer.WriteText(10, 4, "고요한 새벽의 나라는 전쟁 기술을 통달하고",ConsoleColor.Red);
                buffer.WriteText(10, 5, "지구에서 가장 유명한 플레이어들의 고향이 되었습니다", ConsoleColor.Red);
                buffer.WriteText(10, 6, "이 무시무시한 전쟁터에",ConsoleColor.DarkRed);
                buffer.WriteText(10, 7, "생각 없이 발을 들이지 마십시오.", ConsoleColor.Red);
                break;
        }

        buffer.WriteTextCentered(14, "ESC: Quit");
        buffer.WriteTextCentered(15, "Press ENTER to Start", ConsoleColor.Green);
    }
}
