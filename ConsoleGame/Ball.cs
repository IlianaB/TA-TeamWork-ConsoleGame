using System;

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

        public Ball(int x, int y, ConsoleColor color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.rightDirection = true;
            this.topDirection = true;
        }

        public void CheckWallCollision(Pad pad)
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
                if (pad.X <= this.x && pad.X + pad.Width >= this.x)
                {
                    this.topDirection = true;
                    this.y -= 2;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        public void CheckBrickCollision(Brick[,] bricks)
        {
            try
            {
                if (!bricks[this.y, this.x].IsBroken)
                {
                    bricks[this.y, this.x].IsBroken = true;
                    this.topDirection = !this.topDirection;
                    this.rightDirection = !this.rightDirection;
                }
            }
            catch (Exception) { }
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
            return this.x == ConsoleGame.windowWidth;
        }

        public bool IsLeftWallCollision()
        {
            return this.x == -1;
        }

        public bool IsBottomWallCollision()
        {
            return this.y == ConsoleGame.windowHeight - 1;
        }

        public bool IsTopWallCollision()
        {
            return this.y == -1;
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
