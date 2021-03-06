﻿using System;
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
        private static Brick[,] bricks = new Brick[9, WindowWidth - 1];
        public static string[] colors = { "Green", "Yellow" };
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
                    Console.Beep(333, 75);

                }
                else if (lines[4] == false && chosenLine.Key == ConsoleKey.DownArrow)
                {
                    lines[currentSelectedItem] = false;
                    lines[++currentSelectedItem] = true;
                    Console.Beep(333, 75);
                }
                else if (chosenLine.Key == ConsoleKey.Enter)
                {
                    goIntoSelectedMenu(lines);
                }

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
                DrawScores();
            }
            else if (array[2] == true)
            {
                DrawInstructions();
            }
            else if (array[3] == true)
            {
                DrawAbout();
            }
            else if (array[4] == true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(WindowHeight / 2, 10);
                Environment.Exit(0);
            }
        }

        private static void DrawAbout()
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
            Console.WriteLine(@"The B-Breaker game was developed by TEAM IMP as a");
            Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 12);
            Console.WriteLine(@"team project in the Telerik Academy C#2 course.");
            Console.SetCursorPosition(WindowWidth / 2 - 15, WindowHeight - 10);
            Console.WriteLine(@"The marvelous TEAM IMP:");
            Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 8);
            Console.WriteLine(@"Iliana Bobeva, Lyuba Gerassimova");
            Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 7);
            Console.WriteLine(@"Bistra Gospodinova, Dimitar Bakardzhiev");
            Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 6);
            Console.WriteLine(@"Dragomir Tachev, Emo Penovski");
            Console.SetCursorPosition(WindowWidth - 49, WindowHeight - 5);
            Console.WriteLine(@"Kiril Mihaylov, Petar Zubev");
            Console.SetCursorPosition(WindowWidth / 2 - 11, WindowHeight - 2);

            GoBackToMenu();
        }

        private static void DrawScores()
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

            try
            {
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
            }
            catch (Exception)
            {
                Console.SetCursorPosition(WindowWidth / 2 - 14, 10);
                Console.WriteLine("There are no High Scores yet!");
            }

            GoBackToMenu();
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
            printScore();
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
                for (int row = 0; row <= Console.WindowHeight - 2; row++)
                {
                    Console.SetCursorPosition(Console.WindowWidth - 1, row);
                    Console.Write("|");
                }
                for (int row = 0; row <= Console.WindowHeight - 2; row++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write("|");
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
            for (int row = 3; row <= 8; row++)
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
                bricks[row, 2].IsBroken = true;
                bricks[row, 3].IsBroken = true;
                bricks[row, 46].IsBroken = true;
                bricks[row, 47].IsBroken = true;
                bricks[row, 48].IsBroken = true;
            }
            for (int row = 0; row <= 5; row++)
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
            Random changeColor = new Random();
            for (int y = 0; y < bricks.GetLength(0); y++)
            {
                for (int x = 0; x < bricks.GetLength(1); x++)
                {
                    changeColor.Next(0, 2);
                    bricks[y, x] = new Brick(x, y, (ConsoleColor)Enum.Parse(Brick.type, colors[changeColor.Next(0, 2)]));
                }
            }
        }

        private static void CheckGameOver()
        {
            if (over)
            {
                string gameOverTitle = @"
              _____    
     ___..--""      `. 
..-'               ,' ___                     
                  ,' / _ \__ _ _ __ ___   ___   
   (|\          ,'  / /_\/ _` | '_ ` _ \ / _ \
      ________,'   / /_\\ (_| | | | | | |  __/
   ,.`/`./\/`/     \_____\__,_|_| |_| |_|\___|
  /-'             /___\_   _____ _ __      
   `',^/\/\      //  /| \ / / _ \ '__|     
________,'      / \_// \ V /  __/ |        
                \___/   \_/ \___|_|        
";
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(0, 0);
                Console.Write(gameOverTitle);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(WindowWidth / 2 - 9, WindowHeight - 5);
                Console.WriteLine("Your score is {0}!", player.Score);

                try
                {
                    using (StreamWriter file = new StreamWriter(HighscoresFile, true))
                    {
                        file.WriteLine(player.Score);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                GoBackToMenu();
            }
        }

        public static void DrawGameOverScreen()
        {
            ball.Draw('*');
            string gameOver = "G A M E   O V E R";
            Console.Beep(222, 200);
            Console.Beep(200, 200);
            Console.SetCursorPosition(WindowWidth / 2 - gameOver.Length / 2, WindowHeight / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(gameOver);
            Thread.Sleep(30);
            Console.Beep(180, 200);
            Console.Beep(130, 700);
            over = true;
        }

        private static void DrawInstructions()
        {
            string title = @"
    _  _            _____    ___ _           
   | || |_____ __ _|_   _|__| _ \ |__ _ _  _ 
   | __ / _ \ V  V / | |/ _ \  _/ / _` | || |
   |_||_\___/\_/\_/  |_|\___/_| |_\__,_|\_, |
                                        |__/ 
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

            GoBackToMenu();
        }

        private static void GoBackToMenu()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 11, WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Press M to enter menu.");

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

        public static void DrawLostLivesScreen()
        {
            ball.Draw('*');
            Console.Beep(150, 350);
            player.Lives--;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(WindowWidth / 3 - 1, WindowHeight / 2);
            if (player.Lives == 1)
            {
                Console.Write("You have {0} life left", player.Lives);
            }
            else
            {
                Console.Write("You have {0} lives left", player.Lives);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1500);
            ball.X = WindowWidth / 2;
            ball.Y = WindowHeight - 2;
            pad.X = WindowWidth / 2 - pad.Width / 2;
            ball.TopDirection = true;
            Play();
        }

        public static void printScore()
        {
            Console.SetCursorPosition((WindowWidth / 2) - 9, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Score: {0} Level: {1}", player.Score, gameLevel);
            Console.SetCursorPosition((WindowWidth / 2) - 13, 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Press P to pause the game.");
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
