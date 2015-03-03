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
        private bool rightDirection;
        private bool topDirection;
        private ConsoleColor color;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public bool TopDirection
        {
            get { return topDirection; }
            set { topDirection = value; }
        }

        public Ball(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
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
                    if (this.y == Console.WindowHeight - 1 && this.x == pad.X)
                    {
                        this.topDirection = true;
                        rightDirection = false;
                        this.y = y - 1;
                    }
                    else if (this.y == Console.WindowHeight - 1 && (this.x == pad.X + 1) || (this.x == pad.X + 2) || (this.x == pad.X + 3))
                    {
                        this.topDirection = true;
                        rightDirection = true;
                        this.y = y - 1;
                    }
                    else if (this.y == Console.WindowHeight - 1 && ((this.x == pad.X + 4)))
                    {
                        this.topDirection = true;
                        rightDirection = true;
                        this.y = y - 1;
                    }
                    else
                    {
                        if (player.Lives > 1)
                        {
                            ConsoleGame.DrawLostLivesScreen();
                        }
                        else
                        {
                            ConsoleGame.DrawGameOverScreen();
                        }
                    }
                }
                else
                {
                    if (this.y == Console.WindowHeight - 1 && this.x == pad.X + 4)
                    {
                        this.topDirection = true;
                        rightDirection = true;
                        this.y = y - 1;
                    }
                    else if (this.y == Console.WindowHeight - 1 && (this.x == pad.X + 3) || (this.x == pad.X + 2) || (this.x == pad.X + 1))
                    {
                        this.topDirection = true;
                        rightDirection = false;
                        this.y = y - 1;

                    }

                    else if (this.y == Console.WindowHeight - 1 && ((this.x == pad.X)))
                    {
                        this.topDirection = true;
                        rightDirection = false;
                        this.y = y - 1;
                    }
                    else
                    {
                        if (player.Lives > 1)
                        {
                            ConsoleGame.DrawLostLivesScreen();
                        }
                        else
                        {
                            ConsoleGame.DrawGameOverScreen();
                        }
                    }
                }
            }
        }

        public void CheckBrickCollision(Brick[,] bricks, Player player)
        {
            if (this.y < 9 && this.x < ConsoleGame.WindowWidth - 1 && this.x > 0)
            {
                if (!bricks[this.y, this.x].IsBroken)
                {
                    Console.Beep(497, 40);
                    player.Score++;
                    if (player.Score % 10 == 0 && player.Score != 0)
                    {
                        ConsoleGame.gameLevel++;
                        ConsoleGame.gameSpeed = ConsoleGame.InitialGameSpeed - ConsoleGame.gameLevel * 4;
                    }

                    ConsoleGame.printScore();
                    bricks[this.y, this.x].IsBroken = true;

                    if (this.topDirection == true)
                    {
                        if (rightDirection)
                        {
                            if (this.y == 8)
                            {
                                topDirection = false;
                            }
                            else if (this.y == 6 && this.x != 3)
                            {
                                if (bricks[this.y + 1, this.x].IsBroken == false || bricks[this.y + 1, this.x + 1].IsBroken == false)
                                {
                                    topDirection = false;
                                    rightDirection = false;
                                }
                                else if (bricks[this.y + 1, this.x].IsBroken == true && bricks[this.y + 1, this.x + 1].IsBroken == true)
                                {
                                    topDirection = false;
                                }
                            }
                            else if (this.x == 3)
                            {
                                rightDirection = false;
                            }
                            else
                            {
                                if ((bricks[this.y + 1, this.x].IsBroken == false || bricks[this.y + 1, this.x + 1].IsBroken == false) &&
                                    (bricks[this.y - 1, this.x - 1].IsBroken == false || bricks[this.y, this.x - 1].IsBroken == false))
                                {
                                    topDirection = false;
                                    rightDirection = false;
                                }
                                else if ((bricks[this.y + 1, this.x].IsBroken == false || bricks[this.y + 1, this.x + 1].IsBroken == false) &&
                                    (bricks[this.y - 1, this.x - 1].IsBroken == true && bricks[this.y, this.x - 1].IsBroken == true))
                                {
                                    rightDirection = false;
                                }
                                else if ((bricks[this.y + 1, this.x].IsBroken == true && bricks[this.y + 1, this.x + 1].IsBroken == true) &&
                                   (bricks[this.y - 1, this.x - 1].IsBroken == false || bricks[this.y, this.x - 1].IsBroken == false))
                                {
                                    topDirection = false;
                                }
                                else
                                {
                                    rightDirection = false;
                                }
                            }
                        }
                        else
                        {
                            if (this.y == 8)
                            {
                                topDirection = false;
                            }
                            else if (this.y == 6 && this.x != 45)
                            {
                                if (bricks[this.y + 1, this.x].IsBroken == false || bricks[this.y + 1, this.x - 1].IsBroken == false)
                                {
                                    topDirection = false;
                                    rightDirection = true;
                                }
                                else if (bricks[this.y + 1, this.x].IsBroken == true && bricks[this.y + 1, this.x - 1].IsBroken == true)
                                {
                                    topDirection = false;
                                }
                            }
                            else if (this.x == 45)
                            {
                                rightDirection = true;
                            }
                            else
                            {
                                if ((bricks[this.y + 1, this.x].IsBroken == false || bricks[this.y + 1, this.x - 1].IsBroken == false) &&
                                    (bricks[this.y - 1, this.x + 1].IsBroken == false || bricks[this.y, this.x + 1].IsBroken == false))
                                {
                                    topDirection = false;
                                    rightDirection = true;
                                }
                                else if ((bricks[this.y + 1, this.x].IsBroken == false || bricks[this.y + 1, this.x - 1].IsBroken == false) &&
                                    (bricks[this.y - 1, this.x + 1].IsBroken == true && bricks[this.y, this.x + 1].IsBroken == true))
                                {
                                    rightDirection = true;
                                }
                                else if ((bricks[this.y + 1, this.x].IsBroken == true && bricks[this.y + 1, this.x - 1].IsBroken == true) &&
                                   (bricks[this.y - 1, this.x + 1].IsBroken == false || bricks[this.y, this.x + 1].IsBroken == false))
                                {
                                    topDirection = false;
                                }
                                else
                                {
                                    rightDirection = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rightDirection)
                        {
                            if (this.y == 6)
                            {
                                topDirection = true;
                            }
                            else if (this.y == 8 && this.x != 3)
                            {
                                if (bricks[this.y - 1, this.x].IsBroken == false || bricks[this.y - 1, this.x + 1].IsBroken == false)
                                {
                                    topDirection = true;
                                    rightDirection = false;
                                }
                                else if (bricks[this.y - 1, this.x].IsBroken == true && bricks[this.y - 1, this.x + 1].IsBroken == true)
                                {
                                    topDirection = true;
                                }
                                else if (this.x == 3)
                                {
                                    rightDirection = false;
                                }
                            }
                            else
                            {
                                if ((bricks[this.y - 1, this.x].IsBroken == false || bricks[this.y - 1, this.x + 1].IsBroken == false) &&
                                    (bricks[this.y + 1, this.x - 1].IsBroken == false || bricks[this.y, this.x - 1].IsBroken == false))
                                {
                                    topDirection = true;
                                    rightDirection = false;
                                }
                                else if ((bricks[this.y - 1, this.x].IsBroken == false || bricks[this.y - 1, this.x + 1].IsBroken == false) &&
                                    (bricks[this.y + 1, this.x - 1].IsBroken == true && bricks[this.y, this.x - 1].IsBroken == true))
                                {
                                    rightDirection = false;
                                }
                                else if ((bricks[this.y - 1, this.x].IsBroken == true && bricks[this.y - 1, this.x + 1].IsBroken == true) &&
                                   (bricks[this.y + 1, this.x - 1].IsBroken == false || bricks[this.y, this.x - 1].IsBroken == false))
                                {
                                    topDirection = true;
                                }
                                else
                                {
                                    rightDirection = false;
                                }
                            }
                        }

                        else
                        {
                            if (this.y == 6)
                            {
                                topDirection = true;
                            }
                            else if (this.y == 8 && this.x != 45)
                            {
                                if (bricks[this.y - 1, this.x].IsBroken == false || bricks[this.y - 1, this.x - 1].IsBroken == false)
                                {
                                    topDirection = true;
                                    rightDirection = true;
                                }
                                else if (bricks[this.y - 1, this.x].IsBroken == true && bricks[this.y - 1, this.x - 1].IsBroken == true)
                                {
                                    topDirection = true;
                                }
                            }
                            else if (this.x == 45)
                            {
                                rightDirection = true;
                            }
                            else
                            {
                                if ((bricks[this.y - 1, this.x].IsBroken == false || bricks[this.y - 1, this.x - 1].IsBroken == false) &&
                                    (bricks[this.y + 1, this.x + 1].IsBroken == false || bricks[this.y, this.x + 1].IsBroken == false))
                                {
                                    topDirection = true;
                                    rightDirection = true;
                                }
                                else if ((bricks[this.y - 1, this.x].IsBroken == false || bricks[this.y - 1, this.x - 1].IsBroken == false) &&
                                    (bricks[this.y + 1, this.x + 1].IsBroken == true && bricks[this.y, this.x + 1].IsBroken == true))
                                {
                                    rightDirection = true;
                                }
                                else if ((bricks[this.y - 1, this.x].IsBroken == true && bricks[this.y - 1, this.x - 1].IsBroken == true) &&
                                   (bricks[this.y + 1, this.x + 1].IsBroken == false || bricks[this.y, this.x + 1].IsBroken == false))
                                {
                                    topDirection = true;
                                }
                                else
                                {
                                    rightDirection = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ChangePosition()
        {
            this.oldX = this.x;
            this.oldY = this.y;
            this.x = this.rightDirection ? this.x + 1 : this.x - 1;
            this.y = this.topDirection ? this.y - 1 : this.y + 1;
        }

        public bool IsRightWallCollision()
        {
            return this.x == ConsoleGame.WindowWidth - 1;
        }

        public bool IsLeftWallCollision()
        {
            return this.x == 0;
        }

        public bool IsBottomWallCollision()
        {
            return this.y == ConsoleGame.WindowHeight - 1;
        }

        public bool IsTopWallCollision()
        {
            return this.y == +2;
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
