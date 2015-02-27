using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ConsoleGame
{
    class ConsoleGame
    {
        public const int WindowWidth = 50;
        public const int WindowHeight = 21;
        public const string HighscoresFile = "highscores.txt";
        public static bool over = false;
        public const int InitialGameSpeed = 100;
        public static int gameSpeed = InitialGameSpeed;
        public static int gameLevel = 1;

        private static Pad pad = new Pad(WindowWidth / 2 - 3, WindowHeight - 1, ConsoleColor.White);
        private static Ball ball = new Ball(WindowWidth / 2 + 1, WindowHeight - 2, ConsoleColor.Red);
        private static Brick[,] bricks = new Brick[7, WindowWidth];
        private static Player player = new Player(0, 4);

        static void Main()
        {
            SetupGameField();
            GenerateBricks();
            GenerateMenu();
        }

        private static void GenerateMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            string[] menu = { "Play", "Highscores", "Instructions", "About", "Exit" };

            int maxLength = menu[2].Length;
            int startLeft = WindowWidth / 2 - maxLength / 2;
            int startTop = WindowHeight / 2 - menu.Length + 3;
            int currentSelectedItem = 0;

            bool[] lines = new bool[5];
            lines[0] = true;

            ConsoleKeyInfo chosenLine = new ConsoleKeyInfo();
            while (true)
            {
                DrawMenu(startTop, startLeft, lines, menu);
                chosenLine = Console.ReadKey(true);

                if (lines[0] == false && chosenLine.Key == ConsoleKey.UpArrow)
                {
                    lines[currentSelectedItem] = false;
                    lines[--currentSelectedItem] = true;
                }
                else if (lines[4] == false && chosenLine.Key == ConsoleKey.DownArrow)
                {
                    lines[currentSelectedItem] = false;
                    lines[++currentSelectedItem] = true;
                }
                else if (chosenLine.Key == ConsoleKey.Enter)
                {
                    goIntoSelectedMenu(lines);
                }
                Console.Beep(333, 75);
            }

        }

        private static void goIntoSelectedMenu(bool[] array)
        {
            if (array[0] == true)
            {
                Console.Clear();
                Play();
            }
            else if (array[1] == true)
            {
                string scoreTitle = @"
            __                         
           / _\ ___ ___  _ __ ___  ___ 
           \ \ / __/ _ \| '__/ _ \/ __|
           _\ \ (_| (_) | | |  __/\__ \
           \__/\___\___/|_|  \___||___/
";
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                Console.Write(scoreTitle);
                Console.ForegroundColor = ConsoleColor.White;

                string[] lines = File.ReadAllLines(HighscoresFile);
                var highscores = new List<int>();

                foreach (var line in lines)
                {
                    highscores.Add(int.Parse(line));
                }

                highscores.Sort();
                highscores.Reverse();
                const int ViewedScoresCount = 5;

                // shows N scores if there are more than N records in the file, else it shows as many scores as the their total count
                for (int i = 0; i < (highscores.Count > ViewedScoresCount ? ViewedScoresCount : highscores.Count); i++)
                {
                    Console.SetCursorPosition(WindowWidth / 2 - 6, 8 + 2 * i);
                    Console.WriteLine("{0}. {1} points", i + 1, highscores[i]);
                }

                Console.SetCursorPosition(WindowWidth / 2 - 11, WindowHeight - 2);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Press m to enter menu.");

                do
                {
                    ConsoleKeyInfo waitedKey = Console.ReadKey(true);
                    if (waitedKey.Key == ConsoleKey.M)
                    {
                        ResetGame();
                    }
                } while (true);
            }
            else if (array[2] == true)
            {
                DrawInstructions();
            }
            else if (array[3] == true)
            {
                string aboutTitle = @"
             _   _                 _   
            /_\ | |__   ___  _   _| |_ 
           //_\\| '_ \ / _ \| | | | __|
          /  _  \ |_) | (_) | |_| | |_ 
          \_/ \_/_.__/ \___/ \__,_|\__|
";

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                Console.Write(aboutTitle);
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 13);
                Console.WriteLine(@"Game B-Breaker is developed by TEAM IMP as a");
                Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 12);
                Console.WriteLine(@"team project in C#2 course in TelerikAcademy.");
                Console.SetCursorPosition(WindowWidth / 2 - 4, WindowHeight - 10);
                Console.WriteLine(@"TEAM IMP:");
                Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 8);
                Console.WriteLine(@"Iliana Bobeva, Luba Gerasimova");
                Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 7);
                Console.WriteLine(@"Bistra Gospodinova, Dimitar Bakardzhiev");
                Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 6);
                Console.WriteLine(@"Dragomir Tachev, Emo Penovski");
                Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 5);
                Console.WriteLine(@"Kiril Mihaylov, Petar Zubev");
                Console.SetCursorPosition(WindowWidth / 2 - 11, WindowHeight - 2);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Press m to enter menu.");
                Console.ForegroundColor = ConsoleColor.White;
                do
                {
                    ConsoleKeyInfo waitedKey = Console.ReadKey(true);
                    if (waitedKey.Key == ConsoleKey.M)
                    {
                        ResetGame();
                    }
                } while (true);
            }
            else if (array[4] == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(WindowHeight / 2, 10);
                Environment.Exit(0);
            }
        }

        private static void DrawMenu(int top, int left, bool[] array, string[] arrayStr)
        {
            string gameName = @"
    ___         ___                _             
   / __\       / __\_ __ ___  __ _| | _____ _ __ 
  /__\//_____ /__\// '__/ _ \/ _` | |/ / _ \ '__|
 / \/  \_____/ \/  \ | |  __/ (_| |   <  __/ |   
 \_____/     \_____/_|  \___|\__,_|_|\_\___|_|   
                                               
";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.Write(gameName);

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
            Console.Beep(350, 250);
            Console.Beep(350, 120);
            Console.Beep(700, 350);

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
            Console.WindowHeight = WindowHeight;
            Console.BufferHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;
            Console.BufferWidth = WindowWidth;
            Console.Title = "B-Breaker";
            Console.CursorVisible = false;
        }

        private static void clearTop3Rows()
        {
            for (int row = 3; row <= 6; row++)
            {
                if (row == 3)
                {
                    for (int j = 0; j < bricks.GetLength(1); j++)
                    {
                        bricks[row, j].IsBroken = true;
                    }
                }
                bricks[row, 0].IsBroken = true;
                bricks[row, 1].IsBroken = true;
                bricks[row, 47].IsBroken = true;
                bricks[row, 48].IsBroken = true;
            }
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
                string gameOverTitle = @"
   ___                       ___                 
  / _ \__ _ _ __ ___   ___  /___\__   _____ _ __ 
 / /_\/ _` | '_ ` _ \ / _ \//  //\ \ / / _ \ '__|
/ /_\\ (_| | | | | | |  __/ \_//  \ V /  __/ |   
\____/\__,_|_| |_| |_|\___\___/    \_/ \___|_| 
";
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                Console.Write(gameOverTitle);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(WindowWidth / 2 - 9, WindowHeight - 9);
                Console.WriteLine("Your score is {0}!", player.Score);
                Console.SetCursorPosition(Console.WindowWidth / 2 - 11, 19);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Press m to enter menu.");

                if (!File.Exists(HighscoresFile))
                {
                    File.Create(HighscoresFile);
                }

                using (StreamWriter file = new StreamWriter(HighscoresFile, true))
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
                        Console.SetCursorPosition(WindowHeight / 2, 10);
                        Environment.Exit(0);
                    }
                } while (true);
            }
        }

        private static void DrawInstructions()
        {
            string title = @"
    _____           _                   _       
    \_   \_ __  ___| |_ _ __ _   _  ___| |_ ___ 
     / /\/ '_ \/ __| __| '__| | | |/ __| __/ __|
  /\/ /_ | | | \__ \ |_| |  | |_| | (__| |_\__ \
  \____/ |_| |_|___/\__|_|   \__,_|\___|\__|___/
";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.Write(title);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(WindowWidth / 2 - 6, 7);
            Console.WriteLine("Directions:");
            Console.SetCursorPosition(WindowWidth / 2 - 13, 9);
            Console.WriteLine(" <- key - move left");
            Console.SetCursorPosition(WindowWidth / 2 - 13, 11);
            Console.WriteLine(" -> key - move right");
            Console.SetCursorPosition(WindowWidth / 2 - 16, 13);
            Console.WriteLine("Move the pad to navigate the ball.");
            Console.SetCursorPosition(WindowWidth / 2 - 12, 14);
            Console.WriteLine("Try to break all bricks.");
            Console.SetCursorPosition(WindowWidth / 2 - 23, 16);
            Console.WriteLine("You have 4 lives. A broken brick gives a point.");
            Console.SetCursorPosition(WindowWidth / 2 - 22, 17);
            Console.WriteLine("For every 10 points collected, you level up.");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 11, 19);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Press m to enter menu.");


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
                    Console.SetCursorPosition(WindowHeight / 2, 10);
                    Environment.Exit(0);
                }
            } while (true);
        }

        private static void ResetGame()
        {
            over = false;
            pad = new Pad(WindowWidth / 2 - 3, WindowHeight - 1, ConsoleColor.White);
            ball = new Ball(WindowWidth / 2 + 1, WindowHeight - 2, ConsoleColor.Red);
            player = new Player(0, 4);
            Console.Clear();
            gameSpeed = InitialGameSpeed;
            gameLevel = 1;
            Main();
        }
    }
}
