using System;
using System.Threading;

namespace ConsoleGame
{
    class Ball
    {
        private int x;
        private int y;
        private int oldX;
        private int oldY;
        private bool stopDiagonal;
        private bool rightDirection;
        private bool topDirection;
        private ConsoleColor color;

        public Ball(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.stopDiagonal = false;
            this.rightDirection = true;
            this.topDirection = true;
        }

        public void CheckWallCollision(Pad pad, Player player)
        {
            if (this.IsRightWallCollision())
            {
                this.rightDirection = false;
                this.x -= 2;
            }

            if (this.IsLeftWallCollision()) 
            {
                this.rightDirection = true;
                this.x += 2;
            }

            if (this.IsTopWallCollision())   
            {
                this.topDirection = false;
                this.y += 2;
            }

            if (this.IsBottomWallCollision())  
            {

                if (rightDirection == true)
                {
                    if (this.y == Console.WindowHeight -1 && this.x == pad.X)
                    {
                        this.topDirection = true;
                        rightDirection = false;
                        this.y = y - 1;
                        this.x = x - 1;
                    }
                    else if (this.y == Console.WindowHeight - 1 && (this.x == pad.X + 1) || (this.x == pad.X + 2) || (this.x == pad.X + 3) || (this.x == pad.X + 4) || (this.x == pad.X + 5))
                    {
                        this.topDirection = true;
                        rightDirection = true;
                        this.y = y - 1;                        
                    }
                    else
                    {
                        if (player.Lives > 1)
                        {
                            Draw('*');
                            player.Lives--;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(ConsoleGame.windowWidth / 3 - 1, ConsoleGame.windowHeight / 2);
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
                            this.x = ConsoleGame.windowWidth / 2;
                            this.y = ConsoleGame.windowHeight - 2;
                            pad.X = ConsoleGame.windowWidth / 2 - pad.Width / 2;
                            this.topDirection = true;
                            ConsoleGame.Play();

                        }
                        else
                        {
                            Draw('*');
                            string gameOver = "G A M E   O V E R";
                            Console.SetCursorPosition(ConsoleGame.windowWidth / 2 - gameOver.Length / 2, ConsoleGame.windowHeight / 2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(gameOver);
                            Thread.Sleep(1000);
                            ConsoleGame.over = true;
                        }

                    }
                }
                else
                {
                      if (this.y == Console.WindowHeight -1 && this.x == pad.X + 4)
                    {
                        this.topDirection = true;
                        rightDirection = true;
                        this.y = y - 1;
                        this.x = x +1;
                    }
                      else if (this.y == Console.WindowHeight - 1 && (this.x == pad.X + 3) || (this.x == pad.X + 2) || (this.x == pad.X + 1) || (this.x == pad.X) || (this.x == pad.X-1))
                      {
                          this.topDirection = true;
                          rightDirection = false;
                          this.y = y - 1;

                      }
                      else
                      {
                          if (player.Lives > 1)
                          {
                              Draw('*');
                              player.Lives--;
                              Console.ForegroundColor = ConsoleColor.Red;
                              Console.SetCursorPosition(ConsoleGame.windowWidth / 3 - 1, ConsoleGame.windowHeight / 2);
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
                              this.x = ConsoleGame.windowWidth / 2;
                              this.y = ConsoleGame.windowHeight - 2;
                              pad.X = ConsoleGame.windowWidth / 2 - pad.Width / 2;
                              this.topDirection = true;
                              ConsoleGame.Play();

                          }
                          else
                          {
                              Draw('*');
                              string gameOver = "G A M E   O V E R";
                              Console.SetCursorPosition(ConsoleGame.windowWidth / 2 - gameOver.Length / 2, ConsoleGame.windowHeight / 2);
                              Console.ForegroundColor = ConsoleColor.Red;
                              Console.WriteLine(gameOver);
                              Thread.Sleep(1000);
                              ConsoleGame.over = true;
                          }
                      }
                }
            }
        }

        public void CheckBrickCollision(Brick[,] bricks, Player player)
        {
            try
            {
                if (!bricks[this.y, this.x].IsBroken)
                {                   
                    player.Score++;
                    if (player.Score % 10 == 0 && player.Score != 0)
                    {
                        ConsoleGame.gameLevel++;
                        ConsoleGame.gameSpeed = ConsoleGame.InitialGameSpeed - ConsoleGame.gameLevel * 6;
                    }

                    printScore(player);
                    bricks[this.y, this.x].IsBroken = true;
                    this.topDirection = !this.topDirection;
                    this.rightDirection = !this.rightDirection;
                }
            }
            catch (Exception) { }
        }


        public static void printScore(Player player)
        {
            Console.SetCursorPosition((Console.WindowWidth/2)-9, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Score: {0} Level: {1}" , player.Score, ConsoleGame.gameLevel);
            Console.SetCursorPosition((Console.WindowWidth / 2) - 13, 1);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Press P to pause the game.");
        }

        public void ChangePosition()
        {
            this.oldX = this.x;
            this.oldY = this.y;

            if (!this.stopDiagonal)
            {
                this.x = this.rightDirection ? this.x + 1 : this.x - 1;
            }

            this.y = this.topDirection ? this.y - 1 : this.y + 1;
        }

        public bool IsRightWallCollision()
        {
            return this.x == ConsoleGame.windowWidth;
        }

        public bool IsLeftWallCollision()
        {
            return this.x == -1;
        }

        public bool IsBottomWallCollision()
        {
            return this.y == ConsoleGame.windowHeight-1; 
        }

        public bool IsTopWallCollision()
        {
            return this.y == +1;
        }

        public void Draw(char symbol)
        {
            Console.ForegroundColor = this.color;

            // remove old ball
            Console.SetCursorPosition(this.oldX, this.oldY);
            Console.Write(' ');

            // write new ball
            Console.SetCursorPosition(this.x, this.y);
            Console.Write(symbol);
        }
    }
}
