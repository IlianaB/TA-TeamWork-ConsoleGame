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

        public void ChangePosition()
        {
            this.oldX = this.x;
            this.oldY = this.y;

            this.x = this.rightDirection ? this.x + 1 : this.x - 1;
            this.y = this.topDirection ? this.y - 1 : this.y + 1;
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
