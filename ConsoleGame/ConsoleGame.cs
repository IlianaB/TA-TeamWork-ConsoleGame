using System;
using System.Threading;

namespace ConsoleGame
{
    class ConsoleGame
    {
        public const int windowWidth = 50;
        public const int windowHeight = 21;
        public static bool over = false;

        private static Pad pad = new Pad(windowWidth / 2 - 3, windowHeight - 1, ConsoleColor.White);
        private static Ball ball = new Ball(windowWidth / 2 + 1, windowHeight - 2, ConsoleColor.Red);
        private static Brick[,] bricks = new Brick[6, windowWidth];
        private static Player player = new Player(0, 3);

        static void Main()
        {
            SetupGameField();
            GenerateBricks();

            Play();
        }

        public static void Play()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Ball.printScore(player);
            Console.ForegroundColor = ConsoleColor.White;
            pad.Draw('#');
            clearTop3Rows();

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
                ball.CheckBrickCollision(bricks, player);
                ball.CheckWallCollision(pad, player);
                ball.Draw('*');

                CheckGameOver();
                Thread.Sleep(100);
            }
        }

        private static void DrawBrick()
        {
            for (int y = 0; y < bricks.GetLength(0); y++)
            {
                if (y == 1)
                {
                    for (int x = 0; x < bricks.GetLength(1)-1; x++)
                    {
                        
                            bricks[y, x].Draw('-');
                        
                    }
                }
                for (int x = 0; x < bricks.GetLength(1)-1; x++)
                {
                    if (bricks[y, x].IsBroken == false)
                    {
                        bricks[y, x].Draw('@');
                    }
                }
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

        private static void clearTop3Rows()
        {
            for (int row = 0; row <= 2; row++)
            {
                for (int col = 0; col < bricks.GetLength(1); col++)
                {
                    bricks[row, col].IsBroken = true;
                }
            }
            DrawBrick();
        }


        private static void GenerateBricks()
        {
            for (int y = 0; y < bricks.GetLength(0); y++)
            {
                for (int x = 0; x < bricks.GetLength(1); x++)
                {
                    bricks[y, x] = new Brick(x, y, ConsoleColor.White);
                }
            }
        }

        private static void CheckGameOver()
        {
            if (over)
            {
                string gameOver = "G A M E   O V E R";
                Console.Clear();
                Console.SetCursorPosition(windowWidth / 2 - gameOver.Length / 2, windowHeight / 3);
                Console.WriteLine(gameOver);
                Console.SetCursorPosition(windowWidth / 2 - gameOver.Length / 2 -1, windowHeight / 3 +2);
                Console.WriteLine("Your high score is {0} ", player.Score);
                Console.WriteLine("\n r - restart , n - new game , h - view highscores");
                player.Score = 0;
                //TODO  -  tuk trqbva da se napi6e vryzkata s faila
                Environment.Exit(0);
            }
        }
    }
}
