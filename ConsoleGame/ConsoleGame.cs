using System;
using System.Threading;

namespace ConsoleGame
{
    class ConsoleGame
    {
        public const int windowWidth = 50;
        public const int windowHeight = 20;

        static void Main()
        {
            SetupGameField();
        }

        private static void SetupGameField()
        {
            Console.WindowHeight = windowHeight;
            Console.BufferHeight = windowHeight;
            Console.WindowWidth = windowWidth;
            Console.BufferWidth = windowWidth;
            Console.Title = "Brick Wall Game";
            Console.CursorVisible = false;
        }
    }
}
