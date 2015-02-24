using System;
using System.IO;
using System.Threading;

namespace ConsoleGame
{
    class ConsoleGame
    {
        public const int windowWidth = 50;
        public const int windowHeight = 21;
        public const string highscoresFile = "highscores.txt";
        public static bool over = false;
        public const int InitialGameSpeed = 100;
        public static int gameSpeed = InitialGameSpeed;
        public static int gameLevel = 1;

        private static Pad pad = new Pad(windowWidth / 2 - 3, windowHeight - 1, ConsoleColor.White);
        private static Ball ball = new Ball(windowWidth / 2 + 1, windowHeight - 2, ConsoleColor.Red);
        private static Brick[,] bricks = new Brick[7, windowWidth];
        private static Player player = new Player(0, 4);

        static void Main()
        {
            SetupGameField();
            GenerateBricks();
            DrawMenu();

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.P)
                {
                    Play();
                }
                if (key.Key == ConsoleKey.I)
                {
                    DrawInstructions();
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            } while (true);
        }

        private static void DrawMenu()
        {
            Console.Clear();           
            Console.ForegroundColor = ConsoleColor.White;
            string[] menu = new string[5];
            menu[0] = "Play";
            menu[1] = "Highscores";
            menu[2] = "Instructions";
            menu[3] = "About";
            menu[4] = "Exit";

            int maxLength = menu[2].Length;
            int startLeft = windowWidth / 2 - maxLength / 2;
            int startTop = windowHeight / 2 - menu.Length;
             int currentSelectedItem = 0;
             bool selected = false;
            for (int i = 0; i < menu.Length; i++)
            {
                Console.SetCursorPosition(startLeft, startTop + 2 * i);
                Console.Write(menu[i]);
            }

            bool[] lines = new bool[5];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = false;
            }
            lines[0] = true;
            redrawOptions(startTop, startLeft, ref lines, ref menu);
           ConsoleKeyInfo chosenLine = new ConsoleKeyInfo();
            while (!selected)
            {
                chosenLine = Console.ReadKey(true);
                if (lines[0] == false && chosenLine.Key == ConsoleKey.UpArrow)
                {
                    lines[currentSelectedItem] = false;
                    currentSelectedItem--;
                    lines[currentSelectedItem] = true;
                    
                }
                else if (lines[4] == false && chosenLine.Key == ConsoleKey.DownArrow)
                {
                    lines[currentSelectedItem] = false;
                    currentSelectedItem++;
                    lines[currentSelectedItem] = true;
                }
                else if (chosenLine.Key == ConsoleKey.Enter)
                {
                    selected = true;
                    break;
                   
                }
                redrawOptions(startTop, startLeft,ref lines, ref menu);
            }
            goIntoSelectedMenu(ref lines);
        }

        private static void  goIntoSelectedMenu(ref bool[] array)
        {
            if (array[0] == true)
            {
                Console.Clear();
                Play();
            }
            else if (array[1] == true)
            {
                Console.Clear();
                Console.WriteLine("Tova ne e gotovo oshte,kolegi :D");  // TODO
            }
            else if (array[2] == true)
            {
                DrawInstructions();
            }
            else if (array[3] == true)
            {
                Console.Clear();
                Console.WriteLine("Tova ne e gotovo oshte,kolegi :D");  //TODO
            }
            else if (array[4] == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(windowHeight / 2, 10 );                
                Environment.Exit(0);
            }
        }

        private static void redrawOptions(int top,int left,ref bool[] array,ref string[] arrayStr)
        {

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == true)
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(left, top + 2 * i);
                    Console.Write(arrayStr[i]);
                    Console.SetCursorPosition(left - 3, top + 2 * i);
                    Console.Write(">>");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(left, top + 2 * i);
                    Console.Write(arrayStr[i]);
                    Console.SetCursorPosition(left - 3, top + 2 * i);
                    Console.Write("  ");
                }
            }
        }

        public static void Play()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Ball.printScore(player);
            Console.ForegroundColor = ConsoleColor.White;
            pad.Draw('▀');
            clearTop3Rows();

            while (true)
            {
                // Move the pad
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    pad.ChangePosition(key);
                    pad.Draw('▀');
                }

                // Move the ball
                ball.ChangePosition();
                ball.CheckBrickCollision(bricks, player);
                ball.CheckWallCollision(pad, player);
                ball.Draw('*');

                CheckGameOver();
                Thread.Sleep(gameSpeed);
            }
        }

        private static void DrawBrick()
        {
            for (int y = 0; y < bricks.GetLength(0); y++)
            {
                if (y == 2)
                {
                    for (int x = 0; x < bricks.GetLength(1) - 1; x++)
                    {

                        bricks[y, x].Draw('-');

                    }
                }
                for (int x = 0; x < bricks.GetLength(1) - 1; x++)
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
                Console.Clear();
                
                Console.SetCursorPosition(windowWidth / 2 -10, windowHeight / 3 -5);
                Console.WriteLine("Your high score is {0} ", player.Score);
                Console.SetCursorPosition(windowWidth / 2 - 11, windowHeight / 3 - 2);
                Console.WriteLine("Press m  to enter menu.");
                Console.SetCursorPosition(windowWidth / 2 - 10, windowHeight / 3 + 1);
                Console.WriteLine("Press escape  to exit.");

                if (!File.Exists(highscoresFile))
                {
                    File.Create(highscoresFile);
                }

                using (StreamWriter file = new StreamWriter(highscoresFile, true))
                {
                    file.WriteLine(player.Score);
                }

                do
                {
                    ConsoleKeyInfo waitedKey = Console.ReadKey(true);
                    if (waitedKey.Key == ConsoleKey.M)
                    {
                        ResetGame();
                    }                   
                    else if (waitedKey.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(windowHeight / 2, 10);
                        Environment.Exit(0);
                    }                   
                } while (true);
            }
        }

        private static void DrawInstructions()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(Console.WindowWidth / 2 - 7, 1);
            Console.Write("INSTRUCTIONS");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 6, 4);
            Console.WriteLine("Directions:");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 17, 6);
            Console.WriteLine(" <- key  - pad direction Left");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 17, 8);
            Console.WriteLine(" -> key  - pad direction Right");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 6, 11);
            Console.WriteLine("HOW TO PLAY:");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, 13);
            Console.WriteLine("Move the pad in order to navigate ");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 16, 14);
            Console.WriteLine("the ball across the playfield and");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 14, 15);
            Console.WriteLine("try to coolect all the bricks");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 11, 18);
            Console.WriteLine("PRESS ANY KEY TO START");
            ConsoleKeyInfo keyToReturnToGame = Console.ReadKey(true);
            Play();
        }

        private static void ResetGame()
        {
            over = false;
            pad = new Pad(windowWidth / 2 - 3, windowHeight - 1, ConsoleColor.White);
            ball = new Ball(windowWidth / 2 + 1, windowHeight - 2, ConsoleColor.Red);
            player = new Player(0, 4);
            Console.Clear();
            gameSpeed = InitialGameSpeed;
            gameLevel = 1;
            Main();
        }
    }
}
