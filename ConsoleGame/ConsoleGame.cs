using System;
using System.Threading;

namespace ConsoleGame
{
    class ConsoleGame
    {
        public const int windowWidth = 50;
        public const int windowHeight = 20;

        private static Pad pad = new Pad(windowWidth / 2 + 1, windowHeight - 1, ConsoleColor.White);
        private static Ball ball = new Ball(windowWidth / 2 + 1, windowHeight - 2, ConsoleColor.Red);
        private static Brick[,] bricks = new Brick[4, windowWidth];

        static void Main()
        {
            SetupGameField();
            pad.Draw('#');
            GenerateBricks();

            while (true)
            {
                // Move the pad
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    pad.ChangePosition(key);
                    pad.Draw('#');
                }

                // Move the ball
                ball.ChangePosition();
                ball.CheckWallCollision(pad);
                ball.Draw('*');

                Thread.Sleep(100);
            }
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

        private static void GenerateBricks()
        {
            for (int y = 0; y < bricks.GetLength(0); y++)
            {
                for (int x = 0; x < bricks.GetLength(1); x++)
                {
                    bricks[y, x] = new Brick(x, y, ConsoleColor.White);
                    bricks[y, x].Draw('@');
                }
            }
        }
    }
}
